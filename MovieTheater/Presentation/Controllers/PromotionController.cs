using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Entity;
using WebAPI.Services;
using WebAPI.Services.DTO;
using WebAPI.Services.DTO.Response;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/Promotion")]
    public class PromotionController : ControllerBase
    {
        private readonly IPromotionService _service;
        private readonly Cloudinary _cloudinary;
        public PromotionController(IPromotionService service, Cloudinary cloudinary)
        {
            _service = service;
            _cloudinary = cloudinary;
        }

        [HttpGet]
        public async Task<IActionResult> GetPromotions([FromQuery] int page = 1, [FromQuery] int pageSize = 5, [FromQuery] string? searchTerm = null)
        {
            var (promotions, totalCount) = await _service.GetPromotionsAsync(page, pageSize, searchTerm);
            return Ok(new { Promotions = promotions, TotalCount = totalCount });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPromotion(int id)
        {
            var promotion = await _service.GetPromotionByIdAsync(id);
            if (promotion == null) return NotFound();
            return Ok(promotion);
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllPromotions()
        {
            var promotions = await _service.GetAllPromotionsAsync();
            return Ok(promotions);
        }

        [HttpPost]
        public async Task<IActionResult> AddPromotion([FromForm] ResponseDTOPromotion promotionResponse, IFormFile? imageFile)
        {
            try
            {
                // Upload image if provided
                if (imageFile != null)
                {
                    var uploadResult = await UploadImageAsync(imageFile);
                    if (!string.IsNullOrEmpty(uploadResult))
                    {
                        promotionResponse.Image = uploadResult;
                    }
                }

                var promotion = new Promotion
                {
                    Title = promotionResponse.Title,
                    Detail = promotionResponse.Detail,
                    DiscountLevel = promotionResponse.DiscountLevel,
                    StartTime = promotionResponse.StartTime,
                    EndTime = promotionResponse.EndTime,
                    Image = promotionResponse.Image
                };

                await _service.AddPromotionAsync(promotion);

                return CreatedAtAction(nameof(GetPromotion), new { id = promotion.PromotionId }, promotion);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePromotion(int id, [FromForm] ResponseDTOPromotion promotionResponse, IFormFile? imageFile)
        {
            try
            {
                // Upload image if provided
                if (imageFile != null)
                {
                    var uploadResult = await UploadImageAsync(imageFile);
                    if (!string.IsNullOrEmpty(uploadResult))
                    {
                        promotionResponse.Image = uploadResult;
                    }
                }

                var promotion = new Promotion
                {
                    PromotionId = id,
                    Title = promotionResponse.Title,
                    Detail = promotionResponse.Detail,
                    DiscountLevel = promotionResponse.DiscountLevel,
                    StartTime = promotionResponse.StartTime,
                    EndTime = promotionResponse.EndTime,
                    Image = promotionResponse.Image
                };

                await _service.UpdatePromotionAsync(promotion);

                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }
        private async Task<string?> UploadImageAsync(IFormFile? imageFile)
        {
            if (imageFile == null || imageFile.Length == 0)
            {
                return null;
            }

            using var stream = imageFile.OpenReadStream();
            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(imageFile.FileName, stream),
                Folder = "promotions"
            };

            var uploadResult = await _cloudinary.UploadAsync(uploadParams);

            if (uploadResult.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return uploadResult.SecureUrl.ToString();
            }

            return null;
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePromotion(int id)
        {
            await _service.DeletePromotionAsync(id);
            return NoContent();
        }
    }
}


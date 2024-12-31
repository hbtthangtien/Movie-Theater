using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using WebAPI.Entity;
using WebAPI.Exception;
using WebAPI.Repository;
using WebAPI.Services.DTO.Request;
using WebAPI.Services.DTO.Response;

namespace WebAPI.Services.Impl
{
    public class AuthenticateServiceImpl : GenericServices, IAuthenticateService
    {
        private readonly IConfiguration _configuration;
        private readonly SignInManager<ApplicationUser> _signInManager;
        public AuthenticateServiceImpl(IMapper mapper, IUnitOfWork unitOfWork, IConfiguration configuration,
            SignInManager<ApplicationUser> signInManager
            )
            : base(mapper, unitOfWork)
        {
            _configuration = configuration;
            _signInManager = signInManager;
        }

        public async Task<ResponseDTOToken> createToken(ApplicationUser user)
        {
            var roles = await _context.Accounts.GetRolesAsync(user);
            List<Claim> claims = new List<Claim>
            {
                new Claim("ID", user.Id),
                new Claim("Fullname", user.Fullname),
                new Claim("Identity cart", user.IdentityCard),
                new Claim("Email", user.Email),
                new Claim("Address",user.Address),
                new Claim("Phone number",user.PhoneNumber),
                new Claim("Birthday",user.DateOfBirth.ToString()),
                new Claim("Gender",user.Gender),
                new Claim(ClaimTypes.Name,user.UserName)

            };
            foreach (var role in roles)
                claims.Add(new Claim(ClaimTypes.Role, role));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                _configuration.GetSection("AppSettings:Token").Value!
               ));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var tokenDescription = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddMinutes(30),
                SigningCredentials = creds

            };
            var token = new JwtSecurityTokenHandler().CreateToken(tokenDescription);
            var accessToken = new JwtSecurityTokenHandler().WriteToken(token);

            return new ResponseDTOToken
            {
                AccessToken = accessToken,
                RefreshToken = GenerateToken()
            };
        }

        public async Task<ResponseDTOToken> Login(RequestDTOLogin account)
        {
            var user = await _context.Accounts.FindByNameAsync(account.Username)
                        ?? throw new NotFoundException("Username is not existed");
            var result = BCrypt.Net.BCrypt.Verify(account.Password, user.Password);
            if (result)
            {
                var response = _mapper.Map<ResponseDTOLogin>(user);
                var token = await createToken(user);
                user.RefreshToken = token.RefreshToken;
                user.RefreshTokenExpire = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(7));
                await _context.Accounts.UpdateAsync(user);
                return token;
            }
            else
            {
                throw new System.Exception("Password is not correct");
            }
        }


        public async Task<ResponseDTOToken> refreshToken(string refresh_token)
        {
            var user = _context.Accounts.Users.FirstOrDefault(u => u.RefreshToken == refresh_token)
                        ?? throw new NotFoundException("Refresh token is invalid!!!");
            if (user.RefreshTokenExpire < DateOnly.FromDateTime(DateTime.Now))
            {
                user.RefreshToken = null;
                user.RefreshTokenExpire = null;
                await _context.Accounts.UpdateAsync(user);
                throw new System.Exception("Refresh token is expired date!!!");
                
            }
            var token = await createToken(user);
            token.RefreshToken = user.RefreshToken;
            return token;
        }

        public async Task RemoveRefreshToken(string usrename)
        {
            var user = await _context.Accounts.FindByNameAsync(usrename)
                ?? throw new NotFoundException("Log out failed!!!");
            user.RefreshToken = null;
            user.RefreshTokenExpire = null;
            await _context.Accounts.UpdateAsync(user);

        }

        private string GenerateToken()
        {
            var random = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(random);
                return Convert.ToBase64String(random);
            }

        }
    }
}

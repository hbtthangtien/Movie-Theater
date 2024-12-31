using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using WebAPI.Entity;
using WebAPI.Repository;
using WebAPI.Services.DTO;
using WebAPI.Services.DTO.Request;
using WebAPI.Services.DTO.Response;
using System;
using WebAPI.Constant;
using Microsoft.EntityFrameworkCore;
using Azure.Core;
using WebAPI.Exception;
using Microsoft.EntityFrameworkCore.Storage;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Azure;


namespace WebAPI.Services.Impl
{
    public class AccountServiceImpl : GenericServices, IAccountService
    {
        private readonly IValidService _validService;
        private readonly IConfiguration _configuration;
        private readonly MovieTheaterContext _movieTheaterContext;
        private readonly Cloudinary _cloudinary;
        private IDbContextTransaction _transaction;
        public AccountServiceImpl(IUnitOfWork accountRepository,
            IValidService validService,
            IConfiguration configuration, IMapper mapper,
            Cloudinary cloudinary,
            MovieTheaterContext movieTheaterContext) : base(mapper, accountRepository)
        {
            _configuration = configuration;
            _validService = validService;
            _movieTheaterContext = movieTheaterContext;
            _cloudinary = cloudinary;
        }

        public async Task<ResponseDTOMemberCheck> CheckExistMember(string member)
        {
            var user = await _context.Accounts.Users.Include(e => e.Members).FirstOrDefaultAsync(u => u.IdentityCard == member
                      || u.Id == member) ?? throw new NotFoundException("Id user or Identity Card is not exist!!!");
            var m = user.Members!.First();
            return new ResponseDTOMemberCheck
            {
                Score = (double)m.Score!,
                MemberId = m.MemberId,
                Fullname = user.Fullname!,
                IdentityCart = user.IdentityCard,
                PhoneNumber = user.PhoneNumber!
            };
        }
        public async Task CreateAccount(RequestDTORegister accountDTO)
        {
            if (accountDTO.Password != accountDTO.ConfirmPassword)
            {
                throw new System.Exception("Password is not same confirm password");
            }
            if (!_validService.IsEmailValid(accountDTO.Email!))
            {

                throw new System.Exception("Email matches one or more alphanumeric characters before the @ symbol. and have at least 1 domain");
            }
            if (!_validService.IsUsernameValid(accountDTO.Username!))
            {
                throw new System.Exception("Username must be alphanumeric and doesn't contain a speacial character");
            }
            if (!_validService.IsPasswordValid(accountDTO.Password!))
            {
                throw new System.Exception("Password must be between 8 and 31 character and must be alphanumeric and contain at least 1 special character");
            }
            if (!_validService.IsAgeValid((DateOnly)accountDTO.DateOfBirth!))
            {
                throw new System.Exception("User's age must be larger than 13");
            }
            try
            {
                await BeginTransaction();
                var newAccount = _mapper.Map<ApplicationUser>(accountDTO);
                newAccount.Status = 1;
                newAccount.Password = BCrypt.Net.BCrypt.HashPassword(accountDTO.Password);
                var status = await _context.Accounts.CreateAsync(newAccount);
                var status2 = await _context.Accounts.AddToRoleAsync(newAccount, UserRole.MEMBER);
                var memberID = GenerateTimestampId();
                var user = await _context.Accounts.FindByNameAsync(newAccount.UserName!);
                if (!status.Succeeded) throw new System.Exception(status.Errors.First().Description);
                var member = new Member
                {
                    Account = user,
                    AccountId = user.Id,
                    MemberId = memberID,
                    Score = 0.0
                };
                await _context.Members.AddAsync(member);
                await _context.CommitAsync();
                await CommitTransaction();
               

            }
            catch (System.Exception ex)
            {
                await RollbackTransaction();
                var message1 = ex.InnerException;
                string x =  ((message1 != null) ? message1.Message : ex.Message);
                throw new System.Exception(x);
            }
            
        }        
    private string GenerateTimestampId()
    {
        string timestampPart = DateTime.Now.Ticks.ToString().Substring(0, 8);
        string randomPart = new Random().Next(10, 99).ToString();
        return timestampPart + randomPart;
    }
    private async Task BeginTransaction()
    {
        if (_transaction == null)
        {
            _transaction = await _movieTheaterContext.Database.BeginTransactionAsync();
        }
    }
    private async Task CommitTransaction()
    {
        if (_transaction != null)
        {
            await _transaction.CommitAsync();
        }
    }
    private async Task RollbackTransaction()
    {
        if (_transaction != null)
        {
            await _transaction.RollbackAsync();
        }
    }

    public async Task<double> GetScoreByIdMember(string member)
    {
        return await _context.Members.getScoreById(member);
    }

    public async Task<ResponseDTOApi> CreateAccountEmployee(RequestDTORegister accountDTO)
    {
        if (accountDTO.Password != accountDTO.ConfirmPassword)
        {
            return new ResponseDTOApi
            {
                StatusCode = HttpStatusCode.BadRequest,
                StatusMessage = "Register is not sucessfully",
                errors = new List<string> { "Password is not same confirm password" }
            };
        }
        ResponseDTOApi response = new ResponseDTOApi();
        if (!_validService.IsEmailValid(accountDTO.Email!))
        {
            response.StatusCode = HttpStatusCode.BadRequest;
            response.errors.Add("Email matches one or more alphanumeric characters before the @ symbol. and have at least 1 domain");
        }
        if (!_validService.IsUsernameValid(accountDTO.Username!))
        {
            response.StatusCode = HttpStatusCode.BadRequest;
            response.errors.Add("Username must be alphanumeric and doesn't contain a speacial character");
        }
        if (!_validService.IsPasswordValid(accountDTO.Password!))
        {
            response.StatusCode = HttpStatusCode.BadRequest;
            response.errors.Add("Password must be between 8 and 31 character and must be alphanumeric and contain at least 1 special character");
        }
        if (!_validService.IsAgeValid((DateOnly)accountDTO.DateOfBirth!))
        {
            response.StatusCode = HttpStatusCode.BadRequest;
            response.errors.Add("User's age must be larger than 13");
        }
        if (!(response.StatusCode == HttpStatusCode.BadRequest))
        {
            try
            {
                await BeginTransaction();
                var newAccount = _mapper.Map<ApplicationUser>(accountDTO);
                newAccount.Status = 1;
                newAccount.Password = BCrypt.Net.BCrypt.HashPassword(accountDTO.Password);
                var status = await _context.Accounts.CreateAsync(newAccount);
                var status2 = await _context.Accounts.AddToRoleAsync(newAccount, UserRole.EMPLOYEE);
                var employeeID = GenerateTimestampId();
                var user = await _context.Accounts.FindByNameAsync(newAccount.UserName!);
                if (!status.Succeeded) throw new System.Exception(status.ToString());
                var employee = new Employee
                {
                    Account = user,
                    AccountId = user.Id,
                    EmployeeId = employeeID,
                };
                await _context.Employees.AddAsync(employee);
                await _context.CommitAsync();
                await CommitTransaction();
                response.StatusMessage = status2.ToString();
                response.StatusCode = (status.Succeeded ? HttpStatusCode.Created : HttpStatusCode.BadRequest);

            }
            catch (System.Exception ex)
            {
                await RollbackTransaction();
                var message1 = ex.InnerException;
                response.StatusCode = HttpStatusCode.BadRequest;
                response.StatusMessage = "Add not successfull!!!";
                response.errors = ((message1 != null) ? message1.Message : ex.Message).Split('\\').ToList();
            }

        }
        return response;
    }

    public async Task<ResponseDTOMyAccount> GetAccountMember(string userName)
    {
        var account = await _context.Accounts.FindByNameAsync(userName) ?? throw new NotFoundException("User not found.");
        var member = await _context.Members.GetMemberByAccountId(account.Id);
        if (member == null) throw new NotFoundException("Not find Account ID");
        return new ResponseDTOMyAccount
        {
            AccountId = member.AccountId,
            Username = member.Account.UserName,
            Fullname = member.Account.Fullname,
            Email = member.Account.Email,
            IdentityCard = member.Account.IdentityCard,
            PhoneNumber = member.Account.PhoneNumber,
            Address = member.Account.Address,
            Gender = member.Account.Gender,
            Score = member.Score,
            DateOfBirth = member.Account.DateOfBirth,
            RegisterDate = member.Account.RegisterDate,
            Image = member.Account.Image,
        };

    }

    public async Task<ResponseDTOApi> EditMember(string userName, RequestDTOMyAccount memberDTO, IFormFile? profileImage)
    {
        ResponseDTOApi response = new ResponseDTOApi();

        try
        {
            if (!string.IsNullOrEmpty(memberDTO.Email) && !_validService.IsEmailValid(memberDTO.Email!))
            {
                response.StatusCode = HttpStatusCode.BadRequest;
                response.errors.Add("Email must match the format and contain at least 1 domain.");
                return response;
            }
            var account = await _context.Accounts.FindByNameAsync(userName) ?? throw new NotFoundException("User not found.");

            var member = await _context.Members.GetMemberByAccountId(account.Id);

            if (member == null)
            {
                response.StatusCode = HttpStatusCode.NotFound;
                response.StatusMessage = "Member not found.";
                return response;
            }

            await BeginTransaction();

            if (!string.IsNullOrEmpty(memberDTO.Fullname))
                member.Account.Fullname = memberDTO.Fullname;

            if (!string.IsNullOrEmpty(memberDTO.Email))
                member.Account.Email = memberDTO.Email;

            if (!string.IsNullOrEmpty(memberDTO.PhoneNumber))
                member.Account.PhoneNumber = memberDTO.PhoneNumber;

            if (!string.IsNullOrEmpty(memberDTO.Address))
                member.Account.Address = memberDTO.Address;

            if (!string.IsNullOrEmpty(memberDTO.DateOfBirth))
            {
                if (DateOnly.TryParse(memberDTO.DateOfBirth, out var parsedDateOfBirth))
                {
                    // Kiểm tra tuổi hợp lệ
                    if (!_validService.IsAgeValid(parsedDateOfBirth))
                    {
                        response.StatusCode = HttpStatusCode.BadRequest;
                        response.errors.Add("User's age is not valid.");
                        return response;
                    }
                }
                else
                {
                    response.StatusCode = HttpStatusCode.BadRequest;
                    response.errors.Add("Invalid date format. Use YYYY-MM-DD.");
                    return response;
                }
            }

            if (!string.IsNullOrEmpty(memberDTO.Gender))
                member.Account.Gender = memberDTO.Gender;

            if (profileImage != null)
            {
                var imageUrl = await UploadImageAsync(profileImage);
                if (imageUrl != null)
                {
                    member.Account.Image = imageUrl; // Cập nhật URL ảnh trong tài khoản
                }
                else
                {
                    response.StatusCode = HttpStatusCode.InternalServerError;
                    response.StatusMessage = "Failed to upload the profile image.";
                    return response;
                }
            }

            _context.Members.Update(member);
            await _context.CommitAsync();

            await CommitTransaction();

            response.StatusCode = HttpStatusCode.OK;
            response.StatusMessage = "Member updated successfully.";
        }
        catch (System.Exception ex)
        {
            await RollbackTransaction();
            response.StatusCode = HttpStatusCode.InternalServerError;
            response.StatusMessage = "Edit operation failed.";
            response.errors = new List<string> { ex.InnerException?.Message ?? ex.Message };
        }

        return response;
    }

    public async Task<ResponseDTOApi> ChangePassword(string userName, RequestDTOChangePassword passwordDTO)
    {
        var response = new ResponseDTOApi();
        try
        {

            if (passwordDTO.NewPassword != passwordDTO.ConfirmNewPassword)
            {
                response.StatusCode = HttpStatusCode.BadRequest;
                response.StatusMessage = "Change password is not successful";
                response.errors.Add("New password and confirm password do not match.");
                return response;
            }
            if (!_validService.IsPasswordValid(passwordDTO.NewPassword!))
            {
                response.StatusCode = HttpStatusCode.BadRequest;
                response.StatusMessage = "Change password is not successful";
                response.errors.Add("New password must be between 8 and 31 characters, alphanumeric, and contain at least 1 special character.");
                return response;
            }

            var account = await _context.Accounts.FindByNameAsync(userName) ?? throw new NotFoundException("User not found.");
            var member = await _context.Members.GetMemberByAccountId(account.Id);
            if (member == null)
            {
                response.StatusCode = HttpStatusCode.NotFound;
                response.StatusMessage = "Member not found.";
                return response;
            }

            if (!BCrypt.Net.BCrypt.Verify(passwordDTO.CurrentPassword, account.Password!))
            {
                response.StatusCode = HttpStatusCode.BadRequest;
                response.StatusMessage = "Change password is not successful";
                response.errors.Add("Current password is incorrect.");
                return response;
            }
            await BeginTransaction();
            account.Password = BCrypt.Net.BCrypt.HashPassword(passwordDTO.NewPassword);
            await _context.Accounts.UpdateAsync(account);
            await _context.CommitAsync();
            await CommitTransaction();

            response.StatusCode = HttpStatusCode.OK;
            response.StatusMessage = "Password changed successfully.";
        }
        catch (System.Exception ex)
        {
            await RollbackTransaction();
            response.StatusCode = HttpStatusCode.InternalServerError;
            response.StatusMessage = "An error occurred while changing the password.";
            response.errors.Add(ex.Message);
        }
        return response;
    }

    private async Task<string?> UploadImageAsync(IFormFile imageFile)
    {
        if (imageFile == null || imageFile.Length == 0)
        {
            return null; // Không thực hiện upload nếu file ảnh không hợp lệ
        }

        using var stream = imageFile.OpenReadStream();
        var uploadParams = new ImageUploadParams
        {
            File = new FileDescription(imageFile.FileName, stream),
            Folder = "profiles" // Thư mục lưu ảnh trên Cloudinary
        };

        var uploadResult = await _cloudinary.UploadAsync(uploadParams);

        if (uploadResult.StatusCode == System.Net.HttpStatusCode.OK)
        {
            return uploadResult.SecureUrl.ToString(); // Trả về URL ảnh
        }

        return null; // Trả về null nếu upload không thành công
    }

}
}

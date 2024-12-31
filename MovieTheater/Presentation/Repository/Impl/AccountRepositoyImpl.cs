using AutoMapper;
using BCrypt.Net;
using Microsoft.EntityFrameworkCore;
using System.Net;
using WebAPI.Entity;
using WebAPI.Services.DTO.Request;
using WebAPI.Services.DTO.Response;

namespace WebAPI.Repository.Impl
{
    public class AccountRepositoyImpl : GenericRepositoryImpl<ApplicationUser>, IAccountRepository
    {
        public AccountRepositoyImpl(MovieTheaterContext context) : base(context)
        {
        }
    }
}   

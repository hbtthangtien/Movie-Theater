
using CloudinaryDotNet;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json;
using CloudinaryDotNet;
using WebAPI.BackgroundServices;
using WebAPI.Entity;
using WebAPI.Helpers;
using WebAPI.Repository;
using WebAPI.Repository.Impl;
using WebAPI.Services;
using WebAPI.Services.Impl;
using WebAPI.Hubs;

namespace Presentation
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            })
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            });
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigin", builder =>
                {
                    builder.WithOrigins("http://localhost:3000") // Specify the exact origin
                            .AllowAnyMethod()
                            .AllowAnyHeader()
                            .AllowCredentials();

            });
            });
            builder.Services.AddSignalR();
            builder.Services.Configure<VnpayConfig>(builder.Configuration.GetSection("VnpayConfig"));
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
                   .AddEntityFrameworkStores<MovieTheaterContext>()
                   .AddDefaultTokenProviders();
            builder.Services.AddHttpClient();
            builder.Services.AddDbContext<MovieTheaterContext>(option =>
            option.UseSqlServer(builder.Configuration.GetConnectionString("Movie_Theater")));
            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            builder.Services.AddAuthentication
                (options =>
                    {
                    options.DefaultAuthenticateScheme =
                       options.DefaultChallengeScheme =
                       options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                    }
                )   
                .AddJwtBearer(option =>
                {
                    option.TokenValidationParameters = new TokenValidationParameters
                    {
                        // generate token
                        ValidateIssuer = false,
                        ValidAudience = "http://localhost:3000",
                        ValidateAudience = false,
                        // sign token
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey
                        (Encoding.UTF8.GetBytes(builder.Configuration["AppSettings:Token"]!)),
                        ClockSkew = TimeSpan.Zero
                    };
                    option.Events = new()
                    {
                        OnMessageReceived = context =>
                        {
                            context.Request.Cookies.TryGetValue(builder.Configuration["AppSettings:Token"]!, out var accessToken);
                            if (!string.IsNullOrEmpty(accessToken))
                                context.Token = accessToken;
                            return Task.CompletedTask;
                        }
                    };
                }).AddCookie("CookieAuth", options =>
                {
                    options.Cookie.HttpOnly = true;
                    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                    options.Cookie.SameSite = SameSiteMode.Strict;
                });
            builder.Services.AddSingleton(provider =>
            {
                var configuration = provider.GetRequiredService<IConfiguration>();
                var cloudinarySettings = configuration.GetSection("Cloudinary");
                return new Cloudinary(new Account(
                    cloudinarySettings["CloudName"],
                    cloudinarySettings["ApiKey"],
                    cloudinarySettings["ApiSecret"]
                ));
            });

            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(5);               
            });
            //builder.Services.AddAuthorization();
            // Add DI repository
                      
            builder.Services.AddScoped<IMovieRepository, MovieRepositoryImpl>();
            builder.Services.AddScoped<ICinemaRoomRepository, CinemaRoomRepositoryImpl>();
            builder.Services.AddScoped<IInvoiceRepository, InvoiceRepositoryImpl>();
            builder.Services.AddScoped<IMovieRepository, MovieRepositoryImpl>();
            builder.Services.AddScoped<IMovieScheduleRepository, MovieScheduleRepositoryImpl>();
            builder.Services.AddScoped<IPromotionRepository, PromotionRepositoryImpl>();
            builder.Services.AddScoped<IScheduleRepository, ScheduleRepositoryImpl>();
            builder.Services.AddScoped<IScheduleSeatRepository, ScheduleSeatRepositoryImpl>();
            builder.Services.AddScoped<ISeatRepository, SeatRepositoryImpl>();
            builder.Services.AddScoped<ITicketRepository, TicketRepositoryImpl>();
            builder.Services.AddScoped<ITypeRepository, TypeRepositoryImpl>();
            builder.Services.AddScoped<IMemberRepository, MemberRepositoryImpl>();
            builder.Services.AddScoped<IEmployeeRepository, EmployeeRepositoryImpl>();
            builder.Services.AddScoped<IPaymentRepository, PaymentRepositoryImpl>();
            
            // add unit of work
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            // add DI Service         
            builder.Services.AddScoped<IValidService, ValidServiceImpl>();
            builder.Services.AddScoped<IAccountService, AccountServiceImpl>();
            builder.Services.AddScoped<IMovieService, MovieServicelmpl>();
            builder.Services.AddScoped<IMovieScheduleService, MovieScheduleServiceImpl>();
            builder.Services.AddScoped<IScheduleService, ScheduleServiceImpl>();
            builder.Services.AddScoped<IAuthenticateService, AuthenticateServiceImpl>();
            builder.Services.AddScoped<IScheduleSeatService, ScheduleSeatServiceImpl>();
            builder.Services.AddScoped<IVnpayService, VnpayServiceImlp>();
            builder.Services.AddScoped<IPaymentService, PaymentServiceImpl>();
            builder.Services.AddScoped<IPromotionService, PromotionServiceImpl>();
            builder.Services.AddScoped<IInvoiceService, InvoiceServiceImpl>();
            builder.Services.AddScoped<ITicketService, TicketServiceImpl>();
            builder.Services.AddScoped<IQRCodeServicee, QRCodeServiceImpl>();
            builder.Services.AddScoped<IEmployeeService, EmployeeServiceImpl>();
            builder.Services.AddHostedService<SeatStatusBackgroundService>();
            builder.Services.AddScoped<IMovieTypeServices, MovieTypeImpl>();
            builder.Services.AddScoped<IMovieRomeServices, MovieRoomServiceImpl>();
            builder.Services.AddScoped<ISeatServices, SeatServicesImpl>();
            
            var app = builder.Build();
            app.UseCors("AllowSpecificOrigin");
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseSession();
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<SeatStatusHub>("/seathub");
            });
            app.Run();

        }
    }
}

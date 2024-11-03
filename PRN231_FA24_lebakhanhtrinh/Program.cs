using BusinessObject;
using Microsoft.EntityFrameworkCore;
using Repository.Implement;
using Repository.Interface;
using Service.Implement;
using Service.Interface;
using Service.Mapper;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Cấu hình JWT Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});

// Thêm dịch vụ cho ứng dụng
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Đăng ký các dịch vụ trong Dependency Injection
builder.Services.AddScoped<IPersonService, PersonService>();
builder.Services.AddScoped<IAuthService, AuthenService>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Đăng ký AutoMapper với assembly chứa các Profile
builder.Services.AddAutoMapper(typeof(PersonProfile).Assembly);
builder.Services.AddAutoMapper(typeof(UserProfile).Assembly);

// Cấu hình DbContext sử dụng SQL Server
builder.Services.AddDbContext<ViroCureFal2024dbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DBDefault")));

var app = builder.Build();

// Cấu hình pipeline xử lý HTTP requests
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication(); // Đặt trước UseAuthorization
app.UseAuthorization();

app.MapControllers();
app.Run();

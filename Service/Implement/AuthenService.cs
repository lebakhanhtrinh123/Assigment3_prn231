using AutoMapper;
using BusinessObject;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Repository.Implement;
using Repository.Infrastructure;
using Repository.Interface;
using Repository.Request;
using Repository.ViewModel;
using Repository.ViewModel.AuthVM;
using Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Data;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;


namespace Service.Implement
{
    public class AuthenService : IAuthService
    {
        private IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly IConfiguration configuration;

        public AuthenService(IUnitOfWork unitOfWork, IMapper mapper, IConfiguration configuration)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.configuration = configuration;
        }

        public async Task<GetLoginDTO> Login(LoginRequest loginRequest)
        {
            ViroCureUser? user = await unitOfWork.GetRepository<ViroCureUser>().Entities.FirstOrDefaultAsync(p => p.Email == loginRequest.Email);
            if (user == null)
            {
                throw new ErrorException(StatusCodes.Status401Unauthorized, ErrorCode.UnAuthorized, "Bạn chưa có tài khoản! Vui lòng đăng ký!");
            }
            if (user.Password != HashPasswordService.HashPasswordThrice(loginRequest.Password))
            {
                throw new ErrorException(StatusCodes.Status401Unauthorized, ErrorCode.UnAuthorized, "Tên đăng nhập hoặc mật khẩu không đúng!");
            }

            string token = GenerateToken(user);

            var userDto = mapper.Map<UserDTO>(user);
            userDto.Role = user.Role == 1 ? "admin" : "user";

            return new GetLoginDTO()
            {
                Message = "Login successful",
                Token = token,
                User = userDto
            };
        }

        public async Task<string> Register(RegisterRequest registerRequest)
        {
            ViroCureUser? users = await unitOfWork.GetRepository<ViroCureUser>().Entities.FirstOrDefaultAsync(p => p.Email == registerRequest.Email);
            if (users != null)
            {
                throw new ErrorException(StatusCodes.Status409Conflict, ErrorCode.Conflicted, "Tên đăng nhập này đã tồn tại!");
            }
            ViroCureUser user = new ViroCureUser
            {
                UserId = 6,
                Email = registerRequest.Email,
                Password = HashPasswordService.HashPasswordThrice(registerRequest.Password),
                Role = 1
            };
            Person person = new Person
            {
                PersonId = 5,
                UserId = user.UserId,
                Fullname = registerRequest.Fullname,
                BirthDay = DateOnly.FromDateTime(registerRequest.BirthDay), 
                Phone = registerRequest.Phone,
            };
            await unitOfWork.GetRepository<ViroCureUser>().AddAsync(user);
            await unitOfWork.GetRepository<Person>().AddAsync(person);
            await unitOfWork.SaveAsync();
            return "thành công";
        }
        public string GenerateToken(ViroCureUser user)
        {
            try
            {
                var jwtSettings = configuration.GetSection("Jwt");
                string? secretKey = jwtSettings["SecretKey"];
                string? issuer = jwtSettings["Issuer"];
                string? audience = jwtSettings["Audience"];
                if (!int.TryParse(jwtSettings["ExpirationInMinutes"], out int expirationInMinutes))
                {
                    throw new Exception("Invalid or missing ExpirationInMinutes configuration");
                }

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
                var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var claims = new[]
                {
            new Claim("UserId", user.UserId.ToString()),
            new Claim("Email", user.Email),
            new Claim("Role", user.Role.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

                var token = new JwtSecurityToken(
                    issuer: issuer,
                    audience: audience,
                    claims: claims,
                    expires: DateTime.UtcNow.AddMinutes(expirationInMinutes),
                    signingCredentials: credentials
                );

                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            catch (Exception ex)
            {
                throw new Exception("Error generating JWT token", ex);
            }
        }
    }
}

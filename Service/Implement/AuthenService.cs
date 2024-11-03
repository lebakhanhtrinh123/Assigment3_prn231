﻿using AutoMapper;
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
                throw new ErrorException(StatusCodes.Status401Unauthorized, ErrorCode.UnAuthorized, "Tên đăng nhâp hoặc hoặc mật khẩu không đúng!");
            }
            GetTokenDTO token = GenerateTokens(user);
            return new GetLoginDTO()
            {
                User = mapper.Map<UserDTO>(user),
                Token = token
            };
        }

        public async Task<string> Register(RegisterRequest registerRequest)
        {
            ViroCureUser? user = await unitOfWork.GetRepository<ViroCureUser>().Entities.FirstOrDefaultAsync(p => p.Email == registerRequest.Email);
            if (user == null)
            {
                throw new ErrorException(StatusCodes.Status409Conflict, ErrorCode.Conflicted, "Tên đăng nhập này đã tồn tại!");
            }

            throw new NotImplementedException();
        }
        public GetTokenDTO GenerateTokens(ViroCureUser user)
        {
            try
            {
                var jwtSettings = configuration.GetSection("Jwt");
                string secretKey = jwtSettings["SecretKey"];
                string issuer = jwtSettings["Issuer"];
                string audience = jwtSettings["Audience"];
                int expirationInMinutes = int.Parse(jwtSettings["ExpirationInMinutes"]);

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

                string tokenString = new JwtSecurityTokenHandler().WriteToken(token);

                return new GetTokenDTO
                {
                    AccessToken = tokenString,
                    ExpiresIn = expirationInMinutes * 60
                };
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi tạo JWT token", ex);
            }
        }
    }
}
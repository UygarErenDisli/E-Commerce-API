﻿using E_Commerce.Application.Abstractions.Token;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace E_Commerce.Infrastructure.Services.Token
{
	public class TokenHandler : ITokenHandler
	{
		private readonly IConfiguration _configuration;

		public TokenHandler(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		public Application.DTOs.Token CreateAccessToken(int minute)
		{
			Application.DTOs.Token token = new();

			SymmetricSecurityKey securityKey = new(Encoding.UTF8.GetBytes(_configuration["Token:SigninKey"]!));

			SigningCredentials signingCredentials = new(securityKey, SecurityAlgorithms.HmacSha256);

			token.Expiration = DateTime.UtcNow.AddMinutes(minute);

			JwtSecurityToken jwtSecurityToken = new(
				audience: _configuration["Token:Audience"],
				issuer: _configuration["Token:Issuer"],
				expires: token.Expiration,
				notBefore: DateTime.UtcNow,
				signingCredentials: signingCredentials
				);

			JwtSecurityTokenHandler tokenHandler = new();

			token.AccessToken = tokenHandler.WriteToken(jwtSecurityToken);

			return token;
		}
	}
}
using E_Commerce.Application.Abstractions.Token;
using E_Commerce.Domain.Entities.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
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

		public Application.DTOs.Token CreateAccessToken(int minute, AppUser user)
		{
			Application.DTOs.Token token = new();

			SymmetricSecurityKey securityKey = new(Encoding.UTF8.GetBytes(_configuration["Token:SigninKey"]!));

			SigningCredentials signingCredentials = new(securityKey, SecurityAlgorithms.HmacSha256);

			token.Expiration = DateTime.UtcNow.AddSeconds(minute);

			JwtSecurityToken jwtSecurityToken = new(
				audience: _configuration["Token:Audience"],
				issuer: _configuration["Token:Issuer"],
				expires: token.Expiration,
				notBefore: DateTime.UtcNow,
				signingCredentials: signingCredentials,
				claims: new List<Claim> { new(ClaimTypes.Name, user.UserName!), new(ClaimTypes.Email, user.Email!), }
				);

			JwtSecurityTokenHandler tokenHandler = new();

			token.AccessToken = tokenHandler.WriteToken(jwtSecurityToken);
			token.RefreshToken = CreateRefreshToken();
			return token;
		}

		public string CreateRefreshToken()
		{
			byte[] number = new byte[32];
			using var numberGenerator = RandomNumberGenerator.Create();
			numberGenerator.GetBytes(number);
			return Convert.ToBase64String(number);

		}
	}
}

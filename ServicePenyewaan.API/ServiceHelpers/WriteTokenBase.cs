using Microsoft.IdentityModel.Tokens;
using ServicePenyewaan.API.Models;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ServicePenyewaan.API.ServiceHelpers
{
    public class WriteTokenBase
    {
		public static string GenerateToken(Users userObj)
		{
			SymmetricSecurityKey mySecurityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(ConstantsClass.JWT_KEY));

			JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
			SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(new Claim[]
				{
					new Claim(ClaimTypes.NameIdentifier, userObj.UserId.ToString()),
					new Claim("Username", userObj.UserName),
					new Claim("UserEmail", userObj.UserEmail),
					new Claim("Role", userObj.Role),
				}),
				Expires = DateTime.UtcNow.AddHours(1),
				Issuer = ConstantsClass.JWT_ISSUER,
				Audience = ConstantsClass.JWT_ISSUER,
				SigningCredentials = new SigningCredentials(mySecurityKey, SecurityAlgorithms.HmacSha256Signature)
			};

			SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);

			return tokenHandler.WriteToken(token);
		}
	}
}

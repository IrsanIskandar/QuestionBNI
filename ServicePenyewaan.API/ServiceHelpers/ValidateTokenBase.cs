using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;

namespace ServicePenyewaan.API.ServiceHelpers
{
    public class ValidateTokenBase
    {
		public static bool ValidateCurrentToken(string token)
		{
			SymmetricSecurityKey mySecurityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(ConstantsClass.JWT_KEY));

			JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
			try
			{
				tokenHandler.ValidateToken(token, new TokenValidationParameters
				{
					ValidateIssuerSigningKey = true,
					ValidateIssuer = true,
					ValidateAudience = true,
					ValidIssuer = ConstantsClass.JWT_ISSUER,
					ValidAudience = ConstantsClass.JWT_ISSUER,
					IssuerSigningKey = mySecurityKey
				}, out SecurityToken validatedToken);
			}
			catch
			{
				return false;
			}
			return true;
		}

		public static string GetClaim(string token, string claimType)
		{
			JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
			var securityToken = tokenHandler.ReadToken(token) as JwtSecurityToken;

			string stringClaimValue = securityToken.Claims.First(claim => claim.Type == claimType).Value;
			return stringClaimValue;
		}
	}
}

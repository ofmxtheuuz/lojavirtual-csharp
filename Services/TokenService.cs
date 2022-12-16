using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace LojaVirtual.Services;

using System.IdentityModel.Tokens.Jwt;

public class TokenService
{
    public string Tokenize(string email)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.UniqueName, email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("MXTHEUZBEBE"));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        // tempo de expiração do token: 1 hora
        var expiration = DateTime.UtcNow.AddHours(1);
        JwtSecurityToken token = new JwtSecurityToken(
            issuer: null,
            audience: null,
            claims: claims,
            expires: expiration,
            signingCredentials: creds);
        return token.EncodedPayload;
    }
}
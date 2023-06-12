using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System;
using Apartment.DataAccess;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using MarkoDasic.Core.TokenStorage;
using Microsoft.Identity.Client.Extensions.Msal;

namespace MarkoDasic.Core
{
    public class JwtManager
    {
        private readonly ApartmentContext _context;
        private readonly JwtSettings _settings;
        private readonly ITokenStorage _storage;

        public JwtManager(ApartmentContext context, JwtSettings settings, ITokenStorage storage)
        {
            _settings = settings;
            _context = context;
            this._storage = storage;
        }

        public string MakeToken(string email, string password)
        {
            var user = _context.Users.Include(x => x.UseCases).FirstOrDefault(x => x.Email == email);

            if (user == null)
            {
                throw new UnauthorizedAccessException();
            }

            var valid = BCrypt.Net.BCrypt.Verify(password, user.Password);

            if (!valid)
            {
                throw new UnauthorizedAccessException();
            }

            //var useCases = _context.UserUseCase.Where(x => x.UserId == user.Id).Select(x => x.UseCaseId);

            var actor = new JwtUser
            {
                Id = user.Id,
                UseCasesIds = user.UseCases.Select(x => x.UseCaseId).ToList(),
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email
            };

            var tokenId = Guid.NewGuid().ToString();
            _storage.AddToken(tokenId);


            var claims = new List<Claim> // Jti : "",
            {
                new Claim(JwtRegisteredClaimNames.Jti, tokenId, ClaimValueTypes.String, _settings.Issuer),
                new Claim(JwtRegisteredClaimNames.Iss, _settings.Issuer),
                new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64, _settings.Issuer),
                new Claim("UserId", actor.Id.ToString(), ClaimValueTypes.String, _settings.Issuer),
                new Claim("UseCases", JsonConvert.SerializeObject(actor.UseCasesIds)),
                new Claim("Email", user.Email),
                new Claim("FirstName", user.FirstName),
                new Claim("LastName", user.LastName),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.SecretKey));

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var now = DateTime.UtcNow;
            var token = new JwtSecurityToken(
                issuer: _settings.Issuer,
                audience: "Any",
                claims: claims,
                notBefore: now,
                expires: now.AddMinutes(_settings.Minutes),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}

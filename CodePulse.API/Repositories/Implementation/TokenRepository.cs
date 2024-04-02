﻿using CodePulse.API.Repositories.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CodePulse.API.Repositories.Implementation
{
    public class TokenRepository : ITokenRepository
    {
        private readonly IConfiguration configuration; //to get the key value form app settings
        public TokenRepository(IConfiguration configuration) 
        {
            this.configuration = configuration;
        }
        public string CreateJwtToken(IdentityUser user, List<string> roles)
        {
            // Create claims from the roles
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.Email)
            };
            //iterating through all the roles in roles list and converting them to the claim
            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            // Define Jwt token security parameters
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            //now creating the token
            var token = new JwtSecurityToken(
                issuer: configuration["Jwt:Issuer"],
                audience: configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: credentials);

            //Return token
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}

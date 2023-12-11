using Cars.Api.Controllers.Identity.Models;
using LearnProject.BLL.Contracts;
using LearnProject.Shared.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Cars.Api.Controllers.Identity
{
    [ApiController]
    [Route("api/[controller]")]
    public class IdentityController : ControllerBase
    {

        readonly IUserService service;

        public IdentityController(IUserService service)
        {
            this.service = service;
        }


        const string TokenSecret = "qwertyqwertyqwertyqwertyqwertyqwerty"; //алгоритм просит побольше
        static readonly TimeSpan TokenLifetime = TimeSpan.FromSeconds(100);

        [HttpPost("token")]
        public async Task<IActionResult> GenerateToken(TokenGenerationRequest request)
        {
            //залезть в базу(по email, новый метод сервиса), достать роль (через сервис) в клейм

            var response = await service.GetUserByNameAsync(request.Login);
            if (!response.IsSuccessful || response.Value == null)
            {
                return NotFound(response.Error);
            }

            var model = response.Value;

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(TokenSecret);

            IDictionary<string,object> claims = new Dictionary<string, object>
            {
                { "role", model.Role }
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Expires = DateTime.UtcNow.Add(TokenLifetime),
                Issuer = "carsApiIdentity",
                Claims =  claims,
                Audience = "carsApi",
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            var jwt = tokenHandler.WriteToken(token);
            return Ok(jwt);
        }
    }
}

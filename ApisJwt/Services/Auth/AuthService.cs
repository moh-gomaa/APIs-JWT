using ApisJwt.Helpers;
using ApisJwt.Models;
using ApisJwt.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ApisJwt.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly JwtInfo jwt;

        public AuthService(UserManager<ApplicationUser> userManager, IOptions<JwtInfo> jwt)
        {
            this.userManager = userManager;
            this.jwt = jwt.Value;
        }
        public async Task<ApiResponse<AuthViewModel>> Register(RegisterViewModel model)
        {
            if(await userManager.FindByEmailAsync(model.Email) is not null)
                return new ApiResponse<AuthViewModel>(false, "Email is already registered", null);

            if (await userManager.FindByNameAsync(model.UserName) is not null)
                return new ApiResponse<AuthViewModel>(false, "Username is alrady registered", null);

            ApplicationUser user = new ApplicationUser
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                UserName = model.UserName,
                Email = model.Email
            };

            var result = await userManager.CreateAsync(user, model.password);

            if (!result.Succeeded)
            {
                string errors = "";

                foreach (var error in result.Errors)
                    errors += $"{error.Description},";

                errors = errors.Trim(',');
                return new ApiResponse<AuthViewModel>(false, errors, null);
            }

            await userManager.AddToRoleAsync(user, "User");

            var JwtSecurityToken = await CreateJwtToken(user);

            AuthViewModel output = new AuthViewModel
            {
                IsAuthenticated = true,
                Username = user.UserName,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = model.LastName,
                Roles = new List<string> { "User" },
                ExpireOn = JwtSecurityToken.ValidTo,
                Token = new JwtSecurityTokenHandler().WriteToken(JwtSecurityToken)
            };

            return new ApiResponse<AuthViewModel>(true, "User created successfully", output);
        }

        private async Task<JwtSecurityToken> CreateJwtToken(ApplicationUser user)
        {
            var userClaims = await userManager.GetClaimsAsync(user);
            var roles = await userManager.GetRolesAsync(user);
            var rolesClaims = new List<Claim>();

            foreach (var role in roles)
                rolesClaims.Add(new Claim("roles", role));

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("uid", user.Id)
            }
            .Union(userClaims)
            .Union(rolesClaims);

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.SecretKey));
            var signignCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: jwt.Issuer,
                audience: jwt.Audience,
                claims: claims,
                expires: DateTime.Now.AddDays(jwt.DurationInDays),
                signingCredentials: signignCredentials);

            return jwtSecurityToken;
        }
    }
}

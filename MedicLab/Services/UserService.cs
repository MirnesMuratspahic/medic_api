using Azure.Core;
using MedicLab.Context;
using MedicLab.Models;
using MedicLab.Models.DTO;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Concurrent;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MedicLab.Services
{
    public class UserService
    {
        public ApplicationDbContext DbContext { get; set; }
        public IConfiguration configuration { get; set; }
        public ErrorProvider error = new ErrorProvider() { Status = false };
        public ErrorProvider defaultError = new ErrorProvider() { Status = true, Name = "Property must not be null" };
        public string EmailClaim = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress";

        public UserService() { }
        public UserService(ApplicationDbContext context, IConfiguration _configuration)
        {
            DbContext = context;
            configuration = _configuration;
        }

        public async Task<(ErrorProvider, string)> Login(dtoUserLogin userDto)
        {
            if (userDto == null)
                return (defaultError,null);

            var userFromDatabase = await DbContext.Users.FirstOrDefaultAsync(x => x.Username == userDto.Username);

            if (userFromDatabase == null)
            {
                error = new ErrorProvider()
                {
                    Status = true,
                    Name = "You have not entered correct information!"
                };
                return (error,null);
            }

            if (!BCrypt.Net.BCrypt.Verify(userDto.Password, userFromDatabase.PasswordHash))
            {
                error = new ErrorProvider()
                {
                    Status = true,
                    Name = "You have not entered correct information!"
                };
                return (error,null);
            }
            var token = CreateToken(userFromDatabase);

            userFromDatabase.LastLoginDate = DateOnly.FromDateTime(DateTime.Today);
            await DbContext.SaveChangesAsync();

            return (error,token);

        }

        public async Task<(ErrorProvider, User)> GetUserById(int id)
        {
            if (id == null || id == 0)
            {
                return (defaultError, null);
            }

            var userFromDatabase = await DbContext.Users.FirstOrDefaultAsync(x => x.Id == id);

            if (userFromDatabase == null)
            {
                error = new ErrorProvider()
                {
                    Status = true,
                    Name = "There is no user with that ID!"

                };
                return (error, null);
            }

            return (error, userFromDatabase);
        }

        public async Task<ErrorProvider> BlockUserById(int id)
        {
            if (id == null || id == 0)
            {
                return defaultError;
            }

            var userFromDatabase = await DbContext.Users.FirstOrDefaultAsync(x => x.Id == id);

            if (userFromDatabase == null)
            {
                error = new ErrorProvider()
                {
                    Status = true,
                    Name = "There is no user with that ID!"

                };
                return error;
            }

            userFromDatabase.Status = "Blocked";
            await DbContext.SaveChangesAsync();

            error = new ErrorProvider()
            {
                Status = true,
                Name = "User blocked"
            };

            return error;
        }

        public async Task<ErrorProvider> LogOut(HttpRequest request)
        {
            var token = request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            
            await Task.CompletedTask;
            return defaultError;
        }

        public async Task<ErrorProvider> Register(dtoUserRegistration userDto)
        {
            if (userDto == null)
                return defaultError;

            var user = await DbContext.Users.FirstOrDefaultAsync(x => x.Username == userDto.Username);

            if (user != null)
            {
                error = new ErrorProvider()
                {
                    Status = true,
                    Name = "This username is already taken!"
                };
                return error;
            }

            string passwordHash = BCrypt.Net.BCrypt.HashString(userDto.Password);

            var newUser = new User()
            {
                Username = userDto.Username,
                PasswordHash = passwordHash,
                Name = userDto.Name,
                Orders = userDto.Orders,
                ImageUrl = userDto.ImageUrl,
                DateOfBirth = userDto.DateOfBirth,
                Status = "Active",
                Role = userDto.Role,
                LastLoginDate = DateOnly.FromDateTime(DateTime.Today)
            };

            await DbContext.Users.AddAsync(newUser);
            await DbContext.SaveChangesAsync();
            var token = CreateToken(newUser);

            error = new ErrorProvider()
            {
                Status = true,
                Name = "User registered!"
            };

            return error;

        }

        public async Task<(ErrorProvider, List<dtoUserInformation>)> GetUsers()
        {
            var users = await DbContext.Users.ToListAsync();
            if (users.Count == 0)
            {
                error = new ErrorProvider()
                {
                    Status = true,
                    Name = "None users in database!"
                };
                return (error, null);
            }

            List<dtoUserInformation> dtoUsers = new List<dtoUserInformation>();

            foreach(var user in users)
            {
                var dtoUser = new dtoUserInformation()
                {
                    UserId = user.Id,
                    Name = user.Name,
                    Username = user.Username,
                    LastLoginDate = user.LastLoginDate,
                };
                dtoUsers.Add(dtoUser);
            }

            return (error, dtoUsers);
        }

        private string CreateToken(User user)
        {
            List<Claim> _claims = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Username),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetSection("AppSettings:Token").Value!));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken
                (
                    claims: _claims,
                    expires: DateTime.Now.AddDays(1),
                    signingCredentials: cred
                );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }
    }
}

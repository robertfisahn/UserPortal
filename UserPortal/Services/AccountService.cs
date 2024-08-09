using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UserPortal.Entities;
using UserPortal.Exceptions;
using UserPortal.Interfaces;
using UserPortal.Models.Dtos;

namespace UserPortal.Services
{
    public class AccountService : IAccountService
    {
        private readonly IMongoCollection<User> _userCollection;
        private readonly IMongoCollection<Role> _roleCollection;
        private readonly IMapper _mapper;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly AuthenticationSettings _authenticationSettings;

        public AccountService(IMongoDatabase database, IMapper mapper, IPasswordHasher<User> passwordHasher,
            AuthenticationSettings authenticationSettings)
        {
            _userCollection = database.GetCollection<User>("Users");
            _roleCollection = database.GetCollection<Role>("Roles");
            _mapper = mapper;
            _passwordHasher = passwordHasher;
            _authenticationSettings = authenticationSettings;
        }

        public void RegisterUser(RegisterUserDto dto)
        {
            var user = _mapper.Map<User>(dto);
            user.PasswordHash = _passwordHasher.HashPassword(user, dto.Password);
            var role = _roleCollection.Find(r => r.Name == "User").FirstOrDefault() ?? throw new NotFoundException("Default role 'User' not found");
            user.RoleId = role.Id;

            _userCollection.InsertOne(user);
        }

        public string GenerateJwt(LoginDto dto)
        {
            var user = _userCollection.Find(u => u.Email == dto.Email).FirstOrDefault() ?? throw new NotFoundException("Invalid email");

            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, dto.Password);
            if (result == PasswordVerificationResult.Failed)
            {
                throw new BadRequestException("Invalid username or password");
            }

            user.Role = _roleCollection.Find(r => r.Id == user.RoleId).FirstOrDefault() ?? throw new NotFoundException("Role not found");

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
                new Claim(ClaimTypes.Role, $"{user.Role.Name}"),
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authenticationSettings.JwtKey));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(_authenticationSettings.JwtExpireDays);

            var token = new JwtSecurityToken(_authenticationSettings.JwtIssuer,
                _authenticationSettings.JwtIssuer,
                claims,
                expires: expires,
                signingCredentials: cred);

            var tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.WriteToken(token);
        }

        public void UpdateUser(UpdateUserDto dto)
        {
            var user = _userCollection.Find(u => u.Email == dto.Email).FirstOrDefault() ?? throw new NotFoundException("Invalid email");
            var role = _roleCollection.Find(r => r.Name == dto.RoleName).FirstOrDefault() ?? throw new NotFoundException("Role not found");
            user.RoleId = role.Id;
            _userCollection.ReplaceOne(u => u.Id == user.Id, user);
        }
    }
}

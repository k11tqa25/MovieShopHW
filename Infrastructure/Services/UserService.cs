using ApplicationCore.Exceptions;
using ApplicationCore.Models;
using ApplicationCore.RepositoryInterfaces;
using ApplicationCore.ServiceInterfaces;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;
using System;
using ApplicationCore.Entities;

namespace Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<UserLoginResponseModel> LoginAsync(string email, string password)
        {
            var dbUser = await _userRepository.GetUserByEmail(email);
            if(dbUser == null)
            {
                throw new NotFoundException("Email does not exist. Please register first.");
            }

            var hashedPassword = HashPassword(password, dbUser.Salt);
            if(hashedPassword == dbUser.HashedPassword)
            {
                // Correct password
                var userLoginResponse = new UserLoginResponseModel
                {
                    Id = dbUser.Id,
                    Email = dbUser.Email,
                    FirstName = dbUser.FirstName,
                    LastName = dbUser.LastName,
                    DateOfBirth = dbUser.DateOfBirth,
                    Password = dbUser.HashedPassword
                };

                return userLoginResponse;
            }

            return null;
        }

        public async Task<UserRegisterResponseModel> RegisterUserAsync(UserRegisterRequestModel requestModel)
        {
            // 1. Make sure the email does not exists in the database User table
            var dbUser = await _userRepository.GetUserByEmail(requestModel.Email);
            if(dbUser != null)
            {
                // Already have user with the same email
                throw new ConflictException("Email already exists.");
            }

            // 2. Create a unique salt for the user password
            var salt = CreateSalt();
            var hashedPassword = HashPassword(requestModel.Password, salt);

            // 3. Save to database
            var user = new User
            {
                Email = requestModel.Email,
                FirstName = requestModel.FirstName,
                LastName = requestModel.LastName,
                DateOfBirth = requestModel.DateOfBirth,
                Salt = salt,
                HashedPassword = hashedPassword
            };
            var createdUser  =  await _userRepository.AddAsync(user);

            // 4. Map the createdUser to the UserRegisterResponseModel
            var userResponse = new UserRegisterResponseModel
            {
                Id = createdUser.Id,
                DateOfBirth = createdUser.DateOfBirth,
                Password = createdUser.HashedPassword,
                FirstName = createdUser.FirstName,
                LastName = createdUser.LastName,
                Email = createdUser.Email
            };

            return userResponse;
        }

        private string CreateSalt()
        {
            // Never write our own code. Use the standard. 
            // https://docs.microsoft.com/en-us/aspnet/core/security/data-protection/consumer-apis/password-hashing?view=aspnetcore-5.0

            // generate a 128-bit salt using a secure PRNG
            byte[] salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
            return Convert.ToBase64String(salt);
        }

        private string HashPassword(string password, string salt)
        {
            // derive a 256-bit subkey (use HMACSHA1 with 10,000 iterations)
            // This Pbkdf2 is US standard 
            // Other protocols are: Aarogon, BCrypt
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: Convert.FromBase64String(salt),
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));
            return hashed;
        }

    }
}

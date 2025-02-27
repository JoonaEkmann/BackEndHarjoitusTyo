﻿using BackEndHarjoitusTyo.Models;
using BackEndHarjoitusTyo.Repositories;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;

namespace BackEndHarjoitusTyo.Middleware
{
    public interface iUserAuthenticationService
    {
        Task<User?> Authenticate(string username, string password);
        public User CreateUserCredentials(User user);
        Task<bool> isMyMessage(string username, long messageId);
    }

    public class UserAuthenticationService : iUserAuthenticationService
    {
        private readonly iUserRepository _userRepository;
        private readonly iMessageRepository _messageRepository;

        public UserAuthenticationService(iUserRepository userRepository, iMessageRepository MessageRepository) 
        {
            _userRepository = userRepository;
            _messageRepository = MessageRepository;
        }

        public async Task<User?> Authenticate(string username, string password)
        {
            User? user;

            user = await _userRepository.GetUserAsync(username);
            if (user == null) 
            {
                return null;
            }
            string hashedPassword = Convert.ToBase64String(KeyDerivation.Pbkdf2(
             password: password,
             salt: user.Salt,
             prf: KeyDerivationPrf.HMACSHA256,
              iterationCount: 10000,
              numBytesRequested: 258 / 8));
            
            if (hashedPassword != user.Password)
            {
                return null;
            }
            return user;
        }

        public User CreateUserCredentials(User user)
        {
            byte[] salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
            string hashedPassword = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: user.Password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 258 / 8));

            user.Password = hashedPassword;
            user.Salt = salt;
            user.JoinDate = user.JoinDate != null ? user.JoinDate : DateTime.Now;
            user.LastLogin = DateTime.Now;

            return user;
        }

        public async Task<bool> isMyMessage(string username, long messageId)
        {
            User? user = await _userRepository.GetUserAsync(username);
            if (user == null)
            {
                return false;
            }
            Message? message = await _messageRepository.GetMessageAsync(messageId);
            if (message == null)
            {
                return false;
            }
            if (message.Sender == user)
            {
               return true;
            }
            return false;
        }
    }
}

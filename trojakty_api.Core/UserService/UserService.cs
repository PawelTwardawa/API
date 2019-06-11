using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using trojakty_api.Core.Exceptions;
using trojakty_api.Core.UserService.DTOs;
using trojkaty_api.DataAccess.Models;
using trojkaty_api.DataAccess.Repositories;

namespace trojakty_api.Core.UserService
{
    public class UserService : IUserService
    {
        private IGenericRepository<User> _userRepository;
        private IMapper _mapper;

        public UserService(IGenericRepository<User> userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public IEnumerable<User> GetAll()
        {
            return _userRepository.GetAll();
        }

        public User GetByEmail(string email)
        {
            return _userRepository.FindBy(u => u.Email == email).SingleOrDefault();
        }

        public User Authenticate(UserDTO userDTO)
        {
            if (string.IsNullOrEmpty(userDTO.Email) || string.IsNullOrEmpty(userDTO.Password))
                return null;

            var user = _userRepository.FindBy(u => u.Email == userDTO.Email).SingleOrDefault();

            if (user == null)
                return null;

            if (!VerifyPasswordHash(userDTO.Password, user.PasswordHash, user.PasswordSalt))
                return null;

            return user;
        }

        //public async Task<User> Update(UserDTO userDTO)
        //{

        //}

        public async Task<User> Create(UserDTO userDTO)
        {
            if (string.IsNullOrWhiteSpace(userDTO.Password)) throw new TrojkatyCoreException("Password is required");
            if( !userDTO.Email.Contains("@") || !userDTO.Email.Contains(".")) throw new TrojkatyCoreException("Incorrect email format");
            var v = _userRepository.FindBy(u => u.Email == userDTO.Email);
            var vv = v.AsQueryable();
            //if (_userRepository.FindBy(u => u.Email == userDTO.Email).AsQueryable().Count() != 0) 
            if (vv.Count() != 0)
                //return null;
                throw new TrojkatyCoreException($"Username '{userDTO.Email}' already exist ");

            byte[] passwordHash, passwordSalt;

            CreatePasswordHash(userDTO.Password, out passwordHash, out passwordSalt);

            var user = _mapper.Map<User>(userDTO);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            await _userRepository.Add(user);
            await _userRepository.SaveAsync();

            return user;
        }

        public async void Delete(UserDTO userDTO)
        {
            var user = _userRepository.FindBy(u => u.Email == userDTO.Email).SingleOrDefault();

            if (user == null)
                return;

            _userRepository.Delete(user);
            await _userRepository.SaveAsync();
        }

        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string", "password");

            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private static bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string", "password");
            if (storedHash.Length != 64) throw new ArgumentException("Invalid length of password hash (64 bytes expected).", "passwordHash");
            if (storedSalt.Length != 128) throw new ArgumentException("Invalid length of password salt (128 bytes expected).", "passwordSalt");

            using (var hmac = new System.Security.Cryptography.HMACSHA512(storedSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for(int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != storedHash[i])
                        return false;
                }
            }
            return true;
        }
    }
}

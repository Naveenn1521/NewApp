using System;
using System.Threading.Tasks;
using DatingApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Data
{
    public class AuthRepositary : IAuthRepositary
    {
        private readonly DataContext _context;
        public AuthRepositary(DataContext context)
        {
            _context = context;

        }
        public async Task<User> Login(string UserName, string Password)
        {

            var userdetails = await _context.Users.FirstOrDefaultAsync(x => x.Username == UserName);

            if(userdetails == null)
                return null;    

                // if(!IsPasswordsMatch(Password,userdetails.passwordhash,userdetails.Passwordsalt))
                //     return null;

            return userdetails;
        }

        private bool IsPasswordsMatch(string password, byte[] passwordhash, byte[] passwordsalt)
        {
             using(var hmac = new System.Security.Cryptography.HMACSHA512(passwordsalt))
             {
                     var IsPasswordSame = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                     for(int i=0; i < IsPasswordSame.Length; i++)
                     {
                         if(IsPasswordSame[i] != passwordhash[i]) return false;
                     }
            }
            return true;
        }

        public async Task<User> Register(User user, string Password)
        {
            byte[] Passwordhash,PasswordSalt;
            createPasswordHash(Password, out Passwordhash,out PasswordSalt);
            user.passwordhash = Passwordhash;
            user.Passwordsalt = PasswordSalt;

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            return user;
        }

        private void createPasswordHash(string password, out byte[] passwordhash, out byte[] passwordSalt)
        {
            using(var hmac = new System.Security.Cryptography.HMACSHA512()){

                     passwordSalt = hmac.Key;
                     passwordhash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        public async Task<bool> UserExist(string userName)
        {
            if(await _context.Users.AnyAsync(x => x.Username == userName))
            return true;
            
            return false;
        }
    }
}
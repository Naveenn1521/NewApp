using System.Threading.Tasks;
using DatingApp.API.Models;

namespace DatingApp.API.Data
{
    public interface IAuthRepositary
    {
         Task<User> Register(User user, string Password);
         Task<User> Login(string UserName,string Password);
         Task<bool> UserExist(string userName);
    }
}
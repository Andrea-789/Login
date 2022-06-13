using System.Collections.Generic;
using System.Threading.Tasks;
using GestUser.Models;

namespace GestUser.Services
{
  public interface IUserService
  {
    Task<Users> GetUserById(string userId);
    Task<Users> GetUserByUsername(string username);
    Task<Users> GetUserToDelete(string userId);
    Task<ICollection<Users>> GetAll();
    Task<bool> InsUser(Users user);
    Task<bool> UpdUser(Users user);
    Task<bool> DelUser(Users user);
    Task<Users> Authenticate(string username, string password);
    Task<string> GetToken(string username);
    Task<bool> UserExists(string userId);
    Task<bool> FirstUser();
    Task<bool> Activation(string id);
    Task<bool> UpdatePwd(ChangePwd changePwd);
    bool SendEmail(string email, string id, string type);

  }
}

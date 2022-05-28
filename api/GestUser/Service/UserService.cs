
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using GestUser.Models;
using GestUser.Security;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System;
using GestUser.Helpers;
using System.Net.Mail;
using Microsoft.Data.SqlClient;
using GestUser.Services;

namespace Services
{
  public class UserService : IUserService
  {
    private readonly LoginDbContext loginDbContent;
    private readonly AppSettings appSettings;
    private readonly EmailSettings emailSettings;
    private readonly InterfaceSettings interfaceSettings;

    public UserService(LoginDbContext loginDbContent, IOptions<AppSettings> appSettings,
        IOptions<EmailSettings> emailSettings, IOptions<InterfaceSettings> intefaceSettings)
    {
      this.loginDbContent = loginDbContent;
      this.appSettings = appSettings.Value;
      this.emailSettings = emailSettings.Value;
      this.interfaceSettings = intefaceSettings.Value;
    }

    public async Task<Users> GetUserById(string userId)
    {
      return await this.loginDbContent.Users
          .Where(c => c.Id == userId)
          //.Include(r => r.Profili)
          .FirstOrDefaultAsync();
    }

    public async Task<Users> GetUserByUsername(string username)
    {
      return await this.loginDbContent.Users
        .Where(c => c.Email == username)
        .FirstOrDefaultAsync();
    }

    public async Task<Users> GetUserToDelete(string username)
    {
      return await this.loginDbContent.Users
          //.AsNoTracking()
          .Where(c => c.Email == username)
          .FirstOrDefaultAsync();
    }

    public async Task<ICollection<Users>> GetAll()
    {
      return await this.loginDbContent.Users
          //.Include(r => r.Profili)
          .OrderBy(c => c.Id)
          .ToListAsync();
    }

    public async Task<Users> Authenticate(string username, string password)
    {
      bool retVal = false;

      PasswordHasher Hasher = new PasswordHasher();

      Users user = await this.GetUserByUsername(username);

      if (user != null)
      {
        string EncryptPwd = user.Password;
        retVal = Hasher.Check(EncryptPwd, password).Verified;

        if (retVal)
          return user;
      }

      return null;
    }

    public async Task<bool> InsUser(Users user)
    {
      this.loginDbContent.Add(user);
      return await Save();
    }

    public async Task<bool> UpdUser(Users user)
    {
      this.loginDbContent.Update(user);
      return await Save();
    }

    public async Task<bool> DelUser(Users user)
    {
      this.loginDbContent.Remove(user);
      return await Save();
    }

    private async Task<bool> Save()
    {
      var saved = await this.loginDbContent.SaveChangesAsync();
      return saved >= 0 ? true : false;
    }

    public async Task<string> GetToken(string username)
    {
      Users user = await this.GetUserByUsername(username);

      //creazione token Jwt
      var tokenHandler = new JwtSecurityTokenHandler();
      var key = Encoding.ASCII.GetBytes(this.appSettings.Secret);

      string userRole = user.Role;
      List<Claim> claims = new List<Claim>
      {
        new Claim(ClaimTypes.Name, user.Email),
        new Claim(ClaimTypes.Role, userRole)
      };
      
      var tokenDescriptor = new SecurityTokenDescriptor
      {
        Subject = new ClaimsIdentity(claims),
        //validita' del token
        Expires = DateTime.UtcNow.AddSeconds(this.appSettings.Expiration),
        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
          SecurityAlgorithms.HmacSha256Signature)
      };

      var token = tokenHandler.CreateToken(tokenDescriptor);

      return tokenHandler.WriteToken(token);
    }

    public async Task<bool> UserExists(string userId)
    {
      return await this.loginDbContent.Users
          .AnyAsync(c => c.Id == userId);
    }

    public async Task<bool> FirstUser()
    {
      int i = await this.loginDbContent.Users.CountAsync();

      return i == 0;
    }

    public async Task<bool> Activation(string id)
    {
      string sql = "UPDATE Users SET Enabled = 1 WHERE Id = @id";

      var param = new SqlParameter("@id", id);

      int i = await this.loginDbContent.Database.ExecuteSqlRawAsync(sql, param);

      return i > 0;
    }

    public async Task<bool> UpdatePwd(ChangePwd changePwd)
    {
      string sql = "UPDATE Users SET Password = @NewPwd, date = @Date " +
                   "WHERE Id = @Id";

      var paramNewPwd = new SqlParameter("@NewPwd", changePwd.NewPwd);
      var paramDate = new SqlParameter("@Date", DateTime.Today);
      var paramId = new SqlParameter("@Id", changePwd.Id);

      int i = await this.loginDbContent.Database.ExecuteSqlRawAsync(sql, paramNewPwd, paramDate, paramId);

      return i > 0;
    }

    public bool SendEmail(string email, string id = "", string type = "")
    {
      try
      {
        MailMessage mail = new MailMessage();
        mail.From = new MailAddress(emailSettings.user);
        mail.To.Add(new MailAddress(email));
        mail.IsBodyHtml = true;        

        if (type == "activation")
        { 
          mail.Subject = "user activation";
          mail.Body = "Click on the link to <br/>" +
                      "<a href=\"" + interfaceSettings.InterfaceUri + "activation/" + id + "\">ACTIVATE</a>";
        }
        else if (type == "send")
        {
          mail.Subject = "Change Password";
          mail.Body = "Click to <br/> " +
                      "<a href=\"" + interfaceSettings.InterfaceUri + "changepwd/\">Change PASSWORD</a>";
        }
        else if (type == "forgotpwd")
        {
          mail.Subject = "forgotten password";
          mail.Body = "Your new password is <b>newpwd</b> <br/>" +
                      "Click to " +
                      "<a href=\"" + interfaceSettings.InterfaceUri + "changepwd?id=" + id + "\">Change PASSWORD</a>";
        }

        SmtpClient client = new SmtpClient(emailSettings.smtp, Convert.ToInt32(emailSettings.port));
        // Credentials are necessary if the server requires the client
        // to authenticate before it will send email on the client's behalf.
        client.UseDefaultCredentials = false;
        client.Credentials = new System.Net.NetworkCredential(emailSettings.user, emailSettings.pwd);
        client.EnableSsl = true;

        client.Send(mail);

        return true;
      }
      catch
      {
        return false;
      }
    }

  }
}

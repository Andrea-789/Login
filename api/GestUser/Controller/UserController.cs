using AutoMapper;
using GestUser.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using GestUser.Models;
using GestUser.Security;
using GestUser.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GestUser.Helpers;
using Microsoft.Extensions.Options;

namespace GestUser.Controllers
{
  [ApiController]
  [Produces("application/json")]
  [Route("api/user")]
  public class UserController : Controller
  {
    private readonly IUserService userRepository;
    private readonly IMapper mapper;
    private readonly AppSettings appSettings;

    public UserController(IUserService userRepository, IMapper mapper, IOptions<AppSettings> appSettings)
    {
      this.userRepository = userRepository;
      this.mapper = mapper;
      this.appSettings = appSettings.Value;

    }

    [HttpPost("auth")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ActionResult<JwtTokenDto>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrMsg))]
    public async Task<ActionResult<JwtTokenDto>> Authenticate([FromBody] AuthDto userParam)
    {
      string tokenJWT = string.Empty;

      //bool IsOk = await userRepository.Authenticate(userParam.User, userParam.Password);
      Users user = await userRepository.Authenticate(userParam.User, userParam.Password);

      if (user == null)
      {
        return BadRequest(new ErrMsg(string.Format($"Wrong User or Password"),
            this.HttpContext.Response.StatusCode));
      }
      else
      {
        tokenJWT = await userRepository.GetToken(userParam.User);

        bool pwdExpired = false;
        DateTime today = DateTime.Now;
        if ((today - Convert.ToDateTime(user.Date)).Days > appSettings.ExpirationPwd)
          pwdExpired = true;
        else
          pwdExpired = false;

        return Ok(new JwtTokenDto(tokenJWT, user.Id, pwdExpired, user.Enabled.ToString()));
      }

    }

    

    [HttpGet("all")]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrMsg))]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ActionResult<UsersDto>))]
    public async Task<ActionResult<UsersDto>> GetAllUser()
    {
      var clientsDto = new List<UsersDto>();

      var users = await this.userRepository.GetAll();

      if (users.Count == 0)
      {
        return NotFound(new ErrMsg("No user found!",
            this.HttpContext.Response.StatusCode));
      }

      foreach (var user in users)
      {
        clientsDto.Add(mapper.Map<UsersDto>(user));
      }

      return Ok(clientsDto);
    }

    [HttpGet("usr/{username}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UsersDto))]
    public async Task<ActionResult<UsersDto>> GetUser(String userName)
    {
      var user = await this.userRepository.GetUserByUsername(userName);

      if (user == null)
      {
        return NotFound(new ErrMsg(string.Format($"{userName} not found!"),
            this.HttpContext.Response.StatusCode));
      }

      return Ok(mapper.Map<UsersDto>(user));
    }

    [HttpGet("id/{username}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UsersDto))]
    public async Task<ActionResult<UsersDto>> GetUserById(string id)
    {
      var user = await this.userRepository.GetUserById(id);

      if (user == null)
      {
        return NotFound(new ErrMsg(string.Format($"User not found!"),
            this.HttpContext.Response.StatusCode));
      }

      return Ok(mapper.Map<UsersDto>(user));
    }

    [HttpPost("insert")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(InfoMsg))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrMsg))]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity, Type = typeof(ErrMsg))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrMsg))]
    public async Task<ActionResult<InfoMsg>> SaveUser([FromBody] Users user)
    {
      if (user == null)
      {
        return BadRequest(new ErrMsg("Insert user data",
            this.HttpContext.Response.StatusCode));
      }

      if (!ModelState.IsValid)
      {
        string ErrVal = "";

        foreach (var modelState in ModelState.Values)
        {
          foreach (var modelError in modelState.Errors)
          {
            ErrVal += modelError.ErrorMessage + " - ";
          }
        }

        return BadRequest(new ErrMsg(ErrVal, this.HttpContext.Response.StatusCode));
      }

      var isPresent = await userRepository.GetUserByUsername(user.Email);

      if (isPresent != null)
      {
        return StatusCode(422, new ErrMsg($"User {user.Email} already in use!",
            this.HttpContext.Response.StatusCode));
      }

      user.Id = Guid.NewGuid().ToString();

      bool isFirst = await userRepository.FirstUser();
      if (isFirst)
        user.Role = "ADMIN";
      else
        user.Role = "USER";
    
      PasswordHasher Hasher = new PasswordHasher();

      user.Password = Hasher.Hash(user.Password);

      bool retVal = await userRepository.InsUser(user);

      if (!retVal)
      {
        return StatusCode(500, new ErrMsg($"Errors in saving user {user.Email}.",
            this.HttpContext.Response.StatusCode));
      }

      retVal = userRepository.SendEmail(user.Email, user.Id, "activation");

      if (retVal)
        return Ok(new InfoMsg(DateTime.Today, $"User {user.Email} saved successfully! \n\r Check your email for activation"));
      else
        return StatusCode(500, new ErrMsg(string.Format($"Send email failed"),
          this.HttpContext.Response.StatusCode));

    }

    [HttpPut("update")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(InfoMsg))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<InfoMsg>> UpdateUser([FromBody] Users user)
    {
      if (user == null)
      {
        return BadRequest(new ErrMsg("Dati user assenti", this.HttpContext.Response.StatusCode));
      }

      bool isPresent = await this.userRepository.UserExists(user.Id);
      if (!isPresent)
      {
        
        return StatusCode(StatusCodes.Status422UnprocessableEntity, new ErrMsg($"User not found", this.HttpContext.Response.StatusCode));
      }

      if (!ModelState.IsValid)
      {
        string ErrVal = "";

        foreach (var modelState in ModelState.Values)
        {
          foreach (var error in modelState.Errors)
          {
            ErrVal += error.ErrorMessage + " - ";
          }
        }
        return BadRequest(new ErrMsg(ErrVal, StatusCodes.Status400BadRequest));
      }

      bool retVal = await userRepository.UpdUser(user);
      if (!retVal)
      {
        return StatusCode(StatusCodes.Status500InternalServerError, new ErrMsg($"User update failed",
            StatusCodes.Status500InternalServerError));
      }

      return Ok(new InfoMsg(DateTime.Today, "Update successfully!"));

    }

    [HttpDelete("delete/{userid}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(InfoMsg))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrMsg))]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity, Type = typeof(ErrMsg))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrMsg))]
    public async Task<ActionResult<InfoMsg>> DeleteUser(string UserId)
    {
      if (UserId == "")
      {
        return BadRequest(new ErrMsg("Insert user userId!",
            this.HttpContext.Response.StatusCode));
      }

      var user = await userRepository.GetUserToDelete(UserId);

      if (user == null)
      {
        return StatusCode(422, new ErrMsg($"User {UserId} not found!",
            this.HttpContext.Response.StatusCode));
      }

      bool retVal = await userRepository.DelUser(user);

      if (!retVal)
      {
        return StatusCode(500, new ErrMsg($"Cancel user {user.Email} failed.",
            this.HttpContext.Response.StatusCode));
      }

      return Ok(new InfoMsg(DateTime.Today, $"Cancel user {user.Email} successfully!"));

    }

    [HttpPut("activation")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(InfoMsg))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrMsg))]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity, Type = typeof(ErrMsg))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrMsg))]
    public async Task<ActionResult<InfoMsg>> Activation([FromBody] UserId userId)
    {
      if (userId == null)
      {
        return BadRequest(new ErrMsg("Insert user userid!",
            this.HttpContext.Response.StatusCode));
      }

      var user = await userRepository.GetUserById(userId.Id);

      if (user == null)
      {
        return StatusCode(422, new ErrMsg($"User not found!",
            this.HttpContext.Response.StatusCode));
      }

      bool retVal = await userRepository.Activation(userId.Id);

      if (!retVal)
      {
        return StatusCode(500, new ErrMsg($"user activation failed.",
            this.HttpContext.Response.StatusCode));
      }

      string activation = "Activation successfully!";

      return Ok(new InfoMsg(DateTime.Today, activation));
    }

    [HttpPut("changepwd")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(InfoMsg))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<InfoMsg>> ChangePassword([FromBody] ChangePwd changePwd)
    {
      if (changePwd == null)
        return BadRequest(new ErrMsg("Password not found", this.HttpContext.Response.StatusCode));

      if (changePwd.Id == null || changePwd.Id == "")
        return BadRequest(new ErrMsg("Id user not found", this.HttpContext.Response.StatusCode));

      bool isPresent = await this.userRepository.UserExists(changePwd.Id);
      if (!isPresent)
        return StatusCode(StatusCodes.Status422UnprocessableEntity, new ErrMsg($"User not found", this.HttpContext.Response.StatusCode));

      PasswordHasher Hasher = new PasswordHasher();      

      changePwd.NewPwd = Hasher.Hash(changePwd.NewPwd);

      bool retVal = await userRepository.UpdatePwd(changePwd);
      if (!retVal)
      {
        return StatusCode(StatusCodes.Status500InternalServerError, new ErrMsg($"Password save failed",
            StatusCodes.Status500InternalServerError));
      }

      return Ok(new InfoMsg(DateTime.Today, "Update successfully!"));
    }

    [HttpGet("sendactivationemail/{username}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UsersDto))]
    public async Task<ActionResult<bool>> SendActivationEmail(String userName)
    {
      var user = await this.userRepository.GetUserByUsername(userName);

      if (user == null)
      {
        return NotFound(new ErrMsg(string.Format($"User {userName} not found!"),
            this.HttpContext.Response.StatusCode));
      }

      bool retVal = userRepository.SendEmail(user.Email, user.Id, "activation");

      if (retVal)
        return Ok(new InfoMsg(DateTime.Today, $"Email sent successfully"));
      else
        return StatusCode(500, new ErrMsg(string.Format($"Email sent fail"),
          this.HttpContext.Response.StatusCode));

    }

    [HttpPut("sendpasswordemail")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(InfoMsg))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<InfoMsg>> SendPasswordEmail([FromBody] UserId userId)
    {
      if (userId == null)
        return BadRequest(new ErrMsg("User not found", this.HttpContext.Response.StatusCode));

      if (userId.Email == null || userId.Email == "")
        return BadRequest(new ErrMsg("User Email not found", this.HttpContext.Response.StatusCode));

      Users user = await this.userRepository.GetUserByUsername(userId.Email);
      if (user == null)
        return StatusCode(StatusCodes.Status422UnprocessableEntity, new ErrMsg($"User not found", this.HttpContext.Response.StatusCode));

      ChangePwd cb = new ChangePwd
      {
        Id = user.Id,
        OldPwd = user.Password,
        NewPwd = ""
      };

      PasswordHasher Hasher = new PasswordHasher();

      cb.NewPwd = Hasher.Hash("newpwd");

      bool retVal = await userRepository.UpdatePwd(cb);
      if (!retVal)
      {
        return StatusCode(StatusCodes.Status500InternalServerError, new ErrMsg($"Reset Password fail",
            StatusCodes.Status500InternalServerError));
      }

      retVal = userRepository.SendEmail(user.Email, user.Id, "forgotpwd");

      if (retVal)
        return Ok(new InfoMsg(DateTime.Today, $"Email sent successfully"));
      else
        return StatusCode(500, new ErrMsg(string.Format($"Email sent fail"),
          this.HttpContext.Response.StatusCode));
    }

  }
}

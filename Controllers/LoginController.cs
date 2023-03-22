using Microsoft.AspNetCore.Mvc;
using AuthUser.DBContext;
using AuthUser.Models;


namespace AuthUser.Controllers;

[ApiController]
[Route("[controller]")]

public class LoginController : Controller
{
    private readonly ILogger<LoginController> _logger_login;
    private readonly DatabaseContext _databaseContext_login;

    public LoginController(ILogger<LoginController> logger, DatabaseContext databaseContext)
    {
        _logger_login = logger;
        _databaseContext_login = databaseContext;
    }

//update status
    [HttpPut] //("{user}")
    public IActionResult updateStatus(Account user,string status)
    {
        try
        {
            var _user = _databaseContext_login.Accounts.SingleOrDefault(o => o.User == user.User);
            // var _status = _databaseContext.Accounts.SingleOrDefault(o => o.Status == status);
            if(_user != null)
            {
                _user.Status = status;
                _databaseContext_login.Accounts.Update(_user); //create Add command
                _databaseContext_login.SaveChanges(); //commit new user to database
            }
            return Ok(new {result = _user});
        } 
        catch (Exception ex)
        {
            return StatusCode(500, new {result = ex.Message, message = "update status fail"});
        }  
    }

// Login Function
    [HttpPost] //("{user}")
    public IActionResult Login(Account user)
    {
        try
        {
            var _user = _databaseContext_login.Accounts.SingleOrDefault(o => o.Id == user.Id && o.User == user.User && o.Password == user.Password);
            if(_user != null)       //Check username is collect or not
            {
                updateStatus(user, "online");
                return Ok(new { result = _user, message = "Login success"});
            }
            else{
                return Ok(new { message = "Login fail"});
            }
        } 
        catch (Exception ex)
        {
            return StatusCode(500, new {result = ex.Message, message = "Login fail"});
        }  
    }
}

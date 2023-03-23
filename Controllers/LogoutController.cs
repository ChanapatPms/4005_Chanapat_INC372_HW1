using Microsoft.AspNetCore.Mvc;
using AuthUser.DBContext;
using AuthUser.Models;


namespace AuthUser.Controllers;

[ApiController]
[Route("[controller]")]

public class LogoutController : Controller
{
    private readonly ILogger<LogoutController> _logger_logout;
    private readonly DatabaseContext _databaseContext_logout;

    public LogoutController(ILogger<LogoutController> logger, DatabaseContext databaseContext)
    {
        _logger_logout = logger;
        _databaseContext_logout = databaseContext;
    }

//update status
    [HttpPut] //("{user}")
    public IActionResult updateStatus(Account user,string status)
    {
        try
        {
            var _user = _databaseContext_logout.Accounts.SingleOrDefault(o => o.User == user.User);
            // var _status = _databaseContext_logout.Accounts.SingleOrDefault(o => o.Status == status);
            if(_user != null)
            {
                _user.Status = status;
                _databaseContext_logout.Accounts.Update(_user); //create Add command
                _databaseContext_logout.SaveChanges(); //commit new user to database
            }
            return Ok(new {result = _user});
        } 
        catch (Exception ex)
        {
            return StatusCode(500, new {result = ex.Message, message = "Add user fail"});
        }  
    }

// Logout Function
    [HttpPost] //("{user}")
    public IActionResult Logout(Account user)
    {
        try
        {
            var _user = _databaseContext_logout.Accounts.SingleOrDefault(o => o.Id == user.Id && o.User == user.User);
            if(_user != null)       //Check username is collect or not
            {
                if(_user.Status == "offline")
                {
                    updateStatus(user, "offline");
                    return Ok(new { result = _user, message = "Logout success"});
                }
                else
                {
                    return Ok(new { result = _user, message = "Already logout"});
                }
            }
            else{
                return Ok(new { message = "Logout fail"});
            }
        } 
        catch (Exception ex)
        {
            return StatusCode(500, new {result = ex.Message, message = "Logout fail"});
        }  
    }
}

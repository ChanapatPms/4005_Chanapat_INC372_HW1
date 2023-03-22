using Microsoft.AspNetCore.Mvc;
using AuthUser.DBContext;
using AuthUser.Models;


namespace AuthUser.Controllers;

[ApiController]
[Route("[controller]")]

public class AccountController : Controller
{
    private readonly ILogger<AccountController> _logger;
    private readonly DatabaseContext _databaseContext;

    public AccountController(ILogger<AccountController> logger, DatabaseContext databaseContext)
    {
        _logger = logger;
        _databaseContext = databaseContext;
    }

//Get Account
    [HttpGet (Name = "GetAccount")]
    public IActionResult GetAccount()
    {
        try
        {
            var _user = _databaseContext.Accounts.ToList(); //in SQL --> SELECT * FROM USERS
            if(_user != null){
                return Ok(new {result = _user, message = "success"});
            }
            else{
                return Ok(new {message = "fail"});
            }
        } 
        catch (Exception ex)
        {
            return StatusCode(500, new {result = ex.Message, message = "fail"});
        }  
    }

    [HttpGet("{id}")]
    public IActionResult GetAccountById(int id)
    {
        try
        {
            var user = _databaseContext.Accounts.SingleOrDefault(o => o.Id == id); // in SQL --> SELECT * FROM USERS ID
            return Ok(new {result = user, message = "Get user success"});
        } 
        catch (Exception ex)
        {
            return StatusCode(500, new {result = ex.Message, message = "Get user fail"});
        }   
    }

//Create Account function
   [HttpPost]
   public IActionResult CreateAccount(Account user)
   {
       try
       {
           _databaseContext.Accounts.Add(user); //create Add command
           _databaseContext.SaveChanges(); //commit new user to database
           return Ok(new {message = "Create user success"});
       } 
       catch (Exception ex)
       {
           return StatusCode(500, new {result = ex.Message, message = "Create user fail"});
       }  
   }

//Re-password function
    [HttpPut] //("{user}")
    public IActionResult RePassword(Account user)
    {
        try
        {
            var _user = _databaseContext.Accounts.SingleOrDefault(o => o.Id == user.Id && o.User == user.User);
            if(_user != null){
                _user.Password = user.Password;
                _databaseContext.Accounts.Update(_user); //create Add command
                _databaseContext.SaveChanges(); //commit new user to database
                return Ok(new { result = _user, message = "Change password success"});
            }
            else{
                return Ok(new { message = "Change password fail"});
            }
        }
        catch (Exception ex)
        {
            return StatusCode(500, new {result = ex.Message, message = "Change password fail"});
        } 
    }

//Delete Account
    [HttpDelete("{id}")]
    public IActionResult DeleteAccount(int id)
    {
        try
        {
            var _user = _databaseContext.Accounts.SingleOrDefault(o => o.Id == id);
            if(_user != null)
            {
                _databaseContext.Remove(_user);
                _databaseContext.SaveChanges();
                return Ok(new {message = "delete user success"});
            }
            else
            {
                return Ok(new {message = "delete user fail"});
            }
        }
        catch (Exception ex)
        {
            return StatusCode(500, new {result = ex.Message, message = "fail"});
        }
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PassesProject.Controllers.Dtos;
using PassesProject.Services;

namespace PassesProject.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class AccountController : ControllerBase
{
    private readonly AccountService _accountService;
    
    public AccountController(AccountService accountService)
    {
        _accountService = accountService;
    }
    
    [AllowAnonymous]
    [HttpPost]
    public string Login([FromBody] UserLoginRequest request)
    {
        string token = _accountService.Login(request);
        
        return token;
    }
}
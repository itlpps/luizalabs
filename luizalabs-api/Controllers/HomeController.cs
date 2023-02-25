using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using LuizaLabsApi.Service;

namespace LuizaLabsApi.Controllers;

[Route("v1/account")]
public class HomeController : ControllerBase
{
    [HttpPost]
    [Route("login")]
    [AllowAnonymous]
    public async Task<ActionResult<dynamic>> Authenticate()
    {
        var user = new Models.User
        {
            Id = Guid.NewGuid(),
            Name = "ITALOO",
            Email = "ITALO@MAIL.COM",
            IsVerified = 1,
            Password = "123456",
            Status = 1
        };

        var token = AuthService.GenerateToken(user);

        user.Password = "";
        return new { user = user, token = token };
    }

    [HttpGet]
    [Route("anonymous")]
    [AllowAnonymous]
    public string Anonymous() => "Anônimo";

    [HttpGet]
    [Route("authenticated")]
    [Authorize]
    public string Authenticated() => String.Format("Autenticado - {0}", User.Identity.Name);

    [HttpGet]
    [Route("employee")]
    [Authorize(Roles = "employee,manager")]
    public string Employee() => "Funcionário";

    [HttpGet]
    [Route("manager")]
    [Authorize(Roles = "manager")]
    public string Manager() => "Gerente";
}

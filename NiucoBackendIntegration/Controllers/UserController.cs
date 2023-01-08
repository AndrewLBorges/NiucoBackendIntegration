using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NiucoBackendIntegration.Entities;
using NiucoBackendIntegration.Interfaces;
using System;

namespace NiucoBackendIntegration.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly ILogger<User> _logger;
    private readonly IUserService _userService;

    public UserController(ILogger<User> logger, IUserService userService)
    {
        _logger = logger;
        _userService = userService;
    }

    [HttpGet]
    public async Task<IEnumerable<User>> Get()
    {
        return  await _userService.GetUsersAsync();
    }

    [HttpGet]
    [Route("/{id}")]
    public async Task<IActionResult> Get(Guid id)
    {
        var users = await _userService.GetUsersAsync();
        var requestedUser = users.FirstOrDefault(users => users.Id == id);

        if (requestedUser is null)
        {
            _logger.LogError($"User with Id {id} not found.", requestedUser);
            return NotFound();
        }

        _logger.LogInformation($"User fetched and being returned. Id: {id}", requestedUser);
        return Ok(requestedUser);
    }
}

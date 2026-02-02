using Backend.Models;
using Backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Api;

[ApiController]
[Route("users")]
public class UsersController : ControllerBase
{
    private readonly UserService _users;

    public UsersController(UserService users)
    {
        _users = users;
    }

    [HttpGet]
    public IEnumerable<User> Get()
    {
        return _users.GetAll();
    }

    [HttpPost]
    public IActionResult Create(CreateUserDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Email))
            return BadRequest("Email is required");

        try
        {
            _users.Create(dto);
            return Created("", null);
        }
        catch (SqliteException)
        {
            return Conflict("User already exists");
        }
    }
}
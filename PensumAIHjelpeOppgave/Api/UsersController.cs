using Backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using PensumAIHjelpeOppgave.Models;
using PensumAIHjelpeOppgave.Services;

namespace PensumAIHjelpeOppgave.Api;

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
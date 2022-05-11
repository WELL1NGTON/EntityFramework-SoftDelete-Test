using AutoMapper;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using SoftDelete.Test.Data;
using SoftDelete.Test.Dtos;
using SoftDelete.Test.Entities;

namespace SoftDelete.Test.Controllers;

[Route("[controller]")]
public class UsersController : Controller
{
    private readonly ILogger<UsersController> _logger;
    private readonly SoftDeleteContext _context;
    private readonly IMapper _mapper;

    public UsersController(ILogger<UsersController> logger, SoftDeleteContext context, IMapper mapper)
    {
        _logger = logger;
        _context = context;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<List<UserDto>>> GetAsync()
    {
        _logger.LogInformation("Getting all users");

        var users = await _context.Users.ToListAsync();

        return Ok(_mapper.Map<List<UserDto>>(users));
    }

    [HttpGet("{id:Guid}")]
    public async Task<ActionResult<UserDto>> GetAsync(Guid id)
    {
        _logger.LogInformation("Getting user with id {id}", id);

        var user = await _context.Users.FindAsync(id);

        if (user is null)
        {
            return NotFound();
        }

        return Ok(_mapper.Map<UserDto>(user));
    }

    [HttpPost]
    public async Task<ActionResult<UserDto>> PostAsync([FromBody] UserDto userDto)
    {
        _logger.LogInformation("Creating user {Name}", userDto.Name);

        var user = new UserEntity(userDto.Name);

        await _context.Users.AddAsync(user);

        var isSuccess = (await _context.SaveChangesAsync()) > 0;

        if (!isSuccess)
        {
            return BadRequest();
        }

        return Ok(_mapper.Map<UserDto>(user));
    }

    [HttpPut("{id:Guid}")]
    public async Task<ActionResult<UserDto>> PutAsync(Guid id, [FromBody] UserDto userDto)
    {
        _logger.LogInformation("Updating user with id {id}", id);

        var user = await _context.Users.FindAsync(id);

        if (user is null)
        {
            return NotFound();
        }

        user.Name = userDto.Name;

        _context.Users.Update(user);

        var isSuccess = (await _context.SaveChangesAsync()) > 0;

        if (!isSuccess)
        {
            return BadRequest();
        }

        return Ok(_mapper.Map<UserDto>(user));
    }

    [HttpDelete("{id:Guid}")]
    public async Task<ActionResult> DeleteAsync(Guid id)
    {
        _logger.LogInformation("Deleting user with id {id}", id);

        var user = await _context.Users.FindAsync(id);

        if (user is null)
        {
            return NotFound();
        }

        _context.Users.Remove(user);

        var isSuccess = (await _context.SaveChangesAsync()) > 0;

        if (!isSuccess)
        {
            return BadRequest();
        }

        return Ok();
    }

}

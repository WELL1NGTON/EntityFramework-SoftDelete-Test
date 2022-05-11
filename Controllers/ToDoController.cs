using AutoMapper;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using SoftDelete.Test.Data;
using SoftDelete.Test.Dtos;
using SoftDelete.Test.Entities;

namespace SoftDelete.Test.Controllers;

[Route("[controller]")]
public class ToDoController : Controller
{
    private readonly ILogger<ToDoController> _logger;
    private readonly SoftDeleteContext _context;
    private readonly IMapper _mapper;

    public ToDoController(ILogger<ToDoController> logger, SoftDeleteContext context, IMapper mapper)
    {
        _logger = logger;
        _context = context;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<List<ToDoEntryDto>>> GetAsync([FromQuery] bool includeDeleted = false)
    {
        _logger.LogInformation("Getting all todo entries");

        IQueryable<ToDoEntryEntity> queryable = _context.ToDoEntries;

        if (includeDeleted)
        {
            queryable = queryable.IgnoreQueryFilters();
        }

        var todoEntries = await queryable.ToListAsync();

        return Ok(_mapper.Map<List<ToDoEntryDto>>(todoEntries));
    }

    [HttpGet("{id:Guid}")]
    public async Task<ActionResult<ToDoEntryDto>> GetAsync(Guid id)
    {
        _logger.LogInformation("Getting todo entry with id {id}", id);

        var todoEntry = await _context.ToDoEntries.FindAsync(id);

        if (todoEntry is null)
        {
            return NotFound();
        }

        return Ok(_mapper.Map<ToDoEntryDto>(todoEntry));
    }

    [HttpPost]
    public async Task<ActionResult<ToDoEntryDto>> PostAsync([FromBody] ToDoEntryDto todoEntryDto)
    {
        _logger.LogInformation("Creating todo entry {Name}", todoEntryDto.Title);

        var user = await _context.Users.FindAsync(todoEntryDto.UserId);

        if (user is null)
        {
            return NotFound("User not found");
        }

        var todoEntry = new ToDoEntryEntity(todoEntryDto.Title, todoEntryDto.Description, todoEntryDto.DueDate, user);

        await _context.ToDoEntries.AddAsync(todoEntry);

        var isSuccess = (await _context.SaveChangesAsync()) > 0;

        if (!isSuccess)
        {
            return BadRequest();
        }

        return Ok(_mapper.Map<ToDoEntryDto>(todoEntry));
    }

    [HttpPut("{id:Guid}")]
    public async Task<ActionResult<ToDoEntryDto>> PutAsync(Guid id, [FromBody] ToDoEntryDto todoEntryDto)
    {
        _logger.LogInformation("Updating todo entry with id {id}", id);

        var todoEntry = await _context.ToDoEntries.FindAsync(id);

        if (todoEntry is null)
        {
            return NotFound();
        }

        todoEntry.Title = todoEntryDto.Title;
        todoEntry.Description = todoEntryDto.Description;
        todoEntry.DueDate = todoEntryDto.DueDate;
        todoEntry.IsDone = todoEntryDto.IsDone;

        _context.ToDoEntries.Update(todoEntry);

        var isSuccess = (await _context.SaveChangesAsync()) > 0;

        if (!isSuccess)
        {
            return BadRequest();
        }

        return Ok(_mapper.Map<ToDoEntryDto>(todoEntry));
    }

    [HttpDelete("{id:Guid}")]
    public async Task<ActionResult<ToDoEntryDto>> DeleteAsync(Guid id, [FromQuery] bool forceDelete = false)
    {
        _logger.LogInformation("Deleting todo entry with id {id}", id);

        IQueryable<ToDoEntryEntity> todoQueryable = _context.ToDoEntries;

        if (forceDelete)
        {
            todoQueryable = todoQueryable.IgnoreQueryFilters();
        }

        var todoEntry = await todoQueryable.FirstOrDefaultAsync(t => t.Id == id);

        if (todoEntry is null)
        {
            return NotFound();
        }

        _context.ToDoEntries.Remove(todoEntry);

        var isSuccess = (await _context.SaveChangesAsync()) > 0;

        if (!isSuccess)
        {
            return BadRequest();
        }

        return Ok(_mapper.Map<ToDoEntryDto>(todoEntry));
    }
}

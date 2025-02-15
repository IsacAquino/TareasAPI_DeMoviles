using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TareasAPI.Data;

[Route("api/[controller]")]
[ApiController]
public class TasksController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public TasksController(ApplicationDbContext context)
    {
        _context = context;
    }

    // Obtener todas las tareas
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TaskModel>>> GetTasks()
    {
        return await _context.Tareas.ToListAsync();
    }

    // Agregar una nueva tarea
    [HttpPost]
    public async Task<ActionResult<TaskModel>> PostTask(TaskModel task)
    {
        _context.Tareas.Add(task);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetTasks), new { id = task.Id }, task);
    }

    // Marcar tarea como completada
    [HttpPut("{id}")]
    public async Task<IActionResult> CompleteTask(int id)
    {
        var task = await _context.Tareas.FindAsync(id);
        if (task == null) return NotFound();

        task.Completed = true;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    // Editar tarea
    [HttpPut("edit/{id}")]
    public async Task<IActionResult> EditTask(int id, TaskModel updatedTask)
    {
        var task = await _context.Tareas.FindAsync(id);
        if (task == null) return NotFound();

        task.Title = updatedTask.Title;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    // Eliminar tarea
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTask(int id)
    {
        var task = await _context.Tareas.FindAsync(id);
        if (task == null) return NotFound();

        _context.Tareas.Remove(task);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}

using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

public class UsuarioLogin
{
    public string NombreUsuario { get; set; }
    public string Contraseña { get; set; }
}

[Route("api/[controller]")]
[ApiController]
public class UsuarioController : ControllerBase
{
    private readonly AppDbContext _context;

    public UsuarioController(AppDbContext context)
    {
        _context = context;
    }

    // GET: api/Usuario
    [HttpGet]
    public ActionResult<IEnumerable<Usuario>> GetUsuarios()
    {
        return _context.Usuarios.ToList();
    }

    // GET: api/Usuario/5
    [HttpGet("{id}")]
    public ActionResult<Usuario> GetUsuario(int id)
    {
        var usuario = _context.Usuarios.Find(id);

        if (usuario == null)
        {
            return NotFound();
        }

        return usuario;
    }

    // POST: api/Usuario
    [HttpPost]
    public ActionResult<Usuario> PostUsuario(Usuario usuario)
    {
        _context.Usuarios.Add(usuario);
        _context.SaveChanges();

        return CreatedAtAction(nameof(GetUsuario), new { id = usuario.IdUsuario }, usuario);
    }

    // PUT: api/Usuario/5
    [HttpPut("{id}")]
    public IActionResult PutUsuario(int id, Usuario usuario)
    {
        if (id != usuario.IdUsuario)
        {
            return BadRequest();
        }

        _context.Entry(usuario).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
        _context.SaveChanges();

        return NoContent();
    }

    // DELETE: api/Usuario/5
    [HttpDelete("{id}")]
    public IActionResult DeleteUsuario(int id)
    {
        var usuario = _context.Usuarios.Find(id);

        if (usuario == null)
        {
            return NotFound();
        }

        _context.Usuarios.Remove(usuario);
        _context.SaveChanges();

        return NoContent();
    }
}

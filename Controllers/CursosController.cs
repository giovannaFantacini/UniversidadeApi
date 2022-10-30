using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UniversidadeApi.Models;

namespace UniversidadeApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CursosController : ControllerBase
    {
        private readonly UniversidadeContext _context;

        public CursosController(UniversidadeContext context)
        {
            _context = context;
        }

        // GET: api/Cursos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Curso>>> GetCursos()
        {
          if (_context.Cursos == null)
          {
              return NotFound();
          }
            return await _context.Cursos.ToListAsync();
        }

        // GET: api/Cursos/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Curso>> GetCurso(long id)
        {
          if (_context.Cursos == null)
          {
              return BadRequest();
          }
            var curso = await _context.Cursos.FindAsync(id);

            if (curso == null)
            {
                return NotFound();
            }

            return curso;
        }


        //GET: api/Cursos/LES
        [HttpGet("{sigla}")]
        public async Task<ActionResult<Curso>> GetCursoSigla([FromRoute] string sigla)
        {
            if (_context.Cursos == null)
            {
                return NotFound();
            }
            
           var curso = await _context.Cursos.FirstOrDefaultAsync(a => a.Sigla == sigla);

            if (curso == null)
            {
               return NotFound();
            }

            return curso;
        }

    
        // PUT: api/Cursos/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCurso(long id, Curso curso)
        {
            if (id != curso.Id)
            {
                return BadRequest();
            }

            _context.Entry(curso).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CursoExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Cursos
        [HttpPost]
        public async Task<ActionResult<Curso>> PostCurso(Curso curso)
        {
          if (_context.Cursos == null)
          {
              return Problem("Entity set 'UniversidadeContext.Cursos'  is null.");
          }
            _context.Cursos.Add(curso);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCursos", new { id = curso.Id }, curso);
        }

        // DELETE: api/Cursos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCurso(long id)
        {
            if (_context.Cursos == null)
            {
                return NotFound();
            }
            var curso = await _context.Cursos.FindAsync(id);
            if (curso == null)
            {
                return NotFound();
            }

            _context.Cursos.Remove(curso);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CursoExists(long id)
        {
            return (_context.Cursos?.Any(e => e.Id == id)).GetValueOrDefault();
        }


    }
}

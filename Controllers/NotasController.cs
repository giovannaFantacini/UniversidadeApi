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
    public class NotasController : ControllerBase
    {
        private readonly UniversidadeContext _context;

        public NotasController(UniversidadeContext context)
        {
            _context = context;
        }

        // GET: api/Nota
        [HttpGet]
        public async Task<ActionResult<IEnumerable<NotaDTO>>> GetNotas()
        {
          if (_context.Notas == null)
          {
              return NotFound();
          }
            return await _context.Notas.Include(a=>a.Aluno).Include(u=>u.UnidadeCurricular)
                   .Select(x => NotaToDTO(x))
                   .ToListAsync();
        }

        // GET: api/Nota/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<NotaDTO>> GetNota(long id)
        {
          if (_context.Notas == null)
          {
              return NotFound();
          }
            var nota = await _context.Notas.Include(a=>a.Aluno).Include(u=>u.UnidadeCurricular).FirstAsync(i=>i.Id == id);

            if (nota == null)
            {
                return NotFound();
            }

            return NotaToDTO(nota);
        }

        //GET: api/notas/SINF2
        [HttpGet("{sigla}")]
        public async Task<ActionResult<NotaDTO>> GetNotaSigla([FromRoute] string sigla)
        {
            if (_context.Notas == null)
            {
                return NotFound();
            }
            
           var nota = await _context.Notas
                      .Include(a => a.Aluno).Include(u => u.UnidadeCurricular)
                      .FirstAsync(u => u.UnidadeCurricular.Sigla == sigla);

            if (nota == null)
            {
               return NotFound();
            }

            return NotaToDTO(nota);
        }

        // PUT: api/Nota/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutNota(long id, NotaDTO notaDTO)
        {
            if (id != notaDTO.Id)
            {
                return BadRequest();
            }

            var nota = await _context.Notas.FindAsync(id);
            if (nota == null)
            {
                return NotFound();
            }
            
            nota.Valor = notaDTO.Valor;


            var uc = await _context.unidadeCurriculares.FirstOrDefaultAsync(i=>i.Sigla == notaDTO.SiglaUC);
            if(uc != null){
                nota.UnidadeCurricular = uc;
            }else{
                return Problem("Curso doesn't exist");
            }

            var aluno = await _context.Alunos.FirstOrDefaultAsync(i=>i.NumeroRec == notaDTO.NumeroRecAluno);
            if(aluno != null){
                nota.Aluno = aluno;
            }else{
                return Problem("Aluno doesn't exist");
            }

             try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) when (!NotaExists(id))
            {
                return NotFound();
            }

            return NoContent();
        }

        // POST: api/Nota
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Nota>> PostNota(NotaDTO notaDTO)
        {
          if (_context.Notas == null)
          {
              return Problem("Entity set 'UniversidadeContext.Notas'  is null.");
          }

          var nota = new Nota
          {
            Valor = notaDTO.Valor
          };

          var uc = await _context.unidadeCurriculares.FirstOrDefaultAsync(i=>i.Sigla == notaDTO.SiglaUC);
          if(uc != null){
            nota.UnidadeCurricular = uc;
          }else{
            return Problem("Curso doesn't exist");
          }

          var aluno = await _context.Alunos.FirstOrDefaultAsync(i=>i.NumeroRec == notaDTO.NumeroRecAluno);
          if(aluno != null){
            nota.Aluno = aluno;
          }else{
            return Problem("Aluno doesn't exist");
          }
          
           _context.Notas.Add(nota);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetNotas),
                new { id = nota.Id },
                NotaToDTO(nota));
        }

        // DELETE: api/Nota/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNota(long id)
        {
            if (_context.Notas == null)
            {
                return NotFound();
            }
            var nota = await _context.Notas.FindAsync(id);
            if (nota == null)
            {
                return NotFound();
            }

            _context.Notas.Remove(nota);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool NotaExists(long id)
        {
            return (_context.Notas?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        private static NotaDTO NotaToDTO(Nota nota) =>
        new NotaDTO
        {
            Id = nota.Id,
            Valor = nota.Valor,
            SiglaUC = nota.UnidadeCurricular?.Sigla,
            NumeroRecAluno = nota.Aluno?.NumeroRec,

        };
    }
}

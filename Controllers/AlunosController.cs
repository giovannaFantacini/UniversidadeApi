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
    public class AlunosController : ControllerBase
    {
        private readonly UniversidadeContext _context;

        public AlunosController(UniversidadeContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AlunoDTO>>> GetAlunos()
        {
            return await _context.Alunos.Include(c => c.Curso)
                .Select(x => AlunoToDTO(x))
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AlunoDTO>> GetAlunos(long id)
        {
            var aluno = await _context.Alunos.Include(c => c.Curso).FirstAsync(i=>i.Id == id);

            if (aluno == null)
            {
                return NotFound();
            }

            return AlunoToDTO(aluno);
        }

        [HttpPost]
        public async Task<ActionResult<AlunoDTO>> CreateTodoItem(AlunoDTO alunoDTO)
        {
            var aluno = new Aluno
            {   
                Nome = alunoDTO.Nome,
                NumeroRec = alunoDTO.NumeroRec,
            };
            var curso = await _context.Cursos.FirstOrDefaultAsync(i=>i.Sigla == alunoDTO.siglaCurso);
            if(curso != null){
                aluno.Curso = curso;
                _context.Alunos.Add(aluno);
                await _context.SaveChangesAsync();
            }else{
                return Problem("Curso doesn't exist");
            }

            return CreatedAtAction(
                nameof(GetAlunos),
                new { id = aluno.Id },
                AlunoToDTO(aluno));
        }

         // PUT: api/Alunos/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAluno(long id, AlunoDTO alunoDTO)
        {
             if (id != alunoDTO.Id)
            {
                return BadRequest();
            }

            var aluno = await _context.Alunos.FindAsync(id);

            if (aluno == null)
            {
                return NotFound();
            }

            aluno.Nome = alunoDTO.Nome;
            aluno.NumeroRec = alunoDTO.NumeroRec;

            var curso = await _context.Cursos.FirstOrDefaultAsync(i=>i.Sigla == alunoDTO.siglaCurso);
            if(curso != null){
                aluno.Curso = curso;
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) when (!AlunoExists(id))
            {
                return NotFound();
            }

            return NoContent();
        }

        // DELETE: api/Alunos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAluno(long id)
        {
            if (_context.Alunos == null)
            {
                return NotFound();
            }
            var aluno = await _context.Alunos.FindAsync(id);
            if (aluno == null)
            {
                return NotFound();
            }

            _context.Alunos.Remove(aluno);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AlunoExists(long id)
        {
            return (_context.Alunos?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        private static AlunoDTO AlunoToDTO(Aluno aluno) =>
        new AlunoDTO
        {
            Id = aluno.Id,
            Nome = aluno.Nome,
            NumeroRec = aluno.NumeroRec,
            siglaCurso = aluno.Curso?.Sigla

        };
    }
}

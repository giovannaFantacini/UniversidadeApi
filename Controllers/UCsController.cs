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
    [Route("api/ucs")]
    [ApiController]
    public class UCsController : ControllerBase
    {
        private readonly UniversidadeContext _context;

        public UCsController(UniversidadeContext context)
        {
            _context = context;
        }
       
        // GET: api/UCs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UnidadeCurricularDTO>>> GetunidadeCurriculares()
        {
          if (_context.unidadeCurriculares == null)
          {
              return NotFound();
          }
           return await _context.unidadeCurriculares
                .Include(c => c.Curso).Include(a => a.ListAluno)
                .Select(x => UCToDTO(x))
                .ToListAsync();
        }

        // GET: api/UCs/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<UnidadeCurricularDTO>> GetUnidadeCurricular(long id)
        {
          if (_context.unidadeCurriculares == null)
          {
              return NotFound();
          }
            var unidadeCurricular = await _context.unidadeCurriculares.Include(c => c.Curso)
                                    .Include(a => a.ListAluno).FirstAsync(i=>i.Id == id);

            if (unidadeCurricular == null)
            {
                return NotFound();
            }

            return UCToDTO(unidadeCurricular);
        }

        //GET: api/ucs/SINF2
        [HttpGet("{sigla}")]
        public async Task<ActionResult<UnidadeCurricularDTO>> GetUCSigla([FromRoute] string sigla)
        {
            if (_context.unidadeCurriculares == null)
            {
                return NotFound();
            }
            
           var uc = await _context.unidadeCurriculares.Include(c => c.Curso)
                                    .Include(a => a.ListAluno).FirstAsync(s => s.Sigla == sigla);

            if (uc == null)
            {
               return NotFound();
            }

            return UCToDTO(uc);
        }

        // PUT: api/UCs/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUnidadeCurricular(long id, UnidadeCurricularDTO ucDTO)
        {
              if (id != ucDTO.Id)
            {
                return BadRequest();
            }

            var uc = await _context.unidadeCurriculares.FindAsync(id);

            if (uc == null)
            {
                return NotFound();
            }

            uc.Ano = ucDTO.Ano;
            uc.Nome = ucDTO.Nome;
            uc.Sigla = ucDTO.Sigla;
            uc.ListAluno = new  List<Aluno>();

           var curso = await _context.Cursos.FirstOrDefaultAsync(i=>i.Sigla == ucDTO.siglaCurso);
            if(curso != null){
                uc.Curso = curso;
            }else{
                return Problem("Curso doesn't exist");
            }

            foreach (var num in ucDTO.numeroAluno){
                var aluno = await _context.Alunos.FirstOrDefaultAsync(i=>i.NumeroRec == num);
                if(aluno != null){
                    uc?.ListAluno?.Add(aluno);
                }else{
                    return Problem("Aluno doesn't exist");
                }
            }
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) when (!UnidadeCurricularExists(id))
            {
                return NotFound();
            }

            return NoContent();
        }


        // POST: api/UCs
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<UnidadeCurricularDTO>> PostUnidadeCurricular(UnidadeCurricularDTO unidadeCurricularDTO)
        {
          if (_context.unidadeCurriculares == null)
          {
              return Problem("Entity set 'UniversidadeContext.unidadeCurriculares'  is null.");
          }

            var unidadeCurricular = new UnidadeCurricular
            {   
                Nome = unidadeCurricularDTO.Nome,
                Sigla = unidadeCurricularDTO.Sigla,
                Ano = unidadeCurricularDTO.Ano
                
            };
            unidadeCurricular.ListAluno = new List<Aluno>();

            var curso = await _context.Cursos.FirstOrDefaultAsync(i=>i.Sigla == unidadeCurricularDTO.siglaCurso);
            if(curso != null){
                unidadeCurricular.Curso = curso;
            }else{
                return Problem("Curso doesn't exist");
            }

            foreach (var num in unidadeCurricularDTO.numeroAluno){
                var aluno = await _context.Alunos.FirstOrDefaultAsync(i=>i.NumeroRec == num);
                if(aluno != null){
                    unidadeCurricular?.ListAluno?.Add(aluno);
                }else{
                    return Problem("Aluno doesn't exist");
                }
            }

             _context.unidadeCurriculares.Add(unidadeCurricular);
                await _context.SaveChangesAsync();
        
            return CreatedAtAction(
                nameof(GetunidadeCurriculares),
                new { id = unidadeCurricular.Id },
                UCToDTO(unidadeCurricular));
        }

        // DELETE: api/UCs/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUnidadeCurricular(long id)
        {
            if (_context.unidadeCurriculares == null)
            {
                return NotFound();
            }
            var unidadeCurricular = await _context.unidadeCurriculares.FindAsync(id);
            if (unidadeCurricular == null)
            {
                return NotFound();
            }

            _context.unidadeCurriculares.Remove(unidadeCurricular);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UnidadeCurricularExists(long id)
        {
            return (_context.unidadeCurriculares?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        private static UnidadeCurricularDTO UCToDTO(UnidadeCurricular uc){
            var ucDTO = new UnidadeCurricularDTO
            {
                Id = uc.Id,
                Nome = uc.Nome,
                siglaCurso = uc.Curso?.Sigla,
                Sigla = uc.Sigla,
                Ano = uc.Ano,
                numeroAluno = new List<int>()
            };


            foreach (var aluno in uc?.ListAluno)
            {
                var numero = aluno.NumeroRec;
                ucDTO?.numeroAluno?.Add(numero);
                
            }
            return ucDTO;
        }
    }
}

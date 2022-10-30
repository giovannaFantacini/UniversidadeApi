using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace UniversidadeApi.Models
{
    public class UniversidadeContext : DbContext
    {
        public UniversidadeContext(DbContextOptions<UniversidadeContext> options)
            : base(options) 
        {
        }

        public DbSet<Curso> Cursos { get; set; }
        public DbSet<Aluno> Alunos { get; set; }
        public DbSet<Nota> Notas { get; set; }
        public DbSet<UnidadeCurricular> unidadeCurriculares { get; set; }

    }
}

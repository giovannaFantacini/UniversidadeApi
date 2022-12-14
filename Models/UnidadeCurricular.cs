namespace UniversidadeApi.Models
{
    public class UnidadeCurricular
    {
        public long Id { get; set; }
        public string? Nome { get; set; }
        public Curso? Curso { get; set; }
        public string? Sigla { get; set; }
        public int? Ano { get; set; }
        public virtual ICollection<Aluno>? ListAluno {get;set;}
    }
}
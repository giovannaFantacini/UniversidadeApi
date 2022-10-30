namespace UniversidadeApi.Models
{
    public class Nota
    {
        public long Id { get; set; }
        public int Valor { get; set; }
        public UnidadeCurricular? UnidadeCurricular { get; set; }
        public Aluno? Aluno { get; set; }
    }
}
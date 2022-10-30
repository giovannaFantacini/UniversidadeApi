namespace UniversidadeApi.Models
{
    public class NotaDTO
    {
        public long Id { get; set; }
        public int Valor { get; set; }
        public string? SiglaUC { get; set; }
        public int? NumeroRecAluno { get; set; }
    }
}
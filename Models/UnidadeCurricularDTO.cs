namespace UniversidadeApi.Models
{
    public class UnidadeCurricularDTO
    {
        public long Id { get; set; }
        public string? Nome { get; set; }
        public string? siglaCurso { get; set; }
        public string? Sigla { get; set; }
        public int? Ano { get; set; }
        public virtual ICollection<int>? numeroAluno {get; set;}
    }
}
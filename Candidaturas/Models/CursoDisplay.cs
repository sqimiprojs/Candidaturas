using System;
using System.Collections.Generic;

namespace Candidaturas.Models
{
    public class CursoDisplay
    {
        public int prioridade { get; set; }
        public string nome { get; set; }
        public int ID { get; set; }
        public List<ExameObrigatorioDisplay> ExamesNecessarios { get; set; }
    }
}
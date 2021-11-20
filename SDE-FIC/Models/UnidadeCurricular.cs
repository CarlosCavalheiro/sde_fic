using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SDE_FIC.Models
{
    [Serializable]
    public class UnidadeCurricular
    {
        public long IdUnidadeCurricular { get; set; }
        public string Descricao { get; set; }
        public string Sigla { get; set; }
        public int CargaHoraria { get; set; }

        public virtual Curso Curso { get; set; }

    }
}
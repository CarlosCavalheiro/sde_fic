using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SDE_FIC.Models
{
    [Serializable]
    public class Aproveitamentos
    {

        /// <summary>
        /// Código de identificação da frequencia
        /// </summary>
        public long IdAproveitamento { get; set; }

        /// <summary>
        /// Define a Matricula que estão contidas na frequencia da Turma
        /// </summary>
        public virtual Matricula Matricula { get; set; }
  
        /// <summary>
        /// Define o Diario para o dia em que a frequencia esta sendo lançadas
        /// </summary>
        public virtual UnidadeCurricular UnidadeCurricular { get; set; }


    }
}
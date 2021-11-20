using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SDE_FIC.Models
{
    [Serializable]
    public class Notas
    {

        /// <summary>
        /// Código de identificação da frequencia
        /// </summary>
        public long IdNotas { get; set; }

        /// <summary>
        /// Hora/Aula do dia
        /// </summary>
        ///         
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:0.00}")]
        public decimal Nota { get; set; }

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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SDE_FIC.Models
{
    [Serializable]
    public class Frequencia
    {

        /// <summary>
        /// Código de identificação da frequencia
        /// </summary>
        public long IdFrequencia { get; set; }

        
        //private DateTime _Data = DateTime.Now;
        ///// <summary>
        ///// Data da frequencia
        ///// </summary>
        //[DisplayName("Data de Lançamento")]
        //[DataType(DataType.Date, ErrorMessage = "Data em formato inválido")]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-dd-MM}")]
        //public System.DateTime Data { get { return _Data; } set { _Data = value; } }

        /// <summary>
        /// Hora/Aula do dia
        /// </summary>
        ///         
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:0.0}")]
        public decimal HoraAula { get; set; }

        /// <summary>
        /// Define a situação de presença de cada Matricula
        /// </summary>
        [Required]
        [MaxLength(2)]
        public string Presenca { get; set; }

        /// <summary>
        /// Define a Matricula que estão contidas na frequencia da Turma
        /// </summary>
        public virtual Matricula Matricula { get; set; }
  
        /// <summary>
        /// Define o Diario para o dia em que a frequencia esta sendo lançadas
        /// </summary>
        public virtual Diario Diario { get; set; }


    }
}
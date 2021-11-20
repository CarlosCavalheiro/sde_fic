using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SDE_FIC.Models
{
    [Serializable]
    public class Aluno
    {
        //public Aluno()
        //{
        //    this.matriculas = new HashSet<Matricula>();
        //}
    /// <summary>
    /// Descreve o código do aluno
    /// </summary>
        public int IdAluno { get; set; }
        private DateTime _Data = DateTime.Now;
        /// <summary>
        /// Data do Cadastro
        /// </summary>
        [DisplayName("Data de Cadastro")]
        [DataType(DataType.Date, ErrorMessage = "Data em formato inválido")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime DataCadastro { get { return _Data; } set { _Data = value; } }
        [Required(ErrorMessage = "Informe o Nome do Aluno.")]
        public string Nome{ get; set; }
        public string CPF { get; set; }
        public string TelefoneResidencial { get; set; }       
        public string TelefoneCelular { get; set; }
        public string Email { get; set; }
        public string Observacao { get; set; }

        public virtual ICollection<Matricula> matriculas { get; set; }
    }
}
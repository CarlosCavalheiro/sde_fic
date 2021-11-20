using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SDE_FIC.Models
{
    [Serializable]
    public class Matricula
    {
        public Matricula()
        {
            this.Frequencias = new HashSet<Frequencia>();
            this.listaNotas = new List<Notas>();
            this.listaAproveitamentos = new List<Aproveitamentos>();
        }
         

        public long IdMatricula { get; set; }

        private DateTime _Data = DateTime.Now; 
        [DisplayName("Data de Matricula")]
        [DataType(DataType.Date, ErrorMessage = "Data em formato inválido")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime DataMatricula { get { return _Data; } set { _Data = value; } }   
    
        public string Situacao { get; set; }

        //[Display(Name = "Data Situação")]
        //[DataType(DataType.Date, ErrorMessage = "Data em formato inválido")]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        private DateTime _DataS = DateTime.Now; 
        [DisplayName("Data de Situação")]
        [DataType(DataType.Date, ErrorMessage = "Data em formato inválido")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime DataSituacao { get { return _DataS; } set { _DataS = value; } }

        public int IdAluno { get; set; }
        public int IdTurma { get; set; }

        public virtual Aluno Aluno { get; set; }                
        public virtual Turma Turma { get; set; }

        //Listar as Matriculas na View Parcial
        public virtual ICollection<Matricula> listaMatriculas { get; set; }
        public virtual ICollection<Frequencia> Frequencias { get; set; }
        public virtual List<Notas> listaNotas { get; set; }
        public virtual List<Aproveitamentos> listaAproveitamentos { get; set; }

    }
}
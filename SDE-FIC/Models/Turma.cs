using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SDE_FIC.Models
{
    [Serializable]
    public class Turma
    {
        
        public Turma()
        {
            this.Matriculas = new HashSet<Matricula>();
            this.Diario = new HashSet<Diario>();
   //         this.listaAproveitamentos = new List<Aproveitamentos>();
        }
    
        public int Idturma { get; set; }
        [Required(ErrorMessage = "Informe a Descrição para sua Turma, FC-XX-XXXX-XX.")]
        public string Descricao { get; set; }
        [Required(ErrorMessage = "Informe o Horário de Aula.")]
        public string Horario { get; set; }
        [DisplayName("Data de Inicio")]
        [DataType(DataType.Date, ErrorMessage = "Data em formato inválido.")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        [Required(ErrorMessage = "Informe a Data de Inicio para a Turma")]
        public Nullable<DateTime> DataInicio { get; set; }
        //[DisplayName("Data de Fim")]
        //[DataType(DataType.Date, ErrorMessage = "Data em formato inválido")]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]  
        [DisplayName("Data de Fim")]
        [DataType(DataType.Date, ErrorMessage = "Data em formato inválido.")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        [Required(ErrorMessage = "Informe a Data Final para a Turma.")]
        public Nullable<DateTime> DataFim { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:0.0}")]
        [Required(ErrorMessage = "Informe o número de Horas Aula no dia.")]
        public decimal HorasAula { get; set; }
        public bool Status { get; set; }
        public bool Domingo { get; set; }
        public bool Segunda { get; set; }
        public bool Terca { get; set; }
        public bool Quarta { get; set; }
        public bool Quinta { get; set; }
        public bool Sexta { get; set; }
        public bool Sabado { get; set; }
        [Required(ErrorMessage = "Informe o nome do Cliente.")]
        public string Cliente { get; set; }
        [Required(ErrorMessage = "Informe o Nome do Coordenador responsável.")]
        public string Responsavel { get; set; }

        public string PropostaAno { get; set; }
        [Required(ErrorMessage = "Informe o Local de Realização.")]
        public string LocalRealizacao { get; set; }
        [Required(ErrorMessage = "Informe o Tipo de Atendimento.")]
        public string Atendimento { get; set; }

        public Curso Curso { get; set; }
        public Funcionario Funcionario { get; set; }

        public virtual ICollection<Matricula> Matriculas { get ; set ; }
        public virtual List<Matricula> ListaMatriculas { get; set; }
        public virtual ICollection<Diario> Diario { get; set; }
 //       public virtual List<Aproveitamentos> listaAproveitamentos { get; set; }
    }
}
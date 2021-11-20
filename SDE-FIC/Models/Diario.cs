using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.Reflection;
using System.ComponentModel.DataAnnotations;

namespace SDE_FIC.Models
{
    /// <summary>
    /// Classe utilizada para lançamento de diário (diário de bordo da Turma)
    /// </summary>
    [Serializable]
    public class Diario
    {
        public Diario()
        {
            this.Frequencias = new List<Frequencia>();

        }

        /// <summary>
        /// Código do Diário
        /// </summary>
        public long IdDiario { get; set; }

        /// <summary>
        /// Define o conteudo aplicado no treinamento
        /// </summary>
        [DisplayName("Capacidade Técnica")]
        [MaxLength(300)]
        [Required(ErrorMessage = "Conteúdo deve ser digitado")]
        public string Conteudo { get; set; }

        /// <summary>
        /// Ocorrências extra - classe
        /// </summary>
        [DisplayName("Ocorrências")]
        [MaxLength(300)]
        public string Ocorrencia { get; set; }

        private DateTime _Data = DateTime.Now;         
        /// <summary>
        /// Data do treinamento
        /// </summary>
        [DisplayName("Data de Lançamento")]
        [DataType(DataType.Date, ErrorMessage = "Data em formato inválido")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        [Required(ErrorMessage = "A Data do dia de lançamento deve ser informada.")]
        public DateTime Data { get { return _Data; } set { _Data = value; } }

        [Required(ErrorMessage = "Informar a quantidade de Hora realizada na Aula.")]
        public decimal HoraAulaDia { get; set; }

        
        public int IdTurma { get; set; }

        [Required(ErrorMessage = "Informar uma Unidade Curricular.")]
        public int IdUnidadeCurricular { get; set; }

        /// <summary>
        /// Turma a qual se refere o treinamento
        /// </summary>
        public virtual Turma Turma { get; set; }

        /// <summary>
        /// Define a unidade curricular utilizada para realização do treinamento
        /// </summary>

        public virtual UnidadeCurricular UnidadeCurricular { get; set; }

        /// <summary>
        /// Resume as frequencias do diário
        /// </summary>
        public virtual List<Frequencia> Frequencias { get; set; }


        public IEnumerable<UnidadeCurricular> GetUnidadesCurriculares
        {
            get
            {
                return (IEnumerable<UnidadeCurricular>)this.Turma.Curso.UnidadesCurriculares;

            }
        }

    }
}
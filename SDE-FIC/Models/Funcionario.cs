using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SDE_FIC.Models
{
    [Serializable]
    public class Funcionario
    {
        //public Funcionario()
        //{
        //    this.Turmas = new HashSet<Turma>();
        //}

        [Required(ErrorMessage = "Defina o nome do Professor")]
        public int IdFuncionario { get; set; }

        //[Required(ErrorMessage = "Informe um Nome para o Funcionário")]
        public string NomeCompleto { get; set; }
        public string Email { get; set; }
        public string TelefoneCelular { get; set; }
        public string TelefoneResidencial { get; set; }
        public string Especialidade { get; set; }
        public string Observacao { get; set; }

        [Required(ErrorMessage = "Informe um usuário para o Funcionário")]
        public int IdUsuario { get; set; }       
        //Chave Estrangeira
        public virtual Usuario Usuario { get; set; }

        public virtual ICollection<Turma> Turmas { get; set; }

    }
}
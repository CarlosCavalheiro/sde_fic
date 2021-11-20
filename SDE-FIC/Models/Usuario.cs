using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SDE_FIC.Models
{
    [Serializable]
    public class Usuario
    {
        public int IdUsuario { get; set; }
        public string Perfil { get; set; }
        [Required(ErrorMessage = "Informe o Nome do Usuário.")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Informe uma Senha para o Usuário.")]      
        public string Password { get; set; }
        [Required(ErrorMessage = "Informe Nome Completo do Usuário.")]
        public string NomeCompleto { get; set; }

        public virtual ICollection<Funcionario> Funcionario { get; set; }
        public virtual ICollection<Sugestoes> Sugestoes { get; set; }
    }
}
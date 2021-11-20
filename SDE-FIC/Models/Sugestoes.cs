using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SDE_FIC.Models
{
    [Serializable]
    public class Sugestoes
    {
        public int IdSugestao { get; set; }
        public string Tipo { get; set; }
        [Required(ErrorMessage = "Informe um Titulo para a Sugestão")]
        public string Titulo { get; set; }
        [Required(ErrorMessage = "Informe uma Descrição para a Sugestão")]
        public string Descricao { get; set; }
        public string Status { get; set; }

        [Required(ErrorMessage = "Informe um usuário para o Funcionário")]
        public int IdUsuario { get; set; }
        //Chave Estrangeira
        public virtual Usuario Usuario { get; set; }

    }
}
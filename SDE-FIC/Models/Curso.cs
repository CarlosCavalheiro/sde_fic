using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SDE_FIC.Models
{
    public class Curso
    {
        public Curso()
        {
            this.Turmas = new HashSet<Turma>();
            this.Unidadecurricular = new HashSet<UnidadeCurricular>();
        }
        [Required(ErrorMessage = "Informe um Curso.")]
        public int IdCurso { get; set; }
        public string CursoNome { get; set; }
        public string Descricao { get; set; }
        public int CargaHoraria { get; set; }

        public virtual ICollection<Turma> Turmas { get; set; }
        public virtual ICollection<UnidadeCurricular> Unidadecurricular { get; set; }

        public IEnumerable<UnidadeCurricular> UnidadesCurriculares { get; set; }
        public List<UnidadeCurricular> lUnidadesCurriculares { get; set; }



    }
}
using SDE_FIC.Models;
using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace SDE_FIC.Util
{
    public class ContextoDB : DbContext 
    {
        public ContextoDB() { 
        
        }

        public DbSet<Aluno> Alunos { get; set; }
        public DbSet<Curso> Cursos { get; set; }
        public DbSet<Frequencia> Frequencias { get; set; }
        public DbSet<Matricula> Matriculas { get; set; }
        public DbSet<Funcionario> Funcionario { get; set; }
        public DbSet<Turma> Turmas { get; set; }
        public DbSet<UnidadeCurricular> Unidades_Curriculares { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Diario> Diarios { get; set; }
        public DbSet<Sugestoes> Sugestoes { get; set; }
    }
}
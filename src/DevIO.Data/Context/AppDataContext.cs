using DevIO.Business.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DevIO.Data.Context
{
    public class AppDataContext : DbContext
    {
        public AppDataContext(DbContextOptions options) : base(options) { }

        public DbSet<Produto> Produtos { get; set; }
        public DbSet<Endereco> Enderecos { get; set; }
        public DbSet<Fornecedor> Fornecedores { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            SetDefaultModelColumnsType(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDataContext).Assembly);
            SetDefaultBehaviorForeignKeys(modelBuilder);

            base.OnModelCreating(modelBuilder);
        }

        private void SetDefaultModelColumnsType(ModelBuilder modelBuilder)
        {
            // Todas colunas que são do tipo texto (string) terão como default o tamanho de varchar(100)
            IEnumerable<IMutableProperty> stringColumnsType = GetAllPropertiesByType(modelBuilder, typeof(string));

            foreach (var property in stringColumnsType) property.SetColumnType("varchar(100)");

            // Todas colunas que são do tipo decimal (valor numérico) terão como default o tamanho de decimal(18,2)
            IEnumerable<IMutableProperty> decimalColumnsType = GetAllPropertiesByType(modelBuilder, typeof(decimal));

            foreach (var property in decimalColumnsType) property.SetColumnType("decimal(18,2)");
        }

        private void SetDefaultBehaviorForeignKeys(ModelBuilder modelBuilder)
        {
            var foreignKeys = modelBuilder.Model.GetEntityTypes()
                .SelectMany(e => e.GetForeignKeys());

            foreach (var relationship in foreignKeys)
            {
                relationship.DeleteBehavior = DeleteBehavior.ClientSetNull;
            }
        }

        private IEnumerable<IMutableProperty> GetAllPropertiesByType(ModelBuilder modelBuilder, Type type)
        {
            return modelBuilder.Model.GetEntityTypes()
                .SelectMany(e => e.GetProperties().Where(p => p.ClrType == type));
        }
    }
}

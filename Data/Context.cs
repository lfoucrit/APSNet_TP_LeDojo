using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Data.Entity.ModelConfiguration.Conventions;
using BO;

namespace TP_LeDojo.Data
{
    public class Context : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, please use data migrations.
        // For more information refer to the documentation:
        // http://msdn.microsoft.com/en-us/data/jj591621.aspx
    
        public Context() : base("name=Context")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //Supprimer le pluriel sur le nom des tables en BDD
            //modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            //Ne pas stocker un champ dans la BDD
            //modelBuilder.Entity<Album>().Ignore(a => a.NombreDePistes);

            //OneToMany avec relation obligatoire (non nullable)
            //modelBuilder.Entity<Album>().HasMany(a => a.Pistes).WithRequired();

            //ManyToMany avec collection que d'un coté
            //modelBuilder.Entity<Album>().HasMany(a => a.Artistes).WithMany();

            //Un art martial peut être associé à zéro ou plusieurs samouraïs.
            modelBuilder.Entity<Samourai>().HasMany(s => s.ArtMartials).WithMany();

            //Potientiel pas dans la bdd => calcul
            modelBuilder.Entity<Samourai>().Ignore(s => s.Potentiel);

            base.OnModelCreating(modelBuilder);
        }

        public System.Data.Entity.DbSet<BO.Samourai> Samourais { get; set; }
        public System.Data.Entity.DbSet<BO.Arme> Armes { get; set; }
        public System.Data.Entity.DbSet<BO.ArtMartial> ArtMartials { get; set; }

    }
}

using System.Data.Entity;
using BeeApi.Entities;

namespace BeeApi.Contexts
{
    /// <summary>
    /// Represents a context for manipulating Bee App data.
    /// </summary>
    /// <seealso cref="System.Data.Entity.DbContext" />
    public class BeeAppContext : DbContext
    {
        // DbSets
        public DbSet<Beekeeper> Beekeepers { get; set; }
        public DbSet<Apiary> Apiaries { get; set; }
        public DbSet<Work> Works { get; set; }
        public DbSet<Hive> Hives { get; set; }
        public DbSet<Monitoring> Monitoring { get; set; }
        public DbSet<Queen> Queens { get; set; }
        public DbSet<Inspection> Inspections { get; set; }
        public DbSet<Harvest> Harvests { get; set; }
        public DbSet<Feeding> Feedings { get; set; }
        public DbSet<Treatment> Treatments { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BeeAppContext"/> class.
        /// </summary>
        public BeeAppContext()
            : base("BeeAppConnection")
        {
            Database.SetInitializer<BeeAppContext>(null);

            Configuration.ProxyCreationEnabled = false;
            Configuration.LazyLoadingEnabled = false;
        }

        /// <summary>
        /// Maps table names, and sets up relationships between the various user entities
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Works
            modelBuilder.Entity<Work>()
                .HasRequired(x => x.Beekeeper)
                .WithMany(x => x.Works)
                .Map(x => x.MapKey("BeekeeperId"));

            // Apiaries
            modelBuilder.Entity<Apiary>()
                .HasRequired(x => x.Beekeeper)
                .WithMany(x => x.Apiaries)
                .Map(x => x.MapKey("BeekeeperId"));

            // Hives
            modelBuilder.Entity<Hive>()
                .HasRequired(x => x.Apiary)
                .WithMany(x => x.Hives)
                .Map(x => x.MapKey("ApiaryId"));

            // Monitoring
            modelBuilder.Entity<Monitoring>()
                .HasRequired(x => x.Hive)
                .WithMany(x => x.Monitoring)
                .Map(x => x.MapKey("HiveId"));

            // Queens
            modelBuilder.Entity<Queen>()
                .HasRequired(x => x.Hive)
                .WithMany(x => x.Queens)
                .Map(x => x.MapKey("HiveId"));

            // Inspections
            modelBuilder.Entity<Inspection>()
                .HasRequired(x => x.Hive)
                .WithMany(x => x.Inspections)
                .Map(x => x.MapKey("HiveId"));

            // Harvest
            modelBuilder.Entity<Harvest>()
                .HasRequired(x => x.Hive)
                .WithMany(x => x.Harvests)
                .Map(x => x.MapKey("HiveId"));

            // Feeding
            modelBuilder.Entity<Feeding>()
                .HasRequired(x => x.Hive)
                .WithMany(x => x.Feedings)
                .Map(x => x.MapKey("HiveId"));

            // Treatment
            modelBuilder.Entity<Treatment>()
                .HasRequired(x => x.Hive)
                .WithMany(x => x.Treatments)
                .Map(x => x.MapKey("HiveId"));
        }

        /// <summary>
        /// Creates this instance.
        /// </summary>
        /// <returns></returns>
        public static BeeAppContext Create()
        {
            var context = new BeeAppContext();

            // Return
            return context;
        }
    }
}
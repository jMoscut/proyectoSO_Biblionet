

namespace DigitalRepository.Server.Context
{
    using Entities.Models;
    using Microsoft.EntityFrameworkCore;

    public class DataContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DataContext"/> class.
        /// </summary>
        public DataContext()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataContext"/> class.
        /// </summary>
        /// <param name="options">The options<see cref="DbContextOptions{DataContext}"/></param>
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        /// <summary>
        /// The OnConfiguring
        /// </summary>
        /// <param name="optionsBuilder">The optionsBuilder<see cref="DbContextOptionsBuilder"/></param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.ConfigureWarnings(warn => { warn.Default(WarningBehavior.Ignore); });

            if (!optionsBuilder.IsConfigured)
                optionsBuilder.UseNpgsql("Name=ConnectionStrings:DigitalRepository");
        }

        // Add DbSet for each entity

        /// <summary>
        /// Gets or sets the Users
        /// </summary>
        public DbSet<User> Users { get; set; }

        /// <summary>
        /// Gets or sets the Modules
        /// </summary>
        public DbSet<Module> Modules { get; set; }

        /// <summary>
        /// Gets or sets the Operations
        /// </summary>
        public DbSet<Operation> Operations { get; set; }

        /// <summary>
        /// Gets or sets the Roles
        /// </summary>
        public DbSet<Rol> Roles { get; set; }

        /// <summary>
        /// Gets or sets the RolOperations
        /// </summary>
        public DbSet<RolOperation> RolOperations { get; set; }
 

        /// <summary>
        /// Gets or sets the Documents
        /// </summary>
        public DbSet<Document> Documents { get; set; }

        /// <summary>
        /// Gets or sets the Downloads
        /// </summary>
        public DbSet<Download> Downloads { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Rol>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.Name)
                    .HasMaxLength(255);
                entity.Property(e => e.Description)
                    .HasMaxLength(255);
                entity.HasData(
                    new Rol
                    {
                        Id = 1,
                        Name = "SA",
                        Description = "Super Administrador",
                        State = 1,
                        CreatedAt = new DateTime(2025, 2, 17, 0, 0, 0, 0, DateTimeKind.Utc),
                        CreatedBy = 1,
                        UpdatedAt = null,
                        UpdatedBy = null
                    },
                    new Rol
                    {
                        Id = 2,
                        Name = "DG",
                        Description = "Cargador",
                        State = 1,
                        CreatedAt = new DateTime(2025, 2, 17, 0, 0, 0, 0, DateTimeKind.Utc),
                        CreatedBy = 1,
                        UpdatedAt = null,
                        UpdatedBy = null
                    },
                    new Rol
                    {
                        Id = 3,
                        Name = "CD",
                        Description = "Visualizador",
                        State = 1,
                        CreatedAt = new DateTime(2025, 2, 17, 0, 0, 0, 0, DateTimeKind.Utc),
                        CreatedBy = 1,
                        UpdatedAt = null,
                        UpdatedBy = null
                    }
                );
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.Name)
                    .HasMaxLength(255);
                entity.Property(e => e.Password)
                    .HasMaxLength(255);
                entity.Property(e => e.Number)
                    .HasMaxLength(255);
                entity.Property(e => e.Email)
                    .HasMaxLength(255);
                entity.Property(e => e.RecoveryToken)
                    .HasMaxLength(255);

                entity.HasOne(e => e.Rol)
                        .WithMany(e => e.Users)
                    .HasForeignKey(e => e.RolId);

                //password: Guatemala1.
                entity.HasData(
                    new User
                    {
                        Id = 1,
                        RolId = 1,
                        Password = "$2a$12$86Ty8oUVWKPbU8JqCII9VO.FgM1C10dweQ4xKhM4jj1LWL9jwNu7.",
                        Name = "Super Administrador",
                        Number = "12345678",
                        Email = "super@gmail.com",
                        RecoveryToken = "",
                        Reset = false,
                        State = 1,
                        CreatedAt = new DateTime(2025, 2, 17, 0, 0, 0, 0, DateTimeKind.Utc),
                        CreatedBy = 1,
                        DateToken = null,
                        UpdatedAt = null,
                        UpdatedBy = null
                    },
                    new User
                    {
                        Id = 2,
                        RolId = 2,
                        Password = "$2a$12$86Ty8oUVWKPbU8JqCII9VO.FgM1C10dweQ4xKhM4jj1LWL9jwNu7.",
                        Name = "Cargador",
                        Number = "12345678",
                        Email = "emerson@gmail.com",
                        RecoveryToken = "",
                        Reset = false,
                        State = 1,
                        CreatedAt = new DateTime(2025, 2, 17, 0, 0, 0, 0, DateTimeKind.Utc),
                        CreatedBy = 1,
                        DateToken = null,
                        UpdatedAt = null,
                        UpdatedBy = null
                    },
                    new User
                    {
                        Id = 3,
                        RolId = 3,
                        Password = "$2a$12$86Ty8oUVWKPbU8JqCII9VO.FgM1C10dweQ4xKhM4jj1LWL9jwNu7.",
                        Name = "Visualizador",
                        Number = "12345678",
                        Email = "jackie@gmail.com",
                        RecoveryToken = "",
                        Reset = false,
                        State = 1,
                        CreatedAt = new DateTime(2025, 2, 17, 0, 0, 0, 0, DateTimeKind.Utc),
                        CreatedBy = 1,
                        DateToken = null,
                        UpdatedAt = null,
                        UpdatedBy = null
                    }
                );
            });

            modelBuilder.Entity<Module>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.Name)
                    .HasMaxLength(255);
                entity.Property(e => e.Description)
                    .HasMaxLength(255);
                entity.Property(e => e.Image)
                    .HasMaxLength(255);
                entity.Property(e => e.Path)
                        .HasMaxLength(255);

                entity.HasData(
                    new Module
                    {
                        Id = 1,
                        Name = "Archivos",
                        Description = "Modulo de Archivos",
                        Image = "file",
                        Path = "Files",
                        State = 1,
                        CreatedAt = new DateTime(2025, 2, 17, 0, 0, 0, 0, DateTimeKind.Utc),
                        CreatedBy = 1,
                        UpdatedAt = null,
                        UpdatedBy = null
                    }
                );
            });

            modelBuilder.Entity<Operation>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.Name)
                    .HasMaxLength(255);
                entity.Property(e => e.Guid)
                   .HasMaxLength(255);
                entity.Property(e => e.Description)
                    .HasMaxLength(255);
                entity.Property(e => e.Policy)
                    .HasMaxLength(255);
                entity.Property(e => e.Icon)
                    .HasMaxLength(255);
                entity.Property(e => e.Path)
                    .HasMaxLength(255);
                entity.HasOne(e => e.Module)
                    .WithMany(e => e.Operations)
                    .HasForeignKey(e => e.ModuleId);

                entity.HasOne(e => e.Module)
                    .WithMany(e => e.Operations)
                    .HasForeignKey(e => e.ModuleId);

                entity.HasData(
                    new Operation
                    {
                        Id = 1,
                        Guid = "6451551a-b05c-455b-b1b9-97616e1c8892",
                        Name = "Listar Documentos",
                        Description = "Consultar Documentos",
                        Policy = "Files.List",
                        Icon = "list",
                        Path = "Files",
                        ModuleId = 1,
                        IsVisible = true,
                        State = 1,
                        CreatedAt = new DateTime(2025, 2, 17, 0, 0, 0, 0, DateTimeKind.Utc),
                        CreatedBy = 1,
                        UpdatedAt = null,
                        UpdatedBy = null
                    },
                    new Operation
                    {
                        Id = 2,
                        Guid = "2e26b4ca-bd5d-4c4f-a027-ba09f5bd448f",
                        Name = "Cargar Documento",
                        Description = "Creacion de documentos",
                        Policy = "Files.Create",
                        Icon = "plus",
                        Path = "Files/Create",
                        ModuleId = 1,
                        IsVisible = false,
                        State = 1,
                        CreatedAt = new DateTime(2025, 2, 17, 0, 0, 0, 0, DateTimeKind.Utc),
                        CreatedBy = 1,
                        UpdatedAt = null,
                        UpdatedBy = null
                    },
                    new Operation
                    {
                        Id = 3,
                        Guid = "3fd82baf-a73d-4809-8508-60dbec6119b0",
                        Name = "Descargar Documento",
                        Description = "Descarga de documentos",
                        Policy = "Files.Download",
                        Icon = "download",
                        Path = "Files/Download",
                        ModuleId = 1,
                        IsVisible = true,
                        State = 1,
                        CreatedAt = new DateTime(2025, 2, 17, 0, 0, 0, 0, DateTimeKind.Utc),
                        CreatedBy = 1,
                        UpdatedAt = null,
                        UpdatedBy = null
                    }
                );
            });

            modelBuilder.Entity<RolOperation>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.HasOne(e => e.Rol)
                    .WithMany(e => e.RolOperations)
                    .HasForeignKey(e => e.RolId);

                entity.HasOne(e => e.Operation)
                    .WithMany(e => e.RolOperations)
                    .HasForeignKey(e => e.OperationId);

                entity.HasData(
                    // Rol SA
                    new RolOperation
                    {
                        Id = 1,
                        RolId = 1,
                        OperationId = 1,
                        State = 1,
                        CreatedAt = new DateTime(2025, 2, 17, 0, 0, 0, 0, DateTimeKind.Utc),
                        CreatedBy = 1,
                        UpdatedAt = null,
                        UpdatedBy = null
                    },
                    new RolOperation
                    {
                        Id = 2,
                        RolId = 1,
                        OperationId = 2,
                        State = 1,
                        CreatedAt = new DateTime(2025, 2, 17, 0, 0, 0, 0, DateTimeKind.Utc),
                        CreatedBy = 1,
                        UpdatedAt = null,
                        UpdatedBy = null
                    },
                    new RolOperation
                    {
                        Id = 3,
                        RolId = 1,
                        OperationId = 3,
                        State = 1,
                        CreatedAt = new DateTime(2025, 2, 17, 0, 0, 0, 0, DateTimeKind.Utc),
                        CreatedBy = 1,
                        UpdatedAt = null,
                        UpdatedBy = null
                    },
                    new RolOperation
                    {
                        Id = 4,
                        RolId = 2,
                        OperationId = 1,
                        State = 1,
                        CreatedAt = new DateTime(2025, 2, 17, 0, 0, 0, 0, DateTimeKind.Utc),
                        CreatedBy = 1,
                        UpdatedAt = null,
                        UpdatedBy = null
                    },
                    new RolOperation
                    {
                        Id = 5,
                        RolId = 2,
                        OperationId = 2,
                        State = 1,
                        CreatedAt = new DateTime(2025, 2, 17, 0, 0, 0, 0, DateTimeKind.Utc),
                        CreatedBy = 1,
                        UpdatedAt = null,
                        UpdatedBy = null
                    },
                    new RolOperation
                    {
                        Id = 6,
                        RolId = 3,
                        OperationId = 1,
                        State = 1,
                        CreatedAt = new DateTime(2025, 2, 17, 0, 0, 0, 0, DateTimeKind.Utc),
                        CreatedBy = 1,
                        UpdatedAt = null,
                        UpdatedBy = null
                    },
                    new RolOperation
                    {
                        Id = 7,
                        RolId = 3,
                        OperationId = 3,
                        State = 1,
                        CreatedAt = new DateTime(2025, 2, 17, 0, 0, 0, 0, DateTimeKind.Utc),
                        CreatedBy = 1,
                        UpdatedAt = null,
                        UpdatedBy = null
                    }
                );
            });

            modelBuilder.Entity<Document>(e =>
            {
                e.HasKey(e => e.Id);
                e.Property(e => e.Id).ValueGeneratedOnAdd();
                e.Property(e => e.DocumentNumber)
                    .HasMaxLength(255);
                e.Property(e => e.Path)
                        .HasMaxLength(255);
                e.Property(e => e.UserIp)
                        .HasMaxLength(255);
                e.Property(e => e.Author)
                    .HasMaxLength(255);

                e.HasOne(e => e.User)
                        .WithMany(e => e.Documents)
                    .HasForeignKey(e => e.UserId);
            });

            modelBuilder.Entity<Download>(e =>
            {
                e.HasKey(e => e.Id);
                e.Property(e => e.Id).ValueGeneratedOnAdd();
                e.Property(e => e.UserIp)
                    .HasMaxLength(255);
                e.HasOne(e => e.Document)
                    .WithMany(e => e.Downloads)
                    .HasForeignKey(e => e.DocumentId);
            });
        }
    }
}

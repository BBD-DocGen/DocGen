using DocGen.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace DocGen.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<User> User { get; set; }
        
        public DbSet<DocumentType> DocumentType { get; set; }
        
        public DbSet<UploadDocument> UploadDocument { get; set; }
        
        public DbSet<GeneratedDocument> GeneratedDocument { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UploadDocument>()
                .HasMany(uploadedDocument => uploadedDocument.GeneratedDocuments)
                .WithOne(generatedDocument => generatedDocument.UploadDocument)
                .HasForeignKey(generatedDocument => generatedDocument.UpDocID);
            
        }


    }
}
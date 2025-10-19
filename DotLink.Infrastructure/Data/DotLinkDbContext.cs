using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.EntityFrameworkCore;
using DotLink.Domain.Entities;

namespace DotLink.Infrastructure.Data
{
    public class DotLinkDbContext : DbContext
    {
        public DotLinkDbContext(DbContextOptions<DotLinkDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<PostVote> Votes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasMany(u => u.Posts)
                      .WithOne(p => p.Author)
                      .HasForeignKey(p => p.AuthorId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(u => u.Comments)
                      .WithOne(c => c.Author)
                      .HasForeignKey(c => c.AuthorId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(u => u.Votes)
                      .WithOne(v => v.User)
                      .HasForeignKey(v => v.UserId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasIndex(u => u.Email).IsUnique();
                entity.Property(u => u.Username).HasMaxLength(50).IsRequired();
            });

            modelBuilder.Entity<Post>(entity =>
            {
                entity.HasMany(p => p.Votes)
                      .WithOne(v => v.Post)
                      .HasForeignKey(v => v.PostId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(p => p.Comments)
                      .WithOne(c => c.Post)
                      .HasForeignKey(c => c.PostId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.Property(p => p.Title).HasMaxLength(250).IsRequired();
            });

            modelBuilder.Entity<PostVote>(entity =>
            {
                entity.HasKey(v => new { v.PostId, v.UserId });
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}

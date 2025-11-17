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
        public DbSet<PostVote> PostVotes { get; set; }
        public DbSet<UserFollow> UserFollows { get; set; }

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

                entity.HasMany(u => u.Followers)
                      .WithOne(f => f.Followee)
                      .HasForeignKey(f => f.FolloweeId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(u => u.Following)
                      .WithOne(f => f.Follower)
                      .HasForeignKey(f => f.FollowerId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Post>(entity =>
            {
                entity.HasMany(p => p.PostVotes)
                      .WithOne(v => v.Post)
                      .HasForeignKey(v => v.PostId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(p => p.Comments)
                      .WithOne(c => c.Post)
                      .HasForeignKey(c => c.PostId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.Property(p => p.Title).HasMaxLength(250).IsRequired();
            });

            modelBuilder.Entity<Comment>(entity =>
            {
                entity.HasOne(c => c.ParentComment)
                      .WithMany(c => c.Replies)
                      .HasForeignKey(c => c.ParentCommentId)
                      .IsRequired(false)
                      .OnDelete(DeleteBehavior.Restrict);
                                                          
            });

            modelBuilder.Entity<PostVote>(entity =>
            {
                entity.HasKey(v => new { v.PostId, v.UserId });
            });

            modelBuilder.Entity<UserFollow>(entity =>
            {
                entity.HasKey(uf => new { uf.FollowerId, uf.FolloweeId });

                entity.HasOne(uf => uf.Follower)
                      .WithMany(u => u.Following)
                      .HasForeignKey(uf => uf.FollowerId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(uf => uf.Followee)
                      .WithMany(u => u.Followers)
                      .HasForeignKey(uf => uf.FolloweeId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}

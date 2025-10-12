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
    }
}

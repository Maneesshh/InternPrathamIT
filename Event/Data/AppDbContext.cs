using System.Collections.Generic;
using System.Net;
using System;
using Microsoft.EntityFrameworkCore;
using Event.Models;

namespace Event.Data
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        public DbSet<Eventss> Events { get; set; }

    }
}

﻿using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using FactoryProject.Models;

namespace FactoryProject.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options)
        {
        
        }

        public DbSet<Users> Users { get; set; }
        public DbSet<Employees> Employees { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }
    }
}
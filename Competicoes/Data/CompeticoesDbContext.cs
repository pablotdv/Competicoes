﻿using Competicoes.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Competicoes.Data
{
    public class CompeticoesDbContext : DbContext
    {
        public CompeticoesDbContext(DbContextOptions<CompeticoesDbContext> options)
            :base(options)
        {

        }

        public DbSet<Pais> Paises { get; set; }
    }
}
using Entities;
using Entities.Utils;
using Microsoft.EntityFrameworkCore;
using System;

namespace Data.Migrations
{
    internal static class DbInitializer
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            try
            {

                GenericUtil genericUtil = new GenericUtil();
                DateTime currentDate = genericUtil.GetDateZone();

                modelBuilder.Entity<Profile>().HasData(
                  new Profile { Id = 1, Description = "Administrator", CreatedDate = currentDate, UpdatedDate = currentDate },
                  new Profile { Id = 2, Description = "User", CreatedDate = currentDate, UpdatedDate = currentDate },
                  new Profile { Id = 3, Description = "PlatformAdministrator", CreatedDate = currentDate, UpdatedDate = currentDate }
                );

                modelBuilder.Entity<TransactionType>().HasData(
                 new TransactionType { Id = 1, Description = "Consignación Inicial", CreatedDate = currentDate, UpdatedDate = currentDate },
                 new TransactionType { Id = 2, Description = "Inversión", CreatedDate = currentDate, UpdatedDate = currentDate },
                 new TransactionType { Id = 3, Description = "Consignación ", CreatedDate = currentDate, UpdatedDate = currentDate }
               );

                modelBuilder.Entity<Dependency>().HasData(
                 new TransactionType { Id = 1, Description = "Sin dependencia", CreatedDate = currentDate, UpdatedDate = currentDate }
               );

                modelBuilder.Entity<ProjectType>().HasData(
                new TransactionType { Id = 1, Description = "Davivienda", CreatedDate = currentDate, UpdatedDate = currentDate }
              );


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
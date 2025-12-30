using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogisticsCore.Infrastructure.Persistence
{
    public class LogisticsDbContextFactory : IDesignTimeDbContextFactory<LogisticsDbContext>
    {
        public LogisticsDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<LogisticsDbContext>();

            // COPIA AQUÍ TU CONNECTION STRING DEL APPSETTINGS.JSON
            // Asegúrate de que tenga tu usuario y contraseña correctos.
            var connectionString = "Server=DESKTOP-8LS8I0A;Database=LogisticsDb;User Id=sa;Password=**nrcm**;TrustServerCertificate=True;";

            optionsBuilder.UseSqlServer(connectionString);

            return new LogisticsDbContext(optionsBuilder.Options);
        }
    }
}

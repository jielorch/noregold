using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Noregold.Entities
{
    public class ExecuteSql
    {
        public static bool Scripts(MigrationBuilder migrationBuilder)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceNames = assembly.GetManifestResourceNames()
                                .Where(str => str.EndsWith(".sql"));
            string sql = "";

            foreach (string resourceName in resourceNames)
            {
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
                using Stream stream = assembly.GetManifestResourceStream(resourceName);
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
#pragma warning disable CS8604 // Possible null reference argument.
                using StreamReader reader = new StreamReader(stream);
#pragma warning restore CS8604 // Possible null reference argument.
                sql += reader.ReadToEnd();
            }
            migrationBuilder.Sql(sql);
            return true;
        }
    }
}

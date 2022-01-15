using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using CsvHelper;
using GeospatialLocation.Domain.Entities;

namespace GeospatialLocation.API.ExampleData
{
    public static class ExampleDataReader
    {
        public static ICollection<Location> Read()
        {
            var currentDirectory = Directory.GetCurrentDirectory();
            var path = Path.Combine(currentDirectory, "ExampleData\\locations.csv");

            using var reader = new StreamReader(path);
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
            ICollection<Location> records = csv.GetRecords<Location>().ToList();

            return records;
        }
    }
}
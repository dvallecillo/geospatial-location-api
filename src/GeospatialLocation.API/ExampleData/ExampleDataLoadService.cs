using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CsvHelper;
using GeospatialLocation.Application.Commands;
using GeospatialLocation.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Hosting;

namespace GeospatialLocation.API.ExampleData
{
    public class ExampleDataLoadService : BackgroundService
    {
        private readonly IMediator _bus;

        public ExampleDataLoadService(IMediator bus)
        {
            _bus = bus;
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            try
            {
                var currentDirectory = Directory.GetCurrentDirectory();
                var path = Path.Combine(currentDirectory, "ExampleData\\locations.csv");

                using var reader = new StreamReader(path);
                using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
                ICollection<Location> records = csv.GetRecords<Location>().ToList();
                var command = new CreateLocationInitialLoadCommand(records);

                await _bus.Send(command);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
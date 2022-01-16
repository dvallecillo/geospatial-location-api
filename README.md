# Geospatial Location API

This is an Open API project that allows clients to query locations with the constraints of maximum distance and results. The order of these results will be ascending by distance.

## Problem

The problem of having a huge number of locations that you want to display on a map, is that you have to create a way where the speed of fetching the data is as fast as possible. A direct call to the DB without any optimizations regarding the storage of those locations would be a bad idea.

## Solution

The key idea of this solution is to group nearby points that are geographically close enough to each other into one object, called cluster, that contains combined information. The idea came from my reading on R-Tree data structure, which will do something similar to store nearby objects.

Each of these clusters will contain a center latitude/longitude, where we can calculate distances against. This way, the amount of distance calculations to be performed are pruned drastically.

I found out that with a 5 KM distance to each of the clusters border the amount of clusters generated is just around 6000 with the provided CSV file which is a huge optimization from the >150,000 locations provided in a file. Probably more tweaking is needed to find the sweet spot.

![alt text](https://github.com/dvallecillo/geospatial-location-api/blob/main/location-search.JPG?raw=true)

The image above depicts a graphical simplification of how the data will be stored and how a query would look.

To know which of these clusters are reachable, a square is created with the provided Offset calculation functionality. It's a simplified approach that for this MVP should suffice in accuracy. With the search box square diagonal + cluster diagonal I can quickly filter the amount of clusters that might have potential hits for the query.

In this case Redis has been used as a DB for the solution, as I saw it a perfect fit. In the next iterations of the system, I would probably use a relational DB to store the locations and Redis as a cache/hash table in memory. With the architectural approach taken, any DB technology would be easy to implement in the solution.

## Studied Approaches

Inside this repo you may find a branch `feature/using-geospatial-index` completely functional, where I implemented the solution using [Redis's Geospatial Indexes](https://redis.io/commands/geodist). This approach would have been the fastest to develop and probably in query speed, but I considered that the scope of this exercise was to see my problem-solving skills/approach and the main solution shows it best.

I've also worked with [SQL Server Spatial Data](https://docs.microsoft.com/es-es/sql/relational-databases/spatial/spatial-data-sql-server?view=sql-server-ver15) in the past and It would also have been an easy implementation to provide, but again without much logic in the code.

## Remarks / Improvements

- In the `CreateLocationInitialLoadCommandHandler` a check against Redis should be added for repeated address-lat-lon values.
- With more time I'd improve the way of calculating the distance to the corner of the searchbox. I found out that the bigger the distance of 2 points the bigger the deviation error of the current way.
- A great idea for next iterations would be to have different layers/resolutions of clusters so you don't have to search too many boxes. Maybe It could be useful to index each point on several different grids (e.g. resolutions of 1Km, 5Km, 25Km, 125Km etc).
- There might be some percentage of error in the calculations for the cluster diagonal, so I added a 10% deviation to be on the safe side and for the simplicity.
- I´d also add benchmarks in a next iteration, to study any potential improvement.
- docker-compose.yml and Dockerfile are not included as I think they are out of scope of this exercise.
- These kind of endpoints (GetLocations) should be implemented with pagination in mind (take/skip/total), specially if end-users are going to be the consumers.
- The provided CalculateDistance method was doing unnecessary calculations so it has been simplified:

            var rlat1 = Math.PI * origin.Latitude / 180;
            var rlat2 = Math.PI * currentLocation.Latitude / 180;
            var rlon1 = Math.PI * origin.Longitude / 180;           //never used
            var rlon2 = Math.PI * currentLocation.Longitude / 180;  //never used
            var theta = origin.Longitude - currentLocation.Longitude;
            var rtheta = Math.PI * theta / 180;
            var dist = Math.Sin(rlat1) * Math.Sin(rlat2) +
                       Math.Cos(rlat1) * Math.Cos(rlat2) * Math.Cos(rtheta);
            dist = Math.Acos(dist);

            dist = dist * 180 / Math.PI;    //These 3 operations could be done in a single one and store it as a constant
            dist = dist * 60 * 1.1515;
            return dist * 1609.344;

## Architecture

This project follows the clean architecture idea, which is easy to extend and test. The project is structured as follows:

#### API

The entry point of the application. It includes its configuration system, which uses the default appsettings.json file and is configured in Startup.cs. The project delegates to the Infrastructure project to wire up its services using Autofac.

#### Domain

This layer contains all entities, models, interfaces, types and logic specific to the domain layer.

#### Application

This layer contains all application logic. It is dependent on the domain layer, but has no dependencies on any other layer or project. This layer defines interfaces that are implemented by outside layers. For example, if the application needs to access a database, a new interface would be added to application and an implementation would be created within infrastructure.

#### Infrastructure

This layer contains classes for accessing external resources such as databases. These classes are based on interfaces defined within the application layer.

## Tests

- `API Integration`, are performed against the real database, with an HttpClient created with TestServer, is mandatory to have the Redis DB created, and with the CSV data inside.
- `Controllers` endpoints.
- `Command/Query handlers` where the main logic is implemented.
- `Helpers` where most of the mathematical calculations are performed.

## Getting Started

You need to have [Docker](https://www.docker.com/get-started) installed and run the following commands in the cmd/powershell.

1.  `docker create --name redisDev -p 6385:6379 redis`
2.  `docker start redisDev`
3.  `docker run -it --link redisDev:redis --rm redis redis-cli -h redis -p 6379`

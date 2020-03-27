using GreenDonut;
using StarWars.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace StarWars.Loaders
{
    interface IPlanetLoader : IDataLoader<string, Planet> { }
    public class PlanetLoader : DataLoaderBase<string, Planet>, IPlanetLoader
    {
        private readonly StarWarsData data;

        public PlanetLoader(StarWarsData data)
        {
            this.data = data;
        }

        protected async override Task<IReadOnlyList<Result<Planet>>> FetchAsync(IReadOnlyList<string> keys, CancellationToken cancellationToken)
        {
            var planetTasks = keys.Select(async _ =>
                Result<Planet>.Resolve(await data.GetPlanetByNameAsync(_))
            );
            await Task.Delay(1000);
            var planets = await Task.WhenAll(planetTasks);

            return planets.ToList().AsReadOnly();

        }
    }
}

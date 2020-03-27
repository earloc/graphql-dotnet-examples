using GreenDonut;
using StarWars.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace StarWars.Loaders
{
    interface IHumanLoader : IDataLoader<string, Human> { }
    public class HumanLoader : DataLoaderBase<string, Human>, IHumanLoader
    {
        private readonly StarWarsData data;

        public HumanLoader(StarWarsData data)
        {
            this.data = data;
        }

        protected async override Task<IReadOnlyList<Result<Human>>> FetchAsync(IReadOnlyList<string> keys, CancellationToken cancellationToken)
        {
            var tasks = keys.Select(async _ =>
                Result<Human>.Resolve(await data.GetHumanByIdAsync(_))
            );

            var values = await Task.WhenAll(tasks);

            return values.ToList().AsReadOnly();

        }
    }
}

using GraphQL.Types;
using StarWars.Loaders;
using System.Threading;

namespace StarWars.Types
{
    public class PlanetType : ObjectGraphType<Planet>
    {
        public PlanetType(StarWarsData data, IScoped<IHumanLoader> humanLoader)
        {
            Name = "Planet";
            Description = "A planet in the Star Wars universe.";

            Field(d => d.Id).Description("The id of the planet.");
            Field(d => d.Name, nullable: true).Description("The name of the dplanetroid.");

            FieldAsync<HumanType>(
                "mostFamousSith",
                resolve: async context =>
                    await humanLoader.Instance.LoadAsync(context.Source.MostFamousSith, CancellationToken.None)
            );
            FieldAsync<HumanType>(
                "mostFamousJedi",
                resolve: async context =>
                    await humanLoader.Instance.LoadAsync(context.Source.MostFamousJedi, CancellationToken.None)
            );
        }
    }
}

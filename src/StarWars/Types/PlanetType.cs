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
            FieldAsync<BooleanGraphType>(
                "IsDangerous",
                resolve: async context =>
                {
                    var jedi = default(Human);
                    if (context.Source.MostFamousJedi != null)
                        jedi = await humanLoader.Instance.LoadAsync(context.Source.MostFamousJedi, CancellationToken.None);

                    var sith = default(Human);
                    if (context.Source.MostFamousSith != null)
                        sith = await humanLoader.Instance.LoadAsync(context.Source.MostFamousSith, CancellationToken.None);

                    if (sith is null)
                        return false;
                    if (jedi is null)
                        return true;
                    return false;
                }

            );
        }
    }
}

using GraphQL.DataLoader;
using GraphQL.Types;

namespace StarWars.Types
{
    public class DroidType : ObjectGraphType<Droid>
    {
        public DroidType(StarWarsData data, IDataLoaderContextAccessor loaderContext)
        {
            Name = "Droid";
            Description = "A mechanical creature in the Star Wars universe.";

            Field(d => d.Id).Description("The id of the droid.");
            Field(d => d.Name, nullable: true).Description("The name of the droid.");

            Field<ListGraphType<CharacterInterface>>(
                "friends",
                resolve: context => data.GetFriends(context.Source)
            );
            Field<ListGraphType<EpisodeEnum>>("appearsIn", "Which movie they appear in.");
            Field(d => d.PrimaryFunction, nullable: true).Description("The primary function of the droid.");

            Interface<CharacterInterface>();

            FieldAsync<PlanetType>(
                "manufacturedOn",
                resolve: async context =>
                    await loaderContext.Context.GetOrAddLoader($"planet_{context.Source.ManufacturdOn}", () =>
                        data.GetPlanetByNameAsync(context.Source.ManufacturdOn)
                    ).LoadAsync()
            );
        }
    }
}

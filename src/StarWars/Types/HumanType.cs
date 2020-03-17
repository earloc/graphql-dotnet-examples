using GraphQL.DataLoader;
using GraphQL.Types;

namespace StarWars.Types
{
    public class HumanType : ObjectGraphType<Human>
    {
        public HumanType(StarWarsData data, IDataLoaderContextAccessor loaderContext)
        {
            Name = "Human";

            Field(h => h.Id).Description("The id of the human.");
            Field(h => h.Name, nullable: true).Description("The name of the human.");

            Field<ListGraphType<CharacterInterface>>(
                "friends",
                resolve: context => data.GetFriends(context.Source)
            );
            Field<ListGraphType<EpisodeEnum>>("appearsIn", "Which movie they appear in.");

            FieldAsync<PlanetType>(
                "homePlanet",
                description: "The home planet of the human.",
                resolve: async context =>
                    await loaderContext.Context.GetOrAddLoader($"planet_{context.Source.HomePlanet}", () =>
                        data.GetPlanetByNameAsync(context.Source.HomePlanet)
                    ).LoadAsync()
            );

            Interface<CharacterInterface>();
        }
    }
}

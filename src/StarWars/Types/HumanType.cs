using GraphQL.DataLoader;
using GraphQL.Types;
using StarWars.Loaders;
using System.Threading;

namespace StarWars.Types
{
    public class HumanType : ObjectGraphType<Human>
    {
        public HumanType(StarWarsData data, IScoped<IPlanetLoader> planetLoader)
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
                        await planetLoader.Instance.LoadAsync(context.Source.HomePlanet, CancellationToken.None)
            );

            Interface<CharacterInterface>();
        }
    }
}

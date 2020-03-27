using GraphQL.DataLoader;
using GraphQL.Types;
using StarWars.Loaders;
using System.Threading;

namespace StarWars.Types
{
    public class DroidType : ObjectGraphType<Droid>
    {
        public DroidType(StarWarsData data, IScoped<IPlanetLoader> planetLoader)
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
                        await planetLoader.Instance.LoadAsync(context.Source.ManufacturdOn, CancellationToken.None)
            );
        }
    }
}

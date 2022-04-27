using Data;

namespace ConsoleApp.Interfaces
{
    public interface IDataSeeder
    {
        Context Context { get; }

        void SeedGenres();

        void SeedSpectacles();

        void SeedPeople();

        void SeedMovies();

        void SeedActorsOnSpectacles();

        void SeedActorsOnMovies();

        void SeedData();

        void SeedPersonOfProfession();

        void SeedTheatricalCharacter();

        void SeedActorTheatricalCharacters();
    }
}

using Data;
using System;

namespace Business
{
    public class Queries
    {
        private readonly Context _context;

        public Queries(Context context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context), "Context cannot be null");
        }

        /* TODO: write queries:
         * 1) Get all actors. Sort by fullname, then - year of birth
         * 2) Get all films starting from $year. Sort by year, then by name
         * 3) Get all films and spectacles where actor with $id starred in one IEnumerable. Sort by: name
         * 4) Get all actors joined with their roles, then with films/spectacles from roles are. Should be saved:
         * actor info, name of the role, is it role a main one, name of the spectacle/movie, type (spectacle/movie)
         * sort by: for actors: fullname, year of birth; for roles: main or not, then name
         * 5) Get the spectacle by $id joined with actors on roles. Should be saved:
         * spectacle info, is role main or not, actor's fullname
         * sort by: type of the role
         * 6) Get movies grouped by genres. Sort by name of the genre, then by name of the movie
         * 7) Get top-N actors, sorted by quantity of main roles both in movies and speactacles
         * 8) Find all actors that fullname contains $name
         * 9) Get the joined spectacles with roles and actors, grouped by genre, sorted by genre name, spectacle name (with GroupJoin)
         * 10) Get all actors that were directors at least in one movie. Sort by fullname, then - year of birth
         * 11) Get all actors that starred in at least one movie or spectagle with genre $id. Sort by fullname, then - year of birth
         * 12) Get all films by director whose fullname contains $name. Sort by year
         * 13) Find all films and spectacles by name
         * 14) Get genres with quantity of movies and spectacles of them. Sort by quantity of movies, then - spectacles
         * 15) Get all genres that were used at least in one movie or spectacle (with Distinct)
        */
    }
}

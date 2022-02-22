using Data;
using Data.Comparers;
using Data.Interfaces;
using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;

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
         * 2) Get all films starting from $year. Sort by year descending, then by name ascending
         * 3) Get all films and spectacles where actor with $actorId starred in one IEnumerable. Sort by: name
         * 4) Get all actors joined with their roles, then with films/spectacles from roles are.
         * sort by: for actors: fullname, year of birth; for roles: main or not, then name
         * 5) Get actors on roles by the $spectacleId. Should be saved:
         * spectacle info, is role main or not, actor's full name
         * sort by: type of the role
         * 6) Get movies grouped by genres. Sort by name of the genre
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

        /// <summary>
        /// 1) Get all actors. Sort by full name, then - year of birth
        /// </summary>
        /// <returns>IEnumerable of Actors, sorted by fullname, then - year of birth</returns>
        public IEnumerable<Actor> GetActors()
        {
            return _context.People
                .Where(p => p is Actor)
                .Select(p => p as Actor);
        }

        /// <summary>
        /// 2) Get all films starting from $year. Sort by year descending, then by name ascending
        /// </summary>
        /// <param name="year">Start from</param>
        /// <returns>IEnumerable of Movies starting from $year. Sort by year descending, then by name ascending</returns>
        public IEnumerable<Movie> GetMoviesFromYear(ushort year)
        {
            return from movie in _context.Movies
                   where movie.Year >= year
                   orderby movie.Year descending, movie.Name
                   select movie;
        }

        /// <summary>
        /// 3) Get all films and spectacles where actor with $actorId starred in one IEnumerable. Sort by: name
        /// </summary>
        /// <param name="actorId"></param>
        /// <returns>IEnumerable of IPerformance with films and spectacles where actor with $actorId starred, sorted by name</returns>
        public IEnumerable<IPerformance> GetActorPerformances(int actorId)
        {
            var movies = _context.Movies
                .Where(m => _context.ActorsOnMovies
                    .Any(aom => aom.MovieId == m.Id && aom.ActorId == actorId))
                .Select(m => m as IPerformance);
            var spectacles = _context.Spectacles
                .Where(s => _context.ActorsOnSpectacles
                    .Any(aos => aos.SpectacleId == s.Id && aos.ActorId == actorId))
                .Select(s => s as IPerformance);
            return movies.Concat(spectacles)
                .OrderBy(p => p.Name);
        }

        /// <summary>
        /// 4) Get all actors joined with their roles, then with films/spectacles from roles are.
        /// </summary>
        /// <returns>IEnumerable of Grouping with the key - actor and value - tuple of roles and performances</returns>
        public IEnumerable<IGrouping<Actor, (Actor Actor, string Role, bool IsMainRole, IPerformance Performance)>> GetActorsWithFilmography()
        {
            var performances = _context.ActorsOnMovies
                .Join(_context.Movies, 
                    a => a.MovieId, m => m.Id, 
                    (a, m) => new { a.ActorId, a.Role, a.IsMainRole, Performance = m as IPerformance })
                .Concat(_context.ActorsOnSpectacles
                    .Join(_context.Spectacles,
                        a => a.SpectacleId,
                        s => s.Id,
                        (a, s) => new { a.ActorId, a.Role, a.IsMainRole, Performance = s as IPerformance }));
            var actors = _context.People
                .Where(p => p is Actor)
                .Select(p => p as Actor)
                .Join(performances,
                    a => a.Id,
                    p => p.ActorId,
                    (a, p) => new { Actor = a, p.Role, p.IsMainRole, p.Performance })
                .Select(p => (p.Actor, p.Role, p.IsMainRole, p.Performance))
                .OrderByDescending(t => t.IsMainRole)
                .ThenBy(t => t.Role)
                .GroupBy(t => t.Actor, new ActorEqualityComparer())
                .OrderBy(g => (g.Key as IPerson).GetFullName())
                .ThenBy(g => g.Key.BirthYear);
            return actors;
        }

        /// <summary>
        /// 5) Get actors on roles by the $spectacleId. sort by: type of the role
        /// </summary>
        /// <param name="spectacleId"></param>
        /// <returns>IEnumerable of tuple with actor, role and bool role status</returns>
        public IEnumerable<(Actor Actor, string Role, bool IsMainRole)> GetSpectacleCast(int spectacleId)
        {
            var actors = from a in _context.People
                         where a is Actor
                         select a as Actor;
            return from aos in _context.ActorsOnSpectacles
                   where aos.SpectacleId == spectacleId
                   join actor in actors
                   on aos.ActorId equals actor.Id
                   orderby aos.IsMainRole descending
                   select (actor, aos.Role, aos.IsMainRole);
        }

        /// <summary>
        /// 6) Get movies grouped by genres. Sort by name of the genre
        /// </summary>
        /// <returns></returns>
        public IEnumerable<IGrouping<Genre, Movie>> GetMoviesGroupedByGenres()
        {
            return from movie in _context.Movies
                   join genre in _context.Genres
                       on movie.GenreId equals genre.Id
                   group movie by genre into movieGroup
                   orderby movieGroup.Key
                   select movieGroup;
        }

        /// <summary>
        /// 7) Get top-N actors, sorted by quantity of main roles both in movies and speactacles
        /// </summary>
        /// <param name="quantity">needed quantity (top N)</param>
        /// <returns></returns>
        public IEnumerable<(Actor, int mainRolesQuantity)> GetTopMainRolesPopularActors(int quantity)
        {
            var actors = from a in _context.People
                         where a is Actor
                         select a as Actor;
            var spectaclesMainRoles = from a in actors
                                              join aos in _context.ActorsOnSpectacles
                                                on a.Id equals aos.Id into j
                                              from subaos in j.DefaultIfEmpty(null)
                                              select new
                                              {
                                                  Actor = a,
                                                  RoleInSpectacle = subaos
                                              };
            var spectaclesMainRolesQuantity = from a in spectaclesMainRoles
                                              group spectaclesMainRoles by a.Actor.Id into spectaclesGroup
                                              select new
                                              {
                                                  Actor = spectaclesGroup.Key,
                                                  SpectaclesQuantity = (spectaclesGroup.Count() == 1 && spectaclesGroup.First() is null)
                                                  ? 0 : spectaclesGroup.Count()
                                              };
            throw new NotImplementedException();
        }
    }
}

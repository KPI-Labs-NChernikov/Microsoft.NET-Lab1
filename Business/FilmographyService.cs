using Business.TempModels;
using Data;
using Data.Comparers;
using Data.Interfaces;
using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Business
{
    public class FilmographyService
    {
        private readonly Context _context;

        public FilmographyService(Context context)
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
         * 11) Get all actors that starred in at least one movie or spectacle with genre $id. Sort by fullname, then - year of birth
         * 12) Get all films by director whose fullname contains $name. Sort by year
         * 13) Find all films and spectacles by name
         * 14) Get genres with quantity of movies and spectacles of them. Sort by quantity of movies, then - spectacles
         * 15) Get all genres that were used at least in one movie (with Distinct)
        */

        /// <summary>
        /// 1) Get all actors. Sort by full name, then - year of birth
        /// </summary>
        /// <returns>IEnumerable of Actors, sorted by fullname, then - year of birth</returns>
        public IEnumerable<Actor> GetActors()
        {
            return _context.People
                .Where(p => p is Actor)
                .Select(p => p as Actor)
                .OrderBy(a => a.GetFullName())
                .ThenByDescending(a => a.BirthYear);
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
        /// <returns>IEnumerable of ActorWithFilmography, that contains Actors' names and filmography for each of them</returns>
        public IEnumerable<ActorWithFilmography> GetActorsWithFilmography()
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
                .GroupBy(t => t.Actor, 
                (k, v) => new ActorWithFilmography()
                {
                    Actor = k,
                    Filmography = v.Select(t => (t.Role, t.IsMainRole, t.Performance))
                }, new ActorEqualityComparer())
                .OrderBy(g => g.Actor.GetFullName())
                .ThenBy(g => g.Actor.BirthYear);
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
        /// <returns>IEnumerable of groups with genres and movies</returns>
        public IEnumerable<IGrouping<Genre, Movie>> GetMoviesGroupedByGenres()
        {
            return from movie in _context.Movies
                   join genre in _context.Genres
                       on movie.GenreId equals genre.Id
                   group movie by genre into movieGroup
                   orderby movieGroup.Key.Id
                   select movieGroup;
        }

        /// <summary>
        /// 7) Get top-N actors, sorted by quantity of main roles both in movies and speactacles.
        /// </summary>
        /// <param name="quantity">needed quantity (top N)</param>
        /// <returns>IEnumerable of tuple with actors and quantity of theire main roles</returns>
        public IEnumerable<(Actor Actor, int mainRolesQuantity)> GetTopMainRolesPopularActors(int quantity)
        {
            var actors = from a in _context.People
                         where a is Actor
                         select a as Actor;

            var filteredSpectacleRoles = from aos in _context.ActorsOnSpectacles
                                         where aos.IsMainRole
                                         select aos;
            var spectaclesMainRoles = from a in actors
                                              join aos in filteredSpectacleRoles
                                                on a.Id equals aos.ActorId into j
                                              from subaos in j.DefaultIfEmpty(null)
                                              select new
                                              {
                                                  Actor = a,
                                                  RoleInSpectacle = subaos
                                              };
            var spectaclesMainRolesQuantity = from a in spectaclesMainRoles
                                              group a by a.Actor into spectaclesGroup
                                              select new
                                              {
                                                  Actor = spectaclesGroup.Key,
                                                  SpectaclesMainRolesQuantity = (spectaclesGroup.Count() == 1 && spectaclesGroup.First().RoleInSpectacle is null)
                                                  ? 0 : spectaclesGroup.Count()
                                              };

            var filteredMovieRoles = from aom in _context.ActorsOnMovies
                                         where aom.IsMainRole
                                         select aom;
            var moviesMainRoles = from a in actors
                                      join aom in filteredMovieRoles
                                        on a.Id equals aom.ActorId into j
                                      from subaom in j.DefaultIfEmpty(null)
                                      select new
                                      {
                                          Actor = a,
                                          RoleInMovie = subaom
                                      };
            var moviesMainRolesQuantity = from a in moviesMainRoles
                                          group a by a.Actor into moviesGroup
                                              select new
                                              {
                                                  Actor = moviesGroup.Key,
                                                  MoviesMainRolesQuantity = (moviesGroup.Count() == 1 && moviesGroup.First().RoleInMovie is null)
                                                  ? 0 : moviesGroup.Count()
                                              };

            var mainRolesQuantity = from spec in spectaclesMainRolesQuantity
                                    join mov in moviesMainRolesQuantity
                                        on spec.Actor.Id equals mov.Actor.Id
                                    select (spec.Actor, spec.SpectaclesMainRolesQuantity + mov.MoviesMainRolesQuantity);
            return (from actorMainRolesQuantity in mainRolesQuantity
                   orderby actorMainRolesQuantity.Item2 descending
                   select actorMainRolesQuantity).Take(quantity);
        }

        /// <summary>
        /// 8) Find all actors that fullname contains $name
        /// </summary>
        /// <param name="fullName"></param>
        /// <returns></returns>
        public IEnumerable<Actor> FindActorByName(string name)
        {
            var actors = _context.People
                .Where(p => p is Actor)
                .Select(a => a as Actor);
            return actors
                .Where(a => a.GetFullName().ToLower().Contains(name.ToLower()));
        }
    }
}

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
        /// 4) Get all actors joined with their roles, then with films/spectacles.
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
        public IEnumerable<(Actor Actor, int MainRolesQuantity)> GetTopMainRolesPopularActors(int quantity)
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

        /// <summary>
        /// 9) Get genres that were used both in movies and spectacles
        /// </summary>
        /// <returns>IEnumerable of Genre with genres that were used both in movies and spectacles</returns>
        public IEnumerable<Genre> GetUniversalGenres()
        {
            var comparer = new GenreEqualityComparer();
            var movieGenres = _context.Movies
                .Select(m => m.GenreId)
                .Join(_context.Genres,
                id => id,
                g => g.Id,
                (id, g) => new Genre()
                {
                    Id = id,
                    Name = g.Name
                })
                .Distinct(comparer);

            var spectaclesGenres = _context.Spectacles
                .Select(s => s.GenreId)
                .Join(_context.Genres,
                id => id,
                g => g.Id,
                (id, g) => new Genre()
                {
                    Id = id,
                    Name = g.Name
                })
                .Distinct(comparer);

            return movieGenres
                .Intersect(spectaclesGenres, comparer);
        }

        /// <summary>
        /// 10) Get all actors that were directors at least in one movie. Sort by year of birth
        /// </summary>
        /// <returns>IEnumerable of Actor that contains actors that were directors too, sorted by year of birth</returns>
        public IEnumerable<Actor> GetActorsDirectors()
        {
            var actors = _context.People
                .Where(p => p is Actor)
                .Select(a => a as Actor);
            return actors
                .Where(a => _context.Movies
                    .Any(m => m.DirectorId == a.Id))
                .OrderBy(a => a.BirthYear);
        }

        /// <summary>
        /// 11) Get all actors that starred in at least one movie or spectacle with genre $genreId. Sort by fullname, then - year of birth
        /// </summary>
        /// <returns>IEnumerable of Actor that contains actors that starred in at least one movie or spectacle with genre $genreId.</returns>
        public IEnumerable<Actor> GetActorsByGenre(int genreId)
        {
            var allActors = from actor in _context.People
                         where actor is Actor
                         select actor as Actor;
            var spectacleActorsIds = from sp in _context.Spectacles
                             where sp.GenreId == genreId
                             join aos in _context.ActorsOnSpectacles
                             on sp.Id equals aos.SpectacleId
                             select aos.ActorId;
            var movieActorsIds = from mov in _context.Movies
                           where mov.GenreId == genreId
                           join aom in _context.ActorsOnMovies
                             on mov.Id equals aom.MovieId
                           select aom.ActorId;
            var actorsIds = spectacleActorsIds
                .Union(movieActorsIds);
            return from actorId in actorsIds
                   join actor in allActors
                   on actorId equals actor.Id
                   orderby actor.GetFullName(), actor.BirthYear
                   select actor;
        }

        /// <summary>
        /// 12) Find all films by director whose full name contains $name. Sort by film year descending
        /// </summary>
        /// <param name="name"></param>
        /// <returns>IEnumerable of Tuple of Movie and Person (its' director) that contains all films 
        /// by director whose fullname contains $name,
        /// sorted by film year descending</returns>
        public IEnumerable<(Movie Movie, Person Director)> FindMoviesByDirectorName(string name)
        {
            return from mov in _context.Movies
                   join director in _context.People
                   on mov.DirectorId equals director.Id
                   where director.GetFullName().ToLower()
                        .Contains(name.ToLower())
                   orderby mov.Year descending
                   select (mov, director);
        }

        /// <summary>
        /// 13) Find all films and spectacles by name. Group by type - spectacle or movie
        /// </summary>
        /// <param name="name"></param>
        /// <returns>IEnumerable of Grouping of Type and its' performaances</returns>
        public IEnumerable<IGrouping<Type, IPerformance>> FindPerformancesByName(string name)
        {
            return _context.Movies
                .Select(m => m as IPerformance)
                .Concat(_context.Spectacles
                    .Select(s => s as IPerformance))
                .Where(p => p.Name.ToLower().Contains(name.ToLower()))
                .GroupBy(p => p.GetType())
                .OrderBy(g => g.Key.Name);
        }

        /// <summary>
        /// 14) Get genres with quantity of movies and spectacles of them. 
        /// Sort by quantity of movies desc., then - spectacles desc.
        /// </summary>
        /// <returns>IEnumerable of GenreStats</returns>
        public IEnumerable<GenreStats> GetGenresStats()
        {
            var spectacles = from genre in _context.Genres
                                      join sp in _context.Spectacles
                                        on genre.Id equals sp.GenreId into j
                                      from subsp in j.DefaultIfEmpty(null)
                                      select new
                                      {
                                          Genre = genre,
                                          Spectacle = subsp
                                      };
            var spectaclesStats = from s in spectacles
                                              group s by s.Genre into spectaclesGroup
                                              select new
                                              {
                                                  Genre = spectaclesGroup.Key,
                                                  SpectaclesQuantity = (spectaclesGroup.Count() == 1 && spectaclesGroup.First().Spectacle is null)
                                                  ? 0 : spectaclesGroup.Count()
                                              };

            var movies = from genre in _context.Genres
                             join mov in _context.Movies
                               on genre.Id equals mov.GenreId into j
                             from submov in j.DefaultIfEmpty(null)
                             select new
                             {
                                 Genre = genre,
                                 Movie = submov
                             };
            var moviesStats = from movie in movies
                                  group movie by movie.Genre into moviesGroup
                                  select new
                                  {
                                      Genre = moviesGroup.Key,
                                      MoviesQuantity = (moviesGroup.Count() == 1 && moviesGroup.First().Movie is null)
                                      ? 0 : moviesGroup.Count()
                                  };

            var stats = from sp in spectaclesStats
                        join mov in moviesStats
                            on sp.Genre.Id equals mov.Genre.Id
                        orderby mov.MoviesQuantity descending, sp.SpectaclesQuantity descending
                        select new GenreStats()
                        {
                            Genre = sp.Genre.Name,
                            SpectaclesQuantity = sp.SpectaclesQuantity,
                            MoviesQuantity = mov.MoviesQuantity
                        };
            return stats;
        }

        /// <summary>
        /// 15) Find spectacles of genre that name starts with $nameStart
        /// </summary>
        /// <param name="nameStart"></param>
        /// <returns>IEnumerable of SpectacleExtended that contains spectacles of genre (with genre object)
        /// that name starts with $nameStart</returns>
        public IEnumerable<SpectacleExtended> FindSpectaclesByGenreNameStart(string nameStart)
        {
            return from spec in _context.Spectacles
                   join genre in _context.Genres
                   on spec.GenreId equals genre.Id
                   where genre.Name.ToLower().StartsWith(nameStart.ToLower())
                   select new SpectacleExtended()
                   {
                       Id = spec.Id,
                       Name = spec.Name,
                       Genre = genre
                   };
        }
    }
}

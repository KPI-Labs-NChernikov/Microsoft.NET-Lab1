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
                .Where(p => _context.PersonOnProfessions
                    .Any(pop => pop.PersonId == p.Id && pop.Profession == Profession.Actor))
                .Select(p => new Actor()
                {
                    Id = p.Id,
                    FirstName = p.FirstName,
                    LastName = p.LastName,
                    BirthYear = p.BirthYear,
                    Patronymic = p.Patronymic,
                    TheatricalCharacters = new TheatricalCharactersContainer()
                    { 
                        Value = _context.ActorTheatricalCharacters
                        .Where(a => a.ActorId == p.Id)
                        .Join(_context.TheatricalCharacters,
                            a => a.TheatricalCharacterId,
                            t => t.Id,
                        (a, t) => t)
                    }
                })
                .OrderBy(a => a.FullName)
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
        /// <returns>IEnumerable of IPerformance with films and spectacles where actor with $actorId starred, 
        /// sorted by name</returns>
        public IEnumerable<IPerformance> GetActorPerformances(int actorId)
        {
            return (_context.Movies
                    .Where(m => _context.ActorsOnMovies
                    .Any(aom => aom.MovieId == m.Id && aom.ActorId == actorId))
                    .Select(m => m as IPerformance))
                .Concat(_context.Spectacles
                    .Where(s => _context.ActorsOnSpectacles
                        .Any(aos => aos.SpectacleId == s.Id && aos.ActorId == actorId))
                    .Select(s => s as IPerformance))
                .OrderBy(p => p.Name);
        }

        /// <summary>
        /// 4) Get all actors joined with their roles, then with films/spectacles.
        /// </summary>
        /// <returns>IEnumerable of ActorWithFilmography, that contains Actors' names 
        /// and filmography for each of them</returns>
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
            return _context.People
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
                    Filmography = v.Select(t => new FilmographyItem 
                    { 
                        IsMainRole = t.IsMainRole,
                        Role = t.Role,
                        Performance = t.Performance
                    })
                }, new PersonEqualityComparer())
                .OrderBy(g => g.Actor.FullName)
                .ThenBy(g => g.Actor.BirthYear);
        }

        /// <summary>
        /// 5) Get actors on roles by the $spectacleId. sort by: type of the role
        /// </summary>
        /// <param name="spectacleId"></param>
        /// <returns>IEnumerable of tuple with actor, role and bool role status</returns>
        public IEnumerable<ActorOnPerformance> GetSpectacleCast(int spectacleId)
        {
            return from aos in _context.ActorsOnSpectacles
                   where aos.SpectacleId == spectacleId
                   join actor in _context.People
                   on aos.ActorId equals actor.Id
                   orderby aos.IsMainRole descending
                   select new ActorOnPerformance 
                   { 
                       Actor = actor, 
                       Role = aos.Role, 
                       IsMainRole = aos.IsMainRole
                   };
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
        public IEnumerable<ActorStats> GetTopMainRolesPopularActors(int quantity)
        {
            var actors = from a in _context.People
                         join p in _context.PersonOnProfessions on a.Id equals p.PersonId
                         where p.Profession == Profession.Actor
                         select a;

            var aops = (from a in _context.ActorsOnSpectacles
                        select (IActorOnPerformance) a)
                .Concat(from a in _context.ActorsOnMovies
                        select (IActorOnPerformance)a);
            var mainRoles = from a in actors
                              join aop in from aop in aops
                                           where aop.IsMainRole
                                           select aop
                              on a.Id equals aop.ActorId into j
                              from subaos in j.DefaultIfEmpty(null)
                              group subaos by a into grouped
                              select new
                              {
                                  Actor = grouped.Key,
                                  MainRolesQuantity = grouped.Count(t => t != null)
                              };

            var top =  (from actorMainRolesQuantity in mainRoles
                    orderby actorMainRolesQuantity.MainRolesQuantity descending
                    select actorMainRolesQuantity).Take(quantity);
            var result = from a in top
                    join atc in _context.ActorTheatricalCharacters on a.Actor.Id equals atc.ActorId into atcJoined
                    from subatc in atcJoined.DefaultIfEmpty(null)
                    join tc in _context.TheatricalCharacters on subatc?.TheatricalCharacterId equals tc.Id into tcJoined
                    from subtc in tcJoined.DefaultIfEmpty(null)
                    select new
                    {
                         a,
                         subtc
                    } into joined
                    group joined by joined.a into grouped
                    let actor = grouped.Key.Actor
                    select new ActorStats
                    {
                        Actor = new Actor()
                        {
                            Id = actor.Id,
                            FirstName = actor.FirstName,
                            LastName = actor.LastName,
                            BirthYear = actor.BirthYear,
                            Patronymic = actor.Patronymic,
                            TheatricalCharacters = new TheatricalCharactersContainer
                            {
                                Value = (from g in grouped
                                         select g.subtc)
                            }
                        },
                        MainRolesQuantity = grouped.Key.MainRolesQuantity
                    };
            return result;
        }

        /// <summary>
        /// 8) Find all actors that fullname contains $name
        /// </summary>
        /// <param name="fullName"></param>
        /// <returns></returns>
        public IEnumerable<Actor> FindActorByName(string name)
        {
            return _context.People
                .Where(p => _context.PersonOnProfessions
                    .Any(pop => pop.PersonId == p.Id && pop.Profession == Profession.Actor)
                    && p.FullName.ToLower().Contains(name.ToLower()))
                .Select(p => new Actor()
                {
                    Id = p.Id,
                    FirstName = p.FirstName,
                    LastName = p.LastName,
                    BirthYear = p.BirthYear,
                    Patronymic = p.Patronymic,
                    TheatricalCharacters = new TheatricalCharactersContainer()
                    {
                        Value = _context.ActorTheatricalCharacters
                        .Where(a => a.ActorId == p.Id)
                        .Join(_context.TheatricalCharacters,
                            a => a.TheatricalCharacterId,
                            t => t.Id,
                        (a, t) => t)
                    }
                });
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
        /// 10) Get all actors that are also directors. Sort by year of birth
        /// </summary>
        /// <returns>IEnumerable of Actor that contains actors that were directors too, sorted by year of birth</returns>
        public IEnumerable<Actor> GetActorsDirectors()
        {
            return _context.People
                .Where(p => _context.PersonOnProfessions
                    .Any(pop => pop.PersonId == p.Id && pop.Profession == Profession.Actor)
                    && _context.PersonOnProfessions
                    .Any(pop => pop.PersonId == p.Id && pop.Profession == Profession.Director))
                .OrderBy(a => a.BirthYear)
                .Select(p => new Actor()
                {
                    Id = p.Id,
                    FirstName = p.FirstName,
                    LastName = p.LastName,
                    BirthYear = p.BirthYear,
                    Patronymic = p.Patronymic,
                    TheatricalCharacters = new TheatricalCharactersContainer()
                    { 
                        Value = _context.ActorTheatricalCharacters
                        .Where(a => a.ActorId == p.Id)
                        .Join(_context.TheatricalCharacters,
                            a => a.TheatricalCharacterId,
                            t => t.Id,
                        (a, t) => t)
                    }
                });
        }

        /// <summary>
        /// 11) Get all actors that starred in at least one movie or spectacle with genre $genreId. 
        /// Sort by fullname, then - year of birth
        /// </summary>
        /// <returns>IEnumerable of Actor that contains actors that starred in at least one movie 
        /// or spectacle with genre $genreId.</returns>
        public IEnumerable<Actor> GetActorsByGenre(int genreId)
        {
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
                   join actor in _context.People
                   on actorId equals actor.Id
                   orderby actor.FullName, actor.BirthYear
                   join atc in _context.ActorTheatricalCharacters on actor.Id equals atc.ActorId into atcJoined
                   from subatc in atcJoined.DefaultIfEmpty(null)
                   join tc in _context.TheatricalCharacters on subatc?.TheatricalCharacterId equals tc.Id into tcJoined
                   from subtc in tcJoined.DefaultIfEmpty(null)
                   select new
                   {
                       actor,
                       subtc
                   } into joined
                   group joined by joined.actor into grouped
                   select new Actor
                   {
                         Id = grouped.Key.Id,
                         FirstName = grouped.Key.FirstName,
                         LastName = grouped.Key.LastName,
                         BirthYear = grouped.Key.BirthYear,
                         Patronymic = grouped.Key.Patronymic,
                         TheatricalCharacters = new TheatricalCharactersContainer
                         {
                             Value = (from g in grouped
                                      select g.subtc)
                         }
                   };
        }

        /// <summary>
        /// 12) Find all films by director whose full name contains $name. Sort by film year descending
        /// </summary>
        /// <param name="name"></param>
        /// <returns>IEnumerable of Tuple of Movie and Person (its' director) that contains all films 
        /// by director whose fullname contains $name,
        /// sorted by film year descending</returns>
        public IEnumerable<MovieWithDirector> FindMoviesByDirectorName(string name)
        {
            return from mov in _context.Movies
                   join director in _context.People
                   on mov.DirectorId equals director.Id
                   where director.FullName.ToLower()
                        .Contains(name.ToLower())
                   orderby mov.Year descending
                   select new MovieWithDirector
                   {
                       Movie = mov,
                       Director = director
                   };
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
            var spectaclesStats = from genre in _context.Genres
                             join sp in _context.Spectacles
                               on genre.Id equals sp.GenreId into j
                             from subsp in j.DefaultIfEmpty(null)
                             group subsp by genre into grouped
                             select new
                             {
                                 Genre = grouped.Key,
                                 SpectaclesQuantity = grouped.Count(t => t != null)
                             };

            var moviesStats = from genre in _context.Genres
                             join mov in _context.Movies
                               on genre.Id equals mov.GenreId into j
                             from submov in j.DefaultIfEmpty(null)
                              group submov by genre into grouped
                              select new
                              {
                                  Genre = grouped.Key,
                                  MoviesQuantity = grouped.Count(t => t != null)
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

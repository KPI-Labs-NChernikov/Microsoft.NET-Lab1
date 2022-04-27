using ConsoleApp.Interfaces;
using Data;
using Data.Models;
using System;
using System.Collections.Generic;

namespace ConsoleApp.Data
{
    public class DataSeeder : IDataSeeder
    {
        public Context Context { get; }

        public DataSeeder(Context context)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context), "Context cannot be null");
        }

        public void SeedGenres()
        {
            var genres = new List<Genre>()
            {
                new Genre()
                {
                    Name = "Action"
                },
                new Genre()
                {
                    Name = "Romantic"
                },
                new Genre()
                {
                    Name = "Fantasy"
                },
                new Genre()
                {
                    Name = "Drama"
                },
                new Genre()
                {
                    Name = "Science fiction"
                },
                new Genre()
                {
                    Name = "Dramedy"
                },
                new Genre()
                {
                    Name = "Fairy tale"
                },
                new Genre()
                {
                    Name = "Western"
                },
                new Genre()
                {
                    Name = "Comedy"
                }
            };
            for (int i = 0; i < genres.Count; i++)
            {
                genres[i].Id = i + 1;
                Context.Genres.Add(genres[i]);
            }
        }

        public void SeedSpectacles()
        {
            var spectacles = new List<Spectacle>()
            {
                new Spectacle()
                {
                    Name = "Mowgli",
                    GenreId = 7
                },
                new Spectacle()
                {
                    Name = "Caidashi",
                    GenreId = 6
                },
                new Spectacle()
                {
                    Name = "The Master and Margarita",
                    GenreId = 4
                }
            };
            for (int i = 0; i < spectacles.Count; i++)
            {
                spectacles[i].Id = i + 1;
                Context.Spectacles.Add(spectacles[i]);
            }
        }

        public void SeedPeople()
        {
            var people = new List<Person>()
            {
                new Person() //1
                {
                    FirstName = "Sandra",
                    LastName = "Anderson",
                    Patronymic = "Louise",
                    BirthYear = 1944
                },
                new Person() //2
                {
                    FirstName = "Clint",
                    LastName = "Eastwood",
                    BirthYear = 1930
                },
                new Person()  //3
                {
                    FirstName = "Yuriy",
                    LastName = "Chaika",
                    Patronymic = "Viktorovych",
                    BirthYear = 1943
                },
                new Person()  //4
                {
                    FirstName = "Ihor",
                    LastName = "Bilyts",
                    BirthYear = 1980
                },
                new Person() //5
                {
                    FirstName = "Ella",
                    LastName = "Sanko",
                    Patronymic = "Ivanivna",
                    BirthYear = 1947
                },
                new Person() //6
                {
                    FirstName = "Bohdan",
                    LastName = "Stupka",
                    Patronymic = "Sylvestrovych",
                    BirthYear = 1941
                },
                new Person()  //7
                {
                    FirstName = "Iryna",
                    LastName = "Molostova",
                    Patronymic = "Oleksandrivna",
                    BirthYear = 1929
                },
                new Person() //8
                {
                    FirstName = "William",
                    LastName = "Pitt",
                    Patronymic = "Bradley",
                    BirthYear = 1963
                },
                new Person() //9
                {
                    FirstName = "Leonardo",
                    LastName = "DiCaprio",
                    Patronymic = "Wilhelm",
                    BirthYear = 1974
                },
                new Person() //10
                {
                    FirstName = "Orlando",
                    LastName = "Bloom",
                    BirthYear = 1977
                },
                new Person() //11
                {
                    FirstName = "Johnny",
                    LastName = "Depp",
                    BirthYear = 1963
                },
                new Person()  //12
                {
                    FirstName = "Gregor",
                    LastName = "Verbinski",
                    Patronymic = "Justin",
                    BirthYear = 1963
                },
                new Person()  //13
                {
                    FirstName = "Peter",
                    LastName = "Jackson",
                    Patronymic = "Robert",
                    BirthYear = 1961
                },
                new Person()  //14
                {
                    FirstName = "Martin",
                    LastName = "Scorsese",
                    Patronymic = "Charles",
                    BirthYear = 1942
                },
                new Person()  //15
                {
                    FirstName = "James",
                    LastName = "Cameron",
                    Patronymic = "Francis",
                    BirthYear = 1954
                },
                new Person()  //16
                {
                    FirstName = "Yuri",
                    LastName = "Illienko",
                    Patronymic = "Herasymovych",
                    BirthYear = 1936
                },
                new Person()  //17
                {
                    FirstName = "Sergio",
                    LastName = "Leone",
                    BirthYear = 1929
                },
                new Person() //18
                {
                    FirstName = "Volodymyr",
                    LastName = "Velyanyk",
                    Patronymic = "Volodymyrovych",
                    BirthYear = 1970
                }
            };
            for (int i = 0; i < people.Count; i++)
            {
                people[i].Id = i + 1;
                Context.People.Add(people[i]);
            }
        }

        public void SeedMovies()
        {
            var movies = new List<Movie>()
            {
                new Movie() //1
                {
                    Name = "The White Bird Marked with Black",
                    Year = 1971,
                    DirectorId = 16,
                    GenreId = 4
                },
                new Movie() //2
                {
                    Name = "Bronco Billy",
                    Year = 1980,
                    DirectorId = 2,
                    GenreId = 8
                },
                new Movie() //3
                {
                    Name = "Titanic",
                    Year = 1997,
                    DirectorId = 15,
                    GenreId = 2
                },
                new Movie() //4
                {
                    Name = "The Wolf of Wall Street",
                    Year = 2013,
                    DirectorId = 14,
                    GenreId = 4
                },
                new Movie() //5
                {
                    Name = "Dollars Trilogy",
                    Year = 1966,
                    DirectorId = 17,
                    GenreId = 8
                },
                new Movie() //6
                {
                    Name = "The Lord of the Rings: The Fellowship of the Ring",
                    Year = 2001,
                    DirectorId = 13,
                    GenreId = 5
                },
                new Movie() //7
                {
                    Name = "Pirates of the Caribbean: The Curse of the Black Pearl",
                    Year = 2003,
                    DirectorId = 12,
                    GenreId = 1
                }
            };
            for (int i = 0; i < movies.Count; i++)
            {
                movies[i].Id = i + 1;
                Context.Movies.Add(movies[i]);
            }
        }

        public void SeedActorsOnSpectacles()
        {
            var aos = new List<ActorOnSpectacle>()
            {
                new ActorOnSpectacle()
                {
                    SpectacleId = 1,
                    ActorId = 5,
                    Role = "Raksha",
                    IsMainRole = true,
                },
                new ActorOnSpectacle()
                {
                    SpectacleId = 2,
                    ActorId = 5,
                    Role = "Paraska",
                    IsMainRole = false,
                },
                new ActorOnSpectacle()
                {
                    SpectacleId = 2,
                    ActorId = 18,
                    Role = "Omelko",
                    IsMainRole = true,
                },
                new ActorOnSpectacle()
                {
                    SpectacleId = 3,
                    ActorId = 6,
                    Role = "Jeshua",
                    IsMainRole = true
                }
            };
            for (int i = 0; i < aos.Count; i++)
            {
                aos[i].Id = i + 1;
                Context.ActorsOnSpectacles.Add(aos[i]);
            }
        }

        public void SeedActorsOnMovies()
        {
            var aom = new List<ActorOnMovie>()
            {
                new ActorOnMovie()
                {
                    MovieId = 1,
                    ActorId = 6,
                    Role = "Orest",
                    IsMainRole = true,
                },
                new ActorOnMovie()
                {
                    MovieId = 2,
                    ActorId = 2,
                    Role = "Billy \"Bronco Billy\" McCoy",
                    IsMainRole = true
                },
                new ActorOnMovie()
                {
                    MovieId = 2,
                    ActorId = 1,
                    Role = "Antoinette Lily",
                    IsMainRole = false
                },
                new ActorOnMovie()
                {
                    MovieId = 3,
                    ActorId = 9,
                    Role = "Jack Dawson",
                    IsMainRole = true
                },
                new ActorOnMovie()
                {
                    MovieId = 4,
                    ActorId = 9,
                    Role = "Jordan Belfort",
                    IsMainRole = true
                },
                new ActorOnMovie()
                {
                    MovieId = 5,
                    ActorId = 2,
                    Role = "Man with No Name",
                    IsMainRole = true
                },
                new ActorOnMovie()
                {
                    MovieId = 6,
                    ActorId = 10,
                    Role = "Legolas Greenleaf",
                    IsMainRole = true
                },
                new ActorOnMovie()
                {
                    MovieId = 7,
                    ActorId = 10,
                    Role = "Will Turner",
                    IsMainRole = false
                },
                new ActorOnMovie()
                {
                    MovieId = 7,
                    ActorId = 11,
                    Role = "Jack Sparrow",
                    IsMainRole = true
                }
            };
            for (int i = 0; i < aom.Count; i++)
            {
                aom[i].Id = i + 1;
                Context.ActorsOnMovies.Add(aom[i]);
            }
        }


        public void SeedData()
        {
            SeedGenres();
            SeedSpectacles();
            SeedPeople();
            SeedPersonOfProfession();
            SeedTheatricalCharacter();
            SeedActorTheatricalCharacters();
            SeedMovies();
            SeedActorsOnSpectacles();
            SeedActorsOnMovies();
        }

        public void SeedPersonOfProfession()
        {
            var pops = new List<PersonOnProfession>()
            {
                new PersonOnProfession
                {
                    Profession = Profession.Actor,
                    PersonId = 1
                },
                new PersonOnProfession
                {
                    Profession = Profession.Actor,
                    PersonId = 2
                },
                new PersonOnProfession
                {
                    Profession = Profession.Actor,
                    PersonId = 5
                },
                new PersonOnProfession
                {
                    Profession = Profession.Actor,
                    PersonId = 6
                },
                new PersonOnProfession
                {
                    Profession = Profession.Actor,
                    PersonId = 8
                },
                new PersonOnProfession
                {
                    Profession = Profession.Actor,
                    PersonId = 9
                },
                new PersonOnProfession
                {
                    Profession = Profession.Actor,
                    PersonId = 10
                },
                new PersonOnProfession
                {
                    Profession = Profession.Actor,
                    PersonId = 11
                },
                new PersonOnProfession
                {
                    Profession = Profession.Actor,
                    PersonId = 18
                },
                new PersonOnProfession
                {
                    Profession = Profession.Director,
                    PersonId = 2
                },
                new PersonOnProfession
                {
                    Profession = Profession.Director,
                    PersonId = 3
                },
                new PersonOnProfession
                {
                    Profession = Profession.Director,
                    PersonId = 4
                },
                new PersonOnProfession
                {
                    Profession = Profession.Director,
                    PersonId = 7
                },
                new PersonOnProfession
                {
                    Profession = Profession.Director,
                    PersonId = 12
                },
                new PersonOnProfession
                {
                    Profession = Profession.Director,
                    PersonId = 13
                },
                new PersonOnProfession
                {
                    Profession = Profession.Director,
                    PersonId = 14
                },
                new PersonOnProfession
                {
                    Profession = Profession.Director,
                    PersonId = 15
                },
                new PersonOnProfession
                {
                    Profession = Profession.Director,
                    PersonId = 16
                },
                new PersonOnProfession
                {
                    Profession = Profession.Director,
                    PersonId = 17
                },
            };
            foreach (var pop in pops)
            {
                Context.PersonOnProfessions.Add(pop);
            }
        }

        public void SeedTheatricalCharacter()
        {
            var characters = new List<TheatricalCharacter>()
            {
                new TheatricalCharacter() //1
                {
                    Name = "The Good, the Bad, and the Very Ugly"
                },
                new TheatricalCharacter() //2
                {
                    Name = "Typical western cowboy"
                },
                new TheatricalCharacter() //3
                {
                    Name = "Many different characters"
                },
                new TheatricalCharacter() //4
                {
                    Name = "Negative characters"
                },
                new TheatricalCharacter() //5
                {
                    Name = "Hero lover"
                },
                new TheatricalCharacter() //6
                {
                    Name = "Villain"
                },
                new TheatricalCharacter() //7
                {
                    Name = "Hero"
                },
                new TheatricalCharacter() //8
                {
                    Name = "Villain"
                },
                new TheatricalCharacter() //9
                {
                    Name = "Roles of an acute plan"
                }
            };
            for (int i = 0; i < characters.Count; i++)
            {
                characters[i].Id = i + 1;
                Context.TheatricalCharacters.Add(characters[i]);
            }
        }

        public void SeedActorTheatricalCharacters()
        {
            var atcs = new List<ActorTheatricalCharacter>()
            {
                new ActorTheatricalCharacter()
                {
                    TheatricalCharacterId = 1,
                    ActorId = 1
                },
                new ActorTheatricalCharacter()
                {
                    TheatricalCharacterId = 2,
                    ActorId = 2
                },
                new ActorTheatricalCharacter()
                {
                    TheatricalCharacterId = 3,
                    ActorId = 5
                },
                new ActorTheatricalCharacter()
                {
                    TheatricalCharacterId = 4,
                    ActorId = 6
                },
                new ActorTheatricalCharacter()
                {
                    TheatricalCharacterId = 5,
                    ActorId = 8
                },
                new ActorTheatricalCharacter()
                {
                    TheatricalCharacterId = 6,
                    ActorId = 9
                },
                new ActorTheatricalCharacter()
                {
                    TheatricalCharacterId = 7,
                    ActorId = 10
                },
                new ActorTheatricalCharacter()
                {
                    TheatricalCharacterId = 8,
                    ActorId = 11
                },
                new ActorTheatricalCharacter()
                {
                    TheatricalCharacterId = 9,
                    ActorId = 1
                }
            };
            foreach (var atc in atcs)
            {
                Context.ActorTheatricalCharacters.Add(atc);
            }
        }
    }
}

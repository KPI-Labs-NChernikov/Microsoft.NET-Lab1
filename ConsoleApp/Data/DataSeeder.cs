using Data;
using Data.Interfaces;
using Data.Models;
using System;
using System.Collections.Generic;

namespace ConsoleApp.Data
{
    public class DataSeeder
    {
        public Context Context { get; }

        public DataSeeder(Context context)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context), "Context cannot be null");
        }

        private void SeedGenres()
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
                }
            };
            for (int i = 0; i < genres.Count; i++)
            {
                genres[i].Id = i + 1;
                Context.Genres.Add(genres[i]);
            }
        }

        private void SeedSpectacles()
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

        private void SeedPeople()
        {
            var people = new List<IPerson>()
            {
                new Actor() //1
                {
                    FirstName = "Sandra",
                    LastName = "Anderson",
                    Patronymic = "Louise",
                    BirthYear = 1944,
                    TheatricalCharacter = "The Good, the Bad, and the Very Ugly"
                },
                new Actor() //2
                {
                    FirstName = "Clint",
                    LastName = "Eastwood",
                    BirthYear = 1930,
                    TheatricalCharacter = "Typical western cowboy"
                },
                new Director()  //3
                {
                    FirstName = "Yuriy",
                    LastName = "Chaika",
                    Patronymic = "Viktorovych",
                    BirthYear = 1943
                },
                new Director()  //4
                {
                    FirstName = "Ihor",
                    LastName = "Bilyts",
                    BirthYear = 1980
                },
                new Actor() //5
                {
                    FirstName = "Ella",
                    LastName = "Sanko",
                    Patronymic = "Ivanivna",
                    BirthYear = 1947,
                    TheatricalCharacter = "Many different characters"
                },
                new Actor() //6
                {
                    FirstName = "Bohdan",
                    LastName = "Stupka",
                    Patronymic = "Sylvestrovych",
                    BirthYear = 1941,
                    TheatricalCharacter = "Negative characters"
                },
                new Director()  //7
                {
                    FirstName = "Iryna",
                    LastName = "Molostova",
                    Patronymic = "Oleksandrivna",
                    BirthYear = 1929
                },
                new Actor() //8
                {
                    FirstName = "William",
                    LastName = "Pitt",
                    Patronymic = "Bradley",
                    BirthYear = 1963,
                    TheatricalCharacter = "Hero lover"
                },
                new Actor() //9
                {
                    FirstName = "Leonardo",
                    LastName = "DiCaprio",
                    Patronymic = "Wilhelm",
                    BirthYear = 1974,
                    TheatricalCharacter = "Villain"
                },
                new Actor() //10
                {
                    FirstName = "Orlando",
                    LastName = "Bloom",
                    BirthYear = 1977,
                    TheatricalCharacter = "Hero"
                },
                new Actor() //11
                {
                    FirstName = "Johnny",
                    LastName = "Depp",
                    BirthYear = 1963,
                    TheatricalCharacter = "Villain"
                },
                new Director()  //12
                {
                    FirstName = "Gregor",
                    LastName = "Verbinski",
                    Patronymic = "Justin",
                    BirthYear = 1963
                },
                new Director()  //13
                {
                    FirstName = "Peter",
                    LastName = "Jackson",
                    Patronymic = "Robert",
                    BirthYear = 1961
                },
                new Director()  //14
                {
                    FirstName = "Martin",
                    LastName = "Scorsese",
                    Patronymic = "Charles",
                    BirthYear = 1942
                },
                new Director()  //15
                {
                    FirstName = "James",
                    LastName = "Cameron",
                    Patronymic = "Francis",
                    BirthYear = 1954
                },
                new Director()  //16
                {
                    FirstName = "Yuri",
                    LastName = "Illienko",
                    Patronymic = "Herasymovych",
                    BirthYear = 1936
                },
                new Director()  //17
                {
                    FirstName = "Sergio",
                    LastName = "Leone",
                    BirthYear = 1929
                }
            };
            for (int i = 0; i < people.Count; i++)
            {
                people[i].Id = i + 1;
                Context.People.Add(people[i]);
            }
        }

        private void SeedMovies()
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

        public void SeedData()
        {
            SeedGenres();
            SeedSpectacles();
            SeedPeople();
            SeedMovies();
            //TODO: write and call SeedActorsOnMovies(); SeedActorsOnSpectacles();
        }
    }
}

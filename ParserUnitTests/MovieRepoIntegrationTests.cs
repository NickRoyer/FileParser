using System;
using Xunit;
using FileParser;
using System.Collections.Generic;
using FileParser.Repos;

namespace ParserUnitTests
{
    public class MovieRepoIntegrationTests
    {

        List<Movie> MockMovieList = new List<Movie>();
        public MovieRepoIntegrationTests()
        {
            MockMovieList.Add(new Movie() { Genre="Test Genre", GenreId = 1, Gross =100, Id=1, Name="Test 1", Year= 2005  });
            MockMovieList.Add(new Movie() { Genre = "Test Genre", GenreId = 1, Gross = 200, Id = 2, Name = "Test 2", Year = 2008 });
            MockMovieList.Add(new Movie() { Genre = "Drama:Western", GenreId = 2, Gross = 1000, Id = 3, Name = "Test Western 1", Year = 2015 });
            MockMovieList.Add(new Movie() { Genre = "Drama:Western", GenreId = 2, Gross = 2000, Id = 4, Name = "Test Western 2", Year = 2016 });
            MockMovieList.Add(new Movie() { Genre = "Drama:Western", GenreId = 2, Gross = 1005, Id = 5, Name = "Test Western 3", Year = 2016 });
        }

        [Fact]
        public void Verify_Dictionary_YearFirstLoads()
        {
            MovieDictionaryRepo mr = new MovieDictionaryRepo();
            mr.Init(MockMovieList, FirstField.Year);

            Dictionary<string, List<Movie>> genreDict;
            mr.DictionaryByYearThenGenre.TryGetValue(2016, out genreDict);

            List<Movie> MovieList;
            genreDict.TryGetValue("Drama:Western", out MovieList);

            Assert.Equal(2, MovieList.Count);
        }

        [Fact]
        public void Verify_Dictionary_GenreFirstLoads()
        {
            MovieDictionaryRepo mr = new MovieDictionaryRepo();
            mr.Init(MockMovieList, FirstField.Genre);

            mr.DictionaryByGenreThenYear.TryGetValue("Drama:Western", out Dictionary<long, List<Movie>> yearDict);

            yearDict.TryGetValue(2016, out List<Movie> MovieList);

            Assert.Equal(2, MovieList.Count);
        }

        [Fact]
        public void Verify_DictionaryGrossCnt()
        {
            MovieDictionaryRepo mr = new MovieDictionaryRepo();
            mr.Init(MockMovieList, FirstField.Genre);

            Assert.NotNull(mr.MoneyGrossDictionary);
            Assert.Equal(2, mr.FindMoviesInGrossReceiptRange(1000, 1999));
        }

        [Fact]
        public void Verify_SortDictionary_YearFirstLoads()
        {
            MovieSortedDictionaryRepo mr = new MovieSortedDictionaryRepo();
            mr.Init(MockMovieList, FirstField.Year);

            mr.SortedDictByYearByGenre.TryGetValue(2016, out Dictionary<string, List<Movie>> genreDict);

            genreDict.TryGetValue("Drama:Western", out List<Movie> MovieList);

            Assert.Equal(2, MovieList.Count);
        }

        [Fact]
        public void Verify_SortedDictionary_GenreFirstLoads()
        {
            MovieSortedDictionaryRepo mr = new MovieSortedDictionaryRepo();
            mr.Init(MockMovieList, FirstField.Genre);

            mr.SortedDictByGenreByYear.TryGetValue("Drama:Western", out SortedDictionary<long, List<Movie>> yearDict);

            yearDict.TryGetValue(2016, out List<Movie> MovieList);

            Assert.Equal(2, MovieList.Count);
        }

        [Fact]
        public void Verify_ILookup_YearFirstLoads()
        {
            MovieLookupRepo mr = new MovieLookupRepo();
            mr.Init(MockMovieList, FirstField.Year);                      

            Assert.NotNull(mr.LookupByYearByGenre);
            Assert.Null(mr.LookupByGenreByYear);
            Assert.Equal(3, mr.FindMovies(2015, 2016, "Drama:Western"));
        }

        [Fact]
        public void Verify_ILookup_GenreFirstLoads()
        {
            MovieLookupRepo mr = new MovieLookupRepo();
            mr.Init(MockMovieList, FirstField.Genre);

            Assert.NotNull(mr.LookupByGenreByYear);
            Assert.Null(mr.LookupByYearByGenre);
            Assert.Equal(3, mr.FindMovies(2015, 2016, "Drama:Western"));

        }

        [Fact]
        public void Verify_TreeDictionary_YearFirstLoad()
        {
            MovieC5SearchTreeRepo mr = new MovieC5SearchTreeRepo();
            mr.Init(MockMovieList, FirstField.Year);

            Assert.NotNull(mr.SearchTreeByYearByGenre);
            Assert.Null(mr.SearchTreeByGenreByYear);
            Assert.Equal(3, mr.FindMovies(2015, 2016, "Drama:Western"));
        }

        //Not implemented
        //[Fact]
        //public void Verify_TreeDictionary_GenreFirstLoad()
        //{
        //    MovieC5SearchTreeRepo mr = new MovieC5SearchTreeRepo();
        //    mr.Init(MockMovieList, FirstField.Genre);

        //    Assert.NotNull(mr.SearchTreeByGenreByYear);
        //    Assert.Null(mr.SearchTreeByYearByGenre);
        //    Assert.Equal(3, mr.FindMovies(2015, 2016, "Drama:Western"));
        //}

        [Fact]
        public void Verify_BinaryTreeDictionary_YearFirstLoad()
        {
            MovieBinaryTreeRepo mr = new MovieBinaryTreeRepo();
            mr.Init(MockMovieList, FirstField.Year);

            Assert.NotNull(mr.BinaryTreeByYearByGenre);
            Assert.Null(mr.BinaryTreeByGenreByYear);
            Assert.Equal(3, mr.FindMovies(2015, 2016, "Drama:Western"));
        }

        [Fact]
        public void Verify_BinaryTreeDictionary_GenreFirstLoad()
        {
            MovieBinaryTreeRepo mr = new MovieBinaryTreeRepo();
            mr.Init(MockMovieList, FirstField.Genre);

            Assert.NotNull(mr.BinaryTreeByGenreByYear);
            Assert.Null(mr.BinaryTreeByYearByGenre);
            Assert.Equal(3, mr.FindMovies(2015, 2016, "Drama:Western"));
        }


        [Fact]
        public void Verify_BinaryTreeGrossCnt()
        {
            MovieBinaryTreeRepo mr = new MovieBinaryTreeRepo();
            mr.Init(MockMovieList, FirstField.Genre);

            Assert.NotNull(mr.MoneyGrossBinaryTree);
            Assert.Equal(2, mr.FindMoviesInGrossReceiptRange(1000, 1999));
        }

    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace FileParser.Repos
{
    public class MovieLookupRepo : IMovieRepo
    {
        //By EF lookup (NOTE: this is an immutable collection)
        public ILookup<string, ILookup<long, Movie>> LookupByGenreByYear { get; set; }
        public ILookup<long, ILookup<string, Movie>> LookupByYearByGenre { get; set; }

        public FirstField Field { get; set; }

        public string Type()
        {
            return "Lookup";
        }

        public void Init(ICollection<Movie> movies, FirstField ff)
        {
            Field = ff;

            if (FirstField.Year == Field)
                LookupByYearByGenre = movies.GroupBy(m => m.Year)
                    .ToLookup(t => t.Key, t => t.ToLookup(m => m.Genre, m => m));
            else
                LookupByGenreByYear = movies.GroupBy(m => m.Genre)
                    .ToLookup(t => t.Key, t => t.ToLookup(m => m.Year, m => m));
        }

        public long FindMovies(long startYear, long endYear, string genre)
        {
            long returnCnt = 0;
            if (FirstField.Year == Field)
            {
                var s = LookupByYearByGenre.Where(x => x.Key >= startYear && x.Key <= endYear)
                                .SelectMany(a => a);
                foreach (var lookup in s)
                {
                    returnCnt += lookup.Where(x => x.Key == genre)
                        .SelectMany(a => a).Count();
                }
            }
            else
            {
                var s = LookupByGenreByYear.Where(x => x.Key == genre)
                                .SelectMany(a => a);
                foreach (var lookup in s)
                {
                    returnCnt += lookup.Where(x => x.Key >= startYear && x.Key <= endYear)
                        .SelectMany(a => a).Count();
                }
            }

            return returnCnt;
        }

        public long FindMoviesInGrossReceiptRange(long minGross, long maxGross)
        {
            throw new Exception("Not Yet Implemented!");
        }
    }
}

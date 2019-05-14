using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace FileParser.Repos
{
    public class MovieDictionaryRepo : IMovieRepo
    {
        public Dictionary<long, Dictionary<string, List<Movie>>> DictionaryByYearThenGenre;
        public Dictionary<string, Dictionary<long, List<Movie>>> DictionaryByGenreThenYear;

        public Dictionary<long, List<Movie>> MoneyGrossDictionary { get; set; }

        public FirstField Field { get; set; }

        public string Type()
        {
            return "Dictionary";
        }

        public void Init(ICollection<Movie> movies, FirstField ff)
        {
            Field = ff;
            if (FirstField.Year == Field)
                DictionaryByYearThenGenre = movies.GroupBy(m => m.Year)
                        .ToDictionary(t => t.Key, t => t.GroupBy(x => x.Genre).ToDictionary(x => x.Key, x => x.ToList()));
            else
                DictionaryByGenreThenYear = movies.GroupBy(m => m.Genre)
                        .ToDictionary(t => t.Key, t => t.GroupBy(x => x.Year).ToDictionary(x => x.Key, x => x.ToList()));

            MoneyGrossDictionary = movies.GroupBy(m => m.Gross)
                                        .ToDictionary(t => t.Key, t => t.ToList());
        }

        public long FindMovies(long startYear, long endYear, string genre)
        {
            if (DictionaryByYearThenGenre == null && DictionaryByGenreThenYear == null)
                throw new Exception("Init must be run ont the repo prior to querying for data.");

            long returnCnt = 0;
            if (FirstField.Year == Field)
            {
                for (long i = startYear; i <= endYear; i++)
                {
                    DictionaryByYearThenGenre.TryGetValue(i, out Dictionary<string, List<Movie>> yearDict);
                    if (yearDict != null)
                    {
                        yearDict.TryGetValue(genre, out List<Movie> movies);
                        if (movies != null)
                            returnCnt += movies.Count();
                    }
                }
            }
            else
            {
                DictionaryByGenreThenYear.TryGetValue(genre, out Dictionary<long, List<Movie>> genreDict);
                if (genreDict != null)
                {
                    for (long i = startYear; i <= endYear; i++)
                    {
                        genreDict.TryGetValue(i, out List<Movie> movies);
                        if (movies != null)
                            returnCnt += movies.Count();
                    }
                }
            }
            return returnCnt;
        }

        public long FindMoviesInGrossReceiptRange(long minGross, long maxGross)
        {
            if (DictionaryByYearThenGenre == null && DictionaryByGenreThenYear == null)
                throw new Exception("Init must be run ont the repo prior to querying for data.");

            long returnCnt = 0;

            for(long i = minGross; i <= maxGross; i++)
            {
                MoneyGrossDictionary.TryGetValue(i, out List<Movie> movies);
                if (movies != null)
                    returnCnt += movies.Count();
            }

            return returnCnt;
        }
    }
}

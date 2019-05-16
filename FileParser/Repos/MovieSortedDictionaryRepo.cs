using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace FileParser.Repos
{
    public class MovieSortedDictionaryRepo : IMovieRepo
    {
        //SortedDictionary for Years and regular dictionary for Genre
        //SortedDictionary has a read access of O(log n), Dictionary has a read access of O(1) -> O(n) 

        //NOTE: SortedDictionary implements a Red / Black Binary search tree HOWEVER it does NOT expose the ability to traverse 
        //from one key value to the next key value without Enumerating the entire collection.
        
        public SortedDictionary<long, Dictionary<string, List<Movie>>> SortedDictByYearByGenre { get; set; }
        public Dictionary<string, SortedDictionary<long, List<Movie>>> SortedDictByGenreByYear { get; set; }

        public SortedDictionary<long, List<Movie>> MoneyGrossSearchTree { get; set; }

        public FirstField Field { get; set; }

        public string Type()
        {
            return "Sorted Dictionary";
        }

        public void Init(ICollection<Movie> movies, FirstField ff)
        {
            Field = ff;
            if (FirstField.Year == Field)
                SortedDictByYearByGenre = new SortedDictionary<long, Dictionary<string, List<Movie>>>(movies.GroupBy(m => m.Year)
                            .ToDictionary(t => t.Key, t => t.GroupBy(x => x.Genre).ToDictionary(x => x.Key, x => x.ToList())));
            else
                SortedDictByGenreByYear = movies.GroupBy(m => m.Genre)
                        .ToDictionary(t => t.Key,
                            t => new SortedDictionary<long, List<Movie>>(t.GroupBy(x => x.Year).ToDictionary(x => x.Key, x => x.ToList()))
                         );

            MoneyGrossSearchTree = new SortedDictionary<long, List<Movie>>(movies.GroupBy(m => m.Gross)
                    .ToDictionary(t => t.Key, t => t.ToList()));
        }

        public long FindMovies(long startYear, long endYear, string genre)
        {
            long returnCnt = 0;
            if (FirstField.Year == Field)
            {
                List<KeyValuePair<long, Dictionary<string, List<Movie>>>> s = SortedDictByYearByGenre
                                .Where(x => x.Key >= startYear && x.Key <= endYear)
                                .ToList();
                for(int i = 0; i<s.Count; i++)
                {
                    s[i].Value.TryGetValue(genre, out List<Movie> movies);
                    if (movies != null)
                        returnCnt += movies.Count();
                }
            }
            else
            {
                SortedDictByGenreByYear.TryGetValue(genre, out SortedDictionary<long, List<Movie>> YearSortedDict);

                if (YearSortedDict != null)
                {
                    List<KeyValuePair<long, List<Movie>>> s = YearSortedDict.Where(x => x.Key >= startYear && x.Key <= endYear).ToList();
                    for(int i =0; i<s.Count; i++)
                    {
                        returnCnt += s[i].Value.Count();
                    }
                }
            }

            return returnCnt;
        }

        public long FindMoviesInGrossReceiptRange(long minGross, long maxGross)
        {
            
            return MoneyGrossSearchTree.Where(x => x.Key >= minGross && x.Key <= maxGross).SelectMany(a => a.Value).Count();
            //throw new Exception("Not Implemented");
        }
    }
}

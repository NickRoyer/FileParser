using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using c5T = C5;

namespace FileParser.Repos
{
    public class MovieC5SearchTreeRepo : IMovieRepo
    {
        //Implemented via the "C5" project: https://github.com/sestoft/C5/
        //Using the Red / Black binary tree that implements very useful methods to expose the Next / Previous iteration through the nodes
        //HOWEVER After some research it is still basically generating a List and enumerating over the values and not taking advantage of the sorted nature of the tree

        //NOTE: Using the .Net Generic Dictionary so as to not introduce additional potential error
        public c5T.TreeDictionary<long, Dictionary<string, List<Movie>>> SearchTreeByYearByGenre;
        public Dictionary<string, c5T.TreeDictionary<long, List<Movie>>> SearchTreeByGenreByYear;

        public FirstField Field { get; set; }

        public string Type()
        {
            return "C5 Tree Dictionary";
        }

        public void Init(ICollection<Movie> movies, FirstField ff)
        {
            Field = ff;
            if(Field == FirstField.Year)
            {
                SearchTreeByYearByGenre = new c5T.TreeDictionary<long, Dictionary<string, List<Movie>>>();

                List<c5T.KeyValuePair<long, Dictionary<string, List<Movie>>>> KVPList = new List<c5T.KeyValuePair<long, Dictionary<string, List<Movie>>>>();

                foreach (var YearGrp in movies.GroupBy(m => m.Year))
                {
                    long year = YearGrp.Key;
                    Dictionary<string, List<Movie>> genreDict = new Dictionary<string, List<Movie>>();

                    foreach (var genreGrp in YearGrp.GroupBy(m => m.Genre))
                    {
                        genreDict.Add(genreGrp.Key, genreGrp.ToList());
                    }

                    KVPList.Add(new c5T.KeyValuePair<long, Dictionary<string, List<Movie>>>(year, genreDict));
                }

                SearchTreeByYearByGenre.AddAll(KVPList);
            }
            else
            {
                SearchTreeByGenreByYear = new Dictionary<string, c5T.TreeDictionary<long, List<Movie>>>();
                throw new Exception("Not Implemented");
            }
        }

        public long FindMovies(long startYear, long endYear, string genre)
        {
            long returnCnt = 0;
            if (FirstField.Year == Field)
            {
                //foreach( var kvp in SearchTreeByYearByGenre.Filter(f => f.Key >= startYear && f.Key <= endYear))
                foreach (var kvp in SearchTreeByYearByGenre.RangeFromTo(startYear, endYear + 1))
                {
                    kvp.Value.TryGetValue(genre, out List<Movie> movies);
                    if (movies != null)
                        returnCnt += movies.Count();
                }
            }
            else
            {
                throw new Exception("Not Implemented");
            }
            
            return returnCnt;
        }

        public long FindMoviesInGrossReceiptRange(long minGross, long maxGross)
        {
            throw new Exception("Not Implemented");
        }
    }
}

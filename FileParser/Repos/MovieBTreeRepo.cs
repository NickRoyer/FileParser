using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using FileParser.DataStructures;

namespace FileParser.Repos
{
    public class MovieBTreeRepo : IMovieRepo
    {
        public FirstField Field { get; set; }

        public BTree<long, Dictionary<string, IList<Movie>>> BTreeByYearByGenre;
        public Dictionary<string, BTree<long, IList<Movie>>> BTreeByGenreByYear;

        public BTree<long, IList<Movie>> MoneyGrossBTree { get; set; }

        public string Type()
        {
            return "B*Tree";
        }

        public MovieBTreeRepo() { }

        private void InitDataStructure()
        {
            if (Field == FirstField.Year)
            {
                BTreeByYearByGenre = new BTree<long, Dictionary<string, IList<Movie>>>();
            }
            else
            {
                BTreeByGenreByYear = new Dictionary<string, BTree<long, IList<Movie>>>();
            }
        }

        public void Init(ICollection<Movie> movies, FirstField ff)
        {
            Field = ff;

            InitDataStructure();
            if (Field == FirstField.Year)
            {
                foreach (var YearGrp in movies.GroupBy(m => m.Year))
                {
                    Dictionary<string, IList<Movie>> genreDict = new Dictionary<string, IList<Movie>>();

                    foreach (var genreGrp in YearGrp.GroupBy(m => m.Genre))
                    {
                        genreDict.Add(genreGrp.Key, genreGrp.ToList());
                    }

                    long year = YearGrp.Key;
                    BTreeByYearByGenre.Add(year, genreDict);
                }
            }
            else
            {
                foreach (var genreGrp in movies.GroupBy(m => m.Genre))
                {
                    BTree<long, IList<Movie>> yearTree = new BTree<long, IList<Movie>>();                   

                    foreach (var YearGrp in genreGrp.GroupBy(m => m.Year))
                    {
                        long year = YearGrp.Key;
                        yearTree.Add(year, YearGrp.ToList());
                    }

                    BTreeByGenreByYear.Add(genreGrp.Key, yearTree);
                }
            }
   
            MoneyGrossBTree = new BTree<long, IList<Movie>>();
            
            foreach (var grossGrp in movies.GroupBy(m => m.Gross))
            {
                MoneyGrossBTree.Add(grossGrp.Key, grossGrp.ToList());
            }
        }

        public long FindMovies(long startYear, long endYear, string genre)
        {
            long returnCnt = 0;
            if (Field == FirstField.Year)
            {
                IList<Dictionary<string, IList<Movie>>> rng = BTreeByYearByGenre.Range(startYear, endYear);
                for (int i = 0; i < rng.Count(); i++)
                {
                    rng[i].TryGetValue(genre, out IList<Movie> movies);
                    if (movies != null)
                        returnCnt += movies.Count();
                }
            }
            else
            {
                BTreeByGenreByYear.TryGetValue(genre, out BTree<long, IList<Movie>> yearTree);
                if (yearTree != null)
                {
                    IList<IList<Movie>> l = yearTree.Range(startYear, endYear);
                    for (int i = 0; i < l.Count; i++)
                    {
                        returnCnt += l[i].Count();
                    }
                }
            }
            return returnCnt;
        }

        public long FindMoviesInGrossReceiptRange(long minGross, long maxGross)
        {
            long returnCnt = 0;

            IList<IList<Movie>> list = MoneyGrossBTree.Range(minGross, maxGross);
            for (int i = 0; i < list.Count; i++)
            {
                IList<Movie> l = list[i];
                returnCnt += l.Count();
            }

            return returnCnt;
        }
    }
}



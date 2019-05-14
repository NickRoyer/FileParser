using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace FileParser.Repos
{
    public class MovieBinaryTreeRepo : IMovieRepo
    {
        public FirstField Field { get; set; }

        public BinaryTree<long, Dictionary<string, List<Movie>>> BinaryTreeByYearByGenre;
        public Dictionary<string, BinaryTree<long, List<Movie>>> BinaryTreeByGenreByYear;

        public BinaryTree<long, List<Movie>> MoneyGrossBinaryTree { get; set; }

        public string Type()
        {
            return "Binary Tree";
        }

        public void Init(ICollection<Movie> movies, FirstField ff)
        {
            Field = ff;
            if (Field == FirstField.Year)
            {
                BinaryTreeByYearByGenre = new BinaryTree<long, Dictionary<string, List<Movie>>>();

                foreach (var YearGrp in movies.GroupBy(m => m.Year))
                {                    
                    Dictionary<string, List<Movie>> genreDict = new Dictionary<string, List<Movie>>();

                    foreach (var genreGrp in YearGrp.GroupBy(m => m.Genre))
                    {
                        genreDict.Add(genreGrp.Key, genreGrp.ToList());
                    }

                    long year = YearGrp.Key;
                    BinaryTreeByYearByGenre.Add(year, genreDict);
                }
            }
            else
            {
                BinaryTreeByGenreByYear = new Dictionary<string, BinaryTree<long, List<Movie>>>();

                foreach (var genreGrp in movies.GroupBy(m => m.Genre))                    
                {                    
                    BinaryTree<long, List<Movie>> yearTree = new BinaryTree<long, List<Movie>>();

                    foreach (var YearGrp in genreGrp.GroupBy(m => m.Year))
                    {
                        long year = YearGrp.Key;
                        yearTree.Add(year, YearGrp.ToList());
                    }
                    BinaryTreeByGenreByYear.Add(genreGrp.Key, yearTree);
                }
            }

            MoneyGrossBinaryTree = new BinaryTree<long, List<Movie>>();
            foreach( var grossGrp in movies.GroupBy( m => m.Gross))
            {
                MoneyGrossBinaryTree.Add(grossGrp.Key, grossGrp.ToList());
            }
        }

        public long FindMovies(long startYear, long endYear, string genre)
        {
            long returnCnt = 0;
            if (Field == FirstField.Year)
            {
                List<Dictionary<string, List<Movie>>> rng = BinaryTreeByYearByGenre.Range(startYear, endYear);
                for(int i = 0; i<rng.Count(); i++)
                {
                    rng[i].TryGetValue(genre, out List<Movie> movies);
                    if (movies != null)
                        returnCnt += movies.Count();
                }
            }
            else
            {
                BinaryTreeByGenreByYear.TryGetValue(genre, out BinaryTree<long, List<Movie>> yearTree);
                if (yearTree != null)
                {
                    foreach (var l in yearTree.Range(startYear, endYear))
                    {
                        returnCnt += l.Count();
                    }
                }
            }
            return returnCnt;
        }

        public long FindMoviesInGrossReceiptRange(long minGross, long maxGross)
        {
            long returnCnt = 0;

            List<List<Movie>> list = MoneyGrossBinaryTree.Range(minGross, maxGross);
            for (int i = 0; i < list.Count; i++)
            {
                List<Movie> l = list[i];
                returnCnt += l.Count();
            }

            return returnCnt;
        }
    }
}

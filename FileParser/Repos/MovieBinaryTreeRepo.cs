using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace FileParser.Repos
{
    public class MovieBinaryTreeRepo : IMovieRepo
    {
        public  enum BinaryTreeType { BinaryTree, RedBlackBinaryTree }

        public FirstField Field { get; set; }
        public BinaryTreeType TreeType { get; set; }

        public BinaryTree<long, Dictionary<string, IList<Movie>>> BinaryTreeByYearByGenre;
        public Dictionary<string, BinaryTree<long, IList<Movie>>> BinaryTreeByGenreByYear;

        public BinaryTree<long, IList<Movie>> MoneyGrossBinaryTree { get; set; }

        public string Type()
        {
            if (TreeType == BinaryTreeType.BinaryTree)
                return "Binary Tree";
            else
                return "Red Black Binary Tree";
        }

        public MovieBinaryTreeRepo(BinaryTreeType treeType)
        {
            TreeType = treeType;
        }

        private void InitDataStructure()
        {
            if(Field == FirstField.Year)
            {
                if(TreeType == BinaryTreeType.BinaryTree)
                    BinaryTreeByYearByGenre = new BinaryTree<long, Dictionary<string, IList<Movie>>>();
                else
                    BinaryTreeByYearByGenre = new RedBlackTree<long, Dictionary<string, IList<Movie>>>();
            }
            else
            {
                //Since the BinaryTree in this case is not being instantiated we delay the check till the init method
                BinaryTreeByGenreByYear = new Dictionary<string, BinaryTree<long, IList<Movie>>>();                    
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
                    BinaryTreeByYearByGenre.Add(year, genreDict);
                }
            }
            else
            {               
                foreach (var genreGrp in movies.GroupBy(m => m.Genre))                    
                {
                    BinaryTree<long, IList<Movie>> yearTree;
                    if (BinaryTreeType.BinaryTree == TreeType)
                        yearTree = new BinaryTree<long, IList<Movie>>();
                    else
                        yearTree = new RedBlackTree<long, IList<Movie>>();

                    foreach (var YearGrp in genreGrp.GroupBy(m => m.Year))
                    {
                        long year = YearGrp.Key;
                        yearTree.Add(year, YearGrp.ToList());
                    }

                    BinaryTreeByGenreByYear.Add(genreGrp.Key, yearTree);
                }
            }
            
            if (BinaryTreeType.BinaryTree == TreeType)
                MoneyGrossBinaryTree = new BinaryTree<long, IList<Movie>>();
            else
                MoneyGrossBinaryTree = new RedBlackTree<long, IList<Movie>>();

            foreach ( var grossGrp in movies.GroupBy( m => m.Gross))
            {
                MoneyGrossBinaryTree.Add(grossGrp.Key, grossGrp.ToList());
            }
        }

        public long FindMovies(long startYear, long endYear, string genre)
        {
            long returnCnt = 0;
            if (Field == FirstField.Year)
            {
                IList<Dictionary<string, IList<Movie>>> rng = BinaryTreeByYearByGenre.Range(startYear, endYear);
                for(int i = 0; i<rng.Count(); i++)
                {
                    rng[i].TryGetValue(genre, out IList<Movie> movies);
                    if (movies != null)
                        returnCnt += movies.Count();
                }
            }
            else
            {
                BinaryTreeByGenreByYear.TryGetValue(genre, out BinaryTree<long, IList<Movie>> yearTree);
                if (yearTree != null)
                {
                    IList<IList<Movie>> l = yearTree.Range(startYear, endYear);
                    for(int i = 0; i< l.Count; i++)
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

            IList<IList<Movie>> list = MoneyGrossBinaryTree.Range(minGross, maxGross);
            for (int i = 0; i < list.Count; i++)
            {
                IList<Movie> l = list[i];
                returnCnt += l.Count();
            }

            return returnCnt;
        }
    }
}

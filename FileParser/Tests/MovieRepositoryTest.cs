using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Linq;
using FileParser.Repos;


namespace FileParser.Tests
{
    public class MovieRepositoryTest
    {
        public enum MovieRepoType { Dictionary, SearchTree, SortedDictionary, BinarySearchTree, RedBlackBinaryTree, Lookup }

        public void TestRepositories()
        {
            MovieFileParser fp = new MovieFileParser();
            List<Movie> movieList = fp.LoadFile("MovieData.csv");

            CreateResult DictByYear = TestRepoGeneration(MovieRepoType.Dictionary, FirstField.Year, movieList);
            PrintResult(DictByYear);

            CreateResult BTreeYear = TestRepoGeneration(MovieRepoType.BinarySearchTree, FirstField.Year, movieList);
            PrintResult(BTreeYear);

            CreateResult BTreeGenre = TestRepoGeneration(MovieRepoType.BinarySearchTree, FirstField.Genre, movieList);
            PrintResult(BTreeGenre);

            CreateResult RBTreeYear = TestRepoGeneration(MovieRepoType.RedBlackBinaryTree, FirstField.Year, movieList);
            PrintResult(RBTreeYear);

            CreateResult RBTreeGenre = TestRepoGeneration(MovieRepoType.RedBlackBinaryTree, FirstField.Genre, movieList);
            PrintResult(RBTreeGenre);

            CreateResult DictByGenre = TestRepoGeneration(MovieRepoType.Dictionary, FirstField.Genre, movieList);
            PrintResult(DictByGenre);

            CreateResult STreeByYear = TestRepoGeneration(MovieRepoType.SearchTree, FirstField.Year, movieList);
            PrintResult(STreeByYear);

            CreateResult LookupByYear = TestRepoGeneration(MovieRepoType.Lookup, FirstField.Year, movieList);
            PrintResult(LookupByYear);

            CreateResult LookupByGenre = TestRepoGeneration(MovieRepoType.Lookup, FirstField.Genre, movieList);
            PrintResult(LookupByGenre);

            CreateResult SortedDictGenre = TestRepoGeneration(MovieRepoType.SortedDictionary, FirstField.Genre, movieList);
            PrintResult(SortedDictGenre);

            CreateResult SortedDictYear = TestRepoGeneration(MovieRepoType.SortedDictionary, FirstField.Year, movieList);
            PrintResult(SortedDictYear);

            List<IMovieRepo> repoList = new List<IMovieRepo>
            {
                DictByYear.Repo,
                DictByGenre.Repo,
                BTreeYear.Repo,
                BTreeGenre.Repo,
                STreeByYear.Repo,
                LookupByGenre.Repo,
                LookupByYear.Repo,
                RBTreeYear.Repo,
                RBTreeGenre.Repo
            };

            //PrintAndExecuteForRepos(repoList, new YearGenreTest() { QueryCnt = 1, StartYear = 1960, EndYear = 2010, Genre = "Drama:Western" });

            //Single item in range Favors Dictionary
            PrintAndExecuteForRepos(repoList, new YearGenreTest() { QueryCnt = 20000, StartYear = 2015, EndYear = 2015, Genre = "Drama:Western" });
            PrintAndExecuteForRepos(repoList, new YearGenreTest() { QueryCnt = 6000, StartYear = 1995, EndYear = 2015, Genre = "Drama:Western" });
            PrintAndExecuteForRepos(repoList, new YearGenreTest() { QueryCnt = 6000, StartYear = 1965, EndYear = 2015, Genre = "Drama:Western" });
            PrintAndExecuteForRepos(repoList, new YearGenreTest() { QueryCnt = 6000, StartYear = 1915, EndYear = 2015, Genre = "Drama:Western" });
            PrintAndExecuteForRepos(repoList, new YearGenreTest() { QueryCnt = 6000, StartYear = 1880, EndYear = 2017, Genre = "Drama:Western" });

            //These are the only 2 implemented currently 
            List<IMovieRepo> grossRepos = new List<IMovieRepo>();
            grossRepos.Add(DictByYear.Repo);
            grossRepos.Add(BTreeYear.Repo);
            grossRepos.Add(RBTreeYear.Repo);
            grossRepos.Add(SortedDictYear.Repo);

            MovieBinaryTreeRepo btr = (MovieBinaryTreeRepo)BTreeYear.Repo;
            Console.WriteLine("Binary Tree Node Depth: " + btr.MoneyGrossBinaryTree.MaxNodeDepth());

            MovieBinaryTreeRepo rbtr = (MovieBinaryTreeRepo)RBTreeYear.Repo;
            Console.WriteLine("RedBlack Tree Node Depth: " + rbtr.MoneyGrossBinaryTree.MaxNodeDepth());

            //PrintAndExecuteForRepos(grossRepos, new GrossRevTest() { QueryCnt = 1, MinGross = 10000, MaxGross = 100000 });
            PrintAndExecuteForRepos(grossRepos, new GrossRevTest() { QueryCnt = 10, MinGross = 1000, MaxGross = 1000000 });
        }

        private CreateResult TestRepoGeneration(MovieRepoType rt, FirstField ff, List<Movie> movieList)
        {
            Stopwatch sw = new Stopwatch();
            IMovieRepo repo = null; 
            sw.Start();

            repo = MovieRepositoryFactory(rt, ff, movieList);

            sw.Stop();

            return new CreateResult { Repo = repo, TestTime = sw.Elapsed };
        }


        private IMovieRepo MovieRepositoryFactory(MovieRepoType rt, FirstField ff, List<Movie> movieList)
        {
            IMovieRepo returnRepo;
            switch (rt)
            {
                case MovieRepoType.Dictionary:
                    returnRepo = new MovieDictionaryRepo();                    
                    break;
                case MovieRepoType.SortedDictionary:
                    returnRepo = new MovieSortedDictionaryRepo();
                    break;
                case MovieRepoType.SearchTree:
                    returnRepo = new MovieC5SearchTreeRepo();
                    break;
                case MovieRepoType.Lookup:
                    returnRepo = new MovieLookupRepo();
                    break;
                case MovieRepoType.BinarySearchTree:
                    returnRepo = new MovieBinaryTreeRepo(MovieBinaryTreeRepo.BinaryTreeType.BinaryTree);
                    break;
                case MovieRepoType.RedBlackBinaryTree:
                    returnRepo = new MovieBinaryTreeRepo(MovieBinaryTreeRepo.BinaryTreeType.RedBlackBinaryTree);
                    break;
                default:
                    throw new Exception("RepoType: " + rt.ToString() + "Not Implemented");
            }

            returnRepo.Init(movieList, ff);

            return returnRepo;     
        }

        private void PrintAndExecute(IMovieRepo repo, ITest t)
        {
            PrintResult(ExecuteTest(repo, t));
        }

        private void PrintAndExecuteForRepos(List<IMovieRepo> repoList, ITest t)
        {
            PrintResult(ExecuteTestForRepos(repoList, t));
        }

        private void PrintResult(IResult r)
        {
            Console.WriteLine(r.ToString());
        }

        private void PrintResult(ICollection<IResult> results)
        {
            Console.WriteLine();
            if (results.Count() > 0)
            {
                Console.WriteLine(results.ElementAt(0).Test.ToString());
                var a = results.OrderBy(q => q.TestTime);

                for (int i = 0; i < Math.Min(a.Count(), 4); i++)
                {
                    switch (i)
                    {
                        case 0:
                            Console.WriteLine("First Place:");
                            break;
                        case 1:
                            Console.WriteLine("Second Place:");
                            break;
                        case 2:
                            Console.WriteLine("Third Place:");
                            break;
                        case 3:
                            Console.WriteLine("Forth Place:");
                            break;
                    }

                    PrintResult(a.ElementAt(i));
                }
            }
        }

        private IResult ExecuteTest(IMovieRepo repo, ITest t)
        {
            IResult returnResult;
            Stopwatch sw = new Stopwatch();
            sw.Start();

            returnResult = t.RunTest(repo);

            sw.Stop();
            returnResult.TestTime = sw.Elapsed;

            return returnResult;
        }

        public List<IResult> ExecuteTestForRepos(List<IMovieRepo> repoList, ITest t)
        {
            List<IResult> r = new List<IResult>();
            foreach (IMovieRepo repo in repoList)
                r.Add(ExecuteTest(repo, t));
            return r;
        }
    }
}

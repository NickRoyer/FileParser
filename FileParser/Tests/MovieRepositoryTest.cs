using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Linq;
using FileParser.Repos;
using System.IO;

namespace FileParser.Tests
{
    public class MovieRepositoryTest
    {        
        public void TestRepositories()
        {
            MovieFileParser fp = new MovieFileParser();
            List<Movie> movieList = fp.LoadFile("MovieData.csv");

            CreateResult DictByYear = TestRepoGeneration(MovieRepoFactory.Type.Dictionary, FirstField.Year, movieList);
            PrintResult(DictByYear);

            CreateResult BinaryTreeYear = TestRepoGeneration(MovieRepoFactory.Type.BinarySearchTree, FirstField.Year, movieList);
            PrintResult(BinaryTreeYear);

            CreateResult BinaryTreeGenre = TestRepoGeneration(MovieRepoFactory.Type.BinarySearchTree, FirstField.Genre, movieList);
            PrintResult(BinaryTreeGenre);

            CreateResult RBTreeYear = TestRepoGeneration(MovieRepoFactory.Type.RedBlackBinaryTree, FirstField.Year, movieList);
            PrintResult(RBTreeYear);

            CreateResult RBTreeGenre = TestRepoGeneration(MovieRepoFactory.Type.RedBlackBinaryTree, FirstField.Genre, movieList);
            PrintResult(RBTreeGenre);

            CreateResult DictByGenre = TestRepoGeneration(MovieRepoFactory.Type.Dictionary, FirstField.Genre, movieList);
            PrintResult(DictByGenre);

            CreateResult STreeByYear = TestRepoGeneration(MovieRepoFactory.Type.SearchTree, FirstField.Year, movieList);
            PrintResult(STreeByYear);

            CreateResult LookupByYear = TestRepoGeneration(MovieRepoFactory.Type.Lookup, FirstField.Year, movieList);
            PrintResult(LookupByYear);

            CreateResult LookupByGenre = TestRepoGeneration(MovieRepoFactory.Type.Lookup, FirstField.Genre, movieList);
            PrintResult(LookupByGenre);

            CreateResult SortedDictGenre = TestRepoGeneration(MovieRepoFactory.Type.SortedDictionary, FirstField.Genre, movieList);
            PrintResult(SortedDictGenre);

            CreateResult SortedDictYear = TestRepoGeneration(MovieRepoFactory.Type.SortedDictionary, FirstField.Year, movieList);
            PrintResult(SortedDictYear);

            CreateResult BTreeYear = TestRepoGeneration(MovieRepoFactory.Type.BTree, FirstField.Year, movieList);
            PrintResult(BTreeYear);

            CreateResult BTreeGenre = TestRepoGeneration(MovieRepoFactory.Type.BTree, FirstField.Genre, movieList);
            PrintResult(BTreeGenre);

            CreateResult LinqListGenre = TestRepoGeneration(MovieRepoFactory.Type.LinqList, FirstField.Genre, movieList);
            PrintResult(LinqListGenre);

            CreateResult LinqListYear = TestRepoGeneration(MovieRepoFactory.Type.LinqList, FirstField.Year, movieList);
            PrintResult(LinqListYear);

            //Parallel results were very similar to Standard linq 
            //CreateResult LinqListParGenre = TestRepoGeneration(MovieRepoType.LinqParList, FirstField.Genre, movieList);
            //PrintResult(LinqListParGenre);

            //CreateResult LinqListParYear = TestRepoGeneration(MovieRepoType.LinqParList, FirstField.Year, movieList);
            //PrintResult(LinqListParYear);

            List<IMovieRepo> repoList = new List<IMovieRepo>
            {
                DictByYear.Repo,
                DictByGenre.Repo,
                BinaryTreeYear.Repo,
                BinaryTreeGenre.Repo,
                STreeByYear.Repo,
                LookupByGenre.Repo,
                LookupByYear.Repo,
                RBTreeYear.Repo,
                RBTreeGenre.Repo,
                BTreeYear.Repo,
                BTreeGenre.Repo,
                LinqListGenre.Repo,
                LinqListYear.Repo
                //, LinqListParYear.Repo,
                //LinqListParGenre.Repo
            };

            //PrintAndExecuteForRepos(repoList, new YearGenreTest() { QueryCnt = 1, StartYear = 1960, EndYear = 2010, Genre = "Drama:Western" });
            //Easy query to validate it is working
            PrintAndExecuteForRepos(repoList, new YearGenreTest() { QueryCnt = 100, StartYear = 2015, EndYear = 2015, Genre = "Drama:Western" });

            //Processing the Linq List queries takes signficantly longer than the other tests so they are removed
            repoList.Remove(LinqListGenre.Repo);
            repoList.Remove(LinqListYear.Repo);

            //Single item in range Favors Dictionary
            PrintAndExecuteForRepos(repoList, new YearGenreTest() { QueryCnt = 20000, StartYear = 2015, EndYear = 2015, Genre = "Drama:Western" });

            PrintAndExecuteForRepos(repoList, new YearGenreTest() { QueryCnt = 6000, StartYear = 1995, EndYear = 2015, Genre = "Drama:Western" });
            PrintAndExecuteForRepos(repoList, new YearGenreTest() { QueryCnt = 6000, StartYear = 1965, EndYear = 2015, Genre = "Drama:Western" });
            PrintAndExecuteForRepos(repoList, new YearGenreTest() { QueryCnt = 6000, StartYear = 1915, EndYear = 2015, Genre = "Drama:Western" });
            PrintAndExecuteForRepos(repoList, new YearGenreTest() { QueryCnt = 6000, StartYear = 1880, EndYear = 2017, Genre = "Drama:Western" });

            List<IMovieRepo> grossRepos = new List<IMovieRepo>();
            grossRepos.Add(DictByYear.Repo);
            grossRepos.Add(BinaryTreeYear.Repo);
            grossRepos.Add(RBTreeYear.Repo);
            grossRepos.Add(SortedDictYear.Repo);
            grossRepos.Add(BTreeYear.Repo);
            grossRepos.Add(LinqListYear.Repo);
            //grossRepos.Add(LinqListParYear.Repo);

            //MovieBinaryTreeRepo btr = (MovieBinaryTreeRepo)BTreeYear.Repo;
            //Console.WriteLine("Binary Tree Node Depth: " + btr.MoneyGrossBinaryTree.MaxNodeDepth());

            //MovieBinaryTreeRepo rbtr = (MovieBinaryTreeRepo)RBTreeYear.Repo;
            //Console.WriteLine("RedBlack Tree Node Depth: " + rbtr.MoneyGrossBinaryTree.MaxNodeDepth());

            //PrintAndExecuteForRepos(grossRepos, new GrossRevTest() { QueryCnt = 1, MinGross = 10000, MaxGross = 100000 });
            PrintAndExecuteForRepos(grossRepos, new GrossRevTest() { QueryCnt = 10, MinGross = 1000, MaxGross = 1000000 });
        }

        private CreateResult TestRepoGeneration(MovieRepoFactory.Type rt, FirstField ff, List<Movie> movieList)
        {
            return (CreateResult)ExecuteTest(MovieRepoFactory.Repo(rt), new RepoGenerationTest() { MovieList = movieList, Field = ff });
        }       

        private void PrintAndExecuteForRepos(List<IMovieRepo> repoList, ITest t)
        {
            PrintResult(ExecuteTestForRepos(repoList, t));
        }

        private string FileOutput(IResult r)
        {
            StringBuilder a = new StringBuilder();
            a.Append(r.Test.TestDataString());
            a.Append(",");
            a.Append(r.Test.QueryCnt);
            a.Append(",");
            a.Append(r.Repo.Type());
            a.Append(",");
            a.Append(r.Repo.Field.ToString());
            a.Append(",");
            a.Append(r.TestTime.TotalMilliseconds);
            return a.ToString();
        }

        private void PrintResult(IResult r)
        {
            Console.WriteLine(r.ToConsoleString());

            //Move to a config var
            using (StreamWriter sw = File.AppendText("FileParserResults.csv"))
            {                
                sw.WriteLine(FileOutput(r));
            }
        }

        private void PrintResult(ICollection<IResult> results)
        {
            Console.WriteLine();
            if (results.Count() > 0)
            {
                Console.WriteLine(results.ElementAt(0).Test.ToConsoleString());
                var a = results.OrderBy(q => q.TestTime);

                for (int i = 0; i < a.Count(); i++) //Math.Min(a.Count(), 4)
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
                        default:
                            Console.WriteLine((i + 1).ToString() + ":");
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

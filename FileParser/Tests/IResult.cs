using System;
using System.Collections.Generic;
using System.Text;
using FileParser.Repos;

namespace FileParser.Tests
{
    public interface IResult
    {
        IMovieRepo Repo { get; set; }
        ITest Test { get; set; }
        TimeSpan TestTime { get; set; }

        string ToString();
    }

    public interface IQueryResult : IResult
    {
        long? FoundMovieCnt { get; set; }
    }

    public class YearGenreTestResult : IQueryResult
    {
        public IMovieRepo Repo { get; set; }

        public ITest Test { get; set; }

        public long? FoundMovieCnt { get; set; } = null;

        public TimeSpan TestTime { get; set; }

        public override string ToString()
        {
            if (Test == null)
                throw new Exception("ERROR Query not set");

            StringBuilder a = new StringBuilder();
            a.AppendLine("Type: " + Repo.Type() + " FirstField: " + Repo.Field.ToString());
            if (FoundMovieCnt == null)
                a.AppendLine("Error Query Failed");
            else a.AppendLine("Found: " + FoundMovieCnt.ToString());

            a.AppendLine(String.Format("{0:00}:{1:00}.{2:00}",
                TestTime.Minutes, TestTime.Seconds,
                TestTime.Milliseconds / 10));
            return a.ToString();
        }
    }

    public class GrossRevResult : IQueryResult
    {
        public IMovieRepo Repo { get; set; }
        public ITest Test { get; set; }
        public TimeSpan TestTime { get; set; }

        public long? FoundMovieCnt { get; set; } = null;

        public override string ToString()
        {
            StringBuilder a = new StringBuilder();
            a.AppendLine("Type: " + Repo.Type() + " FirstField: " + Repo.Field.ToString());
            if (FoundMovieCnt == null)
                a.AppendLine("Error Query Failed");
            else a.AppendLine("Found: " + FoundMovieCnt.ToString());

            a.AppendLine(String.Format("{0:00}:{1:00}.{2:00}",
                TestTime.Minutes, TestTime.Seconds,
                TestTime.Milliseconds / 10));
            return a.ToString();
        }
    }

    public class CreateResult : IResult
    {
        public IMovieRepo Repo { get; set; }

        public ITest Test { get; set; }

        public TimeSpan TestTime { get; set; }

        public override string ToString()
        {
            //return String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
            //    TestTime.Hours, TestTime.Minutes, TestTime.Seconds,
            //    TestTime.Milliseconds / 10); ;

            StringBuilder a = new StringBuilder();
            a.AppendLine("Repository for: " + Repo.Type());
            a.AppendLine("First Field: " + Repo.Field.ToString());
            a.AppendLine(String.Format("{0:00}:{1:00}.{2:00}",
                TestTime.Minutes, TestTime.Seconds,
                TestTime.Milliseconds / 10));
            return a.ToString();
        }
    }

}

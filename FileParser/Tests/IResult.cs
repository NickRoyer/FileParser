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

        string TimeAsString();

        string ToConsoleString();
    }

    public interface IQueryResult : IResult
    {
        long? FoundMovieCnt { get; set; }
    }

    public abstract class QueryResultBase : IQueryResult
    {
        public virtual TimeSpan TestTime { get; set; }
        public virtual long? FoundMovieCnt { get; set; }
        public virtual IMovieRepo Repo { get; set; }
        public virtual ITest Test { get; set; }

        public virtual string TimeAsString()
        {
            return String.Format("{0:00}:{1:00}.{2:00}",
                TestTime.Minutes, TestTime.Seconds,
                TestTime.Milliseconds / 10);
        }

        public abstract string ToConsoleString();

    }

    public class YearGenreTestResult : QueryResultBase
    {
        public override string ToConsoleString()
        {
            if (Test == null)
                throw new Exception("ERROR Query not set");

            StringBuilder a = new StringBuilder();
            a.AppendLine("Type: " + Repo.Type() + " FirstField: " + Repo.Field.ToString());
            if (FoundMovieCnt == null)
                a.AppendLine("Error Query Failed");
            else a.AppendLine("Found: " + FoundMovieCnt.ToString());

            a.AppendLine(TimeAsString());
            return a.ToString();
        }
    }

    public class GrossRevResult : QueryResultBase
    {
        public override string ToConsoleString()
        {
            StringBuilder a = new StringBuilder();
            a.AppendLine("Type: " + Repo.Type() + " FirstField: " + Repo.Field.ToString());
            if (FoundMovieCnt == null)
                a.AppendLine("Error Query Failed");
            else a.AppendLine("Found: " + FoundMovieCnt.ToString());

            a.AppendLine(TimeAsString());
            return a.ToString();
        }
    }

    public class CreateResult : QueryResultBase
    {
        public override string ToConsoleString()
        {
            //return String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
            //    TestTime.Hours, TestTime.Minutes, TestTime.Seconds,
            //    TestTime.Milliseconds / 10); ;

            StringBuilder a = new StringBuilder();
            a.AppendLine("Repository for: " + Repo.Type());
            a.AppendLine("First Field: " + Repo.Field.ToString());
            a.AppendLine(TimeAsString());
            return a.ToString();
        }
    }

}

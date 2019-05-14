using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;

namespace FileParser
{
    public class MovieFileParser
    {
        public List<Movie> LoadFile(string FileName)
        {
            List<Movie> movies = File.ReadLines(FileName).Skip(1).AsParallel()
                    .Where(l => !string.IsNullOrWhiteSpace(l))
                    .Select(l => new { Line = l, Fields = l.Split(',', StringSplitOptions.RemoveEmptyEntries) })
                    .Select(x => new Movie
                    {
                        Id = long.Parse(x.Fields[0]),
                        GenreId = int.Parse(x.Fields[1]),
                        Genre = x.Fields[2],
                        Name = x.Fields[3],
                        //Fake Test worst case where range is VERY LARGE, note not a great test since there is exactly 1 movie per "year" if using the ID for the year field
                        //Year = long.Parse(x.Fields[0])
                        Year = int.Parse(x.Fields[4]),
                        Gross = long.Parse(x.Fields[5])
                    }).ToList();
            return movies;
        }
    }
}

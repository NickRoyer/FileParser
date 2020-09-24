# FileParser
Performance testing: 

Parsing a file and searching various data structures for data with the goal of identifying the best possible in memory performance.

# File Parsing Performance:
Goal is strictly to compare different methods of parsing a csv file to find the fastest method to read in the data and gather the number of float values.

Linq (Serial): Utilize a standard Linq query to read the file and parse the results into a list of values.

Linq (Parallel): Utilize the "AsParallel" feature of the linq query to as opposed to the standard query

SerialRowLock: Utilize a Parallel for each to read the file HOWEVER merge the results in serial merge of the data after the parallel processing completes.

ParallelMergeLock: Utilize a Parallel.ForEach which merges the results in parallel as well

# File Parser Test Data
1,048,575 rows of test data in a single CSV file in the form:

int (id), string, float, float

# File Parser Test Results

Linq (Serial): 2.09 Seconds

Linq (Parallel): 1.31 Seconds

SerialRowLock: 2.15 seconds

ParallelMergeLock: 1.10 Seconds

** Linq using the AsParallel performed very well for how easy it was to change it to be in Parallel. The fastest method however is the Parallel Merge Lock methodology:

        {
            var numbers = new List<List<float>>();
            /*System.Threading.Tasks.*/
            Parallel.ForEach<string, List<List<float>>>(ReadFile(), //Source collection 
                    () => new List<List<float>>(), // method to initialize the local thread storage
                    (line, loop, localNumbers) => // Each iteration executes the following
                    {
                        localNumbers.Add(ProcessLine(line));
                        return localNumbers;
                    },

                (finalResult) =>
                {
                    lock (numbers)
                    {
                        numbers.AddRange(finalResult);
                    }
                }

            );
            return numbers;
          }



# Querying in Memory Data Structures performance
Goal is to load different data structures with data and then query the data structures for results to determine what the fastest data structures are for this set of test data.

Note: the data file is parsed using the Linq AsParallel method and once parsed the same data is used as source data for each of the collections.

# There are 2 queries tested: 

Movies that were produced between start year and end year for a passed in genre

AND

Movies that had gross revenue in a range of min_rev to max_rev

# Test Data Structures:
** NOTE: The queries perform differently in the first query depending on which set you query for first when implementing the search manually.

# REMOVED from testing

These tests were removed from testing due to their poor performance:

Linq Query against a List (Serial)

Linq Query against a List (Parallel)

Linq Query against a Lookup

# Tested

** NOTE: Each of the remaining have a version By Year, with a Dictionary of movies by Genre AND a Dictionary of movies by Year, by Genre version. 

Dictionary

Sorted Dictionary

Binary Tree 

Red Black Tree  

BTree 

C5 Search Tree Dictionary 

# Test Movie Data 
1,048,575 rows of test movie data in the form:

ID,GenreID,Genre,Name,Year,Gross

int, int, string, string, int, money

# Movie Test Results:

Building the Repository:

They all build in roughly the same order of magnitude with the tree structures taking longer. However it surprised me to see the linq order by took longer to sort a list than it did to build other data structures.

** NOTE: I'm only going to list the first field of the data structure for the winners

# Test 1: All Westerns from 2015 to 2015, 20,000 times

1) Dictionary (Genre) : 2 ms

2) Dictionary (Year) : 2 ms

3) B*Tree (Genre) : 9 ms

# Test 2: All Westerns from 1995 to 2015, 6000 queries

1) Dictionary (Genre) : 4 ms

2) Dictionary (Year) : 9 ms

3) B*Tree (Genre) : 12 ms

# Test 3: All Westerns from 1915 to 2015, 6000 Queries

1) Dictionary (genre) : 19 ms

2) B*Tree (Genre) : 43 ms

3) Lookup (Genre) : 54 ms



# Testing Query 2

Find all movies within a gross rev range:

Min Gross: 1000 Max Gross 1000000, 10 Queries

1) B*Tree (Year) : 139 ms

2) Sorted Dictionary (Year) : 141 ms

3) Binary Tree (Year) : 163 ms


# Movie Testing Conclusion

The data that you are querying against and the range of possible values in the query significantly impact performance of the query. If the data being queried has a very small amount of difference (Say 150 years of movie production) then the Dictionary lookups are favored however if the query is looking for a wide variety of possible monetary amounts (even with the values stored in $1 increments) the search favors a tree implmentation. 








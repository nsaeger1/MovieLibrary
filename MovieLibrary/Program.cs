using System;
using System.Configuration;
using System.IO;
using NLog;

namespace MovieLibrary
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            new Program();
        }

        private string[] main =
        {
            "View All Movies",
            "View Movies by Genre",
            "Add Movies",
            "Exit"
        };

        private string[] first =
        {
            "Action",
            "Adventure",
            "Animation",
            "Children",
            "Comedy",
            "Crime",
            "Documentary",
            "More",
            "Exit"
        };

        private string[] second =
        {
            "Previous",
            "Drama",
            "Fantasy",
            "Film-Noir",
            "Horror",
            "IMAX",
            "Musical",
            "More",
            "Exit"
        };

        private string[] third =
        {
            "Previous",
            "Mystery",
            "Romance",
            "Sci-Fi",
            "Thriller",
            "War",
            "Western",
            "Exit"
        };

        private Program()
        {
            int choice = 0;
            while (choice != 4)
            {
                printMenu(main);
                try
                {
                    choice = promptForChoice();
                    switch (choice)
                    {
                        case 1:
                            printAllMovies();
                            break;
                        case 2:
                            PrintMoviesGenreFirst();
                            break;
                        case 3:
                            addMovie();
                            break;
                        case 4:
                            Console.WriteLine("Goodbye");
                            Environment.Exit(0);
                            break;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Invalid choice");
                }
            }
        }

        private int promptForChoice()
        {
            return int.Parse(Console.ReadLine());
        }

        private void printMenu(string[] items)
        {
            int count = 1;
            Console.WriteLine();
            foreach (var menu in items)
            {
                Console.WriteLine(count++ + ". " + menu);
            }

            Console.WriteLine();
        }


        private void PrintMoviesGenreFirst()
        {
            printMenu(first);
            try
            {
                int choice = promptForChoice();
                if (choice == 1 || choice == 2 || choice == 3 || choice == 4 || choice == 5 || choice == 6 ||
                    choice == 7)
                {
                    printGenre(first[choice - 1]);
                }
                else if (choice == 8)
                {
                    PrintMoviesGenreSecond();
                }
                
            }
            catch (Exception e)
            {
                Console.WriteLine("Invalid choice");
                PrintMoviesGenreFirst();
            }
        }

        private void PrintMoviesGenreSecond()
        {
            printMenu(second);
            try
            {
                int choice = promptForChoice();
                if (choice == 1)
                {
                    PrintMoviesGenreFirst();
                }
                else if (choice == 2 || choice == 3 || choice == 4 || choice == 5 || choice == 6 || choice == 7)
                {
                    printGenre(second[choice - 1]);
                }
                else if (choice == 8)
                {
                    PrintMoviesGenreThird();
                }
                
            }
            catch (Exception e)
            {
                Console.WriteLine("Invalid choice");
                PrintMoviesGenreSecond();
            }
        }

        private void PrintMoviesGenreThird()
        {
            printMenu(third);
            try
            {
                int choice = promptForChoice();
                if (choice == 1)
                {
                    PrintMoviesGenreSecond();
                }
                else if (choice == 2 || choice == 3 || choice == 4 || choice == 5 || choice == 6 || choice == 7)
                {
                    printGenre(third[choice - 1]);
                }
                
            }
            catch (Exception e)
            {
                Console.WriteLine("Invalid choice");
                PrintMoviesGenreSecond();
            }
        }


        private void addMovie()
        {
            string file = "movies.csv";
            int highest = 0;
            

            if (File.Exists(file))
            {
                try
                {
                    StreamReader streamReader = new StreamReader(file);
                    while (!streamReader.EndOfStream)
                    {
                        string line = streamReader.ReadLine();
                        try
                        {
                            int movieID = int.Parse(line.Substring(0, line.IndexOf(',')));
                            if (movieID > highest)
                            {
                                highest = movieID;
                            }
                        }
                        catch (Exception e)
                        {
                        }
                        
                        
                    }
                    streamReader.Close();
                    StreamWriter streamWriter = new StreamWriter(file, append: true);


                    Console.Write("Please enter title: ");
                    string title = Console.ReadLine();
                    Console.Write("Please enter Release Date: ");
                    string releaseDate = Console.ReadLine();
                    Console.Write("Please enter genres separated by comma: ");
                    string genre = Console.ReadLine();
                    string[] temp = genre.Split(',');
                    string genres = "";
                    foreach (var item in temp)
                    {
                        genres = genres + item + "|";
                    }

                    streamWriter.WriteLine("{0},{1},{2},{3}", highest++, title, "(" + releaseDate + ")", genres);
                    streamWriter.Close();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }


            }
        }

        private void printAllMovies()
        {
            string file = "movies.csv";

            if (File.Exists(file))
            {
                StreamReader streamReader = new StreamReader(file);
                while (!streamReader.EndOfStream)
                {
                    string line = streamReader.ReadLine();
                    string title = "Title";
                    string releaseDate = "Date";

                    if (line.Contains("movieId,title,genres"))
                    {
                        Console.WriteLine("{0, -10}{1, -60}{2, -6}{3, -8}", "Movie ID", title, releaseDate, "Genres");
                        continue;
                    }

                    if (line.Contains("\""))
                    {
                        int movieID = int.Parse(line.Substring(0, line.IndexOf(',')));
                        int startPos = line.IndexOf('"') + 1;
                        try
                        {
                            title = line.Substring(line.IndexOf('"') + 1, line.LastIndexOf('(') - startPos);
                            releaseDate = line.Substring(line.IndexOf('(') + 1, 4);
                            try
                            {
                                int.Parse(releaseDate);
                            }
                            catch (Exception e)
                            {
                                releaseDate = "N/A";
                            }
                        }
                        catch (Exception e)
                        {
                            title = line.Substring(line.IndexOf('"') + 1, line.LastIndexOf('"') - startPos);
                            releaseDate = "N/A";
                        }

                        startPos = line.LastIndexOf(',') + 1;
                        string[] genres = line.Substring(line.LastIndexOf(',') + 1, line.Length - startPos).Split('|');

                        Console.Write("{0, -10}{1, -60}{2, -6}", movieID, title, releaseDate);
                        foreach (var item in genres)
                        {
                            Console.Write("{0, -8} ", item);
                        }
                    }
                    else
                    {
                        int movieID = int.Parse(line.Substring(0, line.IndexOf(',')));
                        int startPos = line.IndexOf(',') + 1;
                        if (movieID == 149532)
                        {
                        }

                        try
                        {
                            title = line.Substring(line.IndexOf(',') + 1, line.IndexOf('(') - startPos);
                            releaseDate = line.Substring(line.IndexOf('(') + 1, 4);
                            try
                            {
                                int.Parse(releaseDate);
                            }
                            catch (Exception e)
                            {
                                releaseDate = "N/A";
                            }
                        }
                        catch (Exception e)
                        {
                            title = line.Substring(line.IndexOf(',') + 1, line.LastIndexOf(',') - startPos);
                            releaseDate = "N/A";
                        }

                        startPos = line.LastIndexOf(',') + 1;
                        string[] genres = line.Substring(line.LastIndexOf(',') + 1, line.Length - startPos).Split('|');

                        Console.Write("{0, -10}{1, -60}{2, -6}", movieID, title, releaseDate);
                        foreach (var item in genres)
                        {
                            Console.Write("{0, -8} ", item);
                        }
                    }

                    Console.WriteLine();
                }

                streamReader.Close();
            }
            else
            {
                Console.WriteLine("File does not exist");
            }
        }

        private void printGenre(string genre)
        {
            string file = "movies.csv";

            if (File.Exists(file))
            {
                StreamReader streamReader = new StreamReader(file);
                while (!streamReader.EndOfStream)
                {
                    string line = streamReader.ReadLine();
                    string title = "Title";
                    string releaseDate = "Date";


                    if (line.Contains("movieId,title,genres"))
                    {
                        Console.WriteLine("{0, -10}{1, -60}{2, -6}{3, -8}", "Movie ID", title, releaseDate, "Genres");
                        continue;
                    }

                    if (line.Contains(genre))
                    {
                        if (line.Contains("\""))
                        {
                            int movieID = int.Parse(line.Substring(0, line.IndexOf(',')));
                            int startPos = line.IndexOf('"') + 1;
                            try
                            {
                                title = line.Substring(line.IndexOf('"') + 1, line.LastIndexOf('(') - startPos);
                                releaseDate = line.Substring(line.IndexOf('(') + 1, 4);
                                try
                                {
                                    int.Parse(releaseDate);
                                }
                                catch (Exception e)
                                {
                                    releaseDate = "N/A";
                                }
                            }
                            catch (Exception e)
                            {
                                title = line.Substring(line.IndexOf('"') + 1, line.LastIndexOf('"') - startPos);
                                releaseDate = "N/A";
                            }

                            startPos = line.LastIndexOf(',') + 1;
                            string[] genres = line.Substring(line.LastIndexOf(',') + 1, line.Length - startPos)
                                .Split('|');

                            Console.Write("{0, -10}{1, -60}{2, -6}", movieID, title, releaseDate);
                            foreach (var item in genres)
                            {
                                Console.Write("{0, -8} ", item);
                            }
                        }
                        else
                        {
                            int movieID = int.Parse(line.Substring(0, line.IndexOf(',')));
                            int startPos = line.IndexOf(',') + 1;
                            if (movieID == 149532)
                            {
                            }

                            try
                            {
                                title = line.Substring(line.IndexOf(',') + 1, line.IndexOf('(') - startPos);
                                releaseDate = line.Substring(line.IndexOf('(') + 1, 4);
                                try
                                {
                                    int.Parse(releaseDate);
                                }
                                catch (Exception e)
                                {
                                    releaseDate = "N/A";
                                }
                            }
                            catch (Exception e)
                            {
                                title = line.Substring(line.IndexOf(',') + 1, line.LastIndexOf(',') - startPos);
                                releaseDate = "N/A";
                            }

                            startPos = line.LastIndexOf(',') + 1;
                            string[] genres = line.Substring(line.LastIndexOf(',') + 1, line.Length - startPos)
                                .Split('|');

                            Console.Write("{0, -10}{1, -60}{2, -6}", movieID, title, releaseDate);
                            foreach (var item in genres)
                            {
                                Console.Write("{0, -8} ", item);
                            }
                        }

                        Console.WriteLine();
                    }
                }

                streamReader.Close();
            }
            else
            {
                Console.WriteLine("File does not exist");
            }
        }
    }
}
using MySql.Data.MySqlClient;
using System;

namespace ConsoleApp1
{
    public static class Methods
    {
        public static void InsertUserIntoDatabase(string name, string surname, string username, string email, string password, out bool success)
        {
            string connectionToDatabase = "Server=127.0.0.1;Port=3306;Database=MovieApp;Uid=root;Pwd=admin;";
            MySqlConnection connection = new MySqlConnection(connectionToDatabase);

            try
            {
                connection.Open();
                if (DoesUserExist(email))
                {
                    success = false;
                    return;
                }
                string insertQuery = "INSERT INTO Registration (Name, Surname, Username, Email, Password) VALUES (@Name, @Surname, @Username, @Email, @Password)";

                using (MySqlCommand command = new MySqlCommand(insertQuery, connection))
                {
                    command.Parameters.AddWithValue("@Name", name);
                    command.Parameters.AddWithValue("@Surname", surname);
                    command.Parameters.AddWithValue("@Username", username);
                    command.Parameters.AddWithValue("@Email", email);
                    command.Parameters.AddWithValue("@Password", password);

                    command.ExecuteNonQuery();
                }

                success = true;
            }
            catch (Exception ex)
            {
                success = false;
                Console.WriteLine("Error: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }

        public static bool AuthenticateUser(string email, string password)
        {
            string connectionToDatabase = "Server=127.0.0.1;Port=3306;Database=MovieApp;Uid=root;Pwd=admin;";
            MySqlConnection connection = new MySqlConnection(connectionToDatabase);

            try
            {
                connection.Open();
                string query = "SELECT UserID FROM Registration WHERE email = @email AND Password = @Password";
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@email", email);
                    command.Parameters.AddWithValue("@Password", password);

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        return reader.HasRows;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return false;
            }
            finally
            {
                connection.Close();
            }
        }

        public static void MainMenu()
        {
            Console.WriteLine("[1] Registration");
            Console.WriteLine("[2] Login");
            Console.WriteLine("[3] Quit");
        }

        public static void MovieMenu()
        {
            Console.WriteLine("[1] ADD NEW MOVIE");
            Console.WriteLine("[2] VIEW ALL MOVIES");
            Console.WriteLine("[3] SEARCH MOVIE");
            Console.WriteLine("[4] RATE MOVIE");
            Console.WriteLine("[5] TOP RATED MOVIES");
            Console.WriteLine("[6] LOGOUT");
            Console.WriteLine("[7] QUIT");

            string enteredUserSelection = Console.ReadLine();
            switch (enteredUserSelection)
            {
                case "1":
                    Console.WriteLine("ADD NEW MOVIE");
                    Console.WriteLine("Enter name of movie: ");
                    string nameOfMovie = Console.ReadLine();
                    Console.WriteLine("Enter genre of movie: ");
                    string genre = Console.ReadLine();
                    Console.WriteLine("Enter release year: ");
                    string releaseYear = Console.ReadLine();
                    Console.WriteLine("Enter your rating for movie (1-10): ");
                    int userRating = Convert.ToInt32(Console.ReadLine());
                    AddMovie(nameOfMovie, genre, releaseYear, userRating);
                    break;
                case "2":
                    Console.WriteLine("VIEW ALL MOVIES");
                    ViewAllMovies();
                    break;
                case "3":
                    Console.WriteLine("SEARCH MOVIE");
                    Console.WriteLine("Write the name of the movie you want to search: ");
                    string enteredSearchMovie = Console.ReadLine();
                    SearchMovies(enteredSearchMovie);
                    break;
                case "4":
                    Console.WriteLine("RATE MOVIE");
                    Console.WriteLine("Write the name of the movie you want to rate: ");
                    string movieNameToRate = Console.ReadLine();

                    Console.WriteLine("Enter your username: ");
                    string username = Console.ReadLine();

                    Console.WriteLine("Rate the movie (1-10): ");
                    if (double.TryParse(Console.ReadLine(), out double rating) && rating >= 1 && rating <= 10)
                    {
                        RateMovie(movieNameToRate, username, rating);
                    }
                    else
                    {
                        Console.WriteLine("Invalid rating. Please enter a number between 1 and 10.");
                    }
                    break;
                case "5":
                    Console.WriteLine("TOP RATED MOVIES");
                    GetTopRatedMovies();
                    break;
                case "6":
                    Console.WriteLine("LOGOUT");
                    break;
                case "7":
                    Environment.Exit(0);
                    break;
                 default:
                    Console.WriteLine("Wrong choice");
                    MovieMenu();
                    break;
            }
        }

        public static void GetUserInput(out string name, out string surname, out string username, out string email, out string password)
        {
            Console.WriteLine("Enter your first name:");
            name = Console.ReadLine();
            Console.WriteLine("Enter your last name:");
            surname = Console.ReadLine();
            Console.WriteLine("Enter your username:");
            username = Console.ReadLine();
            Console.WriteLine("Enter your email:");
            email = Console.ReadLine();
            Console.WriteLine("Enter your password:");
            password = Console.ReadLine();
        }

        public static bool DoesUserExist(string email)
        {
            string connectionToDatabase = "Server=127.0.0.1;Port=3306;Database=MovieApp;Uid=root;Pwd=admin;";
            MySqlConnection connection = new MySqlConnection(connectionToDatabase);

            try
            {
                connection.Open();
                string query = "SELECT * FROM Registration WHERE email = @Email";
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Email", email);
                    int count = Convert.ToInt32(command.ExecuteScalar());
                    return count > 0;
                }
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("MySQL Error: " + ex.Message);
                return false;
            }
            finally
            {
                connection.Close();
            }
        }

        public static void AddMovie(string movieName, string genre, string releaseYear, int rating)
        {
            string connectionToDatabase = "Server=127.0.0.1;Port=3306;Database=MovieApp;Uid=root;Pwd=admin;";
            MySqlConnection connection = new MySqlConnection(connectionToDatabase);

            try
            {
                connection.Open();
                string insertQuery = "INSERT INTO Movies (NameOfMovie, Genre, ReleaseYear, Rating) VALUES (@movieName, @genre, @releaseYear, @rating)";
                using (MySqlCommand command = new MySqlCommand(insertQuery, connection))
                {
                    command.Parameters.AddWithValue("@movieName", movieName);
                    command.Parameters.AddWithValue("@genre", genre);
                    command.Parameters.AddWithValue("@releaseYear", releaseYear);
                    command.Parameters.AddWithValue("@rating", rating);
                    command.ExecuteNonQuery();
                    Console.WriteLine("Movie added successfully!");
                }
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("MySQL Error: " + ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }

        public static void ViewAllMovies()
        {
            string connectionToDatabase = "Server=127.0.0.1;Port=3306;Database=MovieApp;Uid=root;Pwd=admin;";
            MySqlConnection connection = new MySqlConnection(connectionToDatabase);

            try
            {
                connection.Open();
                string query = "SELECT * FROM Movies";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int movieID = reader.GetInt32("MoviesID");
                            string movieName = reader.GetString("NameOfMovie");
                            string genre = reader.GetString("Genre");
                            int releaseYear = reader.GetInt32("ReleaseYear");
                            double rating = reader.GetDouble("Rating");

                            Console.WriteLine($"ID: {movieID}, Name: {movieName}, Genre: {genre}, Release Year: {releaseYear}, Rating: {rating}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }

        public static void SearchMovies(string searchMovie)
        {
            string connectionToDatabase = "Server=127.0.0.1;Port=3306;Database=MovieApp;Uid=root;Pwd=admin;";
            MySqlConnection connection = new MySqlConnection(connectionToDatabase);
            try
            {
                connection.Open();
                string query = "SELECT * FROM Movies WHERE NameOfMovie LIKE @searchMovie";
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@searchMovie", "%" + searchMovie + "%");
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                string movieName = reader.GetString("NameOfMovie");
                                string genre = reader.GetString("Genre");
                                int releaseYear = reader.GetInt32("ReleaseYear");
                                double rating = reader.GetDouble("Rating");
                                Console.WriteLine($"Name: {movieName}, Genre: {genre}, Release Year: {releaseYear}, Rating: {rating}");
                            }
                        }
                        else
                        {
                            Console.WriteLine("No movies found");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }
        public static void RateMovie(string movieName, string username, double userRating)
        {
            string connectionToDatabase = "Server=127.0.0.1;Port=3306;Database=MovieApp;Uid=root;Pwd=admin;";
            MySqlConnection connection = new MySqlConnection(connectionToDatabase);

            try
            {
                connection.Open();

                string findUserIDQuery = "SELECT UserID FROM Registration WHERE Username = @username";

                int userID = 0;

                using (MySqlCommand findUserIDCommand = new MySqlCommand(findUserIDQuery, connection))
                {
                    findUserIDCommand.Parameters.AddWithValue("@username", username);
                    userID = Convert.ToInt32(findUserIDCommand.ExecuteScalar());
                }

                if (userID > 0)
                {
                    string findMovieIDQuery = "SELECT MoviesID FROM Movies WHERE NameOfMovie = @movieName";

                    int movieID = 0;

                    using (MySqlCommand findMovieIDCommand = new MySqlCommand(findMovieIDQuery, connection))
                    {
                        findMovieIDCommand.Parameters.AddWithValue("@movieName", movieName);
                        movieID = Convert.ToInt32(findMovieIDCommand.ExecuteScalar());
                    }

                    if (movieID > 0)
                    {
                        string insertRatingQuery = "INSERT INTO MovieRatings (MovieID, UserID, Rating) VALUES (@movieID, @userID, @userRating)";

                        using (MySqlCommand insertRatingCommand = new MySqlCommand(insertRatingQuery, connection))
                        {
                            insertRatingCommand.Parameters.AddWithValue("@movieID", movieID);
                            insertRatingCommand.Parameters.AddWithValue("@userID", userID);
                            insertRatingCommand.Parameters.AddWithValue("@userRating", userRating);

                            int rowsAffected = insertRatingCommand.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                Console.WriteLine("You have rated the movie.");
                            }
                            else
                            {
                                Console.WriteLine("Movie rating failed.");
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("Movie not found.");
                    }
                }
                else
                {
                    Console.WriteLine("User not found.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }
        public static void GetTopRatedMovies()
        {
            string connectionToDatabase = "Server=127.0.0.1;Port=3306;Database=MovieApp;Uid=root;Pwd=admin;";
            MySqlConnection connection = new MySqlConnection(connectionToDatabase);

            try
            {
                connection.Open();

                string query = "SELECT M.NameOfMovie, AVG(MR.Rating) AS AvgRating " +
                               "FROM Movies AS M " +
                               "JOIN MovieRatings AS MR ON M.MoviesID = MR.MovieID " +
                               "GROUP BY M.NameOfMovie " +
                               "ORDER BY AvgRating DESC LIMIT 10";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        Console.WriteLine("Top Rated Movies:");
                        while (reader.Read())
                        {
                            string movieName = reader.GetString("NameOfMovie");
                            double avgRating = reader.GetDouble("AvgRating");

                            Console.WriteLine($"Movie: {movieName}, Average Rating: {avgRating:F2}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }





    }
}

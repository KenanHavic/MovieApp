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

                // Ako je unos uspješan, postavi success na true
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
            Console.WriteLine("[6] RECOMMENDATIONS");
            Console.WriteLine("[7] LOGOUT");
            Console.WriteLine("[8] QUIT");

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
                    break;
                case "5":
                    Console.WriteLine("TOP RATED MOVIES");
                    break;
                case "6":
                    Console.WriteLine("RECOMMENDATIONS");
                    break;
                case "7":
                    Console.WriteLine("LOGOUT");
                    break;
                case "8":
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
    }
}

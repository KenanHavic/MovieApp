using System;

namespace ConsoleApp1

public class Methods
{
    static void InsertUserIntoDatabase(string name, string surname, string username, string email, string password)
    {
        string connectionString = "Server=127.0.0.1;Port=3306;Database=MovieApp;Uid=root;Pwd=admin;";
        MySqlConnection connection = new MySqlConnection(connectionString);

        try
        {
            connection.Open();

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

            Console.WriteLine("Registration successful!");
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

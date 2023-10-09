using System;
using MySql.Data.MySqlClient;


namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            Methods.MainMenu();
            string userSelection = Console.ReadLine();
            switch (userSelection)
                {
                case "1":
                    string enteredName, enteredSurname, enteredUsername, enteredEmail, enteredPassword;
                    bool registrationSuccess = false;

                    do
                    {
                        Methods.GetUserInput(out enteredName, out enteredSurname, out enteredUsername, out enteredEmail, out enteredPassword);
                        Methods.InsertUserIntoDatabase(enteredName, enteredSurname, enteredUsername, enteredEmail, enteredPassword, out registrationSuccess);

                        if (!registrationSuccess)
                        {
                            Console.WriteLine("Registration failed. Please try again.");
                        }
                        else
                        {
                            Console.WriteLine("Registration successful!");
                            Methods.MovieMenu();
                        }
                    } while (!registrationSuccess); 
                    break;

                case "2":
                    bool isAuthenticated = false;
                    while (!isAuthenticated)
                    {
                        Console.WriteLine("Enter your email: ");
                        enteredEmail = Console.ReadLine();
                        Console.WriteLine("Enter your password: ");
                        enteredPassword = Console.ReadLine();
                        isAuthenticated = Methods.AuthenticateUser(enteredEmail, enteredPassword);

                        if (isAuthenticated)
                        {
                            Console.WriteLine("Login successfuly");
                            Methods.MovieMenu();

                        }
                        else
                        {
                            Console.WriteLine("Invalid username or password. Try again.");
                        }
                    }
                    break;
                case "3":
                    Console.WriteLine("Thanks for using our MovieApp");
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("Please try again");
                    break;
            }
        }
    }
}

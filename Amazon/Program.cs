using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace Amazon
{
    class Program
    {
        static void displayItems(SqlConnection Connection)
        {

            SqlCommand command = new SqlCommand($"SELECT *  FROM Items ", Connection);
            SqlDataReader reader = command.ExecuteReader();
            Console.WriteLine("Please take a look at our marketplace!");
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    Console.WriteLine(reader["ItemsId"] + ") " + reader["Name"] + " - $" + reader["Cost"]);
                }
            }
           
            reader.Close();
        }

        static void displayCart (string shopper, SqlConnection Connection)
        {
            
            SqlCommand command = new SqlCommand($"SELECT Connector.ItemName, Connector.ItemNumber FROM Connector JOIN[Shopper] ON [Cart].CartID = [Connector].CartIdentifier Where user='{shopper}'", Connection);
            SqlDataReader reader = command.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    Console.WriteLine(reader["ItemName"]+": "+reader["ItemNumber"]);
                }
            }
            else
            {
                Console.WriteLine("No data found");
            }
            reader.Close();
        }

        static decimal cartMath(string shopper, SqlConnection Connection)
        {
            decimal totalCost = 0m;
            SqlCommand command = new SqlCommand($"SELECT Connector.ItemCost, Connector.ItemNumber FROM Connector JOIN[Shopper] ON [Cart].CartID = [Connector].CartIdentifier Where user='{shopper}'", Connection);
            SqlDataReader reader = command.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    decimal itemCost = Convert.ToDecimal (reader["ItemNumber"])* Convert.ToDecimal(reader["ItemCost"]);
                    totalCost += itemCost;
                }
            }
            else
            {
                Console.WriteLine("No data found");
            }
            reader.Close();
            return totalCost;
        }

        static void Main(string[] args)
        {
            SqlConnection connection;
            connection = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDBFilename=C:\Users\lcben_000\Documents\Visual Studio 2015\Projects\Amazon\Amazon\AmazonDatabase.mdf; Integrated Security=True");
            connection.Open();

            Console.WriteLine("Hi, What would you like your username to be?");
            string shopperName = Console.ReadLine();

            bool keepShopping = true;

            while (keepShopping)
                {
                Console.WriteLine("What would you like to do? Please choose from the following.\n1. View Items\n2. Review Cart\n3. Checkout\n4. Exit");
                string choice = Console.ReadLine();

                if (choice == "1")      // View items
                {
                    displayItems(connection);
                    bool keepAdding = true;
                    while (keepAdding)
                    {
                        Console.WriteLine("Would you like to buy an item, y/n?");
                        string answer = Console.ReadLine().ToLower();
                        if (answer == "y")
                        {
                            Console.WriteLine("Please type in the number corresponding to the item you want to buy?");
                            string itembuying = Console.ReadLine();
                            Console.WriteLine("What quantity?");
                            string itemquantity = Console.ReadLine();
                        }
                        else if (answer == "n")
                        {
                            keepAdding = false;
                        }
                        else
                        {
                            Console.WriteLine("Invalid Choice!");
                        }
                    }   
                }
                else if (choice == "2")      // Review Cart
                {
                    displayCart(shopperName, connection);
                }
                else if (choice == "3")     // Checkout
                {
                    displayCart(shopperName, connection);
                    Console.WriteLine(cartMath(shopperName, connection));
                }
                else if (choice == "4")     // Exit
                {
                    Console.WriteLine("Thanks for shopping!");
                    keepShopping = false;
                }
                else
                {
                    Console.WriteLine("Invalid Choice, please choose again.");
                }
                }
            Console.ReadLine();
        }
    }
}

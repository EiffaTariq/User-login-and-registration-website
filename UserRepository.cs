using Microsoft.Data.SqlClient;

namespace UserLoginAndRegistration.Models
{
    public class UserRepository
    {

        private string conn = "Data Source=(localdb)\\ProjectModels;Initial Catalog=User;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";

        public List<User> GetUsers(string email, string password)
        {
            var users = new List<User>();

            try
            {
                using (SqlConnection connection = new SqlConnection(conn))
                {
                    string query = "SELECT Id, Name, Email, Password, CreatedDate FROM [User] WHERE Email = @Email AND Password = @Password";

                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@Email", email);
                    command.Parameters.AddWithValue("@Password", password);

                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var user = new User
                            {
                                Id = reader.GetInt32(0),                // Id
                                Name = reader.GetString(1),             // Name
                                Email = reader.GetString(2),            // Email
                                Password = reader.GetString(3),         // Password (consider not exposing this in real cases)
                                CreatedDate = reader.GetDateTime(4)     // CreatedDate
                            };

                            users.Add(user);
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("SQL Error: " + ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("General Error: " + ex.Message);
            }

            return users;
        }


        //public List<User> GetUsers(string Email, string Password)
        //{
        //    var users = new List<User>();

        //    try
        //    {
        //        using (SqlConnection connection = new SqlConnection(conn))
        //        {
        //            string query = "SELECT Id, Name, Email,Password,CreatedDate FROM [User]"; // Check table/column names

        //            SqlCommand command = new SqlCommand(query, connection);
        //            connection.Open();

        //            using (SqlDataReader reader = command.ExecuteReader())
        //            {
        //                while (reader.Read())
        //                {
        //                    var user = new User
        //                    {
        //                        Email = reader.GetString(100),
        //                        Password = reader.GetString(100),
        //                    };
        //                    users.Add(user);
        //                }
        //            }
        //        }
        //    }
        //    catch (SqlException ex)
        //    {
        //        Console.WriteLine("SQL Error: " + ex.Message);
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine("General Error: " + ex.Message);
        //    }

        //    return users;
        //}
        public void AddUser(User u)
        {

            SqlConnection sqlConnection = new SqlConnection(conn);
            Console.WriteLine("Enter User Name");
            u.Name = Console.ReadLine();
            Console.WriteLine("Enter User email");
            u.Email = Console.ReadLine();
            Console.WriteLine("Enter password");
            u.Password = Console.ReadLine();
            Console.WriteLine("Enter date");
            u.CreatedDate = DateTime.Parse(Console.ReadLine());
            sqlConnection.Open();
            string query = @"INSERT INTO [User] ( '{Name}','{Email}','{Password}','{CreatedDate}') VALUES ('{u.Name}', '{u.Email}', '{u.Password}','{u.CreatedDate}')";

            SqlCommand cmd = new SqlCommand(query, sqlConnection);

            try
            {
                cmd.ExecuteNonQuery(); // Execute the command
                Console.WriteLine("User inserted successfully.");
            }
            catch (SqlException sqlEx)
            {
                Console.WriteLine("An error occurred while inserting the User: " + sqlEx.Message);
            }
            finally
            {
                sqlConnection.Close();
            }

        }
        public void updateUser(User u)
        {

            SqlConnection sqlConnection = new SqlConnection(conn);

            sqlConnection.Open();

            string query = $"UPDATE [User] set Id = '{u.Id}', Name = '{u.Name}',email = '{u.Email}',password = '{u.Password}',createdDate = '{u.CreatedDate}'";
            SqlCommand cmd = new SqlCommand(query, sqlConnection);
            try
            {
                cmd.ExecuteNonQuery();
                Console.WriteLine("User updated successfully.");
            }
            catch (SqlException sqlEx)
            {
                Console.WriteLine("An error occurred while inserting the user: " + sqlEx.Message);
            }
            finally
            {
                sqlConnection.Close();
            }
        }
        public void deleteUser(int id)
        {
            using (SqlConnection sqlConnection = new SqlConnection(conn))
            {
                sqlConnection.Open();
                string query = "DELETE FROM [User] WHERE Id = '{id}'";
                using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection))
                {

                    try
                    {
                        int count = sqlCommand.ExecuteNonQuery(); // Execute the command

                        if (count > 0)
                        {
                            Console.WriteLine("User deleted successfully.");
                        }
                        else
                        {
                            Console.WriteLine("No user found with the given ID.");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("An error occurred while deleting the user");
                    }
                }
            }
        }

        public List<User> getUserById(int id)
        {
            List<User> users = new List<User>();
            using (SqlConnection sqlConnection = new SqlConnection(conn))
            {
                sqlConnection.Open();

                string query = "SELECT Id, Name, Email, Password, CreatedDate FROM [User] where Id = id";

                using (SqlCommand cmd = new SqlCommand(query, sqlConnection))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            User u = new User
                            {
                                Id = reader.GetInt32(0),
                                Name = reader.GetString(1),
                                Password = reader.GetString(2),
                                CreatedDate = reader.GetDateTime(3),
                                Email = reader.GetString(4)
                            };

                            users.Add(u);
                        }
                    }
                }
            }

            return users;
        }
        public bool IsEmailExists(string Email)
        {
            using (SqlConnection connection = new SqlConnection(conn))
            {
                string queryCheck = "select Email from [User] WHERE Email = @Email";
                SqlCommand command = new SqlCommand(queryCheck, connection);
                command.Parameters.AddWithValue("@Email", Email);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                bool check;
                if (reader.HasRows)
                {
                    check = true;
                }
                else
                {
                    check = false;
                }
                connection.Close();
                return check;

            }
        }
    }
}

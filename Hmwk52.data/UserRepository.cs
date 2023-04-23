using System.Data.SqlClient;
using static System.Net.Mime.MediaTypeNames;

namespace Hmwk52.data
{
    public class Ad
    {
        public string UserName { get; set; }
        public int Id { get; set; }
        public DateTime DateCreated { get; set; }
        public string PhoneNumber { get; set; }
        public string Description { get; set; }
        public int UserId { get; set; }
    }
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
    public class UserRepository
    {
        private string _connectionString;
        public UserRepository(string connectionString)
        {
            _connectionString = connectionString;
        }
        public void Add(Ad ad, int userId)
        {
            using SqlConnection connection = new(_connectionString);
            using SqlCommand command = connection.CreateCommand();
            command.CommandText = $@"INSERT INTO Ads(PhoneNumber, Description, DateCreated, UserId)
VALUES(@phoneNumber,@description,@dateCreated,@userId)";

            command.Parameters.AddWithValue("@phoneNumber", ad.PhoneNumber);
            command.Parameters.AddWithValue("@description", ad.Description);
            command.Parameters.AddWithValue("@dateCreated", DateTime.Now);
            command.Parameters.AddWithValue("@userId", userId);
            connection.Open();
            command.ExecuteNonQuery();
        }
        public List<Ad> GetAds()
        {
            using SqlConnection connection = new(_connectionString);
            using SqlCommand command = connection.CreateCommand();
            command.CommandText = $@"SELECT a.*, u.Name as 'UserName' FROM Ads a
JOIN Users u
on a.UserId = u.Id";
            connection.Open();
            SqlDataReader reader = command.ExecuteReader();
            List<Ad> _ads = new();
            while (reader.Read())
            {
                _ads.Add(new Ad
                {
                    Id = (int)reader["Id"],
                    DateCreated = (DateTime)reader["DateCreated"],
                    PhoneNumber = (string)reader["PhoneNumber"],
                    Description = (string)reader["Description"],
                    UserId = (int)reader["UserId"],
                    UserName = (string)reader["UserName"]
                });

            };

            return _ads;
        }
        public List<Ad> MyAds(int userId)
        {
            using SqlConnection connection = new(_connectionString);
            using SqlCommand command = connection.CreateCommand();
            command.CommandText = $@"SELECT a.*, u.Name as 'UserName' FROM Ads a
JOIN Users u
on a.UserId = u.Id
WHERE u.id = @userId";
            command.Parameters.AddWithValue("@userId", userId);
            connection.Open();
            SqlDataReader reader = command.ExecuteReader();
            List<Ad> _ads = new();
            while (reader.Read())
            {
                _ads.Add(new Ad
                {
                    Id = (int)reader["Id"],
                    DateCreated = (DateTime)reader["DateCreated"],
                    PhoneNumber = (string)reader["PhoneNumber"],
                    Description = (string)reader["Description"],
                    UserId = (int)reader["UserId"],
                    UserName = (string)reader["UserName"]
                });

            };

            return _ads;
        }
        public void Add(User user)
        {
            using SqlConnection connection = new(_connectionString);
            using SqlCommand command = connection.CreateCommand();
            command.CommandText = $@"INSERT INTO Users(Name, Email, Password)
VALUES(@name,@email,@password)";

            command.Parameters.AddWithValue("@name", user.Name);
            command.Parameters.AddWithValue("@email", user.Email);
            string hashPassword = BCrypt.Net.BCrypt.HashPassword(user.Password);
            command.Parameters.AddWithValue("@password", hashPassword);
            connection.Open();
            command.ExecuteNonQuery();
        }       
        public User LoginUserByEmailAndPassword(string email, string password)
        {
           User user = GetByEmail(email);
           if(!BCrypt.Net.BCrypt.Verify(password, user.Password))
            {
                return null;
            }
            return user;
        }
        public User GetByEmail(string email)
        {
            using SqlConnection connection = new(_connectionString);
            using SqlCommand command = connection.CreateCommand();
            command.CommandText = $@"SELECT * FROM Users WHERE Email = @email";
            command.Parameters.AddWithValue("@email", email);

            connection.Open();
            SqlDataReader reader = command.ExecuteReader();
            User user = new();
            if (!reader.Read())
            {
                return null;
            }
            user = new User
            {
                Id = (int)reader["Id"],
                Name = (string)reader["Name"],
                Email = (string)reader["Email"],
                Password = (string)reader["Password"]
            };
            return user;
        }
        public void DeleteAd(int id)
        {
            using SqlConnection connection = new(_connectionString);
            using SqlCommand command = connection.CreateCommand();
            command.CommandText = $@"DELETE FROM Ads WHERE Id = @id";
            command.Parameters.AddWithValue("@id", id);
            connection.Open();
            command.ExecuteNonQuery();
        }
    }
}
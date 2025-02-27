
using Microsoft.Data.Sqlite;

public class UserRepository : IUserRepository
{
    public List<User> GetUsers(int page, int pageSize)
    {
        List<User> users = [];

        using (SqliteConnection connection = new SqliteConnection(@"FileName=UserDb"))
        {
            connection.Open();

            using (SqliteCommand command = connection.CreateCommand())
            {
                command.CommandText = @"SELECT * FROM Users ORDER BY Name LIMIT @pageSize OFFSET @pageXPageSize";
                command.Parameters.Add("@pageSize", SqliteType.Integer).Value = pageSize;
                command.Parameters.Add("@pageXPageSize", SqliteType.Integer).Value = page * pageSize;

                SqliteDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        users.Add(new User(reader.GetGuid(0), reader.GetString(1), reader.GetString(2), reader.GetDateTime(3), reader.GetDateTime(4), reader.GetDateTime(5), reader.GetString(6)));
                    }
                }
            }
        }

        return users;
    }


    public User GetUserById(Guid id)
    {
        User user = new User();

        using (SqliteConnection connection = new SqliteConnection(@"FileName=UserDb"))
        {
            connection.Open();

            using (SqliteCommand command = connection.CreateCommand())
            {
                command.CommandText = @"SELECT * FROM Users WHERE Id = @id";
                command.Parameters.Add("@id", SqliteType.Blob).Value = id;

                SqliteDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        user = new User(reader.GetGuid(0), reader.GetString(1), reader.GetString(2), reader.GetDateTime(3), reader.GetDateTime(4), reader.GetDateTime(5), reader.GetString(6));
                    }
                    return user;
                }
                else
                {
                    return null;
                }
            }
        }
    }

    public void CreateUser(UserDto userDto)
    {
        using (SqliteConnection connection = new SqliteConnection(@"FileName=UserDb"))
        {
            connection.Open();

            using (SqliteCommand command = connection.CreateCommand())
            {
                command.CommandText = @"INSERT INTO Users (Id, Name, Email, DateOfBirth, CreatedAt, UpdatedAt, Password) VALUES (@id, @name, @email, @dateOfBirth, @createdAt, @updatedAt, @password);";
                command.Parameters.Add("@id", SqliteType.Blob).Value = Guid.NewGuid();
                command.Parameters.Add("@name", SqliteType.Text).Value = userDto.Name;
                command.Parameters.Add("@email", SqliteType.Text).Value = userDto.Email;
                command.Parameters.Add("@dateOfBirth", SqliteType.Text).Value = userDto.DateOfBirth;
                command.Parameters.Add("@createdAt", SqliteType.Text).Value = DateTime.Now;
                command.Parameters.Add("@updatedAt", SqliteType.Text).Value = default(DateTime);
                command.Parameters.Add("@password", SqliteType.Text).Value = userDto.Password;

                command.ExecuteNonQuery();
            }
        }
    }

    public int UpdateUser(Guid id, UserDto userDto)
    {
        using (SqliteConnection connection = new SqliteConnection(@"FileName=UserDb"))
        {
            connection.Open();

            using (SqliteCommand command = connection.CreateCommand())
            {
                command.CommandText = @"UPDATE Users SET Name = @name, Email = @email, DateOfBirth = @dateOfBirth, UpdatedAt = @updatedAt, Password = @password WHERE Id = @id";
                command.Parameters.Add("@name", SqliteType.Text).Value = userDto.Name;
                command.Parameters.Add("@email", SqliteType.Text).Value = userDto.Email;
                command.Parameters.Add("@dateOfBirth", SqliteType.Text).Value = userDto.DateOfBirth;
                command.Parameters.Add("@updatedAt", SqliteType.Text).Value = DateTime.Now;
                command.Parameters.Add("@password", SqliteType.Text).Value = userDto.Password;
                command.Parameters.Add("@id", SqliteType.Blob).Value = id;

                int rowsAffected = command.ExecuteNonQuery();
                return rowsAffected;
            }
        }
    }

    public int DeleteUser(Guid id)
    {
        using (SqliteConnection connection = new SqliteConnection(@"FileName=UserDb"))
        {
            connection.Open();

            using (SqliteCommand command = connection.CreateCommand())
            {
                command.CommandText = @"DELETE FROM Users WHERE Id = @id";
                command.Parameters.Add("@id", SqliteType.Blob).Value = id;

                return command.ExecuteNonQuery();
            }
        }
    }
}
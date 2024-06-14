using System;

using Microsoft.Data.Sqlite;

namespace DataSqlite;

public static class SqliteGenerator
{
    public static IEnumerable<User> GenerateUsers()
    {
        // Using a name and a shared cache allows multiple connections to access the same
        // in-memory database
        const string connectionString = "Data Source=InMemorySample;Mode=Memory;Cache=Shared";

        // The in-memory database only persists while a connection is open to it. To manage
        // its lifetime, keep one open connection around for as long as you need it.
        using var connection = new SqliteConnection(connectionString);
        connection.Open();


        var createCommand = connection.CreateCommand();
        createCommand.CommandText = @"
            CREATE TABLE IF NOT EXISTS data (
                user TEXT NOT NULL
            )
            ";
        createCommand.ExecuteNonQuery();

        using (var firstConnection = new SqliteConnection(connectionString))
        {
            firstConnection.Open();

            var updateCommand = firstConnection.CreateCommand();
            updateCommand.CommandText = @"
                INSERT INTO data
                VALUES
                    ('Guest'),
                    ('Admin'),
                    ('Manager')
                ";
            updateCommand.ExecuteNonQuery();

            using (var secondConnection = new SqliteConnection(connectionString))
            {
                secondConnection.Open();
                var queryCommand = secondConnection.CreateCommand();

                queryCommand.CommandText = @"
                    SELECT
                        user
                    FROM
                        data
                    ORDER BY
                        user
                    ";

                using (var reader = queryCommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var value = reader.GetString(0);
                        yield return new User(value);
                    }
                }
            }

            // After all the connections are closed, the database is deleted.
            connection.Close();
        }
    }
}

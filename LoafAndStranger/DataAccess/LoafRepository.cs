using LoafAndStranger.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Data.SqlClient;
using System;

namespace LoafAndStranger.DataAccess
{
    public class LoafRepository
    {
        private Loaf MapLoaf(SqlDataReader reader)
        {
            var id = (int)reader["Id"]; //explicit cast
            var size = (LoafSize)reader["Size"];
            var type = reader["Type"] as string; //implicit casting
            var weightInOunces = (int)reader["WeightInOunces"];
            var price = (decimal)reader["Price"];
            var sliced = (bool)reader["Sliced"];
            var createdDate = (DateTime)reader["CreatedDate"];

            //make a loaf
            var loaf = new Loaf
            {
                Id = id,
                Price = price,
                Size = size,
                Sliced = sliced,
                Type = type,
                WeightInOunces = weightInOunces,
                //createdDate = createdDate,
            };
            return loaf;
        }
        public List<Loaf> GetAll()
        {
            var loaves = new List<Loaf>();
            //create a connection
             using var connection = new SqlConnection("Server=localhost;Database=LoafAndStranger;Trusted_Connection=True;");
            
            //open the connection
            connection.Open();

            //create a command
            var command = connection.CreateCommand();

            //telling the command what you want to do
            var sql = @"SELECT * 
                        FROM Loaves";
            command.CommandText = sql;

            //send the command to sql server
            //execute the command
            var reader = command.ExecuteReader();

            //loop over results
            while (reader.Read()) //reader.read pulls one row at a time from the db
            {
                loaves.Add(MapLoaf(reader));
            }
            return loaves;
        }

        public void Add(Loaf loaf)
        {
            using var connection = new SqlConnection("Server=localhost;Database=LoafAndStranger;Trusted_Connection=True;");
            connection.Open();
            var cmd = connection.CreateCommand();
            cmd.CommandText = @"INSERT INTO [dbo].[Loaves] ([Size] ,[Type] ,[WeightInOunces] ,[Price] ,[Sliced])
            OUTPUT inserted.Id
            VALUES (@Size,@Type,@weightInOunces,@Price,@Sliced)";

            cmd.Parameters.AddWithValue("Size", loaf.Size);
            cmd.Parameters.AddWithValue("Type", loaf.Type);
            cmd.Parameters.AddWithValue("weightInOunces", loaf.WeightInOunces);
            cmd.Parameters.AddWithValue("Price", loaf.Price);
            cmd.Parameters.AddWithValue("Sliced", loaf.Sliced);

            var id = (int)cmd.ExecuteScalar();

            loaf.Id = id;
        }

        public Loaf Get(int id)
        {
            var sql = @"Select * 
                        FROM loaves
                        where Id = @id";
            //create a connection
            using var connection = new SqlConnection("Server=localhost;Database=LoafAndStranger;Trusted_Connection=True;");
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = sql;
            command.Parameters.AddWithValue("id", id);

            var reader = command.ExecuteReader();

            if (reader.Read())
            {
                var loaf = MapLoaf(reader);
                return loaf;
            }

            return null;
        }

        public void Remove(int id)
        {
            using var connection = new SqlConnection("Server=localhost;Database=LoafAndStranger;Trusted_Connection=True;");
            connection.Open();

            var cmd = connection.CreateCommand();
            cmd.CommandText = @"DELETE
                                FROM Loaves 
                                WHERE Id = @id";

            cmd.Parameters.AddWithValue("id", id);

            cmd.ExecuteNonQuery();

            //var loafToRemove = Get(id);
            //_loaves.Remove(loafToRemove);
        }
    }
}

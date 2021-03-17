using LoafAndStranger.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Data.SqlClient;
using Dapper;
using System;

namespace LoafAndStranger.DataAccess
{
    public class LoafRepository
    {
        public List<Loaf> GetAll()
        {
            //create a connection
             using var db = new SqlConnection("Server=localhost;Database=LoafAndStranger;Trusted_Connection=True;");

            //telling the command what you want to do
            var sql = @"SELECT * 
                        FROM Loaves";
            
            var results = db.Query<Loaf>(sql).ToList();
            
            return results;
        }

        public void Add(Loaf loaf)
        { 
        
            var sql = @"INSERT INTO [dbo].[Loaves] ([Size] ,[Type] ,[WeightInOunces] ,[Price] ,[Sliced])
            OUTPUT inserted.Id
            VALUES (@Size,@Type,@weightInOunces,@Price,@Sliced)";
            
            using var db = new SqlConnection("Server=localhost;Database=LoafAndStranger;Trusted_Connection=True;");

            var id = db.ExecuteScalar<int>(sql, loaf);

            loaf.Id = id;
        }

        public Loaf Get(int id)
        {
            var sql = @"Select * 
                        FROM loaves
                        where Id = @id";
            
            using var db = new SqlConnection("Server=localhost;Database=LoafAndStranger;Trusted_Connection=True;");

            var loaf = db.QueryFirstOrDefault<Loaf>(sql, new { id = id });
            return loaf;
        }

        public void Remove(int id)
        {
            using var db = new SqlConnection("Server=localhost;Database=LoafAndStranger;Trusted_Connection=True;");
            var sql = @"DELETE
                        FROM Loaves 
                        WHERE Id = @id";

            db.Execute(sql, new { id });
        }
    }
}

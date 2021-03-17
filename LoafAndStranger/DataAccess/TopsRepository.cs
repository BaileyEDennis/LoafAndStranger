using LoafAndStranger.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;

namespace LoafAndStranger.DataAccess
{
    public class TopsRepository
    {
        const string ConnectionString = "Server=localhost;Database=LoafAndStranger;Trusted_Connection=True;";
        public IEnumerable<Top> GetAll()
        {
            using var db = new SqlConnection(ConnectionString);

            var sql = "SELECT * FROM Tops";

            var results = db.Query<Top>(sql);

            return results;
        }
    }
}

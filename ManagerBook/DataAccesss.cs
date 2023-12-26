using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagerBook
{
    internal class DataAccesss
    {
        public static void InitializeDatabase()
        {
            /*
            using (SqliteConnection db =
                    new SqliteConnection($"Filename=sqliteSample.db"))
            {
                db.Open();
                String tableCommand = "CREATE TABLE IF NOT " +
                    "EXISTS MyTable (Primary_Key INTEGER PRIMARY KEY, " +
                    "Text_Entry NVARCHAR(2048) NULL)";
                SqliteCommand createTable = new SqliteCommand(tableCommand,db);
                createTable.ExecuteReader();
            }
           

            //name email

            using (SqliteConnection db = new SqliteConnection($"Filename=sqLitell.db"))
            {
                db.Open();
                String tableCmd = "CREATE TABLE IF NOT EXISTS Customers " +
                    "(uid INTERGER PRIMARY KEY, " +
                    "first_Name NVARCHAR(2048) NULL ," +
                    "last_Name NVARCHAR(2048) NULL, " +
                    "email NVARCHAR(2048) NULL)";
                SqliteCommand createCmd = new SqliteCommand(tableCmd,db);
                createCmd.ExecuteReader();
            }
             */


        }
        /*
         public static void AppData(int sad, string sad2,string sad3, int sad4)
         {
             using (SqliteConnection db = new SqliteConnection($"Filename=sqliteSample.db"))
             {
                 db.Open();
                 SqliteCommand insertCommand = new SqliteCommand();
                 insertCommand.Connection = db;

                 insertCommand.CommandText = "INSERT INTO Books (ISBN, Title, Description, Price) VALUES (@ISBN, @Title, @Description, @Price);";

                 // Add parameters with values
                 insertCommand.Parameters.AddWithValue("@ISBN", sad);
                 insertCommand.Parameters.AddWithValue("@Title", sad2);
                 insertCommand.Parameters.AddWithValue("@Description", sad3);
                 insertCommand.Parameters.AddWithValue("@Price", sad4);
                 insertCommand.ExecuteReader();
                 db.Close();


             }
         }
        */


        public static List<string> GetData()
        {
            List<string> entries = new List<string>();
            using (SqliteConnection db =
              new SqliteConnection($"Filename=sqliteSample.db"))
            {
                db.Open();
                SqliteCommand selectCommand = new SqliteCommand
                    ("SELECT * FROM Customers", db);
                SqliteDataReader query = selectCommand.ExecuteReader();
                while (query.Read())
                {
                    entries.Add(query.GetString(1));
                    entries.Add(query.GetString(2));
                    entries.Add(query.GetString(3));
                }
                db.Close();
            }
            return entries;
        }

    }
}

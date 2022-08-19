using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using Windows.Storage;
using System.IO;
using System.Diagnostics;

namespace SQLite_Library
{
    public static class DataAccess
    {
        public async static void InitializeDatabase()
        {
            await ApplicationData.Current.LocalFolder.CreateFileAsync("collection.db", CreationCollisionOption.OpenIfExists);
            string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "collection.db");
            Debug.WriteLine($"database: {dbpath}");
            using (SqliteConnection db =
               new SqliteConnection($"Filename={dbpath}"))
            {
                db.Open();

                String tableCommand = "CREATE TABLE IF NOT EXISTS MapDB (" +
                    "Primary_Key INTEGER PRIMARY KEY, " +
                    "FolderName TEXT NULL, " +
                    "AudiofileName TEXT NULL, " +
                    "Title TEXT NULL, " +
                    "TitleUni TEXT NULL, " +
                    "Artist TEXT NULL, " +
                    "ArtistUni TEXT NULL, " +
                    "Creater TEXT NULL, " +
                    "BeatmapID INTEGER NULL " +
                    ")";

                SqliteCommand createTable = new SqliteCommand(tableCommand, db);

                createTable.ExecuteReader();
            }
        }

        public static void AddData(string inputText)
        {
            string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "collection.db");
            using (SqliteConnection db = new SqliteConnection($"Filename={dbpath}"))
            {
                var command = inputText.Split(", ");

                db.Open();

                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = db;

                insertCommand.CommandText = "INSERT INTO MapDB (Primary_Key, FolderName, AudiofileName, Title, TitleUni, Artist, ArtistUni, Creater, BeatmapID) VALUES" +
                    "(NULL, @FolderName, @AudiofileName, @Title, @TitleUni, @Artist, @ArtistUni, @Creater, @BeatmapID);";
                insertCommand.Parameters.AddWithValue("@FolderName", command[0]);
                insertCommand.Parameters.AddWithValue("@AudiofileName", command[1]);
                insertCommand.Parameters.AddWithValue("@Title", command[2]);
                insertCommand.Parameters.AddWithValue("@TitleUni", command[3]);
                insertCommand.Parameters.AddWithValue("@Artist", command[4]);
                insertCommand.Parameters.AddWithValue("@ArtistUni", command[5]);
                insertCommand.Parameters.AddWithValue("@Creater", command[6]);
                insertCommand.Parameters.AddWithValue("@BeatmapID", command[7]);

                insertCommand.ExecuteReader();

                db.Close();
            }
        }
    }
}

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
    }
}

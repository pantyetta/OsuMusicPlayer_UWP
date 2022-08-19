using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using Windows.Storage;
using System.IO;
using System.Diagnostics;
using System.Collections.ObjectModel;


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
                    "BeatmapID INTEGER NULL, " +
                    "Picture TEXT NULL " +
                    ")";

                SqliteCommand createTable = new SqliteCommand(tableCommand, db);

                createTable.ExecuteReader();
            }
        }

        public static void AddData(Metadata inputdata)
        {
            string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "collection.db");
            using (SqliteConnection db = new SqliteConnection($"Filename={dbpath}"))
            {
                db.Open();

                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = db;

                insertCommand.CommandText = "INSERT INTO MapDB (Primary_Key, FolderName, AudiofileName, Title, TitleUni, Artist, ArtistUni, Creater, BeatmapID, Picture) VALUES" +
                    "(NULL, @FolderName, @AudiofileName, @Title, @TitleUni, @Artist, @ArtistUni, @Creater, @BeatmapID, @Picture);";
                insertCommand.Parameters.AddWithValue("@FolderName", inputdata.FolderPath);
                insertCommand.Parameters.AddWithValue("@AudiofileName", inputdata.AudioFilename);
                insertCommand.Parameters.AddWithValue("@Title", inputdata.Title);
                insertCommand.Parameters.AddWithValue("@TitleUni", inputdata.TitleUnicode);
                insertCommand.Parameters.AddWithValue("@Artist", inputdata.Artist);
                insertCommand.Parameters.AddWithValue("@ArtistUni", inputdata.ArtistUnicode);
                insertCommand.Parameters.AddWithValue("@Creater", inputdata.Creator);
                insertCommand.Parameters.AddWithValue("@BeatmapID", inputdata.BeatmapID);
                insertCommand.Parameters.AddWithValue("@Picture", inputdata.Picture);

                insertCommand.ExecuteReader();

                db.Close();
            }
        }

        public static Collection<Metadata> GetData()
        {
            Collection<Metadata> entries = new Collection<Metadata>();

            string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "collection.db");
            using(SqliteConnection db = new SqliteConnection($"Filename={dbpath}"))
            {
                db.Open();

                SqliteCommand selectCommand = new SqliteCommand("Select * from MapDB", db);
                SqliteDataReader query = selectCommand.ExecuteReader();

                while (query.Read())
                {
                    entries.Add(
                        new Metadata
                        {
                            FolderPath = query.GetString(1),
                            AudioFilename = query.GetString(2),
                            Title = query.GetString(3),
                            TitleUnicode = query.GetString(4),
                            Artist = query.GetString(5),
                            ArtistUnicode = query.GetString(6),
                            Creator = query.GetString(7),
                            BeatmapID = query.GetInt32(8),
                            Picture = query.GetString(9),
                        }
                     );
                }

                db.Close();
            }

            return entries;
        }
    }

    public class Metadata
    {
        public string FolderPath { get; set; }
        public string AudioFilename { get; set; }
        public string Title { get; set; }
        public string TitleUnicode { get; set; }
        public string Artist { get; set; }
        public string ArtistUnicode { get; set; }
        public string Creator { get; set; }
        public int BeatmapID { get; set; }
        public string Picture { get; set; }

        public Metadata()
        {
            FolderPath = "";
            AudioFilename = "";
            Title = "";
            TitleUnicode = "";
            Artist = "";
            ArtistUnicode = "";
            Creator = "";
            BeatmapID = -1;
            Picture = "";
        }
    }
}

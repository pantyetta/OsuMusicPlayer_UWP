using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using SQLite_Library;

namespace OsuMusicPlayer_UWP
{
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

        public Metadata() {
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

    public class DataBaseViewModel
    {
        //private Metadata defaultDataBase = new Metadata();
        //public Metadata DefaultDataBase { get { return this.defaultDataBase; } }


        static private ObservableCollection<Metadata> _databases = new ObservableCollection<Metadata>();
        public ObservableCollection<Metadata> Databases { get { return _databases; } set { _databases = value; } }
        public Metadata setDatabase { 
            set {
                DataAccess.AddData(new SQLite_Library.Metadata
                {
                    FolderPath = value.FolderPath,
                    AudioFilename = value.AudioFilename,
                    Title = value.Title,
                    TitleUnicode = value.TitleUnicode,
                    Artist = value.Artist,
                    ArtistUnicode = value.ArtistUnicode,
                    Creator = value.Creator,
                    BeatmapID = value.BeatmapID,
                    Picture = value.Picture,
                });
            }
        }

        public void CreateCashe()
        {
            var db = DataAccess.GetData();
            _databases.Clear();
            foreach (var item in db)
            {
                _databases.Add(new Metadata
                {
                    FolderPath = item.FolderPath,
                    AudioFilename = item.AudioFilename,
                    Title = item.Title,
                    TitleUnicode = item.TitleUnicode,
                    Artist = item.Artist,
                    ArtistUnicode = item.ArtistUnicode,
                    Creator = item.Creator,
                    BeatmapID = item.BeatmapID,
                    Picture = item.Picture,
                });
            }
        }
        //static private Collection<Metadata> databases = new Collection<Metadata>();    //更新すると関連するUIも変わる & 常に読み込まれるよ
        //public Collection<Metadata> Databases { get { return databases; } }  //データ取得
        //public Metadata setDatabas { set{ databases.Add(value); } } //データ追加
    }
}

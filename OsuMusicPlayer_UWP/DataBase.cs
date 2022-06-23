using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace OsuMusicPlayer_UWP
{
    public class Metadata
    {
        public string AudioFilename { get; set; }
        public string Title { get; set; }
        public string TitleUnicode { get; set; }
        public string Artist { get; set; }
        public string ArtistUnicode { get; set; }
        public string Creator { get; set; }
        public int BeatmapID { get; set; }
        public StorageFolder MapFolder { get; set; }
        public Metadata() {
            AudioFilename = "";
            Title = "";
            TitleUnicode = "";
            Artist = "";
            ArtistUnicode = "";
            Creator = "";
            BeatmapID = -1;
            MapFolder = null;
        }
    }

    public class DataBaseViewModel
    {
        //private Metadata defaultDataBase = new Metadata();
        //public Metadata DefaultDataBase { get { return this.defaultDataBase; } }

        static private ObservableCollection<Metadata> _databases = new ObservableCollection<Metadata>();
        public ObservableCollection<Metadata> Databases { get { return _databases; } set { _databases = value; } }
        public Metadata setDatabase { set { _databases.Add(value); } }
        //static private Collection<Metadata> databases = new Collection<Metadata>();    //更新すると関連するUIも変わる & 常に読み込まれるよ
        //public Collection<Metadata> Databases { get { return databases; } }  //データ取得
        //public Metadata setDatabas { set{ databases.Add(value); } } //データ追加
    }
}

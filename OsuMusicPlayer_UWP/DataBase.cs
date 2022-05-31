using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }

    public class DataBaseViewModel
    {
        private Metadata defaultDataBase = new Metadata();
        public Metadata DefaultDataBase { get { return this.defaultDataBase; } }

        static private ObservableCollection<Metadata> databases = new ObservableCollection<Metadata>();    //更新すると関連するUIも変わる & 常に読み込まれるよ
         public ObservableCollection<Metadata> Databases { get { return databases; } }   //データ取得
        public Metadata setDatabas { set{ databases.Add(value); } } //データ追加

    }
}

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OsuMusicPlayer_UWP
{
    public class DataBase
    {
        public string FileName { get; set; }

        public DataBase()   //初期値設定
        {
            this.FileName = "testやで";
        }
    }

    public class DataBaseViewModel
    {
        private DataBase defaultDataBase = new DataBase();
        public DataBase DefaultDataBase { get { return this.defaultDataBase; } }

        static private ObservableCollection<DataBase> databases = new ObservableCollection<DataBase>();    //更新すると関連するUIも変わる & 常に読み込まれるよ
         public ObservableCollection<DataBase> Databases { get { return databases; } }   //取得
        public string setDatabas
        {
            set
            {    //追加
                databases.Add(new DataBase()
                {
                    FileName = value
                });
            }
        }

        public DataBaseViewModel()  //初期化
        {
            //databases.Add(new DataBase()
            //{
            //    FileName = "test File name."
            //});
        }
    }
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.Core;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// 空白ページの項目テンプレートについては、https://go.microsoft.com/fwlink/?LinkId=234238 を参照してください

namespace OsuMusicPlayer_UWP
{
    /// <summary>
    /// それ自体で使用できる空白ページまたはフレーム内に移動できる空白ページ。
    /// </summary>
    public sealed partial class MusicPage : Page
    {
        public DataBaseViewModel ViewModel { get; set; }
        public MusicPlayer musicPlayer { get; set; }

        UI frontend = new UI();
        MusicPlayList musicPlayList = new MusicPlayList();
        DataBaseViewModel dataBaseViewModel = new DataBaseViewModel();

        public MusicPage()
        {
            this.InitializeComponent();
            //this.NavigationCacheMode = NavigationCacheMode.Enabled;
            this.ViewModel = new DataBaseViewModel();
            this.musicPlayer = new MusicPlayer();
            musicPlayer.Musicplayer.Volume = 0.05;
        }


        private async void Music_ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            musicPlayList.Clear();
                        
            for (int i = Music_ListView.SelectedIndex; i < dataBaseViewModel.Databases.Count; i++)  //選択されたものから後ろをプレイリストに追加
            {
                musicPlayList.Add(dataBaseViewModel.Databases[i]);
            }

            var selectmeatadata = musicPlayList.GetMusicPlaylist[0];    //選択されたアイテムのMetadata

            StorageFile audioFile = await selectmeatadata.MapFolder.GetFileAsync(selectmeatadata.AudioFilename);    //オーディオファイル読み込み
            musicPlayer.Musicplayer.Source = MediaSource.CreateFromStorageFile(audioFile);  //メディアにセット

            musicPlayer.Musicplayer.Play();

            Debug.WriteLine(selectmeatadata.AudioFilename);
            frontend.Title = selectmeatadata.Title;
            frontend.Artist = selectmeatadata.Artist;
        }
    }
}

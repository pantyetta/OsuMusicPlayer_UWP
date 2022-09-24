using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Windows.Media.Core;
using Windows.Media.Playback;
using Windows.Storage;
using Windows.Storage.AccessCache;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace OsuMusicPlayer_UWP
{
    public class MusicPlayer
    {
        static private bool MediaEnded = false;
        private bool SourceChange = false;

        private readonly MusicPlayList MusicPlayList = new MusicPlayList();
        static private MediaPlayer Static_MusicPlayer = new MediaPlayer();        
        UI frontend = new UI();
        Storage storage = new Storage();

        public MusicPlayer()
        {
            Static_MusicPlayer.SourceChanged += Static_MusicPlayer_SourceChanged;   // 再生メディア変更
            Static_MusicPlayer.MediaEnded += Static_MusicPlayer_MediaEnded;    //次の曲再生
            Musicplayer.Volume = 0.05;
        }

        private async void Static_MusicPlayer_SourceChanged(MediaPlayer sender, object args)
        {
            if (SourceChange) return;
            SourceChange = true;

            Metadata nowplay = MusicPlayList.CurrentItem();
            Debug.WriteLine(nowplay.AudioFilename);
            frontend.Title = nowplay.Title;
            frontend.Artist = nowplay.Artist;

            frontend.Picture = await storage.getImageAsync(nowplay.FolderPath, nowplay.Picture);

            //BitmapImage bitmapImage = new BitmapImage();
            //bitmapImage.SetSource(filestream);
            //frontend.Picture.SetSource(filestream);

            SourceChange = false;
        }

        private async void Static_MusicPlayer_MediaEnded(MediaPlayer sender, object args) 
        {
            if (MediaEnded) return;
            MediaEnded = true;

            MusicPlayList.MoveNext();
            Metadata Next = MusicPlayList.CurrentItem();    //選択されたアイテムのMetadata

            StorageFolder OsuFolder = await StorageApplicationPermissions.FutureAccessList.GetFolderAsync("OsuFolderToken");
            StorageFile AudioFile = await OsuFolder.GetFileAsync($"Songs\\{Next.FolderPath}\\{Next.AudioFilename}");
            Musicplayer.Source = MediaSource.CreateFromStorageFile(AudioFile);  //メディアにセット

            Debug.WriteLine("next track");
            Musicplayer.Play();

            MediaEnded = false;
        }

        public MediaPlayer Musicplayer { get { return Static_MusicPlayer; } set { Static_MusicPlayer = value; } }
    }

    public class MusicPlayList
    {
        static private int index = 0;
        static private Collection<Metadata> Static_MusicPlaylist = new Collection<Metadata>();

        public Collection<Metadata> GetMusicPlaylist { get { return Static_MusicPlaylist; } }
        public void Clear()
        {
            Static_MusicPlaylist.Clear();    //プレイリストclear
            index = 0;
        }
        public void Add(Metadata metadata) => Static_MusicPlaylist.Add(metadata);   //Add Playlist
        public Metadata CurrentItem() => Static_MusicPlaylist[index];  // 現在のMetadataを返す
        public Metadata GetItem(int index) => Static_MusicPlaylist[index];
        public int CurrentItemIndex() => index; //現在の番号を返す
        public void MoveNext() => index++; //一つ次のMetadata
        public void MovePrevious() => index--; //一つ前のMetadata
    }
}

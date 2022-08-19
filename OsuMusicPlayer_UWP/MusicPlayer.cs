using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Media.Core;
using Windows.Media.Playback;
using Windows.Storage;
using Windows.Storage.AccessCache;

namespace OsuMusicPlayer_UWP
{
    public class MusicPlayer
    {
        static private bool MediaEnded = false;
        static private bool SourceChange = false;

        private readonly MusicPlayList MusicPlayList = new MusicPlayList();
        static private MediaPlayer Static_MusicPlayer = new MediaPlayer();        
        UI frontend = new UI();

        public MusicPlayer()
        {
            Static_MusicPlayer.SourceChanged += Static_MusicPlayer_SourceChanged;
            Static_MusicPlayer.MediaEnded += Static_MusicPlayer_MediaEndedAsync;    //再生終了
            Musicplayer.Volume = 0.05;
        }

        private void Static_MusicPlayer_SourceChanged(MediaPlayer sender, object args)
        {
            if (SourceChange) return;
            SourceChange = true;

            var nowplay = MusicPlayList.CurrentItem();
            Debug.WriteLine(nowplay.AudioFilename);
            frontend.Title = nowplay.Title;
            frontend.Artist = nowplay.Artist;

            SourceChange = false;
        }

        private async void  Static_MusicPlayer_MediaEndedAsync(MediaPlayer sender, object args) //次の曲再生
        {
            if (MediaEnded) return;
            MediaEnded = true;

            var select = MusicPlayList.MoveNext();    //選択されたアイテムのMetadata

            StorageFolder OsuFolder = await StorageApplicationPermissions.FutureAccessList.GetFolderAsync("OsuFolderToken");
            StorageFile AudioFile = await OsuFolder.GetFileAsync($"Songs\\{select.FolderPath}\\{select.AudioFilename}");
            //StorageFile audioFile = await select.MapFolder.GetFileAsync(select.AudioFilename);    //オーディオファイル読み込み
            Musicplayer.Source = MediaSource.CreateFromStorageFile(AudioFile);  //メディアにセット

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
        public int CurrentItemIndex() => index; //現在の番号を返す
        public Metadata MoveNext()  //一つ次のMetadata
        {
            index++;
            return Static_MusicPlaylist[index];
        }

        public Metadata MovePrevious()  //一つ前のMetadata
        {
            index--;
            return Static_MusicPlaylist[index];
        }

    }
}

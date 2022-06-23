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

namespace OsuMusicPlayer_UWP
{
    public class MusicPlayer
    {
        private readonly MusicPlayList MusicPlayList = new MusicPlayList();
        static private MediaPlayer Static_MusicPlayer = new MediaPlayer();
        public MusicPlayer()
        {
            Static_MusicPlayer.MediaEnded += Static_MusicPlayer_MediaEndedAsync;
        }

        private async void Static_MusicPlayer_MediaEndedAsync(MediaPlayer sender, object args)
        {
            MusicPlayList.MoveNext();
            var selectmeatadata = MusicPlayList.CurrentItem();    //選択されたアイテムのMetadata

            StorageFile audioFile = await selectmeatadata.MapFolder.GetFileAsync(selectmeatadata.AudioFilename);    //オーディオファイル読み込み
            Musicplayer.Source = MediaSource.CreateFromStorageFile(audioFile);  //メディアにセット

            Musicplayer.Play();

            Debug.WriteLine(selectmeatadata.AudioFilename);
        }

        public MediaPlayer Musicplayer { get { return Static_MusicPlayer; } set { Static_MusicPlayer = value; } }
    }

    public class MusicPlayList
    {
        static private int index;
        static private Collection<Metadata> Static_MusicPlaylist = new Collection<Metadata>();

        public Collection<Metadata> GetMusicPlaylist { get { return Static_MusicPlaylist; } }
        public void Clear() => Static_MusicPlaylist.Clear();    //プレイリストclear
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

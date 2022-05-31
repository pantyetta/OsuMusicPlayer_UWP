using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.AccessCache;

namespace OsuMusicPlayer_UWP
{
    public class Storage
    {
        private StorageFolder storageFolder { get; set; }
    
        public async Task CheckAccessListAsycn()
        {
            storageFolder = await StorageApplicationPermissions.FutureAccessList.GetFolderAsync("OsuFolderToken");
        }


        public async Task PickeFolderAsync()
        {
            var FolderPicker = new Windows.Storage.Pickers.FolderPicker();
            FolderPicker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.Desktop;
            FolderPicker.FileTypeFilter.Add("*");


            StorageFolder storageFolder = await FolderPicker.PickSingleFolderAsync();
            if (storageFolder != null)
            {
                try
                {
                    if(storageFolder.TryGetItemAsync("osu!.exe") != null) { 
                        this.storageFolder = storageFolder;
                        //アクセスリストを更新する
                        StorageApplicationPermissions.FutureAccessList.AddOrReplace("OsuFolderToken", this.storageFolder);  
                        Debug.WriteLine("Picked Folder: " + this.storageFolder.Path);
                    }
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);
                }


            }
            else
            {
                Debug.WriteLine("can't picked folder");
            }
        }

        public async Task LoadSongListAsync()
        {
            var databases = new DataBaseViewModel();
            try { 
                StorageFolder songsFolder = await storageFolder.GetFolderAsync("Songs");
                foreach (var item in await songsFolder.GetFoldersAsync())
                {
                    databases.setDatabas = item.Name;
                }
            }
            catch(Exception e)
            {
                Debug.WriteLine(e.Message);
            }
                
        }

    }

    /// <summary>
    /// 曲のフォルダを提供すると整頓されたメタデータを返す
    /// </summary>
    class Decoder
    {
        async public void ReadFiles(StorageFolder MapFolder)
        {
            try {
                IReadOnlyList<StorageFile> fileList = await MapFolder.GetFilesAsync();
                foreach (StorageFile file in fileList)
                {
                    if (file.FileType == ".osu")     //ファイルリストから.osuだけ
                    {
                        Metadata metadata = Converter(file);
                        //ファイルを読んでConverterに送る
                        //Converterで必要なmetadataを返してもらう
                    }
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }

        private Metadata Converter(StorageFile osuFile)
        {
            var metadata = new Metadata();

            //データを読んでメタデータに入れる

            return metadata;
        }
    }

    class Metadata
    {
        public string AudioFilename { get; set; }
        public string Title { get; set; }
        public string TitleUnicode { get; set; }
        public string Artist { get; set; }
        public string ArtistUnicode { get; set; }
        public string Creator { get; set; }
        public int BeatmapID { get; set; }
    }

}

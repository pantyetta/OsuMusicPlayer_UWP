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
        public StorageFolder GetStorageFolder { get { return storageFolder; } }
    
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
            var decoder = new Decoder();
            try { 
                StorageFolder songsFolder = await storageFolder.GetFolderAsync("Songs");
                foreach (var MapFolder in await songsFolder.GetFoldersAsync())   //Songsからマップフォルダ
                {
                    //databases.setDatabas = item.Name;
                    var metadataList = await decoder.ReadFilesAsync(MapFolder); //デコーダからメタデータの配列もらう
                    foreach(var metadata in metadataList)
                    {
                        databases.setDatabas = metadata;    //データベースに追加
                    }
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
        async public Task<List<Metadata>> ReadFilesAsync(StorageFolder MapFolder)
        {
            var metadatas = new List<Metadata>();   //複数に対応できるように配列に格納

            try {
                IReadOnlyList<StorageFile> fileList = await MapFolder.GetFilesAsync();
                                                        
                foreach (StorageFile file in fileList)  //曲フォルダーから.osuを読み込む
                {
                    if (file.FileType == ".osu")     
                    {
                        var metadata = await ConverterAsync(file);
                        //metadata.Result.Title
                        //ファイルを読んでConverterに送る
                        //Converterで必要なmetadataを返してもらう
                        //AudioFileが同じじゃないなら追加
                        if(metadatas.Count != 0 && metadata.AudioFilename != metadatas[metadatas.Count-1].AudioFilename)
                            metadatas.Add(metadata);
                        else if (metadatas.Count == 0)  //配列が空なら追加
                        {
                            metadatas.Add(metadata);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }

            return metadatas;
        }

        /// <summary>
        /// .osuファイルからメタデータ生成
        /// </summary>
        /// <param name="osuFile"></param>
        /// <returns></returns>
        async private Task<Metadata> ConverterAsync(StorageFile osuFile)
        {
            var metadata = new Metadata();

            try { 
                //データを読んでメタデータに入れる
                var buffer = await FileIO.ReadBufferAsync(osuFile);
                using(var dataReader = Windows.Storage.Streams.DataReader.FromBuffer(buffer))
                {
                    string text = dataReader.ReadString(buffer.Length);
                    foreach (string textblock in text.Split("\r\n\r\n"))    //段落ごとに読み込む
                    {
                        string[] textline = textblock.Split("\r\n");
                        if (textline[0] == "[Difficulty]") //必要データを過ぎたら読み込み終了
                            break;

                        //一行ずつ読み込み (先頭行はセクション名なのでスキップ)
                        for (int i = 1; i < textline.Length; i++)
                        {
                            string[] content = textline[i].Split(":");
                            switch (content[0])
                            {
                                case "AudioFilename":   
                                    metadata.AudioFilename = content[1].TrimStart();
                                    break;

                                case "Title":   
                                    metadata.Title = content[1].TrimStart();
                                    break;

                                case "TitleUnicode":  
                                    metadata.TitleUnicode = content[1].TrimStart();
                                    break;

                                case "Artist":   
                                    metadata.Artist = content[1].TrimStart();
                                    break;

                                case "ArtistUnicode":  
                                    metadata.ArtistUnicode = content[1].TrimStart();
                                    break;

                                case "Creator":  
                                    metadata.Creator = content[1].TrimStart();
                                    break;
                                
                                case "BeatmapID":
                                    metadata.BeatmapID = int.Parse(content[1]);
                                    break;

                                default:
                                    break;
                            }
                        }
                    }
                
                }
            }catch(Exception e)
            {
                Debug.WriteLine(e.Message);
            }

            return metadata;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
                var MapFolders = await songsFolder.GetFoldersAsync();
                for (int i = 0; i < MapFolders.Count; i++)
                {
                    if (i % 50 == 0)
                    {
                        databases.CreateCashe();
                        Debug.WriteLine("{0} / {1}", i, MapFolders.Count);
                    }
                    //databases.setDatabas = item.Name;
                    var metadataList = await decoder.FolderSerach(MapFolders[i]); //デコーダからメタデータの配列もらう
                    foreach (var metadata in metadataList)
                    {
                        databases.setDatabase = metadata;  //データベースに追加
                    }
                    decoder.clear();
                }
            }
            catch(Exception e)
            {
                Debug.WriteLine("error: addDB" + e.Message);
            }
            
            Debug.WriteLine("Metadata load ok.");
            databases.CreateCashe();
        }

    }

    /// <summary>
    /// 曲のフォルダを提供すると整頓されたメタデータを返す
    /// </summary>
    class Decoder
    {
        private List<Metadata> metadatas = new List<Metadata>();   //複数対応
        /// <summary>
        /// 
        /// </summary>
        /// <param name="MapFolder">各マップのフォルダー</param>
        public async Task<List<Metadata>> FolderSerach(StorageFolder MapFolder)
        {
           var list = await MapFolder.GetFilesAsync();

            foreach(StorageFile file in list)
            {
                if (file.FileType != ".osu") continue;  // マップファイルじゃなかったら飛ばす

                await ReadFileAsync(file);
                foreach(var metadata in metadatas)
                {
                    metadata.FolderPath = MapFolder.Name;
                }

            }
            return metadatas;
        }

        private async Task ReadFileAsync(StorageFile osuFile)
        {
            var metadata = new Metadata();

            try
            {
                //データを読んでメタデータに入れる
                var buffer = await FileIO.ReadBufferAsync(osuFile);
                using (var dataReader = Windows.Storage.Streams.DataReader.FromBuffer(buffer))
                {
                    string text = dataReader.ReadString(buffer.Length);
                    foreach (string textblock in text.Split("\r\n\r\n"))    //段落ごとに読み込む
                    {
                        string[] textline = textblock.Split("\r\n");


                        if (textline[0] == "[Events]")  //サムネ取得
                        {
                            try
                            {
                                metadata.Picture = textline[2].Split('"')[1];
                            }
                            catch(IndexOutOfRangeException)   // BGファイルが設定されていない
                            {
                                Debug.WriteLine($"Skip set Picture: {metadata.Title}");
                            }
                            break;
                        } 

                        if (textline[0] == "[TimingPoints]") //必要データを過ぎたら読み込み終了
                            break;

                        //一行ずつ読み込み (先頭行はセクション名なのでスキップ)
                        for (int i = 1; i < textline.Length; i++)
                        {
                            string[] content = textline[i].Split(":");
                            switch (content[0])
                            {
                                case "AudioFilename":
                                    var AudiofileName = content[1].TrimStart();
                                    if (Check(AudiofileName))
                                        metadata.AudioFilename = AudiofileName;
                                    else
                                        return;
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

                                case "[Difficulty]":
                                    goto NextBlock;

                                default:
                                    break;
                            }
                        }

                        NextBlock:;
                    }

                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("error: read .osu file, " + e.Message + e.Data);
            }

            metadatas.Add(metadata);
        }


        private bool Check(string AudioFileName)
        {
            if (metadatas.Count > 0)
            {
                foreach (var metadata in metadatas)
                {
                    if(metadata.AudioFilename == AudioFileName) return false;
                }
            }
            return true;
        }

        public void clear()
        {
            metadatas.Clear();
        }
        /*
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
                        metadata.MapFolder = MapFolder;
                        Debug.WriteLine(MapFolder.Path);
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
                Debug.WriteLine("error: choose .osu file" + e.Message);
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
                Debug.WriteLine("error: read" + e.Message);
            }

            return metadata;
        }*/
    }
}

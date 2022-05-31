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
        public StorageFolder storageFolder { get; set; }
    
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
                    Debug.WriteLine(item.Name);
                }
            }
            catch(Exception e)
            {
                Debug.WriteLine(e.Message);
            }
                
        }

    }

}

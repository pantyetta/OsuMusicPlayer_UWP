﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
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
    public sealed partial class SettingsPage : Page
    {
        Storage Storage { get; set; }
        public SettingsPage()
        {
            this.InitializeComponent();
            this.Storage = new Storage();
        }

        /// <summary>
        /// osuデータ保存先変更
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Choose_Button_Click(object sender, RoutedEventArgs e)
        {
            await Storage.PickeFolderAsync();
            Choose_text.Text = Storage.GetStorageFolder.Path;
            await this.Storage.LoadSongListAsync();
        }
    }
}

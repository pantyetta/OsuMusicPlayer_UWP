using System;
using System.Collections.Generic;
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

// 空白ページの項目テンプレートについては、https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x411 を参照してください

namespace OsuMusicPlayer_UWP
{
    /// <summary>
    /// それ自体で使用できる空白ページまたはフレーム内に移動できる空白ページ。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private double getRootPageWidth { get { return rootPage.Width - 100; } }

        private void Page_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            var newWindowSize = e.NewSize;
            Palyer_ProgressBar.Width = newWindowSize.Width - 100;
            Player_Metadata_Title.MaxWidth = (newWindowSize.Width - 100) / 2 - 100;
            Player_Metadata_Artist.MaxWidth = (newWindowSize.Width - 100) / 2 - 150;
        }
    }
}

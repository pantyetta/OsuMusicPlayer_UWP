using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace OsuMusicPlayer_UWP
{
    public class UI : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string Static_Title { get; set; }
        public string Title { get { return Static_Title; } set { Static_Title = value; NotifyPropertyChanged(nameof(Title)); } }
        private string Static_Artist { get; set; }
        public string Artist { get { return Static_Artist; } set { Static_Artist = value; } }
        private double Static_Volume { get; set; }
        public double Volume { get { return Static_Volume; } set { Static_Volume = value; } }


        private void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        public UI()
        {
            Title = "None";
            Artist = "None";
            Volume = 50;
        }
    }

}

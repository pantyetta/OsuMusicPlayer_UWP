using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OsuMusicPlayer_UWP
{
    public class Recording
    {
        public string ArtistName { get; set; }
        public string CompositionName { get; set; }
        public DateTime ReleaseDateTime { get; set; }
        public int SecondsTIme { get; set; }
        public Recording()
        {
            this.ArtistName = "Wolfgang Amadeus Mozart";
            this.CompositionName = "Andante in C for Piano";
            this.ReleaseDateTime = new DateTime(1761, 1, 1);
            this.SecondsTIme = 60;
        }
        public string OneLineSummary
        {
            get
            {
                return $"{this.CompositionName} by {this.ArtistName}, released: "
                    + this.ReleaseDateTime.ToString("d");
            }
        }
    }
    public class RecordingViewModel
    {
        private Recording defaultRecording = new Recording();
        public Recording DefaultRecording { get { return this.defaultRecording; } }

        private ObservableCollection<Recording> recordings = new ObservableCollection<Recording>(); //ここに全部のデータが入ってる
        public ObservableCollection<Recording> Recordings { get { return this.recordings; } }   //ここから取り出す setは出来ないよ
        public RecordingViewModel()
        {
            this.recordings.Add(new Recording()
            {
                ArtistName = "Johann Sebastian Bach",
                CompositionName = "Mass in B minor",
                ReleaseDateTime = new DateTime(1748, 7, 8),
                SecondsTIme = 100
        });
            this.recordings.Add(new Recording()
            {
                ArtistName = "Ludwig van Beethoven",
                CompositionName = "Third Symphony",
                ReleaseDateTime = new DateTime(1805, 2, 11),
                SecondsTIme = 110
            });
            this.recordings.Add(new Recording()
            {
                ArtistName = "George Frideric Handel",
                CompositionName = "Serse",
                ReleaseDateTime = new DateTime(1737, 12, 3),
                SecondsTIme = 120
            });
            this.recordings.Add(new Recording()
            {
                ArtistName = "aaaaaaaaaaaaaaaaa",
                CompositionName = "Mass in B minor",
                ReleaseDateTime = new DateTime(1748, 7, 8),
                SecondsTIme = 130
            });
            this.recordings.Add(new Recording()
            {
                ArtistName = "bbbbbbbbbbbbbbbbb",
                CompositionName = "Third Symphony",
                ReleaseDateTime = new DateTime(1805, 2, 11),
                SecondsTIme = 140
            });
            this.recordings.Add(new Recording()
            {
                ArtistName = "cccccccccccccc",
                CompositionName = "Serse",
                ReleaseDateTime = new DateTime(1737, 12, 3),
                SecondsTIme = 150
            });
            this.recordings.Add(new Recording()
            {
                ArtistName = "dddddddddddddddddddddddddddd",
                CompositionName = "Mass in B minor",
                ReleaseDateTime = new DateTime(1748, 7, 8),
                SecondsTIme = 160
            });
            this.recordings.Add(new Recording()
            {
                ArtistName = "eeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeee",
                CompositionName = "Third Symphony",
                ReleaseDateTime = new DateTime(1805, 2, 11),
                SecondsTIme = 170
            });
            this.recordings.Add(new Recording()
            {
                ArtistName = "あいうえおかきくけこさしすせそたちつてと",
                CompositionName = "Serse",
                ReleaseDateTime = new DateTime(1737, 12, 3),
                SecondsTIme = 180
            });
            this.recordings.Add(new Recording()
            {
                ArtistName = "1234567890",
                CompositionName = "Serseeeeeeeeeeeeeeeeeeeeeeeee",
                ReleaseDateTime = new DateTime(1737, 12, 3),
                SecondsTIme = 190
            });
            this.recordings.Add(new Recording()
            {
                ArtistName = "１２３４５６７８９０",
                CompositionName = "Sersee",
                ReleaseDateTime = new DateTime(1737, 12, 3),
                SecondsTIme = 200
            });
        }
    }


    public class StringFormatter : Windows.UI.Xaml.Data.IValueConverter
    {
        // This converts the value object to the string to display.
        // This will work with most simple types.
        public object Convert(object value, Type targetType,
            object parameter, string language)
        {
            // Retrieve the format string and use it to format the value.
            string formatString = parameter as string;
            if (!string.IsNullOrEmpty(formatString))
            {
                return string.Format(formatString, value);
            }

            // If the format string is null or empty, simply
            // call ToString() on the value.
            return value.ToString();
        }

        // No need to implement converting back on a one-way binding
        public object ConvertBack(object value, Type targetType,
            object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}

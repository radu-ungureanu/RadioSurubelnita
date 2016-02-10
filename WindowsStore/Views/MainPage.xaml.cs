using Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media;
using Windows.Storage.Streams;
using Windows.System;
using Windows.UI.Notifications;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace WindowsStore.Views
{
    public sealed partial class MainPage : WindowsStore.Common.LayoutAwarePage, INotifyPropertyChanged
    {
        private DispatcherTimer _timer;
        private bool _isPlaying;
        public bool IsPlaying
        {
            get { return _isPlaying; }
            set
            {
                if (_isPlaying != value)
                {
                    _isPlaying = value;
                    OnChanged("IsPlaying");
                }
            }
        }
        private string _songName;
        public string SongName
        {
            get { return _songName; }
            set
            {
                if (_songName != value)
                {
                    _songName = value;
                    OnChanged("SongName");
                }
            }
        }
        private string _artist;
        public string Artist
        {
            get { return _artist; }
            set
            {
                if (_artist != value)
                {
                    _artist = value;
                    OnChanged("Artist");
                }
            }
        }
        private string _album;
        public string Album
        {
            get { return _album; }
            set
            {
                if (_album != value)
                {
                    _album = value;
                    OnChanged("Album");
                }
            }
        }
        private string _song;
        public string Song
        {
            get { return _song; }
            set
            {
                if (_song != value)
                {
                    _song = value;
                    OnChanged("Song");
                }
            }
        }

        public MainPage()
        {
            this.InitializeComponent();
            this.DataContext = this;
            SongName = "Oprit";
            Artist = string.Empty;
            Album = "Oprit";
            Song = string.Empty;

            MediaControl.PlayPressed += MediaControl_PlayPressed;
            MediaControl.PausePressed += MediaControl_PausePressed;
            MediaControl.PlayPauseTogglePressed += MediaControl_PlayPauseTogglePressed;
            MediaControl.StopPressed += MediaControl_StopPressed;
            MediaControl.FastForwardPressed += MediaControl_FastForwardPressed;
            MediaControl.RewindPressed += MediaControl_RewindPressed;
            MediaControl.ChannelDownPressed += MediaControl_ChannelDownPressed;
            MediaControl.ChannelUpPressed += MediaControl_ChannelUpPressed;

            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromMinutes(1);
            _timer.Tick += PeriodicInfoDownload;
        }

        private void StartPlaying()
        {
            IsPlaying = true;
            mediaplayer.Source = new Uri(StreamInfo.StreamUrl);
            mediaplayer.Play();
            _timer.Start();

            PeriodicInfoDownload(null, null);
        }

        private void StopPlaying()
        {
            IsPlaying = false;
            mediaplayer.Stop();
            mediaplayer.Source = null;
            _timer.Stop();

            SongName = "Oprit";
            Artist = string.Empty;
            Album = "Oprit";
            Song = string.Empty;

            ClearTile();
        }

        private async void MediaControl_PlayPauseTogglePressed(object sender, object e)
        {
            if (IsPlaying)
            {
                await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                {
                    StopPlaying();
                });
            }
            else
            {
                await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                {
                    StartPlaying();
                });
            }
        }

        private void playBtn_Click(object sender, RoutedEventArgs e)
        {
            if (IsPlaying)
            {
                StopPlaying();
            }
            else
            {
                StartPlaying();
            }
        }

        private async void PeriodicInfoDownload(object sender, object e)
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("user-agent", "Mozilla/5.0 (Windows NT 6.2; WOW64; rv:21.0) Gecko/20100101 Firefox/21.0");
            httpClient.DefaultRequestHeaders.Add("accept-encoding", "gzip, deflate");

            try
            {
                var response = await httpClient.GetAsync(StreamInfo.InfoUrl);

                if (response.IsSuccessStatusCode)
                {
                    var nowPlaying = ExtractSongName(response.Content.ReadAsStringAsync().Result);
                    SongName = nowPlaying;
                    var elements = SongName.Split('-');
                    if (elements.Length == 3)
                    {
                        Artist = elements[0].Trim();
                        Album = elements[1].Trim();
                        Song = elements[2].Trim();
                    }
                    else if (elements.Length == 2)
                    {
                        Artist = elements[0].Trim();
                        Album = string.Empty;
                        Song = elements[1].Trim();
                    }
                    UpdateLiveTile();
                }
            }
            catch (Exception) { }
        }

        private static string ExtractSongName(string result)
        {
            var nowPlaying = result.Substring(result.LastIndexOf("class=default"));
            nowPlaying = nowPlaying.Substring(nowPlaying.IndexOf("<b") + 3);
            nowPlaying = nowPlaying.Substring(0, nowPlaying.IndexOf("</"));
            nowPlaying = nowPlaying.Replace("�", "\'").Replace("_", ".");
            return nowPlaying;
        }

        private void UpdateLiveTile()
        {
            //Create the Large Tile
            var largeTile = TileUpdateManager.GetTemplateContent(TileTemplateType.TileWideText05);
            if (!string.IsNullOrEmpty(Album))
            {
                largeTile.GetElementsByTagName("text")[0].InnerText = "Artist: " + Artist;
                largeTile.GetElementsByTagName("text")[1].InnerText = "Album: " + Album;
                largeTile.GetElementsByTagName("text")[2].InnerText = "Song: " + Song;
            }
            else
            {
                largeTile.GetElementsByTagName("text")[0].InnerText = "Artist: " + Artist;
                largeTile.GetElementsByTagName("text")[1].InnerText = "Song: " + Song;
            }

            //Create a Small Tile
            var smallTile = TileUpdateManager.GetTemplateContent(TileTemplateType.TileSquareText03);
            if (!string.IsNullOrEmpty(Album))
            {
                smallTile.GetElementsByTagName("text")[0].InnerText = Artist;
                smallTile.GetElementsByTagName("text")[1].InnerText = Album;
                smallTile.GetElementsByTagName("text")[2].InnerText = Song;
            }
            else
            {
                smallTile.GetElementsByTagName("text")[0].InnerText = Artist;
                smallTile.GetElementsByTagName("text")[1].InnerText = Song;
            }

            //Merge the two updates into one <visual> XML node
            var newNode = largeTile.ImportNode(smallTile.GetElementsByTagName("binding").Item(0), true);
            largeTile.GetElementsByTagName("visual").Item(0).AppendChild(newNode);

            var tileNotification = new TileNotification(largeTile);
            TileUpdateManager.CreateTileUpdaterForApplication().Update(tileNotification);
        }

        private void ClearTile()
        {
            TileUpdateManager.CreateTileUpdaterForApplication().Clear();
        }

        private async void MediaControl_PausePressed(object sender, object e)
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                StopPlaying();
            });
        }

        private async void MediaControl_PlayPressed(object sender, object e)
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                StartPlaying();
            });
        }

        private async void MediaControl_StopPressed(object sender, object e)
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                StopPlaying();
            });
        }

        private void MediaControl_ChannelUpPressed(object sender, object e) { }
        private void MediaControl_ChannelDownPressed(object sender, object e) { }
        private void MediaControl_RewindPressed(object sender, object e) { }
        private void MediaControl_FastForwardPressed(object sender, object e) { }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private async void link_Click(object sender, RoutedEventArgs e)
        {
            await Launcher.LaunchUriAsync(new Uri("http://www.radiosurubelnita.ro", UriKind.Absolute));
        }
    }
}

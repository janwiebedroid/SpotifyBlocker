using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Timers;
using System.Threading.Tasks;
using System.Windows.Input;
using Timer = System.Timers.Timer;

namespace SpotifyBlocker
{
    internal class MainWindowViewModel : INotifyPropertyChanged
    {
        private ICommand _updateCommand;
        private ICommand _deleteCommand;
        private string _artist;
        private string _song;
        private ObservableCollection<string> _blockedArtists;
        private ObservableCollection<string> _blockedSongs;
        public event PropertyChangedEventHandler PropertyChanged;

        ///TODO!!! Create base class for blocked song etc. derive from base class per kind
        public ObservableCollection<string> BlockedArtists
        {
            get { return _blockedArtists; }
            set
            {
                _blockedArtists = value;
                OnPropertyChanged("BlockedArtists");
            }
        }

        public ObservableCollection<string> BlockedSongs
        {
            get { return _blockedSongs; }
            set
            {
                _blockedSongs = value;
                OnPropertyChanged("BlockedSongs");
            }
        }

        /// <summary>
        /// Name of the current artist (can be customized)
        /// </summary>
        public string Artist
        {
            get
            {
                return _artist;
            }
            set
            {
                _artist = value;
                OnPropertyChanged("Artist");
            }

        }

        /// <summary>
        /// Name of the current song (can't be customized)
        /// </summary>
        public string Song
        {
            get
            {
                return _song;
            }
            set
            {
                _song = value;
                OnPropertyChanged("Song");
            }
        }

        public SpotifyAPI.SpotifyAPI API { get; set; }

        /// <summary>
        /// Command to add the artist to the list
        /// </summary>
        public ICommand AddCommand
        {
            get
            {
                return _updateCommand ??
                       (_updateCommand = new RelayCommand(p => { UpdateBlockedArtists(); }, p => true));
            }
            set
            {
                _updateCommand = value;
            }
        }

        /// <summary>
        /// Command to add the artist to the list
        /// </summary>
        public ICommand DeleteCommand
        {
            get
            {
                return _deleteCommand ??
                       (_deleteCommand = new RelayCommand(p => { DeleteBlockedArtists(); }, p => true));
            }
            set
            {
                _deleteCommand = value;
            }
        }

        public string SelectedArtistItem { get; set; }

        private void DeleteBlockedArtists()
        {
            Properties.Settings.Default.BlockedArtist =
                Properties.Settings.Default.BlockedArtist.Replace($";{SelectedArtistItem}", "");
            Properties.Settings.Default.Save();

            BlockedArtists.Remove(SelectedArtistItem);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public MainWindowViewModel()
        {
            List<string> blockedArtists = Properties.Settings.Default.BlockedArtist.Split(';').ToList();
            blockedArtists.Remove(blockedArtists.First());
            _blockedArtists = new ObservableCollection<string>(blockedArtists);

            List<string> blockedSongs = Properties.Settings.Default.BlockedSongs.Split(';').ToList();
            blockedSongs.Remove(blockedSongs.First());

            _blockedSongs = new ObservableCollection<string>(blockedSongs);

            API = new SpotifyAPI.SpotifyAPI();
            API.InitApi();

            Artist = API.SongInfo.ArtistName;

            Timer timer = new Timer { Interval = 1000 };
            timer.Elapsed += new ElapsedEventHandler(TimerEvent);
            timer.Start();

        }

        private void TimerEvent(object sender, EventArgs e)
        {
            API.InitApi();
            Artist = API.SongInfo.ArtistName;


            //Set the artist value to the API value (if not yet set)
            if (Artist != API.SongInfo.ArtistName)
            {
                Artist = API.SongInfo.ArtistName;
            }

            // Same for the song

            if (Song != API.SongInfo.SongName)
            {
                Song = API.SongInfo.SongName;
            }

            foreach (string artist in BlockedArtists)            
            {
                if (API.SongInfo.ArtistName == artist)
                {
                    API.NextTrack();
                }
            }

            foreach (string song in BlockedSongs) {
                if (API.SongInfo.SongName == song)
                {
                    API.NextTrack();
                }
                
            }


            
        }


        /// <summary>
        /// Update the blocked artists
        /// </summary>
        private void UpdateBlockedArtists()
        {
            if (string.IsNullOrEmpty(Artist))
            {
                return;
            }
            Properties.Settings.Default.BlockedArtist = $"{Properties.Settings.Default.BlockedArtist};{Artist}";
            Properties.Settings.Default.Save();
            BlockedArtists.Add(Artist);
        }

        private void UpdateBlockedSongs()
        {
            if (string.IsNullOrEmpty(Song)) {
                return;
            }
            Properties.Settings.Default.BlockedSongs = $"{Properties.Settings.Default.BlockedSongs};{Song}";
            Properties.Settings.Default.Save();
            BlockedSongs.Add(Artist);
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}

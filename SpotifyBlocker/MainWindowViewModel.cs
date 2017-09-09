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
using SpotifyBlocker.Models;
using Timer = System.Timers.Timer;

namespace SpotifyBlocker
{
    internal class MainWindowViewModel : INotifyPropertyChanged
    {
        private ICommand _updateArtistCommand;
        private ICommand _deleteArtistCommand;
        private ICommand _updateSongCommand;
        private ICommand _deleteSongCommand;

        public event PropertyChangedEventHandler PropertyChanged;

        ///TODO Create base class for blocked song etc. derive from base class per kind

        public SpotifyAPI.SpotifyAPI API { get; set; }



        /// <summary>
        /// Command to add the artist to the list
        /// </summary>
        public ICommand AddArtistCommand
        {
            get
            {
                return _updateArtistCommand ??
                       (_updateArtistCommand = new RelayCommand(p => { Artist.Add(); }, p => true));
            }
            set
            {
                _updateArtistCommand = value;
            }
        }

        /// <summary>
        /// Command to add the artist to the list
        /// </summary>
        public ICommand DeleteArtistCommand
        {
            get
            {
                return _deleteArtistCommand ??
                       (_deleteArtistCommand = new RelayCommand(p => { Artist.Delete(); }, p => true));
            }
            set
            {
                _deleteArtistCommand = value;
            }
        }

        /// <summary>
        /// Command to add the artist to the list
        /// </summary>
        public ICommand AddSongCommand
        {
            get
            {
                return _updateSongCommand ??
                       (_updateSongCommand = new RelayCommand(p => { Song.Add(); }, p => true));
            }
            set
            {
                _updateSongCommand = value;
            }
        }

        /// <summary>
        /// Command to add the artist to the list
        /// </summary>
        public ICommand DeleteSongCommand
        {
            get
            {
                return _deleteSongCommand ??
                       (_deleteSongCommand = new RelayCommand(p => { Song.Delete(); }, p => true));
            }
            set
            {
                _deleteSongCommand = value;
            }
        }

        public BaseModel Artist { get; }

        public BaseModel Song { get; }



        /// <summary>
        /// Constructor
        /// </summary>
        public MainWindowViewModel()
        {
            API = new SpotifyAPI.SpotifyAPI();
            API.InitApi();

            Artist = new BaseModel("BlockedArtists");
            Song = new BaseModel("BlockedSongs");

            Artist.Name = API.SongInfo.ArtistName;
            Song.Name = API.SongInfo.SongName;

            Timer timer = new Timer { Interval = 1000 };
            timer.Elapsed += new ElapsedEventHandler(TimerEvent);
            timer.Start();

        }

        private void TimerEvent(object sender, EventArgs e)
        {
            API.InitApi();
            Artist.Name = API.SongInfo.ArtistName;
            Song.Name = API.SongInfo.SongName;


            //Set the artist value to the API value (if not yet set)
            if (Artist.Name != API.SongInfo.ArtistName)
            {
                Artist.Name = API.SongInfo.ArtistName;
            }

            // Same for the song

            if (Song.Name != API.SongInfo.SongName)
            {
                Song.Name = API.SongInfo.SongName;
            }

            foreach (string artist in Artist.Blocked)            
            {
                if (API.SongInfo.ArtistName == artist)
                {
                    API.NextTrack();
                }
            }

            foreach (string song in Song.Blocked) {
                if (API.SongInfo.SongName == song)
                {
                    API.NextTrack();
                }
                
            }

            OnPropertyChanged("Song");
            OnPropertyChanged("Artist");
            
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpotifyBlocker
{
    internal class SongInfoBase
    {
        public SpotifyAPI.SpotifyAPI API { get; set; }

        public SongInfoBase()
        {
            API = new SpotifyAPI.SpotifyAPI();
            API.InitApi();

        }

        //public GetBlocked(string infoType)
        //{
        //    //TODO: ensure string type Xensure?
        //    List<string> blockedType = Properties.Settings.Default[infoType].ToString().Split(';').ToList();


        //    blockedArtists.Remove(blockedArtists.First());
        //    _blockedArtists = new ObservableCollection<string>(blockedArtists);
            
        //}

    }
}

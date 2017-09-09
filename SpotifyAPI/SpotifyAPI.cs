using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using SpotifyAPI.WebHelper;

namespace SpotifyAPI
{
    /// <summary>
    /// Main Spotify API class
    /// </summary>
    public class SpotifyAPI
    {
        #region Constants
        private const int WM_APPCOMMAND = 0x319;
        private const int MEDIA_PLAYPAUSE = 0xE0000;
        private const int MEDIA_NEXTTRACK = 0xB0000;

        #endregion

        #region Fields

        private IntPtr _handle;

        private ClientInfo _clientInfo;
        private SongInfo _songInfo;

        #endregion

        #region Properties

        /// <summary>
        /// Information on the current song
        /// </summary>
        public SongInfo SongInfo => _songInfo;

        /// <summary>
        /// Information on the client
        /// </summary>
        public ClientInfo ClientInfo => _clientInfo;

        #endregion


        #region Public methods


        /// <summary>
        /// Initialise the Spotifiy API
        /// </summary>
        public void InitApi()
        {
            try
            {
                _handle = GetHandle();
                WebHelperHook.GetObjects(out _clientInfo, out _songInfo);
            }
            catch (Exception exception)
            {
                MessageBox.Show($"ERROR: {exception.Message}");
            }
         
        }

        /// <summary>
        /// Toggles the track
        /// </summary>
        public void Pauze()
        {
            SendMessage(_handle, WM_APPCOMMAND, _handle, (IntPtr)MEDIA_PLAYPAUSE);
        }

        public void NextTrack()
        {
            SendMessage(_handle, WM_APPCOMMAND, _handle, (IntPtr)MEDIA_NEXTTRACK);
        }
        #endregion

        #region Private methods

        /// <summary>
        /// Sends the command to the application
        /// TODO: investate how to get otehr IntPtr hex codes
        /// </summary>
        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

        private static IntPtr GetHandle()
        {
            foreach (Process process in Process.GetProcesses().Where(t => t.ProcessName.ToLower().Contains("spotify"))) {
                if (process.MainWindowTitle.Length > 1)
                    return process.MainWindowHandle;
            }
            return IntPtr.Zero;
        }

        #endregion
    }

}

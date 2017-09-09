using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using Newtonsoft.Json.Linq;

namespace SpotifyAPI.WebHelper
{
    internal class WebHelperHook
    {
        private static string _oAuthToken;
        private static string _csrfToken;
        private static string _hostName;

        /// <summary>
        /// Get the info off the client
        /// </summary>
        public static void GetObjects(out ClientInfo clientInfo, out SongInfo songInfo)
        {
            if (string.IsNullOrEmpty(_oAuthToken))
            {
                _oAuthToken = GetOAuth();
            }
            if (string.IsNullOrEmpty(_csrfToken))
            {
                _csrfToken = GetCSRF();
            }
            clientInfo = new ClientInfo();
            songInfo = new SongInfo();

            string result;

            try
            {
                string url = GetUrl($"/remote/status.json?oauth={_oAuthToken}&csrf={_csrfToken}");
                result = GetPageContents(url);
            }
            catch
            {
                return;
            }

            JObject jsonJObject = JObject.Parse(result);
            if(jsonJObject["client_version"] == null || jsonJObject["track"]["artist_resource"]["name"] == null)
            {
                //TODO: create logging
                return;
            }

            //TODO: better validation for object

            clientInfo.Version = jsonJObject["client_version"].ToString();
            clientInfo.Playing = Convert.ToBoolean(jsonJObject["playing"]);
            clientInfo.Repeat = Convert.ToBoolean(jsonJObject["repeat"]); ;
            clientInfo.Shuffle = Convert.ToBoolean(jsonJObject["shuffle"]); ;

            //TODO: create seperate artist class with all the artist data + album class
            songInfo.ArtistName = jsonJObject["track"]["artist_resource"]["name"].ToString();
            songInfo.SongName = jsonJObject["track"]["track_resource"]["name"].ToString();
            songInfo.SongUri = jsonJObject["track"]["track_resource"]["uri"].ToString();
            songInfo.SongUrl = jsonJObject["track"]["track_resource"]["location"]["og"].ToString();
            songInfo.Length = Convert.ToInt32(jsonJObject["track"]["length"]);
        }

        #region Private methods

        /// <summary>
        /// Get the CSRF token
        /// </summary>
        private static string GetCSRF()
        {
            CheckWebHelper();
            string url = GetUrl("/simplecsrf/token.json");
            string json = GetPageContents(url);
            JObject jsonJObject = JObject.Parse(json);
            return jsonJObject["token"].ToString();
        }

        /// <summary>
        /// Get the OAuth token
        /// </summary>
        private static string GetOAuth()
        {
            CheckWebHelper();
            string url = "https://open.spotify.com/token";
            string json = GetPageContents(url);
            JObject jsonJObject = JObject.Parse(json);
            return jsonJObject["t"].ToString();
        }

        /// <summary>
        /// Get the url (cool algorithm by https://github.com/Xeroday/)
        /// </summary>
        private static string GetUrl(string path)
        {
            if (_hostName == null) {
                _hostName = new Random(Environment.TickCount).Next(100000, 100000000).ToString();
            }
            return $"http://127.0.0.1:4381{path}";
        }

        /// <summary>
        /// Get the page contents
        /// </summary>
        private static string GetPageContents(string url)
        {
            WebClient webClient = new WebClient();

            if (url.Contains("spotilocal")) {
                webClient.Proxy = null;
            }

            webClient.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 6.1; WOW64; Trident/4.0; SLCC2; .NET CLR 2.0.50727; .NET CLR 3.5.30729; .NET CLR 3.0.30729; .NET4.0C; .NET4.0E)");
            webClient.Headers.Add("Origin", "https://open.spotify.com");
            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            byte[] bytes = Encoding.Default.GetBytes(webClient.DownloadString(url));
            return Encoding.UTF8.GetString(bytes);
        }

        /// <summary>
        /// Check if the helper is running, if not start it.
        /// </summary>
        private static void CheckWebHelper()
        {
            foreach (Process process in Process.GetProcesses().Where(p => p.ProcessName.ToLower().Equals("spotifywebhelper")))
            {
                return; //If the webhelper is running return
            }
            //Start the webhelper
            if (File.Exists(Environment.GetEnvironmentVariable("APPDATA") + @"\Spotify\Data\SpotifyWebHelper.exe")) {
                Process.Start(Environment.GetEnvironmentVariable("APPDATA") + @"\Spotify\Data\SpotifyWebHelper.exe");
            }
            else if (File.Exists(Environment.GetEnvironmentVariable("APPDATA") + @"\Spotify\SpotifyWebHelper.exe")) {
                Process.Start(Environment.GetEnvironmentVariable("APPDATA") + @"\Spotify\SpotifyWebHelper.exe");
            }
        }

     #endregion
    }
}

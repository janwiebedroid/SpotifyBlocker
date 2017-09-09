namespace SpotifyAPI
{
    /// <summary>
    /// Information on the client (version etc.)
    /// </summary>
    public class ClientInfo
    {
        /// <summary>
        /// The client version
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// Determines if the current song is playing.
        /// </summary>
        public bool Playing { get; set; }
        
        /// <summary>
        /// Determines if the playlist is shuffeleds
        /// </summary>
        public bool Shuffle { get; set; }

        /// <summary>
        /// Determines if the song is on repeat
        /// </summary>
        public bool Repeat { get; set; }

    }
}
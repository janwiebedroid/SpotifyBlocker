namespace SpotifyAPI
{
    /// <summary>
    /// Info on the song
    /// </summary>
    public class SongInfo
    {
        /// <summary>
        /// The artist name
        /// </summary>
        public string ArtistName { get; set; }

        /// <summary>
        /// The name off the song
        /// </summary>
        public string SongName { get; set; }

        /// <summary>
        /// Uri info off the song ex. spotify:track:0gb1J5UrTpzaU1s3nupgCd
        /// </summary>
        public string SongUri { get; set; }

        /// <summary>
        /// The url to the song.
        /// </summary>
        public string SongUrl { get; set; }

        /// <summary>
        /// The length of the track (in seconds)
        /// </summary>
        public int Length { get; set; }

    }
}
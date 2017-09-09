using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SpotifyApiUnitTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            SpotifyAPI.SpotifyAPI api = new SpotifyAPI.SpotifyAPI();
            api.InitApi();

        }
    }
}

using System;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
using HemtentaTdd2017;
using HemtentaTdd2017.MusicPlayer;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;

namespace Test_Hemtenta_Christian_Jarenfors
{
    [TestFixture]
    public class UnitTestMusicPlayer
    {
        MusicPlayer MP;
        bool Connection = false;
        string nowPlaying = "Tystnad råder";
        const string musicSearchString = "Christian";
        [SetUp]
        public void init()
        {
            nowPlaying = "Tystnad råder";
            MP = new MusicPlayer();
            Mock<IMediaDatabase> IMediaDatabaseMock = new Mock<IMediaDatabase>();
            #region Setup IMediaDatabaseMock
            IMediaDatabaseMock.When(() => Connection).Setup((x) => x.OpenConnection()).Throws(new DatabaseAlreadyOpenException());
            IMediaDatabaseMock.When(() => !Connection).Setup((x) => x.OpenConnection()).Callback(() => Connection = true);
            IMediaDatabaseMock.When(() => Connection).Setup((x) => x.CloseConnection()).Callback(() => Connection = false);
            IMediaDatabaseMock.When(() => !Connection).Setup((x) => x.CloseConnection()).Throws(new DatabaseClosedException());
            IMediaDatabaseMock.When(() => Connection).Setup(
                (x) => x.FetchSongs(
                    It.Is<string>(
                        (y) => !string.IsNullOrEmpty(y)
                        )
                     )
                     ).Returns(new List<ISong>()
                     {
                         new Song() {Title="Song One"},
                         new Song() {Title="Song Two"}
                     }
                     );
            IMediaDatabaseMock.When(() => !Connection).Setup((x) => x.FetchSongs(It.IsAny<string>())).Throws(new DatabaseClosedException());
            #endregion
            Mock<ISoundMaker> ISoundMakerMock = new Mock<ISoundMaker>();

            #region Setup ISoundMakerMock
            ISoundMakerMock.Setup((x) => x.NowPlaying).Returns(nowPlaying);
            ISoundMakerMock.Setup((x) => x.Play(null)).Throws(new Exception("Null sång"));
            ISoundMakerMock.Setup((x) => x.Play(It.IsAny<ISong>())).Callback<ISong>((r) =>
            {
                nowPlaying = "Spelar " + r.Title;
                ISoundMakerMock.Setup((x) => x.NowPlaying).Returns(nowPlaying);
            });
            ISoundMakerMock.Setup((x) => x.Stop()).Callback(() =>
            {
                nowPlaying = "Tystnad råder";
                ISoundMakerMock.Setup((x) => x.NowPlaying).Returns(nowPlaying);
            });
            #endregion
            MP.Setup(IMediaDatabaseMock.Object, ISoundMakerMock.Object);
        }
        // Antal sånger som finns i spellistan.
        // Returnerar alltid ett heltal >= 0.

        #region NumSongsInQueue

        public int NumSongsInQueue { get; }
        #endregion


        // Söker i databasen efter sångtitlar som
        // innehåller "search" och lägger till alla
        // sökträffar i spellistan.

        #region LoadSongs
        [Test]
        public void LoadSongs_Success()
        {
            MP.LoadSongs(musicSearchString);
            Assert.AreEqual(2, MP.NumSongsInQueue);
            MP.LoadSongs(musicSearchString);
            Assert.AreEqual(4, MP.NumSongsInQueue);
        }
        [Test]
        [TestCase(null)]
        [TestCase("")]
        public void LoadSongs_Fail_NullOrEmptyParameter(string searchstring)
        {
            Assert.Throws<Exception>(() => MP.LoadSongs(searchstring));
        }

        #endregion


        // Om ingen låt spelas för tillfället ska
        // nästa sång i kön börja spelas. Om en låt
        // redan spelas har funktionen ingen effekt.

        #region Play
        [Test]
        public void Play_Success()
        {
            init();
            Assert.AreEqual(0, MP.NumSongsInQueue);
            Assert.That(MP.NowPlaying().Equals("Tystnad råder"), 
                "Nånting spelas trots att jag inte kört Play");
            MP.Play();
            Assert.That(MP.NowPlaying().Equals("Tystnad råder"), 
                "Det spelas nånting trots att spellistan skall vara tom");
            MP.LoadSongs(musicSearchString);
            MP.Play();
            Assert.That(MP.NowPlaying().Equals("Spelar Song One"), 
                "Nu borde det spelas nått men det gör det inte, hm...");
            MP.Play();
            Assert.That(MP.NowPlaying().Equals("Spelar Song One"), 
                "Nu borde det spelas Song One men det gör det inte, hm...");
        }
        #endregion


        // Om en sång spelas ska den sluta spelas.
        // Sången ligger kvar i spellistan. Om ingen
        // sång spelas har funktionen ingen effekt.

        #region Stop
        [Test]
        public void Stop_Success()
        {
            MP.Stop();//Ska inte hända nått.
            MP.LoadSongs(musicSearchString);//nu ska det vara 2 låtar i listan.
            MP.Play();//nu spelas nått.
            Assert.That(MP.NowPlaying().Equals("Spelar Song One"), 
                "Inget spelas, vilket är fel");
            MP.Stop();//nu skall inget spelas
            Assert.That(MP.NowPlaying().Equals("Tystnad råder"), 
                "Något spelas, vilket är fel");
            MP.Play();//nu ska Song One spelas igen.
            Assert.That(MP.NowPlaying().Equals("Spelar Song One"), 
                "Inget spelas, vilket är fel");
        }
        #endregion


        // Börjar spela nästa sång i kön. Om kön är tom
        // har funktionen samma effekt som Stop().

        #region NextSong
        [Test]
        public void NextSong_Success()
        {
            MP.LoadSongs(musicSearchString);//Sätter en lista på 2 låtar
            MP.Play();//Spelar den första
            MP.NextSong();//Spelar nästa låt, Nu finns det ingen mer låt efter i listan.
            Assert.That(MP.NowPlaying().Equals("Spelar Song Two"), 
                "Inget spelas, vilket är fel");
            MP.NextSong();
            Assert.That(MP.NowPlaying().Equals("Tystnad råder"), 
                "Något spelas, vilket är fel");
            MP.LoadSongs(musicSearchString);//Sätter en lista på 2 låtar
            MP.NextSong();//Spelar nästa låt,Denna startas eftersom jag har tänkt 
                          //så att om ingen låt spelas när NextSong() Anropas
                          //så Startar nästa sång.
            Assert.That(MP.NowPlaying().Equals("Spelar Song One"), 
                "Inget spelas, vilket är fel");
        }
        #endregion


        // Returnerar strängen "Tystnad råder" om ingen
        // sång spelas, annars "Spelar <namnet på sången>".
        // Exempel: "Spelar Born to run".

        #region NowPlaying
        [Test]
        //Return string
        public void NowPlaying_Success()
        {
            Assert.That(MP.NowPlaying().Equals("Tystnad råder"), 
                "Något spelas, vilket är fel");
            MP.LoadSongs(musicSearchString);
            MP.Play();
            Assert.That(MP.NowPlaying().Contains("Spelar "), 
                "Inget spelas, vilket är fel");
        }
        #endregion

    }
}

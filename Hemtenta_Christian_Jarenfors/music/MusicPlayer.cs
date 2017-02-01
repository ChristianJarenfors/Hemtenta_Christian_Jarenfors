using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HemtentaTdd2017.MusicPlayer
{
    public class MusicPlayer : IMusicPlayer
    {
        IMediaDatabase MediaDatabase;
        ISoundMaker SoundMaker;
        string stopMessage = "Tystnad råder";
        List<ISong> SongList = new List<ISong>();
        public void Setup(IMediaDatabase MDB, ISoundMaker SM)
        {
            MediaDatabase = MDB;
            SoundMaker = SM;
        }
        public int NumSongsInQueue
        {
            get
            {
                return SongList.Count();
            }
        }

        public void LoadSongs(string search)
        {
            if (string.IsNullOrEmpty(search))

            {
                throw new Exception("Null or empty search string");
            }
            MediaDatabase.OpenConnection();
            SongList.AddRange(MediaDatabase.FetchSongs(search));
            MediaDatabase.CloseConnection();
        }

        public void NextSong()
        {
            //Kolla om nått spelas om inte: Spela första
            if (SoundMaker.NowPlaying== stopMessage)
            {
                if (SongList.Count!=0)
                {
                    SoundMaker.Play(SongList.First());
                }
                
            }
            //Om nått spelar: ta bort första från listan och spela nästa
            else
            {
                SongList.Remove(SongList.First());
                //Om listan blir tom stanna spelningen
                if (SongList.Count!=0)
                {
                    SoundMaker.Play(SongList.First());
                }
                else
                {
                    SoundMaker.Stop();
                }
                
            }
            
        }

        public string NowPlaying()
        {
            return SoundMaker.NowPlaying;
        }

        public void Play()
        {
            if (SoundMaker.NowPlaying== stopMessage)
            {
                if (SongList.Count!=0)
                {
                    SoundMaker.Play(SongList.First());
                }
                
            }
        }

        public void Stop()
        {
            if (SoundMaker.NowPlaying != stopMessage)
            {

                SoundMaker.Stop();
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;

namespace Platformer.View
{
    class SoundPlayer
    {
        #region static
        private static SoundBank tracks;
        private static Cue currentSong;
        private static Cue soundEffect;
        public static bool EnableSoundPlayer;
        public static AudioEngine Audio;
        public static WaveBank Waves;
        public static Random random = new Random();
        #endregion

        #region fields
        #endregion

        #region properties
       
        #endregion

        #region constructor
        static SoundPlayer()
        {
            EnableSoundPlayer = true;
        }
        #endregion

        #region methods
        public static void Initialize()
        {
            Audio = new AudioEngine("Content//new.xgs");
            Waves = new WaveBank(Audio, "Content//Wave Bank.xwb");
            tracks = new SoundBank(Audio, "Content//Sound Bank.xsb");
  
            //StartSound("shuffledrum");
        }

        /// <summary>
        /// plays the main song in the background, for now only one level so only one song, no overworld song
        /// 
        /// as we implement more levels, I will add to this method to have different songs per level.
        /// 
        ///
        /// </summary>
        /// <param name="musicName">Name of sound effect</param>

        public static void StartSound(String songID)
        {
            currentSong = tracks.GetCue(songID);
            currentSong.Play();
        }
        /// <summary>
        /// Takes in a string that identifies the cue, then plays selected cue.
        /// If new cues are added to the project, they also need to be referenced here
        /// These are the three sound effects i have for now.
        /// To add or remove cues: rebuild xact project, re-add new.gs, Sound Bank.xsb and Wave Bank.xwb as new items to PlatformerContent.
        /// To play a sound effect: call SoundPlayer.playSoundEffects("cuename");
        /// </summary>
        public static void playSoundEffects(String soundID)
        {
            soundEffect = tracks.GetCue(soundID);
            soundEffect.Play();
      

        }


        /// <summary>
        /// Stop playing current sound
        /// </summary>
        public static void StopSound()
        {
            currentSong.Stop(AudioStopOptions.Immediate);
            currentSong.Dispose();
            currentSong = null;
        }
        /// <summary>
        /// Update SoundPlayer, this allows you to loop the cues.
        /// If the cue has stopped looping, then it will re-start it.
        /// 
        /// </summary>
        /// <param name="gameTime"></param>
        public static void Update(String current)
        {
            if (EnableSoundPlayer)
            {
                if ((currentSong == null))
                {
                   // currentSong = tracks.GetCue(current);
                    StartSound(current);
                    
                }
                else if(currentSong.IsPlaying)
                {
                    //
                    //StartSound(current);
                }
            }
            else
            {
                if (currentSong != null)
                    StopSound();
            }
        }

        #endregion
    }
}

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
        static int soundNum = 1;
        private static SoundBank tracks;
        private static Cue currentSong;
        private static Cue soundEffect;
        public static int CurrentSound;
        public static bool EnableSoundPlayer;
        public static AudioEngine Audio;
        // public static SoundBank Sounds;
        public static WaveBank Waves;
        public static Random random = new Random();
        #endregion

        #region fields
        #endregion

        #region properties
        SoundEffect[] sounds = new SoundEffect[soundNum];
        #endregion

        #region constructor
        static SoundPlayer()
        {
            // sounds = Game1.Sounds;
            EnableSoundPlayer = true;
        }
        #endregion

        #region methods
        public static void Initialize()
        {
            int i = 0;
            Audio = new AudioEngine("Content//new.xgs");
            Waves = new WaveBank(Audio, "Content//Wave Bank.xwb");
            tracks = new SoundBank(Audio, "Content//Sound Bank.xsb");

            //currentSound = tracks.GetCue("shuffledrum");
            StartSound();
        }

        /// <summary>
        /// plays the main song in the background, for now only one level so only one song, no overworld song
        /// 
        /// as we implement more levels, I will add to this method to have different songs per level.
        /// 
        ///
        /// </summary>
        /// <param name="musicName">Name of sound effect</param>

        public static void StartSound()
        {
            //int soundidx;

            //  if (currentSound != null && currentSound.IsPlaying)
            //        StopSound();

            //do
            // {
            //     soundidx =  random.Next(soundNum);
            // } while (soundidx == CurrentSound && soundNum > 1);

            currentSong = tracks.GetCue("shuffledrum");
            //CurrentSound = soundidx;
            currentSong.Play();
        }
        /// <summary>
        /// takes in a string that identifies the cue, then plays selected cue
        /// if new cues are added to the project, they also need to be referenced here
        /// </summary>
        public static void playSoundEffects(String soundID)
        {
            if (soundID == "hihat")
            {
                soundEffect = tracks.GetCue("hihat");
                soundEffect.Play();
            }
            else if (soundID == "kick")
            {
                soundEffect = tracks.GetCue("kick");
                soundEffect.Play();
            }
            else if (soundID == "snare")
            {
                soundEffect = tracks.GetCue("snare");
                soundEffect.Play();
            }

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
        /// Update SoundPlayer. Delete this method if updating every frame is not necessary
        /// </summary>
        /// <param name="gameTime"></param>
        public static void Update()
        {
            if (EnableSoundPlayer)
            {
                if ((currentSong == null)) StartSound();
                else if (!currentSong.IsPlaying)
                {
                    StopSound();
                    StartSound();
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

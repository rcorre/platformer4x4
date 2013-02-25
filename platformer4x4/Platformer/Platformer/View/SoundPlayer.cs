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
        /// Start playing the specified sound effect
        /// one case for when you pass a sound index and one default case that selects a random track
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
        public static void playSoundEffect1()
        {
            soundEffect = tracks.GetCue("hihat");
            soundEffect.Play();
        }
        public static void playSoundEffect2()
        {
            soundEffect = tracks.GetCue("kick");
            soundEffect.Play();
        }
        public static void playSoundEffect3()
        {
            soundEffect = tracks.GetCue("snare");
            soundEffect.Play();
        }

        public static void StartSound(int soundidx)
        {

            if (currentSong != null && currentSong.IsPlaying)
                StopSound();
            //currentSong = tracks.GetCue("funkdrum");
            CurrentSound = soundidx;
            currentSong.Play();

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

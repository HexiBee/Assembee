using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Text;

namespace Assembee.Game {
    public class Audio {
        public bool noAudio = false;
        public static List<SoundEffect> soundEffects;
        public static List<SoundEffectInstance> sfxI;

        public enum sfx {
            click,
            place,
            bee
        }

        public Song song;
        private float volume = 1f;

        public Audio(ContentManager content) {
            try {
                soundEffects = new List<SoundEffect>() {
                    content.Load<SoundEffect>("sfx_click"),
                    content.Load<SoundEffect>("sfx_place"),
                    content.Load<SoundEffect>("sfx_bee"),
                };

                sfxI = new List<SoundEffectInstance>() {
                    soundEffects[0].CreateInstance(),
                    soundEffects[1].CreateInstance(),
                    soundEffects[2].CreateInstance(),
                };
                

                song = content.Load<Song>("beeming");
                //PlaySound(sfx.title, 1f, 0f, noAudio);
                //MediaPlayer.Play(song);
                //MediaPlayer.IsRepeating = true;
                MediaPlayer.Volume = 0.1f;

                MediaPlayer.MediaStateChanged += MediaPlayer_MediaStateChanged;
            } catch (Microsoft.Xna.Framework.Audio.NoAudioHardwareException) {
                noAudio = true;
            }
        }


        public void PlaySound(sfx sound, float vol, float pitch) {
            if (!noAudio) {
                sfxI[(int)sound].Volume = vol;
                sfxI[(int)sound].Pitch = pitch;

                sfxI[(int)sound].Play();
            }
        }

        public void StopSound(sfx sound) {
            if (!noAudio) {
                sfxI[(int)sound].Stop();
            }
        }




        void MediaPlayer_MediaStateChanged(object sender, System.EventArgs e) {
            // 0.0f is silent, 1.0f is full volume
            //MediaPlayer.Play(song);
        }




    }
}

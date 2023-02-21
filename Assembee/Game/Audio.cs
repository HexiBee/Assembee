using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Text;

namespace Assembee.Game {
    public class Audio {
        public bool AudioDisabled { get; set; } = false;
        public bool Muted { get; private set; } = false;

        private List<SoundEffect> soundEffects;
        private List<SoundEffectInstance> soundEffectInstances;
        private List<int> soundEffectInstanceCounts;

        private float volumeMusic = 0.1f;
        private float volumeSfx = 1f;

        private Song song;

        public enum sfx {
            click,
            place,
            bee
        }

        public Audio(ContentManager content) {
            try {
                soundEffects = new List<SoundEffect>() {
                    content.Load<SoundEffect>("sfx_click"),
                    content.Load<SoundEffect>("sfx_place"),
                    content.Load<SoundEffect>("sfx_bee"),
                };

                soundEffectInstances = new List<SoundEffectInstance>() {
                    soundEffects[0].CreateInstance(),
                    soundEffects[1].CreateInstance(),
                    soundEffects[2].CreateInstance(),
                };

                soundEffectInstanceCounts = new List<int> {
                    0, 0, 0
                };

                song = content.Load<Song>("beeming");

                MediaPlayer.Volume = volumeSfx;
                MediaPlayer.MediaStateChanged += MediaPlayer_MediaStateChanged;

                // Play test sound to see if audio is working
                PlaySound(sfx.click, 0f, 0f);

            } catch (Microsoft.Xna.Framework.Audio.NoAudioHardwareException) {
                AudioDisabled = true;
            }
        }

        public void StartSong() {
            if (AudioDisabled) return;

            MediaPlayer.Play(song);
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Volume = Muted ? 0 : volumeMusic;
        }

        public void PlaySound(sfx sound, float vol, float pitch) {
            if (AudioDisabled) return;

            soundEffectInstances[(int)sound].Volume = vol;
            soundEffectInstances[(int)sound].Pitch = pitch;

            if (soundEffectInstanceCounts[(int)sound] == 0)
                soundEffectInstances[(int)sound].Play();

            soundEffectInstanceCounts[(int)sound]++;
        }

        public void StopSound(sfx sound) {
            if (AudioDisabled) return;

            soundEffectInstanceCounts[(int)sound]--;
            if (soundEffectInstanceCounts[(int)sound] == 0)
                soundEffectInstances[(int)sound].Stop();
        }

        public void StopMusic() {
            if (AudioDisabled) return;

            MediaPlayer.Stop();
        }


        void MediaPlayer_MediaStateChanged(object sender, System.EventArgs e) {
            // 0.0f is silent, 1.0f is full volume
            //MediaPlayer.Play(song);
        }

        public void ToggleMute() {
            if (AudioDisabled) return;

            if (!Muted) {
                MediaPlayer.Volume = 0f;
                SoundEffect.MasterVolume = 0f;
                Muted = true;
                
            } else {
                MediaPlayer.Volume = volumeMusic;
                SoundEffect.MasterVolume = volumeSfx;
                Muted = false;
            }
        }




    }
}

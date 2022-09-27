/*
ECX UTILITY SCRIPTS
Audio Manager (Singleton)
Last updated: Apr 16, 2022
*/

// TODO: UTILIZE UNITY'S AUDIOMIXERS

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace EcxUtilities {
    public enum AudioCategory { Music, Sfx, UI }

    public class AudioManager : Singleton<AudioManager> {

        [SerializeField] public bool playMusicOnStart = true;
        [SerializeField][Range(0, 3)] private static float pitchRangeUI = 0.1f;
        public static float PitchRangeUI => pitchRangeUI;
        [SerializeField] private MusicManager musicManager;
        public MusicManager MusicManager => musicManager;
        [SerializeField] private SfxManager sfxManager;
        public SfxManager SfxManager => sfxManager;
        [SerializeField] private UISfxManager uISfxManager;
        public UISfxManager UISfxManager => uISfxManager;
        [SerializeField] private AudioSource audioSourceMusic;
        public AudioSource AudioSourceMusic => audioSourceMusic;
        [SerializeField] private AudioSource audioSourceSfx;
        public AudioSource AudioSourceSfx => audioSourceSfx;
        [SerializeField] private AudioSource audioSourceUI;
        public AudioSource AudioSourceUI => audioSourceUI;
        [SerializeField] private AudioSourcePool audioSourceSfxPool;
        public AudioSourcePool AudioSourcePool => AudioSourcePool;


        private bool isMusicPaused = false;
        // TODO: CREATE "LISTENERS" THAT RECALC MUSIC VOLUME WHEN THE SLIDERS BELOW ARE CHANGED IN THE UNITY EDITOR
        [SerializeField][Range(0f, 1f)]
        private float volumeMaster = 1f;
        public float VolumeMaster => volumeMaster;
        [SerializeField][Range(0f, 1f)]
        private float volumeMusic = 0.6f;
        public float VolumeMusic => volumeMusic;
        [SerializeField][Range(0f, 1f)]
        private float volumeSfx = 1f;
        public float VolumeSfx => volumeSfx;
        [SerializeField][Range(0f, 1f)]
        private float volumeUI = 1f;
        public float VolumeUI => volumeUI;


        private void Awake() {
            // destroy any duplicate AudioManagers
            AudioManager[] audioManagers = FindObjectsOfType<AudioManager>();
            if (audioManagers.Length > 1) {
                for (int i=1; i<audioManagers.Length; i++)
                Destroy(audioManagers[i].gameObject);
            }
            if (!musicManager)
                Debug.LogError("Error: AudioManager is missing Music Manager");
            if (!sfxManager)
                Debug.LogError("Error: AudioManager is missing SFX Manager");
            if (!uISfxManager)
                Debug.LogError("Error: AudioManager is missing UI SFX Manager");
            if (!audioSourceSfxPool)
                Debug.LogError("Error: AudioManager is missing Audio Source SFX Pool");
            // subscribe to OnSceneLoaded event to play music for that scene
            SceneManager.sceneLoaded += OnSceneLoaded;  
        }

        private void Start() {
            CalibrateAdjustedVolumes();
        }

        /// <summary>
        /// Fastest and simplest way to play a sound clip that won't be interrupted.  
        /// But note that it can't be controlled (paused, stopped, volume, pitch, etc) after it starts playing.
        /// </summary>
        /// <param name="audioClip"></param>
        /// <param name="ac"></param>
        /// <param name="volume"></param>
        /// <param name="pitch"></param>
        /// <returns></returns>
        public void PlayClip(AudioClip audioClip, AudioCategory ac, float volume=1, float pitch=1) {
            if (audioClip) {
                float adjVolume = CalcAdjustedVolume(ac, volume);
                // start with simplest and fastest option
                audioSourceSfx = audioSourceSfxPool.GetNextAudioSource();
                if (audioSourceSfx.pitch==pitch || !audioSourceSfx.isPlaying) { 
                    audioSourceSfx.pitch = pitch;
                    audioSourceSfx.PlayOneShot(audioClip, adjVolume);
                }
                else
                    PlayClipNewAudioSource(audioClip, ac, AudioManager.Instance.transform, Vector3.zero, volume, pitch);
            } 
            else {
                Debug.LogError("AudioManager.PlayClip() call received a bad or missing audioclip: " + audioClip);
            }
        }

        /// <summary>
        /// Fastest and simplest way to play a sound clip AT A SPECIFIC POSITION that won't be interrupted.  
        /// But note that it can't be controlled (paused, stopped, volume, pitch, etc) after it starts playing.
        /// </summary>
        /// <param name="audioClip"></param>
        /// <param name="ac"></param>
        /// <param name="position"></param>
        /// <param name="volume"></param>
        /// <param name="pitch"></param>
        public void PlayClip(AudioClip audioClip, AudioCategory ac, Vector3 position, float volume=1, float pitch=1) {
            if (audioClip) {
                float adjVolume = CalcAdjustedVolume(ac, volume);
                // start with simplest and fastest option
                if (pitch==1) { 
                    AudioSource.PlayClipAtPoint(audioClip, position, adjVolume);
                }
                else
                    PlayClipNewAudioSource(audioClip, ac, AudioManager.Instance.transform, Vector3.zero, volume, pitch);
            } 
            else {
                Debug.LogError("AudioManager.PlayClip() call received a bad or missing audioclip: " + audioClip);
            }
        }

        /// <summary>
        /// Plays a sound clip that won't be interrupted. 
        /// Also returns the AudioSource so the sound clip can be controlled (paused, stopped, volume, pitch, etc).
        /// Slightly slower and uses more memory than PlayClip().
        /// </summary>
        /// <param name="audioClip"></param>
        /// <param name="ac"></param>
        /// <param name="volume"></param>
        /// <param name="pitch"></param>
        /// <returns></returns>
        public AudioSource PlayClipControllable(AudioClip audioClip, AudioCategory ac, float volume=1, float pitch=1) {
            if (audioClip) {
                float adjVolume = CalcAdjustedVolume(ac, volume);
                // start with simplest and fastest option
                audioSourceSfx = audioSourceSfxPool.GetNextAudioSource();
                if (!audioSourceSfx.isPlaying) {
                    audioSourceSfx.clip = audioClip;
                    audioSourceSfx.volume = adjVolume;
                    audioSourceSfx.pitch = pitch;
                    audioSourceSfx.Play();
                    return audioSourceSfx;
                }
                // Slightly slower option
                else {
                    AudioSource audioSourceTemp = PlayClipNewAudioSource(audioClip, ac, AudioManager.Instance.transform, Vector3.zero, volume, pitch);
                    return audioSourceTemp;       
                }
            } else {
                Debug.LogError("AudioManager.PlayClipControllable() call received a bad or missing audioclip: " + audioClip);
                return null;
            }
        }

        /// <summary>
        /// Plays a sound clip AT A SPECIFIC POSITION that won't be interrupted. 
        /// Also returns the AudioSource so the sound clip can be controlled (paused, stopped, volume, pitch, etc).
        /// Slightly slower and uses more memory than PlayClip().
        /// </summary>
        /// <param name="audioClip"></param>
        /// <param name="ac"></param>
        /// <param name="position"></param>
        /// <param name="volume">Volume prior to any user settings (master volume, sfx volume, etc).</param>
        /// <param name="pitch"></param>
        /// <returns></returns>
        public AudioSource PlayClipControllable(AudioClip audioClip, AudioCategory ac, Vector3 position, float volume=1, float pitch=1) {
            if (audioClip) {
                float adjVolume = CalcAdjustedVolume(ac, volume);
                // start with simplest and fastest option
                audioSourceSfx = audioSourceSfxPool.GetNextAudioSource();
                if (!audioSourceSfx.isPlaying) {
                    audioSourceSfx.clip = audioClip;
                    audioSourceSfx.volume = adjVolume;
                    audioSourceSfx.pitch = pitch;
                    audioSourceSfx.Play();
                    return audioSourceSfx;
                }
                // Slightly slower option
                else {
                    AudioSource audioSourceTemp = PlayClipNewAudioSource(audioClip, ac, AudioManager.Instance.transform, Vector3.zero, volume, pitch);
                    return audioSourceTemp;           
                }
            } else {
                Debug.LogError("AudioManager.PlayClipControllable() call received a bad or missing audioclip: " + audioClip);
                return null;
            }
        }

        private AudioSource PlayClipNewAudioSource(AudioClip audioClip, AudioCategory ac, Transform parent, Vector3 position, float volume=1, float pitch=1) {
            if (audioClip) {
                float adjVolume = CalcAdjustedVolume(ac, volume);    
                GameObject tempGO = new GameObject("Temp AudioSource");
                tempGO.transform.parent = parent;
                AudioSource audioSourceTemp = tempGO.AddComponent<AudioSource>();
                audioSourceTemp.clip = audioClip;
                audioSourceTemp.volume = adjVolume;
                audioSourceTemp.pitch = pitch;
                tempGO.transform.position = position;
                audioSourceTemp.Play();
                Destroy(tempGO, audioClip.length); // sets a timer to destroy the temp audioSource after the clip is done playing
                return audioSourceTemp;
            }
            else {
                Debug.LogError("AudioManager.PlayClipNewAudioSource() call received a bad or missing audioclip: " + audioClip);
                return null;
            }
        }

        /// <summary>
        /// Plays a random clip from an array of clips, at a random pitch. (volume and pitch inputs are optional)
        /// </summary>
        /// <param name="clips"></param>
        /// <param name="ac"></param>
        /// <param name="volume"></param>
        /// <param name="pitchMin"></param>
        /// <param name="pitchMax"></param>
        public void PlayRandomClip(AudioClip[] clips, AudioCategory ac, float volume=1, float pitchMin=1, float pitchMax=1) {
            if(clips.Length!=0) { 
                float pitch = Random.Range(pitchMin, pitchMax);
                int clipIndex = Random.Range(0,clips.Length);
                PlayClip(clips[clipIndex], ac, volume, pitch);
            } else {
                Debug.Log("AudioManager.PlayRandomClip() can't play: clips array is empty. Skipping.");  
                return; 
            }
        }

        /// <summary>
        /// Plays a random clip from a List of clips, at a random pitch. Pitch inputs are optional.
        /// </summary>
        /// <param name="clips"></param>
        /// <param name="ac"></param>
        /// <param name="volume"></param>
        /// <param name="pitchMin"></param>
        /// <param name="pitchMax"></param>
        public void PlayRandomClip(List<AudioClip> clips, AudioCategory ac, float volume=1, float pitchMin=1, float pitchMax=1) {
            if(clips.Count!=0) { 
                float pitch = Random.Range(pitchMin, pitchMax);
                int clipIndex = Random.Range(0,clips.Count);
                PlayClip(clips[clipIndex], ac, volume, pitch);
            } else {
                Debug.LogError("AudioManager.PlayRandomClip() can't play: clips array is empty. Skipping.");  
                return; 
            }
        }


        /// <summary>
        /// Loads a music track to the music AudioSource and plays it. (delay, volume, and pitch are optional)
        /// </summary>
        /// <param name="music"></param>
        /// <param name="volume"></param>
        /// <param name="pitch"></param>
        /// <param name="delay"></param>
        public void PlayMusic(AudioClip music, bool loop=true, float volume=1, float pitch=1, float delay=0) {
            if(music!=null) {
                volume = CalcAdjustedVolume(AudioCategory.Music, volume);
                audioSourceMusic.clip = music;
                audioSourceMusic.loop = loop;
                if (delay>0)
                    audioSourceMusic.PlayDelayed(delay);
                else
                    audioSourceMusic.Play();
            }
            else {
                Debug.LogError("AudioManager.PlayMusic received bad or missing music: " + music.name);
            }
        }

        public void PauseMusic() {
            audioSourceMusic.Pause();
            isMusicPaused = true;
        }

        public void UnPauseMusic() {
            audioSourceMusic.UnPause();
            isMusicPaused = false;
        }

        public void StopMusic() {
            audioSourceMusic.Stop();
        }

        public bool IsMusicPlaying() {
            return (audioSourceMusic.isPlaying);
        }
        
        public bool IsMusicPaused() {
            return isMusicPaused;
        }

        public void CalibrateAdjustedVolumes() {
            if (audioSourceMusic)
                audioSourceMusic.volume = volumeMaster * volumeMusic;
            if (audioSourceSfx)
                audioSourceSfx.volume = volumeMaster * volumeSfx;
            if (audioSourceUI)
                audioSourceUI.volume = volumeMaster * volumeUI;
        }

        /// <summary>
        /// Calculates the adjusted volume after taking into account master volume level and audio type volume level.
        /// </summary>
        /// <param name="ac"></param>
        /// <param name="rawVolume"></param>
        /// <returns></returns>
        private float CalcAdjustedVolume(AudioCategory ac, float rawVolume) {
            float adjustedVolume = 1f;
            if (ac==AudioCategory.Music)
                adjustedVolume = rawVolume * volumeMaster * volumeMusic;
            if (ac==AudioCategory.Sfx)
                adjustedVolume = rawVolume * volumeMaster * volumeSfx;
            if (ac==AudioCategory.UI)
                adjustedVolume = rawVolume * volumeMaster * volumeUI;
            return adjustedVolume;
        }


        // TODO: FIGURE OUT A BETTER WAY TO CONNECT MUSIC WITH RELATED SCENE.  BUILD INDEXES COULD CHANGE.
        /// <summary>
        /// Plays a music track as soon as a scene is finished loading.
        /// use this format in any script (e.g. MusicManager) to play a music track any as soon as a scene is loaded
        /// Note: need to subscribe in Awake (SceneManager.sceneLoaded += OnSceneLoaded;) unsubscribe in OnDestroy (SceneManager.sceneLoaded -= OnSceneLoaded;)
        /// </summary>
        private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
            if (scene.buildIndex == 0)      // Main Menu
                AudioManager.Instance.PlayMusic(MusicManager.mainMenuMusic, true, 0.2f);
            else if (scene.buildIndex == 1) // Game
                AudioManager.Instance.PlayMusic(MusicManager.gameMusic, true, 0.2f);
            else if (scene.buildIndex == 2) // Game Over
                AudioManager.Instance.PlayMusic(MusicManager.gameOverMusic, true, 0.2f);
        }

        private void OnDestroy() {
            SceneManager.sceneLoaded -= OnSceneLoaded;  // unsubscribe to OnSceneLoaded event to play music for that scene
        }

    }
}

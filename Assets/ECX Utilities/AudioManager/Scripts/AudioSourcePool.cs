/*
ECX UTILITY SCRIPTS
Audio Source Pool
Last updated: June 18, 2021
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EcxUtilities {
    /// <summary>
    /// 
    /// </summary>
    public class AudioSourcePool : MonoBehaviour {
        public static AudioSourcePool Instance { get; private set; }
        public int audioSourcePoolQty = 10;
        public List<AudioSource> audioSourcePool { get; private set; }
        public bool verboseLogs = false;

        private void Awake() {
            // check if Instance already exists, if so, delete duplicate instance
            if (Instance)
                Destroy(this);
            else
                Instance = this;
            // populate the audioSourcePool
            if (audioSourcePool == null) {
                Instance.InitializeAudioSourcePool();
            }
        }

        private void InitializeAudioSourcePool() {
            if (verboseLogs)
                Debug.Log("Initializing audiosourcepool");
            audioSourcePool = new List<AudioSource>();
            for (int i=0; i<audioSourcePoolQty; i++) {
                AddNewAudioSourceToPool();
            }
            if (verboseLogs)
                Debug.Log("Initialized " + audioSourcePool.Count + " AudioSources in the pool");
        }

        private AudioSource AddNewAudioSourceToPool() {
            GameObject asgo = new GameObject("AudioSource " + (audioSourcePool.Count+1));
            asgo.transform.parent = Instance.transform;
            AudioSource audioSource = asgo.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
            audioSource.loop = false;
            audioSource.volume = AudioManager.Instance.VolumeSfx;
            audioSourcePool.Add(audioSource);
            return audioSource;
        }

        public AudioSource GetNextAudioSource() {
            if (verboseLogs)
                Debug.Log("Getting next AudioSource out of " + audioSourcePool.Count);
            if (audioSourcePool == null)
                Instance.InitializeAudioSourcePool();
            foreach (AudioSource audioSource in audioSourcePool) {
                if (verboseLogs)
                    Debug.Log("Checking if AudioSource: " + audioSource.name + " is available to use");
                if (!audioSource.isPlaying) {
                    return audioSource;
                }
            }
            // if we reach this point, that means all audioSources are in use.  Create a new one and return it
            if (verboseLogs)
                Debug.Log("No AudioSource available, creating new one");
            AudioSource newAudioSource = AddNewAudioSourceToPool();
            return newAudioSource;
        }

    }
}
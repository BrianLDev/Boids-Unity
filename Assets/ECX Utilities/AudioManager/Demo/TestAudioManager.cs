using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EcxUtilities; // for AudioManager, MusicManager, SfxManager, UISfxManager, AudioCategory

public class TestAudioManager : MonoBehaviour {

  public void PlayMusicTest() {
    AudioManager.Instance.PlayMusic(AudioManager.Instance.MusicManager.gameMusic);
  }

  public void PlayStingerTest() {
    AudioManager.Instance.PlayMusic(AudioManager.Instance.MusicManager.victoryStinger, false);
  }

  public void StopMusicTest() {
    AudioManager.Instance.StopMusic();
  }

  public void PlayUISfxTest() {
    float pitch = Random.Range(.9f, 1.1f);
    float volume = 1;
    AudioManager.Instance.PlayClip(AudioManager.Instance.UISfxManager.buttonClick, AudioCategory.UI, volume, pitch);
  }

  public void PlaySfxTest() {
    float pitch = Random.Range(.7f, 1.3f);
    float volume = 1;
    AudioClip sfxTest = AudioManager.Instance.SfxManager.sfx01;
    AudioManager.Instance.PlayClip(sfxTest, AudioCategory.Sfx, volume, pitch);
  }

}
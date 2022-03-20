using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
  public List<AudioClip> AudioClips;

  AudioSource audioSource;

  void Start()
  {
    audioSource = GetComponent<AudioSource>();
    StartCoroutine("playAllClipsInOrder");
    DontDestroyOnLoad(gameObject);
  }

  IEnumerator playAllClipsInOrder()
  {
    yield return null;

    for (int i = 0; i < AudioClips.Count; i++)
    {
      audioSource.clip = AudioClips[i];
      audioSource.Play();

      yield return new WaitForSeconds(1f);

      // Wait for it to finish playing the current clip before going to the next one
      while (audioSource.isPlaying)
      {
        yield return null;
      }
    }

    StartCoroutine("playAllClipsInOrder");
  }
}

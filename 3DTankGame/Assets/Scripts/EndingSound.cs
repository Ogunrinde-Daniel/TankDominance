using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndingSound : MonoBehaviour
{
    public AudioSource endingSource;
    public List<AudioClip> endingMusicList = new List<AudioClip>();

    void Start()
    {
        int index = Random.Range(0, endingMusicList.Count);
        endingSource.clip = endingMusicList[index];
        endingSource.Play();
    }


}

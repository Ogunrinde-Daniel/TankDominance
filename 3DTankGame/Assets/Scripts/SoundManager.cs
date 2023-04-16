using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SoundManager : MonoBehaviour
{
    public bool soundOn = true;

    public AudioSource bgSource;
    public AudioSource carEngine;
    public AudioSource carMoving;
    public AudioSource sfx;

    public List<AudioClip> bgMusicList = new List<AudioClip>();
    public AudioClip gunShot;
    public AudioClip tankMoving;
    public AudioClip engine;

    private int currentBg = 0;
    private bool engineOn;
    private bool revOn;
    private bool shotOn;

    void Start()
    {
        if (soundOn)
        {
            currentBg = Random.Range(0, bgMusicList.Count);
            playBgMusic();
        }
    }

    public void revEngine()
    {
        if(!soundOn || revOn){return;}
        carMoving.clip = tankMoving;
        carMoving.Play();
        revOn = true;
    }    
    public void stopRevEngine()
    {
        if(!soundOn){return;}
        carMoving.clip = tankMoving;
        carMoving.Stop();
        revOn = false;
    }
    public void startEngine()
    {
        if(!soundOn || engineOn){return;}

        carEngine.clip = engine;
        carEngine.Play();
        engineOn = true;
        
    }

    public void stopEngine()
    {
        if(!soundOn){return;}

        carEngine.Stop();
        revEngine();
        engineOn = false;
    }

    public void playShot()
    {
        if(!soundOn || shotOn){return;}
        sfx.clip = gunShot;
        sfx.Play();
        shotOn = true;
    }

    public void stopShot()
    {
        if (!soundOn){return;}
        sfx.clip = gunShot;
        sfx.Stop();
        shotOn = false;
    }

    public void playBgMusic()
    {
        if(!soundOn){return;}
        
        currentBg = (currentBg + 1) % bgMusicList.Count;
        float clipLength = bgMusicList[currentBg].length;
        bgSource.clip = bgMusicList[currentBg];
        bgSource.Play();
        Invoke("playBgMusic", clipLength);
    }
}

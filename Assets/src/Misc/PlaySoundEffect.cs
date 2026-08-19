using System;
using UnityEngine;

public class PlaySoundEffect : MonoBehaviour
{
    public AudioSource[] SFX;
    public bool isLooping = false;

    public void PlaySFX(int value)
    {
        SFX[value].Play();
    }

    public void LoopSound(int value, bool loopValue)
    {
        SFX[value].loop = loopValue;
        isLooping = loopValue;
        if (isLooping)
        {
            SFX[value].Play();
        }
    }
    public void RandomisePitch(int value, float range1, float range2)
    {
        SFX[value].pitch = UnityEngine.Random.Range(range1, range2);
    }

    public void setClipToEnd(int value)
    {
        SFX[value].Stop();
        SFX[value].time = SFX[value].clip.length - 0.01f;

    }
    public void setClipToStart(int value)
    {
        SFX[value].Stop();
        SFX[value].time = 0;
    }

    public void stopSFX(int value)
    {
        SFX[value].Stop();
    }
}

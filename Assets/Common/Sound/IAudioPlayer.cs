using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

namespace Common.Sound
{
    public interface IAudioPlayer
    {
        GameObject GameObject { get; }
        AudioClip CClip { get; }
        AudioMixerGroup ReverbMixerGroup { get; }
        
        Coroutine StartCoroutine(IEnumerator coroutine);
    }
}
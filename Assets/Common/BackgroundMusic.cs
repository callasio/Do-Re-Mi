using Monotone;
using UnityEngine;

namespace Common
{
    public class BackgroundMusic : MonoBehaviour
    {
        private AudioSource _audioSource;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            _audioSource = GetComponentInChildren<AudioSource>();
        }

        // Update is called once per frame
        void Update()
        {
            _audioSource.volume = Volume.GetBackgroundVolume() * 0.15f;
        }  
    }
}

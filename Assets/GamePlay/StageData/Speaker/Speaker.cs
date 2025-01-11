using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

namespace GamePlay.StageData.Speaker
{
    public class Speaker : StageElementBehaviour
    {
        public bool IsSoundOn { get; set; }
        private ParticleSystem _soundParticleSystem;
        private const float MaxLifetime = 75f;
        
        public override void OnClicked() { }

        public override void Start()
        {
            base.Start();
            _soundParticleSystem = GetComponentInChildren<ParticleSystem>();
            if ((Data.Type & StageElementType.OffSound) == 0)
            {  
                TurnOnSound();
            }
            else
            {
                TurnOffSound();
            }
        }

        public void TurnOnSound()
        {
            IsSoundOn = true;
            _soundParticleSystem.Simulate(MaxLifetime);
            _soundParticleSystem.Play();
        }
        
        public void TurnOffSound()
        {
            IsSoundOn = false;
            _soundParticleSystem.Stop();
        }
    }
}
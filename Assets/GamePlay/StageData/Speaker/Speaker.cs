using Unity.VisualScripting;
using UnityEngine;

namespace GamePlay.StageData.Speaker
{
    public class Speaker : StageElementBehaviour
    {
        public GameObject particleSystem;
        
        public override void OnClicked() { }

        public override void Start()
        {
            base.Start();
            TurnOnSound();
        }

        public void TurnOnSound()
        {
            particleSystem.SetActive(true);
        }
    }
}
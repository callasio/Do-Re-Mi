using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

namespace GamePlay.StageData.Speaker
{
    public class Speaker : MovingElement
    {
        public bool IsSoundOn { get; set; }
        private ParticleSystem _soundParticleSystem;
        private const float MaxLifetime = 75f;
        private Animation _animation;
        private bool _canBePushed;

        public override void Start()
        {
            base.Start();
            _animation = GetComponentInChildren<Animation>();
            _soundParticleSystem = GetComponentInChildren<ParticleSystem>();
            _canBePushed = Data.Metadata.TryGetValue("pushable", out var pushable) && bool.Parse(pushable);
            
            if (Data.Metadata["note"] != null)
            {  
                TurnOnSound();
            }
            else
            {
                TurnOffSound();
            }
        }

        public override void OnClicked()
        {
            _animation.Stop();
            _animation.Play();
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

        public override Direction MovingDirection { get; protected set; }
        public override Coordinates TargetCoordinates { get; set; }

        public override bool Push(Direction direction)
        {
            if (!_canBePushed) return false;
            
            var targetCoordinates = Data.Coordinates + direction;

            var elementsInTarget = Data.CurrentStageData.Where(data => data.Coordinates == targetCoordinates).ToList();
            if (elementsInTarget.All(data => data.Type != StageElementType.Tile)) return false;
            
            foreach (var element in elementsInTarget)
            {
                switch (element.Type)
                {
                    case StageElementType.Speaker:
                        if (element.StageElementInstanceBehaviour is not Speaker speaker) return false;
                        if (!speaker.Push(direction)) return false;
                        break;
                    case StageElementType.Tile:
                    case StageElementType.Player:
                    default:
                        break;
                }
            }

            TargetCoordinates = targetCoordinates;
            return true;
        }

        protected override bool IsMoving => TargetCoordinates != Data.Coordinates;

        protected override bool SetNewTarget()
        {
            return true;
        }

        protected override void OnReachedTarget() { }
    }
}
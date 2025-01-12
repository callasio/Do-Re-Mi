using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace GamePlay.StageData.Speaker
{
    public class Speaker : MovingElement
    {
        public bool IsSoundOn { get; set; }
        public Sprite fix;
        public Sprite move;
        public Sprite rotate;
        public Sprite moveAndRotate;
        
        private const float AngularVelocity = 180f;
        private Direction TargetDirection { get; set; }

        protected override bool IsMoving => TargetCoordinates != Data.Coordinates;
        protected override bool SetNewTarget() => true;
        protected override void OnReachedTarget() { }
        public override Direction MovingDirection { get; protected set; }
        public override Coordinates TargetCoordinates { get; set; }
        
        private ParticleSystem _soundParticleSystem;
        private const float MaxLifetime = 75f;
        private Animation _animation;
        private bool _canBePushed;
        private bool _rotatable;
        private Image _image;

        public override void Start()
        {
            base.Start();
            _animation = GetComponentInChildren<Animation>();
            _soundParticleSystem = GetComponentInChildren<ParticleSystem>();
            _canBePushed = Data.Metadata.TryGetValue("pushable", out var pushable) && bool.Parse(pushable);
            _rotatable = Data.Metadata.TryGetValue("rotatable", out var rotatable) && bool.Parse(rotatable);
            TargetDirection = Data.Direction;
            _image = GetComponentInChildren<Image>();
            
            if (Data.Metadata["note"] != null)
            {  
                TurnOnSound();
            }
            else
            {
                TurnOffSound();
            }
            
            if (_rotatable)
            {
                _image.sprite = _canBePushed ? moveAndRotate : rotate;
            }
            else
            {
                _image.sprite = _canBePushed ? move : fix;
            }
        }

        public override void OnClicked(Vector3 normal, bool forward = true)
        {
            if (forward)
            {
                if (normal == Vector3.up && _rotatable)
                {
                    TargetDirection = Data.Direction.CounterClockwise();
                    return;
                }
                Data.CurrentStageElements
                    .Where(data => data.Coordinates == Data.Coordinates && data != Data).ToList()
                    .ForEach(data => data.StageElementInstanceBehaviour.OnClicked(normal, forward: false));
            }

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
        
        public override void Update()
        {
            base.Update();
            Rotate();
        }

        private void Rotate()
        {
            if (Data.Direction == TargetDirection) return;

            if (Quaternion.Angle(transform.rotation, TargetDirection.ToQuaternion()) <
                AngularVelocity * Time.deltaTime)
            {
                Data.Direction = TargetDirection;
                transform.rotation = TargetDirection.ToQuaternion();
                return;
            }
            
            transform.rotation = Quaternion.RotateTowards(transform.rotation, TargetDirection.ToQuaternion(), AngularVelocity * Time.deltaTime);
        }

        public override bool Push(Direction direction)
        {
            if (!_canBePushed) return false;
            
            var targetCoordinates = Data.Coordinates + direction;

            var elementsInTarget = Data.CurrentStageElements.Where(data => data.Coordinates == targetCoordinates).ToList();
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
    }
}
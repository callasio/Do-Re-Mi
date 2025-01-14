using System.Collections.Generic;
using System.Linq;
using Common.Sound;
using UnityEngine;
using UnityEngine.UI;

namespace GamePlay.StageData.Speaker
{
    public class Speaker : MovingElement
    {
        private bool IsSoundOn { get; set; }
        private HashSet<Note> _playNote = new ();
        public HashSet<Note> PlayNote
        {
            get => _playNote;
            private set
            {
                _playNote = value;
                var noteColor = Note.GetColor(_playNote);
                var mainModule = _soundParticleSystem.main;
                mainModule.startColor = noteColor;
            }
        }
        public Sprite rotate;
        public Sprite download;
        
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
        private ImageSprite _imageSprite;

        private Player.Player Player => 
            Data.CurrentStageData.Elements.First(data => data.Type == StageElementType.Player)
            .StageElementInstanceBehaviour as Player.Player;

        private enum ImageSprite
        {
            Rotate,
            Download,
            None,
        }

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
                PlayNote = new HashSet<Note> { new(Data.Metadata["note"]) };
                TurnOnSound(PlayNote);
            }
            else
            {
                PlayNote = new HashSet<Note>();
                TurnOffSound();
            }
            
            SetImage(_rotatable ? ImageSprite.Rotate : ImageSprite.None);
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
                if (normal == Vector3.up && _imageSprite == ImageSprite.Download)
                {
                    var player = Player;
                    if (player is null) return;
                    if (player.SoundManager.RecordedNotes.Count == 0) return;
                    SetImage(_rotatable ? ImageSprite.Rotate : ImageSprite.None);
                    TurnOnSound(player.SoundManager.RecordedNotes);
                    return;
                }
                Data.CurrentStageElements
                    .Where(data => data.Coordinates == Data.Coordinates && data != Data).ToList()
                    .ForEach(data => data.StageElementInstanceBehaviour.OnClicked(normal, forward: false));
            }

            _animation.Stop();
            _animation.Play();
        }

        public void TurnOnSound(HashSet<Note> note)
        {
            if (note.Count > 0) PlayNote = note;
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
        
        public override void OnElementUpdate()
        {
            var player = Player;
            if (Player is null) return;
            
            if ((player.Data.Coordinates.ToVector3() - Data.Coordinates.ToVector3()).magnitude > 1.1f)
            {
                SetImage(_rotatable ? ImageSprite.Rotate : ImageSprite.None);
            }
            else
            {
                SetImage(IsSoundOn ? _rotatable ? ImageSprite.Rotate : ImageSprite.None : ImageSprite.Download);
            }
        }

        private void SetImage(ImageSprite imageSprite)
        {
            _imageSprite = imageSprite;
            switch (imageSprite)
            {
                case ImageSprite.Rotate:
                    _image.sprite = rotate;
                    _image.color = Color.white;
                    break;
                case ImageSprite.Download:
                    _image.sprite = download;
                    _image.color = Color.white;
                    break;
                case ImageSprite.None:
                    _image.sprite = null;
                    _image.color = Color.clear;
                    break;
            }
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
            UpdateElementState();
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
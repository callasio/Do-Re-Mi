using System;
using System.Collections.Generic;
using System.Linq;
using GamePlay.StageData.Player.PathFinder;
using GamePlay.StageData.Player.Sound;
using UnityEngine;

namespace GamePlay.StageData.Player
{
    public class Player: MovingElement
    {
        public const float DefaultMovingSpeed = 3f;
        
        public float movingSpeed = DefaultMovingSpeed;

        private List<Coordinates> _movingQueue = new ();
        private SoundManager _soundManager;
        
        public static event Action<StageElement> OnElementClicked;
        public static event Action OnRecordClicked;
        public static event Action OnRecordHovered;
        public static event Action OnRecordHoverEnded;
        public static event Action OnStartClicked;
        
        public static void ElementClicked(StageElement clickedElement) => OnElementClicked?.Invoke(clickedElement);
        public static void RecordClicked() => OnRecordClicked?.Invoke();
        public static void RecordHovered() => OnRecordHovered?.Invoke();
        public static void RecordHoverEnded() => OnRecordHoverEnded?.Invoke();
        public static void StartClicked() => OnStartClicked?.Invoke();
        
        public override void Start()
        {
            base.Start();
            MovingDirection = Direction.None;
            _soundManager = new SoundManager(this, AudioSources.Player);
            OnElementClicked += ElementClickedHandler;
            OnRecordClicked += RecordClickedHandler;
            OnRecordHovered += RecordHoveredHandler;
            OnRecordHoverEnded += RecordHoverEndedHandler;
            OnStartClicked += StartClickedHandler;
        }

        public void OnDestroy()
        {
            OnElementClicked -= ElementClickedHandler;
            OnRecordClicked -= RecordClickedHandler;
            OnRecordHovered -= RecordHoveredHandler;
            OnRecordHoverEnded -= RecordHoverEndedHandler;
            OnStartClicked -= StartClickedHandler;
        }

        public override void OnClicked(Vector3 normal, bool forward = true) { }
        
        private void ElementClickedHandler(StageElement clickedElement)
        {
            if (clickedElement.Type != StageElementType.Tile) return;
            
            var clickedCoordinates = clickedElement.Coordinates;
            var movePath = BFS.GetPath(Data.CurrentStageElements, TargetCoordinates, clickedCoordinates);
            
            if (movePath == null) return;
            _movingQueue = movePath;
            if (Data.Coordinates != TargetCoordinates)
            {
                _movingQueue.Insert(0, TargetCoordinates);
            }
        }

        private void RecordClickedHandler()
        {
            _soundManager.Record();
        }
        
        private void RecordHoveredHandler()
        {
            _soundManager.AudioSource = AudioSources.Record;
        }
        
        private void RecordHoverEndedHandler()
        {
            _soundManager.AudioSource = AudioSources.Player;
        }

        private static void StartClickedHandler()
        {
            Debug.Log("StartClickedHandler");
        }

        public override void Update()
        {
            base.Update();
            LookAt(Data.Direction);
            _soundManager.Update();
        }

        public override Direction MovingDirection { get; protected set; }
        public override Coordinates TargetCoordinates { get; set; }

        public override bool Push(Direction direction) => false;

        protected override bool IsMoving => _movingQueue.Count > 0;

        protected override bool SetNewTarget()
        {
            if (TargetCoordinates == _movingQueue[0]) return true;
            if (!OnNewMoveEvent(_movingQueue[0]))
            {
                _movingQueue.Clear();
                return false;
            }
            TargetCoordinates = _movingQueue[0];
            return true;
        }

        protected override void OnReachedTarget()
        {
            _movingQueue.RemoveAt(0);
        }
        
        private bool OnNewMoveEvent(Coordinates targetCoordinates)
        {
            var elements = Data.CurrentStageElements.Where(element => element.Coordinates == targetCoordinates);
            var direction = targetCoordinates - Data.Coordinates;
            foreach (var element in elements)
            {
                switch (element.Type)
                {
                    case StageElementType.Speaker:
                        var speaker = element.StageElementInstanceBehaviour as Speaker.Speaker;
                        return speaker?.Push(direction) ?? true;
                    case StageElementType.Player:
                    case StageElementType.Tile:
                    default:
                        break;
                }
            }

            return true;
        }
    }
}
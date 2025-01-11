using System;
using System.Collections.Generic;
using GamePlay.StageData.Player.PathFinder;
using GamePlay.StageData.Player.Sound;
using UnityEngine;

namespace GamePlay.StageData.Player
{
    public class Player: StageElementBehaviour
    {
        public const float DefaultMovingSpeed = 3f;
        
        public float movingSpeed = DefaultMovingSpeed;

        public Direction MovingDirection { get; private set; }

        private List<Coordinates> _movingQueue = new ();
        public Coordinates TargetCoordinates { get; private set; }
        private SoundManager _soundManager;
        
        public static event Action<StageElementData> OnElementClicked;
        public static event Action OnRecordClicked;
        public static event Action OnRecordHovered;
        public static event Action OnRecordHoverEnded;
        
        public static void ElementClicked(StageElementData clickedElementData) => OnElementClicked?.Invoke(clickedElementData);
        public static void RecordClicked() => OnRecordClicked?.Invoke();
        public static void RecordHovered() => OnRecordHovered?.Invoke();
        public static void RecordHoverEnded() => OnRecordHoverEnded?.Invoke();
        
        
        public override void Start()
        {
            base.Start();
            TargetCoordinates = Data.Coordinates;
            MovingDirection = Direction.None;
            _soundManager = new SoundManager(this, AudioSources.Player);
            OnElementClicked += ElementClickedHandler;
            OnRecordClicked += RecordClickedHandler;
            OnRecordHovered += RecordHoveredHandler;
            OnRecordHoverEnded += RecordHoverEndedHandler;
        }

        public void OnDestroy()
        {
            OnElementClicked -= ElementClickedHandler;
            OnRecordClicked -= RecordClickedHandler;
            OnRecordHovered -= RecordHoveredHandler;
            OnRecordHoverEnded -= RecordHoverEndedHandler;
        }

        public override void OnClicked() { }
        
        private void ElementClickedHandler(StageElementData clickedElementData)
        {
            if (clickedElementData.Type != StageElementType.Tile) return;
            
            var clickedCoordinates = clickedElementData.Coordinates;
            var movePath = BFS.GetPath(Data.CurrentStageData, TargetCoordinates, clickedCoordinates);
            
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

        public void Update()
        {
            LookAt(Data.Direction);
            Move();
            _soundManager.Update();
        }
        
        private void Move()
        {
            if (_movingQueue.Count == 0)
            {
                MovingDirection = Direction.None;
                return;
            }
            
            TargetCoordinates = _movingQueue[0];
            MovingDirection = TargetCoordinates - Data.Coordinates;
            var currentPosition = transform.position;
            var target = TargetCoordinates.ToVector3();

            var distance = Vector3.Distance(currentPosition, target);
            var step = movingSpeed * Time.deltaTime;
            
            if (step >= distance)
            {
                transform.position = target;
                Data.Coordinates = TargetCoordinates;
                _movingQueue.RemoveAt(0);
            }
            else
            {
                transform.position = Vector3.MoveTowards(currentPosition, target, step);
            }
        }
    }
}
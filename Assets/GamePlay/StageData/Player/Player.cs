using System;
using System.Collections.Generic;
using GamePlay.StageData.Player.PathFinder;
using UnityEngine;

namespace GamePlay.StageData.Player
{
    public class Player: StageElementBehaviour
    {
        public const float DefaultMovingSpeed = 4f;
        public static event Action<StageElementData> OnElementClicked;
        public float movingSpeed = DefaultMovingSpeed;
        
        public static void ElementClicked(StageElementData clickedElementData)
        {
            OnElementClicked?.Invoke(clickedElementData);
        }
        
        public override void Start()
        {
            base.Start();
            _targetCoordinates = Data.Coordinates;
            OnElementClicked += ElementClickedHandler;
        }

        public void OnDestroy()
        {
            OnElementClicked -= ElementClickedHandler;
        }

        public override void OnClicked() { }

        private List<Coordinates> _movingQueue = new ();
        private Coordinates _targetCoordinates;
        
        private void ElementClickedHandler(StageElementData clickedElementData)
        {
            if (clickedElementData.Type != StageElementType.Tile) return;
            
            var clickedCoordinates = clickedElementData.Coordinates;
            var movePath = BFS.GetPath(Data.CurrentStageData, _targetCoordinates, clickedCoordinates);
            
            if (movePath == null) return;
            _movingQueue = movePath;
            if (Data.Coordinates != _targetCoordinates)
            {
                _movingQueue.Insert(0, _targetCoordinates);
            }
        }

        public void Update()
        {
            Move();
        }
        
        private void Move()
        {
            if (_movingQueue.Count == 0) return;
            
            _targetCoordinates = _movingQueue[0];
            var currentPosition = transform.position;
            var target = _targetCoordinates.ToVector3();

            var distance = Vector3.Distance(currentPosition, target);
            var step = movingSpeed * Time.deltaTime;
            
            if (step >= distance)
            {
                transform.position = target;
                Data.Coordinates = _targetCoordinates;
                _movingQueue.RemoveAt(0);
            }
            else
            {
                transform.position = Vector3.MoveTowards(currentPosition, target, step);
            }
        }
    }
}
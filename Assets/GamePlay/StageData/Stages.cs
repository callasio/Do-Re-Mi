using System;
using UnityEngine;

namespace GamePlay.StageData
{
    public class Stages
    {
        private GameObject StageLoader { get;  }
        private GameObject PlayerPrefab { get; }
        private GameObject TilePrefab { get; }
        private GameObject SpeakerPrefab { get; }
        
        public Stages(StageLoader stageLoader)
        {
            StageLoader = stageLoader.gameObject;
            PlayerPrefab = stageLoader.playerPrefab;
            TilePrefab = stageLoader.tilePrefab;
            SpeakerPrefab = stageLoader.speakerPrefab;
        }

        private StageElementData[] _currentStageData = Array.Empty<StageElementData>();
        
        public void DestroyCurrentStage()
        {
            foreach (var elementData in _currentStageData)
            {
                elementData.DestroyStageElement();
            }
        }

        public void InitStage(int stageIndex)
        {
            _currentStageData = StagesData[stageIndex - 1];
            var cameraLookingPosition = StagesCameraLookingPositions[stageIndex - 1];
            var camera = Camera.main;
            
            if (camera != null)
            {
                camera.transform.position = cameraLookingPosition + CameraBehaviour.Position;
                camera.transform.LookAt(cameraLookingPosition);
            }
            CreateStageElements(_currentStageData, StageLoader);
        }

        public void InitStageSelect()
        {
            _currentStageData = StageSelectData;
            
            if (Camera.main != null)
            {
                Camera.main.transform.position = StageSelectCameraLookingPosition + CameraBehaviour.Position;
                Camera.main.transform.LookAt(StageSelectCameraLookingPosition);
            }
            CreateStageElements(_currentStageData, StageLoader);
        }
        
        private void CreateStageElements(StageElementData[] elementsData, GameObject parent)
        {
            foreach (var elementData in elementsData)
            {
                elementData.CreateStageElement(parent);
            }
        }

        private StageElementData[][] StagesData => new[]
        {
            FirstStageData,
        };
        
        private static Vector3[] StagesCameraLookingPositions => new[]
        {
            FirstStageCameraLookingPosition,
        };
        
        private StageElementData[] FirstStageData => new StageElementData[]
        {
            NewData(new (0, 0), Direction.Up,  StageElementType.Player),
            NewData(new (0, 0), Direction.Up, StageElementType.Tile),
            NewData(new (1, 0), Direction.Up, StageElementType.Tile),
            NewData(new (2, 0), Direction.Up, StageElementType.Tile),
            NewData(new (3, 0), Direction.Up, StageElementType.Tile),
            NewData(new (4, 0), Direction.Up, StageElementType.Tile),
            NewData(new (5, 0), Direction.Up, StageElementType.Tile),
            NewData(new (6, 0), Direction.Up, StageElementType.Tile),
            NewData(new (7, 0), Direction.Up, StageElementType.Tile),
            NewData(new (8, 0), Direction.Up, StageElementType.Tile),
            NewData(new (9, 0), Direction.Up, StageElementType.Tile),
            NewData(new (4, 1), Direction.Up, StageElementType.Tile),
            NewData(new (4, 2), Direction.Up, StageElementType.Tile),
            NewData(new (4, 3), Direction.Up, StageElementType.Tile),
            NewData(new (4, 4), Direction.Up, StageElementType.Tile),
            NewData(new (4, -1), Direction.Up, StageElementType.Tile),
            NewData(new (4, -2), Direction.Up, StageElementType.Tile),
            NewData(new (4, -3), Direction.Up, StageElementType.Tile),
            NewData(new (4, -4), Direction.Up, StageElementType.Tile),
            NewData(new (5, 1), Direction.Up, StageElementType.Tile),
            NewData(new (5, 2), Direction.Up, StageElementType.Tile),
            NewData(new (5, 3), Direction.Up, StageElementType.Tile),
            NewData(new (5, 4), Direction.Up, StageElementType.Tile),
            NewData(new (5, -1), Direction.Up, StageElementType.Tile),
            NewData(new (5, -2), Direction.Up, StageElementType.Tile),
            NewData(new (5, -3), Direction.Up, StageElementType.Tile),
            NewData(new (5, -4), Direction.Up, StageElementType.Tile),
            NewData(new (4, 4), Direction.Down, StageElementType.Speaker),
            NewData(new (5, -4), Direction.Up, StageElementType.Speaker),
        };
        
        private static Vector3 FirstStageCameraLookingPosition => new (4.5f, 0, 0);

        private StageElementData[] StageSelectData
        {
            get
            {
                const int width = 9;
                _currentStageData = new StageElementData[width + 1];

                for (int i = 0; i < width; i++)
                {
                    _currentStageData[i] = NewData(new(i, 0), Direction.None, StageElementType.Tile);
                }
            
                _currentStageData[width] = NewData(new(width-1, 0), Direction.Down, StageElementType.Player);

                return _currentStageData;
            }
        }
        
        private static Vector3 StageSelectCameraLookingPosition => new (4.5f, 0, -4.5f);

        private StageElementData NewData(Coordinates coordinates, Direction direction, StageElementType type)
        {
            var prefab = type switch
            {
                StageElementType.Tile => TilePrefab,
                StageElementType.Player => PlayerPrefab,
                StageElementType.Speaker => SpeakerPrefab,
                _ => null
            };
            
            return new StageElementData(prefab, coordinates, direction, type, _currentStageData);
        }
    }
}
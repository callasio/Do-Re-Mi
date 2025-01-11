using System;
using System.Collections.Generic;
using JetBrains.Annotations;
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
            foreach (var stageElementData in _currentStageData)
            {
                stageElementData.CurrentStageData = _currentStageData;
            }
            
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
        
        public void InitHome()
        {
            _currentStageData = HomeData;
            if (Camera.main != null)
            {
                Camera.main.transform.position = HomeCameraLookingPosition + CameraBehaviour.Position;
                Camera.main.transform.LookAt(HomeCameraLookingPosition);
            }
            CreateStageElements(HomeData, StageLoader);
        }

        private StageElementData[][] StagesData => new[]
        {
            FirstStageData,
        };
        
        private static Vector3[] StagesCameraLookingPositions => new[]
        {
            FirstStageCameraLookingPosition,
        };
        
        private StageElementData[] HomeData
        {
            get
            {
            var tileList = new List<StageElementData>();
                tileList.Add(NewData(new(3,-5), Direction.Up, StageElementType.Player));
                
                tileList.Add(NewTileData(new(3, 4), Direction.Up, "true", "A"));
                tileList.Add(NewTileData(new(3, 3), Direction.Up, "true", "B"));
                tileList.Add(NewTileData(new(3, 2), Direction.Up, "true", "O"));
                tileList.Add(NewTileData(new(3, 1), Direction.Up, "true", "U"));
                tileList.Add(NewTileData(new(3, 0), Direction.Up, "true", "T"));
                
                tileList.Add(NewTileData(new(4, 4), Direction.Up, "true", "S"));
                tileList.Add(NewTileData(new(4, 3), Direction.Up, "true", "E"));
                tileList.Add(NewTileData(new(4, 2), Direction.Up, "true", "T"));
                tileList.Add(NewTileData(new(4, 1), Direction.Up, "true", "T"));
                tileList.Add(NewTileData(new(4, 0), Direction.Up, "true", "I"));
                tileList.Add(NewTileData(new(4, -1), Direction.Up, "true", "N"));
                tileList.Add(NewTileData(new(4, -2), Direction.Up, "true", "G"));
                tileList.Add(NewTileData(new(4, -3), Direction.Up, "true", "S"));
                
                tileList.Add(NewTileData(new(5, 4), Direction.Up, "true", "S"));
                tileList.Add(NewTileData(new(5, 3), Direction.Up, "true", "T"));
                tileList.Add(NewTileData(new(5, 2), Direction.Up, "true", "A"));
                tileList.Add(NewTileData(new(5, 1), Direction.Up, "true", "R"));
                tileList.Add(NewTileData(new(5, 0), Direction.Up, "true", "T"));
                
                tileList.Add(NewTileData(new(3, -1),Direction.Up,"true", null));
                tileList.Add(NewTileData(new(3, -2),Direction.Up,"true", null));
                tileList.Add(NewTileData(new(3, -3),Direction.Up,"true", null));
                tileList.Add(NewTileData(new(5, -1),Direction.Up,"true", null));
                tileList.Add(NewTileData(new(5, -2),Direction.Up,"true", null));
                tileList.Add(NewTileData(new(5, -3),Direction.Up,"true", null));
                
                for (int i = 3; i <= 5; i++)
                {
                    tileList.Add(NewTileData(new(i, 5),Direction.Up,"true", null));
                    tileList.Add(NewTileData(new(i, -5), Direction.Up, "false", null));
                    tileList.Add(NewSpeakerData(new(i, 5), Direction.Down, null));
                }
            return tileList.ToArray();
            }
        }
        
        private static Vector3 HomeCameraLookingPosition => new (4.5f, 0, 0);

        private StageElementData[] FirstStageData 
        {
            get
            {
                const int size = 4;
                var elementList = new List<StageElementData>();
                for (var i = 0; i <= size; i++)
                {
                    for (var j = -size; j <= size; j++)
                    {
                        if (i + Math.Abs(j) <= size)
                        {
                            elementList.Add(NewData(new(-i, j), Direction.None, StageElementType.Tile));
                            elementList.Add(NewData(new(i + 1, j), Direction.None, StageElementType.Tile));
                        }
                    }
                }
                
                elementList.Add(NewData(new(-4, 0), Direction.Right, StageElementType.Player));
                elementList.Add(NewSpeakerData(new (0, 4), Direction.Down, "C"));
                elementList.Add(NewSpeakerData(new (1, -4), Direction.Up, null));
                
                return elementList.ToArray();
            }
        }
        
        private static Vector3 FirstStageCameraLookingPosition => new (0.5f, 0, 0);

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

        private StageElementData NewSpeakerData(Coordinates coordinates, Direction direction, [CanBeNull] string note)
        {
            var data = NewData(coordinates, direction, StageElementType.Speaker);
            data.Metadata["note"] = note;
            return data;
        }
        
        private StageElementData NewTileData(Coordinates coordinates, Direction direction, string fix, [CanBeNull] string text)
        {
            var data = NewData(coordinates, direction, StageElementType.Tile);
            data.Metadata["fix"] = fix;
            data.Metadata["text"] = text;
            return data;
        }

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
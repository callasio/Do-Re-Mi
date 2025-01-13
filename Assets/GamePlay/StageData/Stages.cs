using System;
using System.Collections.Generic;
using System.Linq;
using GamePlay.StageData.Player.Sound;
using UnityEngine;

namespace GamePlay.StageData
{
    public class Stages
    {
        private StageLoader StageLoader { get;  }
        
        public Stages(StageLoader stageLoader)
        {
            StageLoader = stageLoader;
            StageElement.StageLoader = StageLoader;
        }
        
        private StageData _currentStageData = new ();
        
        public void DestroyCurrentStage()
        {
            foreach (var elementData in _currentStageData.Elements)
            {
                elementData.DestroyStageElement();
            }
        }

        public StageData InitStage(int stageIndex) => InitStage(StagesData[stageIndex-1]);
        
        public StageData InitStageSelect() => InitStage(StageSelectData);

        public StageData InitHome() => InitStage(HomeData);

        private StageData InitStage(StageData stageData)
        {
            SetStageDataToElements(stageData);
            AdjustCameraPosition(stageData.Configuration.CameraLookingPosition);
            CreateStageElements(stageData);

            return _currentStageData = stageData; 
        }

        private void SetStageDataToElements(StageData stageData)
        {
            foreach (var stageElementData in stageData.Elements)
            {
                stageElementData.CurrentStageData = stageData;
            }
        }
        
        private void AdjustCameraPosition(Vector3 cameraLookingPosition)
        {
            var camera = Camera.main;
            if (camera is null) return;
            camera.transform.position = cameraLookingPosition + CameraBehaviour.Position;
            camera.transform.LookAt(cameraLookingPosition);
        }
        
        private void CreateStageElements(StageData stageData) =>
            stageData.Elements.ToList().ForEach(elementData => elementData.CreateStageElement(StageLoader.gameObject));
        
        private StageData HomeData
        {
            get
            {
                var tileList = new List<StageElement>();
                    tileList.Add(StageElement.Player(new(3,-5), Direction.Up));
                    
                    tileList.Add(StageElement.Tile(new(3, 4), Direction.Up, "true", "A"));
                    tileList.Add(StageElement.Tile(new(3, 3), Direction.Up, "true", "B"));
                    tileList.Add(StageElement.Tile(new(3, 2), Direction.Up, "true", "O"));
                    tileList.Add(StageElement.Tile(new(3, 1), Direction.Up, "true", "U"));
                    tileList.Add(StageElement.Tile(new(3, 0), Direction.Up, "true", "T"));
                    
                    tileList.Add(StageElement.Tile(new(4, 4), Direction.Up, "true", "S"));
                    tileList.Add(StageElement.Tile(new(4, 3), Direction.Up, "true", "E"));
                    tileList.Add(StageElement.Tile(new(4, 2), Direction.Up, "true", "T"));
                    tileList.Add(StageElement.Tile(new(4, 1), Direction.Up, "true", "T"));
                    tileList.Add(StageElement.Tile(new(4, 0), Direction.Up, "true", "I"));
                    tileList.Add(StageElement.Tile(new(4, -1), Direction.Up, "true", "N"));
                    tileList.Add(StageElement.Tile(new(4, -2), Direction.Up, "true", "G"));
                    tileList.Add(StageElement.Tile(new(4, -3), Direction.Up, "true", "S"));
                    
                    tileList.Add(StageElement.Tile(new(5, 4), Direction.Up, "true", "S"));
                    tileList.Add(StageElement.Tile(new(5, 3), Direction.Up, "true", "T"));
                    tileList.Add(StageElement.Tile(new(5, 2), Direction.Up, "true", "A"));
                    tileList.Add(StageElement.Tile(new(5, 1), Direction.Up, "true", "R"));
                    tileList.Add(StageElement.Tile(new(5, 0), Direction.Up, "true", "T"));
                    
                    tileList.Add(StageElement.Tile(new(3, -1),Direction.Up,"true", null));
                    tileList.Add(StageElement.Tile(new(3, -2),Direction.Up,"true", null));
                    tileList.Add(StageElement.Tile(new(3, -3),Direction.Up,"true", null));
                    tileList.Add(StageElement.Tile(new(5, -1),Direction.Up,"true", null));
                    tileList.Add(StageElement.Tile(new(5, -2),Direction.Up,"true", null));
                    tileList.Add(StageElement.Tile(new(5, -3),Direction.Up,"true", null));
                    
                    for (int i = 3; i <= 5; i++)
                    {
                        tileList.Add(StageElement.Tile(new(i, 5),Direction.Up,"true", null));
                        tileList.Add(StageElement.Tile(new(i, -5), Direction.Up, "false", null));
                        tileList.Add(StageElement.Speaker(new(i, 5), Direction.Down, null));
                    }
                return new StageData(
                    tileList.ToArray(),
                    new StageConfiguration(new Vector3(4.5f, 0, 0),
                        new List<UIType>{UIType.Start}));
            }
        }

        private StageData[] StagesData => new[]
        {
            new StageData(FirstStageElements, FirstStageConfiguration),
            new StageData(SecondStageElements, SecondStageConfiguration),
        };
        
        private StageElement[] FirstStageElements 
        {
            get
            {
                const int size = 4;
                var elementList = new List<StageElement>();
                for (var i = 0; i <= size; i++)
                {
                    for (var j = -size; j <= size; j++)
                    {
                        if (i + Math.Abs(j) <= size)
                        {
                            elementList.Add(StageElement.Tile(new(-i, j), Direction.None, "false", null));
                            elementList.Add(StageElement.Tile(new(i + 1, j), Direction.None, "false", null));
                        }
                    }
                }
                
                elementList.Add(StageElement.Player(new(-4, 0), Direction.Right));
                elementList.Add(StageElement.Speaker(new (0, 4), Direction.Down, "D1"));
                elementList.Add(StageElement.Speaker(new (1, -4), Direction.Up, null));
                
                return elementList.ToArray();
            }
        }
        
        private static StageConfiguration FirstStageConfiguration => new (
            new Vector3(0.5f, 0, 0),
            new List<UIType> { UIType.Record, UIType.Goal, UIType.Restart },
            finishCoordinates: new Coordinates(5, 0),
            goal: new HashSet<Note> { new ("C1") }
        );

        private StageElement[] SecondStageElements
        {
            get
            {
                var elementList = new List<StageElement>();
                for (var i = -4; i <= 4; i++)
                {
                    for (var j = -4; j <= 4; j++)
                    {
                        if (Math.Abs(i) <= 1 && Math.Abs(j) <= 1) continue;
                        elementList.Add(StageElement.Tile(new (i, j), Direction.None, "false", null));
                    }
                }
                elementList.Add(StageElement.Player(new(-3, 0), Direction.Right));
                elementList.Add(StageElement.Speaker(new (0, 3), Direction.Down, "D1", pushable: true, rotatable: true));
                elementList.Add(StageElement.Speaker(new (0, -3), Direction.Up, "A#1", pushable: true, rotatable: true));
                return elementList.ToArray();
            }
        }
        
        private static StageConfiguration SecondStageConfiguration => new (
            Vector3.zero,
            new List<UIType> { UIType.Record, UIType.Goal, UIType.Restart },
            finishCoordinates: new Coordinates(3, 0),
            goal: new HashSet<Note> { new ("D1"), new ("B1") }
        );

        private StageData StageSelectData
        {
            get
            {
                const int width = 9;
                var element = new StageElement[width + 1];

                for (int i = 0; i < width; i++)
                {
                    element[i] = StageElement.Tile(new(i, 0), Direction.None, "false", null);
                }
            
                element[width] = StageElement.Player(new(width-1, 0), Direction.Down);

                return new StageData(element, new StageConfiguration(
                    new Vector3(4.5f, 0, -4.5f),
                    new List<UIType>()
                ));
            }
        }
    }
}
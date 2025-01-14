using System;
using System.Collections.Generic;
using System.Linq;
using Common.Sound;
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

        public StageData InitStage(int stageIndex) => InitStage(StagesData[stageIndex]);
        
        public StageData InitStageSelect() => InitStage(StageSelectData);

        public StageData InitHome() => InitStage(HomeData);

        private StageData InitStage(StageData stageData)
        {
            SetStageDataToElements(stageData);
            AdjustCamera(stageData.Configuration.CameraLookingPosition, stageData.Configuration.CameraLookingSize);
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
        
        private void AdjustCamera(Vector3 cameraLookingPosition, float cameraLookingSize)
        {
            var camera = Camera.main;
            if (camera is null) return;
            camera.transform.position = cameraLookingPosition + CameraBehaviour.Position;
            camera.transform.LookAt(cameraLookingPosition);
            camera.orthographicSize = cameraLookingSize;
        }
        
        private void CreateStageElements(StageData stageData) =>
            stageData.Elements.ToList().ForEach(elementData => elementData.CreateStageElement(StageLoader.gameObject));
        
        private StageData HomeData
        {
            get
            {
                var tileList = new List<StageElement>();
                    tileList.Add(StageElement.Player(new(5,1), Direction.Up));
                    
                    tileList.Add(StageElement.Tile(new(3, 1), Direction.Up, "false", "C"));
                    tileList.Add(StageElement.Tile(new(3, 0), Direction.Up, "false", "D"));
                    tileList.Add(StageElement.Tile(new(3, -1), Direction.Up, "false", "B"));
                    tileList.Add(StageElement.Tile(new(4, 1), Direction.Up, "false", "0"));
                    tileList.Add(StageElement.Tile(new(4, 0), Direction.Up, "false", "E"));
                    tileList.Add(StageElement.Tile(new(4, -1), Direction.Up, "false", "A"));
                    tileList.Add(StageElement.Tile(new(5, 1), Direction.Up, "false", null));
                    tileList.Add(StageElement.Tile(new(5, 0), Direction.Up, "false", "F"));
                    tileList.Add(StageElement.Tile(new(5, -1), Direction.Up, "false", "G"));
                    
                return new StageData(
                    tileList.ToArray(),
                    new StageConfiguration(new Vector3(4.5f, 0, 0),
                        3,
                        new List<UIType>{UIType.ReHome}));
            }
        }

        private StageData[] StagesData => new[]
        {
            new StageData(DoStageElements, DoStageConfiguration),
            new StageData(ReStageElements, FirstStageConfiguration),
            new StageData(MiStageElements, SecondStageConfiguration),
            new StageData(FaStageElements, FaStageConfiguration),
            new StageData(SoStageElements, SoStageConfiguration),
            new StageData(LaStageElements, LaStageConfiguration),
            new StageData(TiStageElements, TiStageConfiguration),
        };

        public static StageConfiguration[] StageConfigurations => new[]
        {
            DoStageConfiguration,
            FirstStageConfiguration,
            SecondStageConfiguration,
            FaStageConfiguration,
            SoStageConfiguration,
            LaStageConfiguration,
            TiStageConfiguration,
        };

        private StageElement[] DoStageElements => new[]
        {
            StageElement.Tile(new(-2, 2), Direction.None, "false", null),
            StageElement.Tile(new(-2, 1), Direction.None, "false", null),
            StageElement.Tile(new(-2, 0), Direction.None, "false", null),
            StageElement.Tile(new(-2, -1), Direction.None, "false", null),
            StageElement.Tile(new(-2, -2), Direction.None, "false", null),
            StageElement.Tile(new(-1, -2), Direction.None, "false", null),
            StageElement.Tile(new(0, -2), Direction.None, "false", null),
            StageElement.Tile(new(1, -2), Direction.None, "false", null),
            StageElement.Tile(new(2, -2), Direction.None, "false", null),
            StageElement.Tile(new(2, -1), Direction.None, "false", null),
            StageElement.Tile(new(2, -0), Direction.None, "false", null),
            StageElement.Tile(new(2, 1), Direction.None, "false", null),
            StageElement.Tile(new(2, 2), Direction.None, "false", null),
            StageElement.Tile(new(0, 0), Direction.None, "true", null),
            StageElement.Speaker(new(0, 0), Direction.Down, "C1"),
            StageElement.Player(new(-2, 2), Direction.Right),
        };
        
        private static StageConfiguration DoStageConfiguration => new (
            Vector3.zero,
            4,
            new List<UIType> { UIType.Record, UIType.Goal, UIType.Restart, UIType.Back },
            finishCoordinates: new Coordinates(2, 2),
            goal: new HashSet<Note> { new ("C1") }
        );
        
        private StageElement[] ReStageElements 
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
                elementList.Add(StageElement.Speaker(new (0, 4), Direction.Down, "E1"));
                elementList.Add(StageElement.Speaker(new (1, -4), Direction.Up, null));
                
                return elementList.ToArray();
            }
        }
        
        private static StageConfiguration FirstStageConfiguration => new (
            new Vector3(0.5f, 0, 0),
            5,
            new List<UIType> { UIType.Record, UIType.Goal, UIType.Restart, UIType.Back },
            finishCoordinates: new Coordinates(5, 0),
            goal: new HashSet<Note> { new ("D1") }
        );

        private StageElement[] MiStageElements
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
                elementList.Add(StageElement.Speaker(new (0, 3), Direction.Down, "E1", pushable: true, rotatable: true));
                elementList.Add(StageElement.Speaker(new (0, -3), Direction.Up, "C2", pushable: true, rotatable: true));
                return elementList.ToArray();
            }
        }
        
        private static StageConfiguration SecondStageConfiguration => new (
            Vector3.zero,
            5,
            new List<UIType> { UIType.Record, UIType.Goal, UIType.Restart, UIType.Back },
            finishCoordinates: new Coordinates(3, 0),
            goal: new HashSet<Note> { new ("E1"), new ("B2") }
        );

        private StageElement[] FaStageElements => new[]
        {
            StageElement.Tile(new (-1, 1), Direction.None, "false", null),
            StageElement.Tile(new (0, 1), Direction.None, "false", null),
            StageElement.Tile(new (1, 1), Direction.None, "false", null),
            StageElement.Tile(new (2, 1), Direction.None, "false", null),
            StageElement.Tile(new (3, 1), Direction.None, "false", null),
            StageElement.Tile(new (-1, 0), Direction.None, "false", null),
            StageElement.Tile(new (0, 0), Direction.None, "false", null),
            StageElement.Tile(new (3, 0), Direction.None, "false", null),
            StageElement.Tile(new (2, 0), Direction.None, "false", null),
            StageElement.Tile(new (-2, -1), Direction.None, "false", null),
            StageElement.Tile(new (-1, -1), Direction.None, "false", null),
            StageElement.Tile(new (1, -1), Direction.None, "false", null),
            StageElement.Tile(new (2, -1), Direction.None, "false", null),
            StageElement.Tile(new (-2, -2), Direction.None, "false", null),
            StageElement.Tile(new (-1, -2), Direction.None, "false", null),
            StageElement.Tile(new (0, -2), Direction.None, "false", null),
            StageElement.Tile(new (1, -2), Direction.None, "false", null),
            StageElement.Tile(new (2, -2), Direction.None, "false", null),
            StageElement.Speaker(new (-1, 0), Direction.Down, "A#1", rotatable: true),
            StageElement.Speaker(new (2, 0), Direction.Down, null, rotatable: true),
            StageElement.Speaker(new (-1, -2), Direction.Up, "F#1", rotatable: true),
            StageElement.Player(new (-2, -2), Direction.Right), 
        };
        
        private static StageConfiguration FaStageConfiguration => new (
            new Vector3(0.5f, 0, -0.5f),
            4.5f,
            new List<UIType> { UIType.Record, UIType.Goal, UIType.Restart, UIType.Back },
            finishCoordinates: new Coordinates(1, -1),
            goal: new HashSet<Note> { new ("F1"), new ("A1") }
        );

        private StageElement[] SoStageElements => new[]
        {
            StageElement.Tile(new (0, 0), Direction.None, "false", null),
            StageElement.Tile(new (0, 1), Direction.None, "false", null),
            StageElement.Tile(new (1, 1), Direction.None, "false", null),
            StageElement.Tile(new (1, 0), Direction.None, "false", null),
            StageElement.Tile(new (1, -1), Direction.None, "false", null),
            StageElement.Tile(new (0, -1), Direction.None, "false", null),
            StageElement.Tile(new (-1, -1), Direction.None, "false", null),
            StageElement.Tile(new (-1, 1), Direction.None, "false", null),
            StageElement.Tile(new (-1, 0), Direction.None, "false", null),
            StageElement.Tile(new (0, 2), Direction.None, "false", null),
            StageElement.Tile(new (0, 3), Direction.None, "false", null),
            StageElement.Tile(new (-1, 2), Direction.None, "false", null),
            StageElement.Tile(new (-1, 3), Direction.None, "false", null),
            StageElement.Tile(new (-2, 3), Direction.None, "false", null),
            StageElement.Tile(new (-2, 2), Direction.None, "false", null),
            StageElement.Tile(new (-2, 1), Direction.None, "false", null),
            StageElement.Tile(new (2, 0), Direction.None, "false", null),
            StageElement.Tile(new (3, 0), Direction.None, "false", null),
            StageElement.Tile(new (2, -1), Direction.None, "false", null),
            StageElement.Tile(new (3, -1), Direction.None, "false", null),
            StageElement.Tile(new (3, -2), Direction.None, "false", null),
            StageElement.Tile(new (2, -2), Direction.None, "false", null),
            StageElement.Tile(new (1, -2), Direction.None, "false", null),
            StageElement.Tile(new (3, 3), Direction.None, "true", null),
            StageElement.Speaker(new (0, 0), Direction.Up, "C#2", rotatable: true),
            StageElement.Speaker(new (-1, 3), Direction.Right, null, rotatable: true),
            StageElement.Speaker(new (3, 3), Direction.Down, "G#1"),
            StageElement.Player(new (2, -2), Direction.Right),
        };
        
        private static StageConfiguration SoStageConfiguration => new (
            new Vector3(0.5f, 0, 0.5f),
            5,
            new List<UIType> { UIType.Record, UIType.Goal, UIType.Restart, UIType.Back },
            finishCoordinates: new Coordinates(1, 1),
            goal: new HashSet<Note> { new ("G1"), new ("B2"), new ("D2") }
        );

        private StageElement[] LaStageElements
        {
            get
            {
                var elementList = new List<StageElement>();
                for (var i = -2; i <= 2; i++)
                {
                    for (var j = -2; j <= 1; j++)
                    {
                        if (Math.Abs(i) == 2 && (j == -2 || j == 1)) continue;
                        elementList.Add(StageElement.Tile(new (i, j), Direction.None, "false", null));
                    }
                }

                elementList.Add(StageElement.Speaker(new (1, -1), Direction.Right, "B1", rotatable: true));
                elementList.Add(StageElement.Speaker(new (1, 0), Direction.Right, "F1", rotatable: true));
                elementList.Add(StageElement.Speaker(new (-1, 0), Direction.Left, "B1", rotatable: true));
                elementList.Add(StageElement.Speaker(new (-1, -1), Direction.Left, "A#2", rotatable: true));
                elementList.Add(StageElement.Speaker(new (0, -2), Direction.Up, null));
                elementList.Add(StageElement.Speaker(new (0, 1), Direction.Down, null));
                
                elementList.Add(StageElement.Player(new (0, 0), Direction.Right));

                return elementList.ToArray();
            }
        }
        
        private static StageConfiguration LaStageConfiguration => new (
            new Vector3(0, 0, -0.5f), 
            3.5f,
            new List<UIType> { UIType.Record, UIType.Goal, UIType.Restart, UIType.Back },
            finishCoordinates: new Coordinates(0, 0),
            goal: new HashSet<Note> { new ("A1"), new ("C#1"), new ("E1"), new ("B2") }
        );

        private StageElement[] TiStageElements
        {
            get
            {
                var elementList = new List<StageElement>();

                for (var i = -3; i <= 3; i++)
                {
                    for (var j = -3; j <= 3; j++)
                    {
                        elementList.Add(StageElement.Tile(new (i, j), Direction.None, "false", null));
                    }
                }
                elementList.Add(StageElement.Tile(new (1, 5), Direction.Down, "true", null));
                elementList.Add(StageElement.Tile(new (-1, 5), Direction.Down, "true", null));
                elementList.Add(StageElement.Tile(new (5, 1), Direction.Left, "true", null));
                elementList.Add(StageElement.Tile(new (5, -1), Direction.Left, "true", null));
                
                elementList.Add(StageElement.Speaker(new (1, 5), Direction.Down, "A#1"));
                elementList.Add(StageElement.Speaker(new (-1, 5), Direction.Down, "F#1"));
                elementList.Add(StageElement.Speaker(new (5, 1), Direction.Left, "E1"));
                elementList.Add(StageElement.Speaker(new (5, -1), Direction.Left, "D1"));
                
                elementList.Add(StageElement.Speaker(new(-3, 3), Direction.Up, null, rotatable: true));
                elementList.Add(StageElement.Speaker(new(3, -3), Direction.Up, null, rotatable: true));
                elementList.Add(StageElement.Speaker(new(-3, -3), Direction.Up, null, rotatable: true));
                
                elementList.Add(StageElement.Player(new(0, 0), Direction.Right));
                
                return elementList.ToArray();
            }
        }
        
        private static StageConfiguration TiStageConfiguration => new (
            new Vector3(0.5f, 0, 0.5f),
            5,
            new List<UIType> { UIType.Record, UIType.Goal, UIType.Restart, UIType.Back },
            finishCoordinates: new Coordinates(3, 3),
            goal: new HashSet<Note> { new ("B1"), new ("C#1"), new ("E1"), new ("F#1"), new ("G#1") }
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
                    5,
                    new List<UIType>()
                ));
            }
        }
    }
}
using UnityEngine;

namespace GamePlay.StageData
{
    public class Stages
    {
        private GameObject PlayerPrefab { get; }
        private GameObject TilePrefab { get; }
        
        public Stages(StageLoader stageLoader)
        {
            PlayerPrefab = stageLoader.playerPrefab;
            TilePrefab = stageLoader.tilePrefab;
        }

        private StageElementData[] _currentStageData;

        public void InitFirstStage()
        {
            _currentStageData = FirstStageData;
            Debug.Log(Camera.main);
            if (Camera.main != null)
            {
                Camera.main.transform.position = FirstStageCameraLookingPosition + CameraBehaviour.Position;
                Camera.main.transform.LookAt(FirstStageCameraLookingPosition);
            }
            CreateStageElements(FirstStageData, new GameObject("Stage 1"));
        }
        
        private void CreateStageElements(StageElementData[] elementsData, GameObject parent)
        {
            foreach (var elementData in elementsData)
            {
                elementData.CreateStageElement(parent);
            }
        }

        private StageElementData[] FirstStageData => new StageElementData[]
        {
            new (PlayerPrefab, new Coordinates(0, 0), new IsPath(false), _currentStageData),
            new (TilePrefab,   new Coordinates(0, 0), new IsPath(true) , _currentStageData),
            new (TilePrefab,   new Coordinates(1, 0), new IsPath(true) , _currentStageData),
            new (TilePrefab,   new Coordinates(2, 0), new IsPath(true) , _currentStageData),
            new (TilePrefab,   new Coordinates(3, 0), new IsPath(true) , _currentStageData),
            new (TilePrefab,   new Coordinates(4, 0), new IsPath(true) , _currentStageData),
            new (TilePrefab,   new Coordinates(5, 0), new IsPath(true) , _currentStageData),
            new (TilePrefab,   new Coordinates(6, 0), new IsPath(true) , _currentStageData),
            new (TilePrefab,   new Coordinates(7, 0), new IsPath(true) , _currentStageData),
            new (TilePrefab,   new Coordinates(8, 0), new IsPath(true) , _currentStageData),
            new (TilePrefab,   new Coordinates(9, 0), new IsPath(true) , _currentStageData),
            new (TilePrefab,   new Coordinates(4, 1), new IsPath(true) , _currentStageData),
            new (TilePrefab,   new Coordinates(4, 2), new IsPath(true) , _currentStageData),
            new (TilePrefab,   new Coordinates(4, 3), new IsPath(true) , _currentStageData),
            new (TilePrefab,   new Coordinates(4, 4), new IsPath(true) , _currentStageData),
            new (TilePrefab,   new Coordinates(4,-1), new IsPath(true) , _currentStageData),
            new (TilePrefab,   new Coordinates(4,-2), new IsPath(true) , _currentStageData),
            new (TilePrefab,   new Coordinates(4,-3), new IsPath(true) , _currentStageData),
            new (TilePrefab,   new Coordinates(4,-4), new IsPath(true) , _currentStageData),
            new (TilePrefab,   new Coordinates(5, 1), new IsPath(true) , _currentStageData),
            new (TilePrefab,   new Coordinates(5, 2), new IsPath(true) , _currentStageData),
            new (TilePrefab,   new Coordinates(5, 3), new IsPath(true) , _currentStageData),
            new (TilePrefab,   new Coordinates(5, 4), new IsPath(true) , _currentStageData),
            new (TilePrefab,   new Coordinates(5,-1), new IsPath(true) , _currentStageData),
            new (TilePrefab,   new Coordinates(5,-2), new IsPath(true) , _currentStageData),
            new (TilePrefab,   new Coordinates(5,-3), new IsPath(true) , _currentStageData),
            new (TilePrefab,   new Coordinates(5,-4), new IsPath(true) , _currentStageData),
        };
        
        private static Vector3 FirstStageCameraLookingPosition => new (4.5f, 0, 0);
    }
}
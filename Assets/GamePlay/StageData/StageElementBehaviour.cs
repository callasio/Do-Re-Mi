namespace GamePlay.StageData
{
    using UnityEngine;
    
    public struct IsPath
    {
        public bool Value { get; set; }
        
        public IsPath(bool value)
        {
            Value = value;
        }
    }
    
    public class StageElementData
    {
        private GameObject _prefab;
        public Coordinates Coordinates { get; set; }
        public bool IsPath;
        public StageElementData[] CurrentStageData;

        private StageElementBehaviour _stageElementInstanceBehaviour;
        
        public StageElementData(GameObject prefab, Coordinates coordinates, IsPath isPath, StageElementData[] currentStageData)
        {
            _prefab = prefab;
            Coordinates = coordinates;
            IsPath = isPath.Value;
            CurrentStageData = currentStageData;
        }

        public void CreateStageElement(GameObject parent)
        {
            var instance = Object.Instantiate(
                _prefab,
                new Vector3(Coordinates.X, 0, Coordinates.Y),
                Quaternion.identity);
            
            instance.transform.SetParent(parent.transform);
            _stageElementInstanceBehaviour = instance.GetComponent<StageElementBehaviour>();
            _stageElementInstanceBehaviour.Data = this;
        }
    }
    
    public abstract class StageElementBehaviour: MonoBehaviour
    {
        public StageElementData Data { get; set; }

        public virtual void Start()
        {
            transform.position = new Vector3(Data.Coordinates.X, transform.position.y, Data.Coordinates.Y);
        }

        public abstract void OnClicked();
    }
}
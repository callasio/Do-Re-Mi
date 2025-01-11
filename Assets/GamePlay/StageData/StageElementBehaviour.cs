using System.Linq;

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

    public enum StageElementType
    {
        Player,
        Tile,
        Speaker,
    }
    
    public class StageElementData
    {
        private GameObject _prefab;
        public Coordinates Coordinates { get; set; }
        public Direction Direction { get; set; }
        public StageElementType Type { get; set; }
        public StageElementData[] CurrentStageData;

        private StageElementBehaviour _stageElementInstanceBehaviour;
        
        public StageElementData(GameObject prefab, Coordinates coordinates, Direction direction, StageElementType type, StageElementData[] currentStageData)
        {
            _prefab = prefab;
            Coordinates = coordinates;
            Direction = direction;
            Type = type;
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
        
        public void DestroyStageElement()
        {
            CurrentStageData = CurrentStageData.Where(data => data != this).ToArray();
            Object.Destroy(_stageElementInstanceBehaviour.gameObject);
        }
    }
    
    public abstract class StageElementBehaviour: MonoBehaviour
    {
        public StageElementData Data { get; set; }

        public virtual void Start()
        {
            transform.position = new Vector3(Data.Coordinates.X, transform.position.y, Data.Coordinates.Y);
            transform.LookAt(transform.position + Data.Direction.ToVector3()); 
        }

        public abstract void OnClicked();
    }
}
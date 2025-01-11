using System;
using System.Collections.Generic;
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
    [Flags]
    public enum StageElementType
    {
        Player,
        Tile,
        Speaker,
    }
    
    public class StageElementData
    {
        private readonly GameObject _prefab;
        public Coordinates Coordinates { get; set; }
        public Direction Direction { get; set; }
        public StageElementType Type { get; set; }
        public StageElementData[] CurrentStageData;
        
        /*
         * - Speaker
         *   - note?
         *
         * - Tile
         *   - fixed
         *   - text?
         */
        public readonly Dictionary<string, string> Metadata = new();

        public StageElementBehaviour StageElementInstanceBehaviour { get; private set; }
        
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
            StageElementInstanceBehaviour = instance.GetComponent<StageElementBehaviour>();
            StageElementInstanceBehaviour.Data = this;
        }
        
        public void DestroyStageElement()
        {
            CurrentStageData = CurrentStageData.Where(data => data != this).ToArray();
            Object.Destroy(StageElementInstanceBehaviour.gameObject);
        }
    }
    
    public abstract class StageElementBehaviour: MonoBehaviour
    {
        public StageElementData Data { get; set; }

        public virtual void Start()
        {
            transform.position = new Vector3(Data.Coordinates.X, transform.position.y, Data.Coordinates.Y);
            LookAt(Data.Direction);
        }
        
        protected void LookAt(Direction direction)
        {
            if (Data.Direction != Direction.None)
                transform.LookAt(transform.position + direction.ToVector3());
        }

        public abstract void OnClicked();
    }
}
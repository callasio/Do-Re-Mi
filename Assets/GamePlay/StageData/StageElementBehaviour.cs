using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

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
    
    public class StageElement
    {
        private readonly GameObject _prefab;
        public Coordinates Coordinates { get; set; }
        public Direction Direction { get; set; }
        public StageElementType Type { get; set; }
        public StageData CurrentStageData { get; set; }
        public StageElement[] CurrentStageElements => CurrentStageData.Elements;
        public static StageLoader StageLoader { get; set; }
        
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
        
        public StageElement(GameObject prefab, Coordinates coordinates, Direction direction, StageElementType type, StageData currentStageData)
        {
            _prefab = prefab;
            Coordinates = coordinates;
            Direction = direction;
            Type = type;
            CurrentStageData = currentStageData;
        }
        
        public static StageElement Speaker(
            Coordinates coordinates, 
            Direction direction,
            [CanBeNull] string note,
            bool pushable = false,
            bool rotatable = false
        )
        {
            var data = NewData(coordinates, direction, StageElementType.Speaker);
            data.Metadata["note"] = note;
            data.Metadata["pushable"] = pushable.ToString();
            data.Metadata["rotatable"] = rotatable.ToString();
            return data;
        }
        
        public static StageElement Tile(Coordinates coordinates, Direction direction, string fix, [CanBeNull] string text)
        {
            var data = NewData(coordinates, direction, StageElementType.Tile);
            data.Metadata["fix"] = fix;
            data.Metadata["text"] = text;
            return data;
        }
        
        public static StageElement Player(Coordinates coordinates, Direction direction) => NewData(coordinates, direction, StageElementType.Player);

        private static StageElement NewData(Coordinates coordinates, Direction direction, StageElementType type)
        {
            var prefab = type switch
            {
                StageElementType.Tile => StageLoader.tilePrefab,
                StageElementType.Player => StageLoader.playerPrefab,
                StageElementType.Speaker => StageLoader.speakerPrefab,
                _ => null
            };
            
            return new StageElement(prefab, coordinates, direction, type, new StageData());
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
        
        public void DestroyStageElement() => Object.Destroy(StageElementInstanceBehaviour.gameObject);
    }
    
    public abstract class StageElementBehaviour: MonoBehaviour
    {
        public StageElement Data { get; set; }

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

        public void UpdateElementState()
        {
            Data.CurrentStageData.Elements.ToList().ForEach(elementData => elementData.StageElementInstanceBehaviour.OnElementUpdate());
        }

        public virtual void OnElementUpdate() { }
        
        public abstract void OnClicked(Vector3 normal, bool forward = true);
    }
}
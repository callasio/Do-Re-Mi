using System.Collections.Generic;
using GamePlay.StageData.Player.Sound;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;

namespace GamePlay.StageData
{
    public enum UIType
    {
        Record,
        Goal,
    }
    
    public class StageConfiguration
    {
        public Vector3 CameraLookingPosition { get; }
        public HashSet<UIType> UIs { get; }
        [CanBeNull] public HashSet<Note> Goal { get; }
        
        public StageConfiguration(Vector3 cameraLookingPosition, IEnumerable<UIType> uis, [CanBeNull] IEnumerable<Note> goal)
        {
            CameraLookingPosition = cameraLookingPosition;
            UIs = uis.ToHashSet();
            Goal = goal?.ToHashSet();
        }
    }
}
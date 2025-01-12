using System;
using System.Collections.Generic;
using GamePlay.StageData.Player.Sound;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;

namespace GamePlay.StageData
{
    public enum UIType
    {
        Start,
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

    public class StageData
    {
        public StageElement[] Elements { get; set; }
        public StageConfiguration Configuration { get; }

        public StageData()
        {
            Elements = Array.Empty<StageElement>();
        }

        public StageData(StageElement[] elements, StageConfiguration configuration)
        {
            Elements = elements;
            Configuration = configuration;
        }
    }
}
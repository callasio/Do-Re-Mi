using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using System.Linq;
using Common.Sound;

namespace GamePlay.StageData
{
    public enum UIType
    {
        Record,
        Goal,
        Restart,
        ReHome,
        Back,
    }
    
    public class StageConfiguration
    {
        public Vector3 CameraLookingPosition { get; }
        public float CameraLookingSize { get; }
        public List<UIType> UIs { get; }
        [CanBeNull] public Coordinates FinishCoordinates { get; }
        [CanBeNull] public HashSet<Note> Goal { get; }
        
        public StageConfiguration(Vector3 cameraLookingPosition, float cameraLookingSize,IEnumerable<UIType> uis, [CanBeNull] Coordinates finishCoordinates = null, [CanBeNull] IEnumerable<Note> goal = null)
        {
            CameraLookingPosition = cameraLookingPosition;
            CameraLookingSize = cameraLookingSize;
            FinishCoordinates = finishCoordinates;
            UIs = uis.ToList();
            Goal = goal is null ? null : new HashSet<Note>(goal);
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
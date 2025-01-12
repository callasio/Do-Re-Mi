using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;


namespace GamePlay.StageData.Tile
{
    using UnityEngine;
    using Player;
    
    public class Tile: StageElementBehaviour
    {
        // two materials
        public Material material1;
        public Material material2;
        private Animation _animation;

        public TextMeshPro textMesh;
        private static readonly Dictionary<string, string> LetterColorMap = new Dictionary<string, string>
        {
            { "A", "#D04848"},
            { "B", "#F3B95F"},
            { "C", "#FDE767"},
            { "D", "#36AE7C"},
            { "E", "#187498"},
            { "F", "#293462"},
            { "G", "#52006A"},
        };

        
        public override void Start()
        {
            base.Start();
            
            var x = Data.Coordinates.X;
            var y = Data.Coordinates.Y;
            
            var cube = transform.GetChild(0).Find("Cube").gameObject;
            cube.GetComponent<Renderer>().material = (x + y) % 2 == 0 ? material1 : material2;
            
            _animation = GetComponentInChildren<Animation>();

            if (textMesh == null || !Data.Metadata.TryGetValue("text", out var text)) return;
            textMesh.text = text;
                
            if (text != null && LetterColorMap.TryGetValue(text, out var hexColor) && ColorUtility.TryParseHtmlString(hexColor, out var color))
            {
                textMesh.color = color;
            }
            else
            {
                textMesh.color = Color.black; // 기본 색상
            }
        }

        public override void OnClicked(Vector3 normal, bool forward = true)
        {
            if (!Data.Metadata.TryGetValue("fix", out var fix) || fix == "false")
            {  
                if (forward)
                    Data.CurrentStageElements
                        .Where(element => element.Coordinates == Data.Coordinates && element.Type != StageElementType.Tile).ToList()
                        .ForEach(element => element.StageElementInstanceBehaviour.OnClicked(normal, forward: false));
                _animation.Stop();
                _animation.Play();
            }
            Player.ElementClicked(Data);
        }
    }
}
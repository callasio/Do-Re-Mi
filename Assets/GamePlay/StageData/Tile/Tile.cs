using System.Collections.Generic;
using System.Linq;
using TMPro;


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
        //특정 글자에 색깔 지정
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
            
            var cube = transform.Find("Cube").gameObject;
            cube.GetComponent<Renderer>().material = (x + y) % 2 == 0 ? material1 : material2;
            
            _animation = GetComponent<Animation>();

            if (textMesh != null && Data.Metadata.TryGetValue("text", out var text))
            {
                textMesh.text = text;
                
                // HEX 코드를 Color로 변환
                if (LetterColorMap.TryGetValue(text, out var hexColor) && ColorUtility.TryParseHtmlString(hexColor, out var color))
                {
                    textMesh.color = color;
                }
                else
                {
                    textMesh.color = Color.black; // 기본 색상
                }
            }
        }

        public override void OnClicked()
        {
            if (!Data.CurrentStageData.Any(element => element.Type == StageElementType.Speaker && element.Coordinates == Data.Coordinates))
            {
                if (Data.Metadata["fix"] == "false")
                {  
                    _animation.Play();
                }
            }
            Player.ElementClicked(Data);
        }
    }
}
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
        
        public override void Start()
        {
            base.Start();
            
            var x = Data.Coordinates.X;
            var y = Data.Coordinates.Y;
            
            var cube = transform.Find("Cube").gameObject;
            cube.GetComponent<Renderer>().material = (x + y) % 2 == 0 ? material1 : material2;
            
            _animation = GetComponent<Animation>();
        }

        public override void OnClicked()
        {
            _animation.Play();
            Player.ElementClicked(Data);
        }
    }
}
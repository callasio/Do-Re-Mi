using UnityEngine;

namespace GamePlay.StageData
{
    public abstract class MovingElement : StageElementBehaviour
    {
        private const float MovingSpeed = 4f;
        public abstract Direction MovingDirection { get; protected set; }
        public abstract Coordinates TargetCoordinates { get; set; }
        public abstract bool Push(Direction direction);
        
        protected abstract bool IsMoving { get; }
        protected abstract bool SetNewTarget();
        protected abstract void OnReachedTarget();
        
        public override void Start()
        {
            base.Start();
            TargetCoordinates = Data.Coordinates;
        }

        public virtual void Update()
        {
            Move();
        }

        protected void Move()
        {
            if (!IsMoving)
            {
                MovingDirection = Direction.None;
                return;
            }

            if (!SetNewTarget()) return;
            MovingDirection = TargetCoordinates - Data.Coordinates;
            var currentPosition = transform.position;
            var target = TargetCoordinates.ToVector3();

            var distance = Vector3.Distance(currentPosition, target);
            var step = MovingSpeed * Time.deltaTime;
            
            if (step >= distance)
            {
                transform.position = target;
                Data.Coordinates = TargetCoordinates;
                OnReachedTarget();
            }
            else
            {
                transform.position = Vector3.MoveTowards(currentPosition, target, step);
            }
        }
    }
}
using System;
using UnityEngine;

namespace GamePlay.StageData
{
    public readonly struct MovingDirection : IEquatable<MovingDirection>
    {
        public static readonly MovingDirection Up = new MovingDirection(0, 1);
        public static readonly MovingDirection Down = new MovingDirection(0, -1);
        public static readonly MovingDirection Left = new MovingDirection(-1, 0);
        public static readonly MovingDirection Right = new MovingDirection(1, 0);
        
        public int X { get; }
        public int Y { get; }
        
        public MovingDirection(int x, int y)
        {
            X = x;
            Y = y;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y);
        }

        public static bool operator ==(MovingDirection left, MovingDirection right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(MovingDirection left, MovingDirection right)
        {
            return !(left == right);
        }

        public bool Equals(MovingDirection other)
        {
            return X == other.X && Y == other.Y;
        }
        
        public override bool Equals(object obj)
        {
            return obj is MovingDirection other && Equals(other);
        }
    }
    
    public readonly struct Coordinates : IEquatable<Coordinates>
    {
        public int X { get; }
        public int Y { get; }
        
        public Coordinates(int x, int y)
        {
            X = x;
            Y = y;
        }

        public Vector3 ToVector3(int y = 0)
        {
            return new Vector3(X, y, Y);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y);
        }

        public static bool operator ==(Coordinates left, Coordinates right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Coordinates left, Coordinates right)
        {
            return !(left == right);
        }

        public bool Equals(Coordinates other)
        {
            return X == other.X && Y == other.Y;
        }
        
        public override bool Equals(object obj)
        {
            return obj is Coordinates other && Equals(other);
        }
        
        public static Coordinates operator +(Coordinates left, MovingDirection right)
        {
            return new Coordinates(left.X + right.X, left.Y + right.Y);
        }

        public override string ToString()
        {
            return $"({X}, {Y})";
        }
    }
}
using System;
using UnityEngine;

namespace GamePlay.StageData
{
    public class Direction : IEquatable<Direction>
    {
        public static readonly Direction None = new Direction(0, 0);
        public static readonly Direction Up = new Direction(0, 1);
        public static readonly Direction Down = new Direction(0, -1);
        public static readonly Direction Left = new Direction(-1, 0);
        public static readonly Direction Right = new Direction(1, 0);
        
        public int X { get; }
        public int Y { get; }
        
        public Direction(int x, int y)
        {
            // Normalize the direction
            if (x != 0)
            {
                x /= Math.Abs(x);
            }
            if (y != 0)
            {
                y /= Math.Abs(y);
            }
            X = x;
            Y = y;
        }
        
        public bool IsValid()
        {
            return X == 0 || Y == 0;
        }
        
        public Direction CounterClockwise() => new (-Y, X);
        
        public Quaternion ToQuaternion() => Quaternion.LookRotation(ToVector3());
        
        public Vector3 ToVector3()
        {
            return new Vector3(X, 0, Y);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y);
        }

        public static bool operator ==(Direction left, Direction right)
        {
            return left?.Equals(right) ?? right is null;
        }

        public static bool operator !=(Direction left, Direction right)
        {
            return !(left == right);
        }

        public static Direction operator -(Direction d)
        {
            return new Direction(-d.X, -d.Y);
        }

        public bool Equals(Direction other)
        {
            return X == other?.X && Y == other.Y;
        }
        
        public override bool Equals(object obj)
        {
            return obj is Direction other && Equals(other);
        }
        
        public override string ToString()
        {
            return $"Direction({X}, {Y})";
        }
    }
    
    public class Coordinates : IEquatable<Coordinates>, IComparable<Coordinates>
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
            return left?.Equals(right) ?? right is null;
        }

        public static bool operator !=(Coordinates left, Coordinates right)
        {
            return !(left == right);
        }

        public bool Equals(Coordinates other)
        {
            return X == other?.X && Y == other.Y;
        }

        public int CompareTo(Coordinates other)
        {
            if (Y == other.Y)
            {
                return X.CompareTo(other.X);
            }
            return Y.CompareTo(other.Y);
        }

        public override bool Equals(object obj) => obj is Coordinates other && Equals(other);

        public static Coordinates operator +(Coordinates left, Direction right)
        {
            return new Coordinates(left.X + right.X, left.Y + right.Y);
        }
        
        public static Direction operator -(Coordinates left, Coordinates right)
        {
            return new Direction(left.X - right.X, left.Y - right.Y);
        }

        public override string ToString() => $"Coordinates({X}, {Y})";
    }
}
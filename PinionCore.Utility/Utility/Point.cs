using System;

namespace PinionCore.Utility
{


    public struct Point
    {

        public float X;


        public float Y;

        public Point(float x, float y)
        {
            // TODO: Complete member initialization
            X = x;
            Y = y;
        }

        public Vector2 ToVector2()
        {
            return new Vector2(X,Y);
        }
    }
}

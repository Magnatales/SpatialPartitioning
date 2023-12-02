using Graphics.Entities;
using Microsoft.Xna.Framework;

namespace Graphics.Utils;

public static class RectangleExtensions
{
    public static bool Intersects(this RectangleF a, RectangleF b)
    {
        return a.Left < b.Right && a.Right > b.Left && a.Top < b.Bottom && a.Bottom > b.Top;
    }
    
    public static Vector2 TopLeft(this RectangleF rectangle)
    { 
        return new Vector2(rectangle.Left, rectangle.Top);
    }

    public static Vector2 BottomRight(this RectangleF rectangle)
    { 
        return new Vector2(rectangle.Right, rectangle.Bottom);
    }
    
    public static bool Contains(this RectangleF rectangle, Vector2 point)
    {
        return point.X >= rectangle.Left &&
               point.X <= rectangle.Right &&
               point.Y >= rectangle.Top &&
               point.Y <= rectangle.Bottom;
    }
    
    public static bool Contains(this RectangleF rectangleF, Actor entity)
    {
        var boundaries = entity.GetBoundaries();
        var topLeftInside = rectangleF.Contains(boundaries.TopLeft());
        var bottomRightInside = rectangleF.Contains(boundaries.BottomRight());
        
        return topLeftInside && bottomRightInside;
    }
}
using Graphics.Entities;

namespace Graphics.Utils;

public static class CircleExtensions
{
    public static bool IsColliding(this Circle circle, Circle other)
    {
        var dx = other.Position.X - circle.Position.X;
        var dy = other.Position.Y - circle.Position.Y;
        var distanceSquared = (dx * dx) + (dy * dy);
        var radiiSumSquared = (circle.Radius + other.Radius) * (circle.Radius + other.Radius);
        return distanceSquared < radiiSumSquared;
    }
}
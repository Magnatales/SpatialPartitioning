using Graphics.World;

namespace Graphics.Utils;

public static class ActorExtensions
{
    public static bool IsColliding(this Actor actor, Actor other)
    {
        var dx = other.Position.X - actor.Position.X;
        var dy = other.Position.Y - actor.Position.Y;
        var distanceSquared = (dx * dx) + (dy * dy);
        var radiiSumSquared = (actor.Radius + other.Radius) * (actor.Radius + other.Radius);
        return distanceSquared < radiiSumSquared;
    }
}
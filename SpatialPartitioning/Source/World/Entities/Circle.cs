using System;
using Microsoft.Xna.Framework;

namespace Graphics.Entities;

public struct Circle
{
    public Vector2 Position;
    public readonly float Radius;
    public Color Color;
    private readonly Random _random;

    public Circle(Vector2 position, float radius, Color color)
    {
        _random = new Random();
        Position = position;
        Radius = radius;
        Color = color;
    }

    public void Update(GameTime gameTime)
    {
        const int maxSpeed = 100;
        var frameTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
        var speed = maxSpeed * frameTime;
        var angle = (float)_random.NextDouble() * 2 * MathF.PI;
        var xOffset = speed * MathF.Cos(angle);
        var yOffset = speed * MathF.Sin(angle);

        var newPosition = new Vector2(
            Position.X + xOffset,
            Position.Y + yOffset
        );
        Position = newPosition;
    }
}
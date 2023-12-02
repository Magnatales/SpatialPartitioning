﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Graphics.Entities;

public class Circle
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

    public void Update()
    {
        const float maxSpeed = 300f;
        var speed = maxSpeed * Time.Delta;
        var angle = (float)_random.NextDouble() * 2 * MathF.PI;
        var xOffset = speed * MathF.Cos(angle);
        var yOffset = speed * MathF.Sin(angle);

        var newPosition = new Vector2(
            Position.X + xOffset,
            Position.Y + yOffset
        );
        Position = newPosition;
    }
    
    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.DrawCircleFilled(Position, Radius, Color);
    }
}
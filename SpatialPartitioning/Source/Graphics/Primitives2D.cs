using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Graphics;

public static class Primitives2D
{
    public static void DrawCircleFilled(this SpriteBatch spriteBatch, Vector2 position, Vector2 origin, float radius, Color color)
    {
        var scale = radius / 60f;
        spriteBatch.Draw(Textures.CircleFilled, position, null, color, 0, origin, scale, SpriteEffects.None, 0);
    }
    
    public static void DrawCircleFilled(this SpriteBatch spriteBatch, Vector2 position, float radius, Color color)
    {
        var scale = radius / 60f;
        var origin = new Vector2(Textures.CircleFilled.Width / 2f, Textures.CircleFilled.Height / 2f);
        spriteBatch.Draw(Textures.CircleFilled, position, null, color, 0, origin, scale, SpriteEffects.None, 0);
    }
    
    public static void DrawRectangle(this SpriteBatch spriteBatch, Rectangle rect, Color color)
    {
        DrawRectangle(spriteBatch, rect, color, 1.0f);
    }
    
    public static void DrawRectangle(this SpriteBatch spriteBatch, RectangleF rect, Color color)
    {
        DrawRectangle(spriteBatch, rect, color, 1.0f);
    }
    
    public static void DrawRectangle(this SpriteBatch spriteBatch, RectangleF rect, Color color, float thickness)
    {
        DrawLine(spriteBatch, new Vector2(rect.X, rect.Y), new Vector2(rect.Right, rect.Y), color, thickness); // top
        DrawLine(spriteBatch, new Vector2(rect.X + 1f, rect.Y), new Vector2(rect.X + 1f, rect.Bottom + thickness), color, thickness); // left
        DrawLine(spriteBatch, new Vector2(rect.X, rect.Bottom), new Vector2(rect.Right, rect.Bottom), color, thickness); // bottom
        DrawLine(spriteBatch, new Vector2(rect.Right + 1f, rect.Y), new Vector2(rect.Right + 1f, rect.Bottom + thickness), color, thickness); // right
    }
    
    public static void DrawRectangle(this SpriteBatch spriteBatch, Rectangle rect, Color color, float thickness)
    {
        DrawLine(spriteBatch, new Vector2(rect.X, rect.Y), new Vector2(rect.Right, rect.Y), color, thickness); // top
        DrawLine(spriteBatch, new Vector2(rect.X + 1f, rect.Y), new Vector2(rect.X + 1f, rect.Bottom + thickness), color, thickness); // left
        DrawLine(spriteBatch, new Vector2(rect.X, rect.Bottom), new Vector2(rect.Right, rect.Bottom), color, thickness); // bottom
        DrawLine(spriteBatch, new Vector2(rect.Right + 1f, rect.Y), new Vector2(rect.Right + 1f, rect.Bottom + thickness), color, thickness); // right
    }
    
    public static void DrawLine(this SpriteBatch spriteBatch, Vector2 point1, Vector2 point2, Color color, float thickness)
    {
        var distance = Vector2.Distance(point1, point2);
        var angle = (float)Math.Atan2(point2.Y - point1.Y, point2.X - point1.X);

        DrawLine(spriteBatch, point1, distance, angle, color, thickness);
    }

    private static void DrawLine(this SpriteBatch spriteBatch, Vector2 point, float length, float angle, Color color, float thickness)
    {
        // stretch the pixel between the two vectors
        spriteBatch.Draw(Textures.Pixel,
            point,
            null,
            color,
            angle,
            Vector2.Zero,
            new Vector2(length, thickness),
            SpriteEffects.None,
            0);
    }
}
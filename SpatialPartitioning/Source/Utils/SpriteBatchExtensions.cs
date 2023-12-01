using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Graphics.Utils;

public static class SpriteBatchExtensions
{
    public static void DrawStringWithShadow(this SpriteBatch spriteBatch, SpriteFont font, string text, Vector2 position, Color color, Color shadow = default)
    {
        spriteBatch.DrawString(font, text, position + new Vector2(1.5f, 1.5f), shadow);
        spriteBatch.DrawString(font, text, position, color);
    }
    
    public static void DrawStringWithShadow(this SpriteBatch spriteBatch, SpriteFont font, string text, Vector2 position, Color color, float scale)
    {
        spriteBatch.DrawString(font, text, position + new Vector2(1.5f, 1.5f), Color.DarkViolet, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
        spriteBatch.DrawString(font, text, position, color, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
    }
}
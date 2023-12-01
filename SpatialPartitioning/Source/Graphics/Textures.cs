using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Graphics;

public static class Textures
{
    private static Texture2D _quad;

    public static Texture2D Pixel
    {
        get
        {
            if (_quad == null)
            {
                _quad = new Texture2D(GameEnvironment.Graphics.GraphicsDevice, 1, 1);
                _quad.SetData(new Color[] { Color.White });
            }
            return _quad;
        }
        private set => _quad = value;
    }
    
    private static Texture2D _circleFilled;
    public static Texture2D CircleFilled
    {
        get
        {
            if (_circleFilled == null)
            {
                _circleFilled = GameEnvironment.Content.Load<Texture2D>("textures/circle-filled");
            }

            return _circleFilled;
        }
        set => _circleFilled = value;
    }
}
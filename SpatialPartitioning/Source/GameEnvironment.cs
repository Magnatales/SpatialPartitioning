﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Graphics;

public static class GameEnvironment
{
    public static GraphicsDeviceManager Graphics { get; set; }
    public static ContentManager Content { get; set; }
    public static SpriteFont DefaultFont;
    public static GameWindow Window;
}
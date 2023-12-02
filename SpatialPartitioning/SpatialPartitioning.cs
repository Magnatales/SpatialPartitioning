using Graphics;
using Graphics.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Project;

public class SpatialPartitioning : Game
{
    private SpriteBatch _spriteBatch;
    private FpsDrawer _fpsDrawer;
    private World _world;

    public SpatialPartitioning()
    {
        GameEnvironment.Graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        GameEnvironment.Content = Content;
        GameEnvironment.Window = Window;
    }

    protected override void Initialize()
    {
        base.Initialize();
        IsMouseVisible = true;
        Window.AllowUserResizing = true;
        IsFixedTimeStep = false;
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        _world = new World(new Vector2(1920, 1080), 2000);
        _fpsDrawer = new FpsDrawer();
    }

    protected override void LoadContent()
    {
        GameEnvironment.DefaultFont = Content.Load<SpriteFont>("textures/verdana");
        GameEnvironment.DefaultBigFont = Content.Load<SpriteFont>("textures/verdana-big");
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        Time.Delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
        Time.Total = (float)gameTime.TotalGameTime.TotalSeconds;
        _world.Update();
        _fpsDrawer.Update();
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(new Color(0.2f, 0.2f, 0.2f, 1));
        _spriteBatch.Begin();
        _world.Draw(_spriteBatch);
        _fpsDrawer.Draw(_spriteBatch, GameEnvironment.DefaultFont, new Vector2(20, 10));
        var centerOfScreen = new Vector2(GraphicsDevice.Viewport.Width / 2f, 70);
        var title = "Spatial Partitioning - Quadtree";
        var size = GameEnvironment.DefaultBigFont.MeasureString(title);
        var position = centerOfScreen - size / 2;
        _spriteBatch.DrawStringWithShadow(GameEnvironment.DefaultBigFont, title, position, Color.White, Color.Black);
        _spriteBatch.End();
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
        _world.Dispose();
    }
}
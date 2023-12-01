using Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace spatial_partitioning
{
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
            _world = new World(new Vector2(1920, 1080), 1000);
            _fpsDrawer = new FpsDrawer();
        }

        protected override void LoadContent()
        {
            GameEnvironment.DefaultFont = Content.Load<SpriteFont>("textures/verdana");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            
            _world.Update(gameTime);
            _fpsDrawer.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.DodgerBlue);
            _spriteBatch.Begin();
            _world.Draw(_spriteBatch);
            _fpsDrawer.Draw(_spriteBatch, GameEnvironment.DefaultFont, new Vector2(20, 10));
            _spriteBatch.End();
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            _world.Dispose();
        }
    }
}
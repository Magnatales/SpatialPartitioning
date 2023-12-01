using System;
using Graphics;
using Graphics.Entities;
using Graphics.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

public class World : IDisposable
{
    private readonly Vector2 _size;
    private readonly Circle[] _entities;
    private int _entitiesInsideQuadtree;
    private Vector2 _offset;
    private RectangleF _quadRectangle;

    public World(Vector2 size, int entitiesAmount)
    {
        _size = size;
        var random = new Random();
        _entities = new Circle[entitiesAmount];
        for (var i = 0; i < _entities.Length; i++)
        {
            var pos = new Vector2(random.Next((int)_offset.X, (int)(_offset.X + _size.X)), random.Next((int)_offset.Y, (int)(_offset.Y + _size.Y)));
            var radius = 7;
            var color = Color.YellowGreen;
            var entity = new Circle(pos, radius, color);
            _entities[i] = entity;
        }
        _quadRectangle = new RectangleF(0, 0, 320, 180);
        GameEnvironment.Window.ClientSizeChanged += UpdateOffset;
    }

    private void UpdateOffset(object sender, EventArgs e)
    {
        var screenSize = GameEnvironment.Graphics.GraphicsDevice.Viewport;
        _offset = new Vector2((screenSize.Width - _size.X) / 2, (screenSize.Height - _size.Y) / 2);
    }

    public void Update(GameTime gameTime)
    {
        var mouseState = Mouse.GetState();
        _quadRectangle.X = mouseState.X - _quadRectangle.Width / 2;
        _quadRectangle.Y = mouseState.Y - _quadRectangle.Height / 2;

        const float speed = 300f;
        var dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
        
        if (Keyboard.GetState().IsKeyDown(Keys.Q))
        {
            var targetWidth = _quadRectangle.Width + speed * dt;
            var targetHeight = targetWidth / 16 * 9;
            _quadRectangle.Width = Math.Min(targetWidth, 1920);
            _quadRectangle.Height = Math.Min(targetHeight, 1080);
        }

        if (Keyboard.GetState().IsKeyDown(Keys.E))
        {
            var targetWidth = _quadRectangle.Width - speed * dt;
            var targetHeight = targetWidth / 16 * 9;
            _quadRectangle.Width = Math.Max(targetWidth, 320);
            _quadRectangle.Height = Math.Max(targetHeight, 180);
        }

        _entitiesInsideQuadtree = 0;
        for (var i = 0; i < _entities.Length; i++)
        {
            if (!_quadRectangle.Contains(_entities[i]))
                continue;
            
            _entitiesInsideQuadtree++;
            UpdateEntity(ref _entities[i], gameTime);
          
            
            _entities[i].Color = Color.YellowGreen;
            for (int j = 0; j < _entities.Length; j++)
            {
                if (j == i) continue;

                if (_entities[i].IsColliding(_entities[j]))
                {
                    _entities[i].Color = Color.Red;
                    break;
                }
            }
        }
    }
    
    
    
    private void UpdateEntity(ref Circle entity, GameTime gameTime)
    {
        entity.Update(gameTime);
    }
    
    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.DrawRectangle(_quadRectangle, Color.OrangeRed, 3);
        foreach (var entity in _entities)
        {
            if (!_quadRectangle.Contains(entity))
                continue;
            spriteBatch.DrawCircleFilled(entity.Position, entity.Radius, entity.Color);
        }
        
        spriteBatch.DrawStringWithShadow(GameEnvironment.DefaultFont, $"Total Entities: {_entities.Length}", new Vector2(20, 30), Color.White, Color.DarkBlue);
        spriteBatch.DrawStringWithShadow(GameEnvironment.DefaultFont, $"Quadtree size: {_quadRectangle.Width:F0}x{_quadRectangle.Height:F0}", new Vector2(20, 50), Color.White, Color.DarkBlue);
        spriteBatch.DrawStringWithShadow(GameEnvironment.DefaultFont, $"Entities inside quadtree: {_entitiesInsideQuadtree}", new Vector2(20, 70), Color.White, Color.DarkBlue);

    }

    public void Dispose()
    {
        GameEnvironment.Window.ClientSizeChanged -= UpdateOffset;
    }
}
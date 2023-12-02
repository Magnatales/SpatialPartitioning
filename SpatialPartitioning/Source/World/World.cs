using System;
using System.Collections.Generic;
using Graphics;
using Graphics.Entities;
using Graphics.Utils;
using Helpers.quadtree;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

public class World : IDisposable
{
    private readonly Vector2 _size;
    private Vector2 _offset;
    private RectangleF _quadRectangle;
    private readonly Quadtree _quadtree;
    private readonly List<Circle> _circlesInRange = new();

    public World(Vector2 size, int entitiesAmount)
    {
        _size = size;
        var random = new Random();
        var circles = new List<Circle>();
        _quadtree = new Quadtree(_quadRectangle, 3);
        for (var i = 0; i < entitiesAmount; i++)
        {
            var pos = new Vector2(random.Next((int)_offset.X, (int)(_offset.X + _size.X)), random.Next((int)_offset.Y, (int)(_offset.Y + _size.Y)));
            var radius = 6;
            var color = Color.YellowGreen;
            var entity = new Circle(pos, radius, color);
            circles.Add(entity);
        }
        _quadtree.AddActors(circles);
        _quadRectangle = new RectangleF(0, 0, 320, 180);
        _quadtree.OnQuadTreeUpdate += OnQuadtreeUpdated;
        GameEnvironment.Window.ClientSizeChanged += UpdateOffset;
    }
    
    private void OnQuadtreeUpdated(List<Circle> entities)
    {
        foreach (var entity in entities)
        {
            entity.Update();
            var collided = false;
            _circlesInRange.Clear();
            _quadtree.GetActorsWithinActorRange(entity, _circlesInRange);

            foreach (var circle in _circlesInRange)
            {
                if(circle == entity) 
                    continue;
                
                if (entity.IsColliding(circle))
                    collided = true;
            }
            entity.Color = collided ? Color.Red : Color.YellowGreen;
        }
    }

    private void UpdateOffset(object sender, EventArgs e)
    {
        var screenSize = GameEnvironment.Graphics.GraphicsDevice.Viewport;
        _offset = new Vector2((screenSize.Width - _size.X) / 2, (screenSize.Height - _size.Y) / 2);
    }

    public void Update()
    {
        var mouseState = Mouse.GetState();
        _quadRectangle.X = mouseState.X - _quadRectangle.Width / 2;
        _quadRectangle.Y = mouseState.Y - _quadRectangle.Height / 2;

        const float speed = 300f;
        var dt = Time.Delta;
        
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
        
        _quadtree.Update(_quadRectangle, dt);
    }
    
    public void Draw(SpriteBatch spriteBatch)
    {
        _quadtree.Draw(spriteBatch);
        _quadtree.DrawDebug(spriteBatch);
        spriteBatch.DrawStringWithShadow(GameEnvironment.DefaultFont, $"Total Entities: {_quadtree.GetTotalActorsCount()}", new Vector2(20, 30), Color.White, Color.Black);
        spriteBatch.DrawStringWithShadow(GameEnvironment.DefaultFont, $"Total Quadtrees: {_quadtree.GetTotalQuadtreeCount()}", new Vector2(20, 50), Color.White, Color.Black);
        spriteBatch.DrawStringWithShadow(GameEnvironment.DefaultFont, $"Entities inside Quadtree: {_quadtree.GetActorsInsideQuadtree()}", new Vector2(20, 70), Color.White, Color.Black);
        spriteBatch.DrawStringWithShadow(GameEnvironment.DefaultFont, $"{_quadRectangle.Width:F0}x{_quadRectangle.Height:F0}", new Vector2(_quadRectangle.X + _quadRectangle.Width /2 - 40, _quadRectangle.Y - 20), Color.White, Color.Black);
 
    }

    public void Dispose()
    {
        GameEnvironment.Window.ClientSizeChanged -= UpdateOffset;
        _quadtree.OnQuadTreeUpdate -= OnQuadtreeUpdated;
        _quadtree.Dispose();
    }
}
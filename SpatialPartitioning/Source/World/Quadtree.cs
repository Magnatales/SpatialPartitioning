using System;
using System.Collections.Generic;
using Graphics;
using Graphics.Entities;
using Graphics.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Vector2 = System.Numerics.Vector2;

namespace Helpers.quadtree;

public class Quadtree : IDisposable
{
    public int Capacity { get; private set; }
    public event Action<List<Circle>> OnQuadTreeUpdate; 
    
    private RectangleF _boundaries;
    private bool _divided;
    private readonly List<Circle> _totalActors = new();
    private readonly List<Circle> _actorsInQuadtree = new();
    private readonly List<Circle> _actorsWithinBoundaries = new();
    private List<Circle> _actorsInViewPort = new();
    private RectangleF _currentViewPort;
    private Quadtree _northWest;
    private Quadtree _northEast;
    private Quadtree _southWest;
    private Quadtree _southEast;

    public Quadtree(RectangleF boundaries, int capacity)
    {
        _boundaries = boundaries;
        Capacity = capacity;
        _divided = false;
    }

    private void Clear()
    {
        _actorsInQuadtree.Clear();
        _northWest?.Clear();
        _northEast?.Clear();
        _southWest?.Clear();
        _southEast?.Clear();
        _divided = false;
    }

    public void AddActors(List<Circle> actors)
    {
        _totalActors.AddRange(actors);
    }

    public bool InsertInQuadtree(Circle actor)
    {
        if (!_boundaries.Contains(actor.Position))
            return false;

        if (!_divided)
        {
            if (_actorsInQuadtree.Count < Capacity)
            {
                _actorsInQuadtree.Add(actor);
                return true;
            }

            Subdivide();
        }

        return _northEast.InsertInQuadtree(actor) ||
               _northWest.InsertInQuadtree(actor) ||
               _southEast.InsertInQuadtree(actor) ||
               _southWest.InsertInQuadtree(actor);
    }

    public void Update(RectangleF boundaries, float dt)
    {
        Clear();
        _boundaries = boundaries;
        foreach (var actor in _totalActors)
        {
            InsertInQuadtree(actor);
        }

        _currentViewPort = boundaries;

        _actorsInViewPort = GetActorsWithinBoundaries(_currentViewPort); 
        OnQuadTreeUpdate?.Invoke(_actorsInViewPort);
    }

    public bool RemoveActor(Circle actor)
    {
        if (_actorsInQuadtree.Remove(actor))
        {
            if (_actorsInQuadtree.Count <= Capacity)
            {
                _divided = false;
            }
            return true;
        }
        
        if (_divided)
        {
            if (_northWest.RemoveActor(actor) || 
                _northEast.RemoveActor(actor) || 
                _southWest.RemoveActor(actor) || 
                _southEast.RemoveActor(actor))
            {
                return true;
            }
            
            if (!_northWest._divided &&
                !_northEast._divided &&
                !_southWest._divided &&
                !_southEast._divided &&
                _northWest._actorsInQuadtree.Count == 0 &&
                _northEast._actorsInQuadtree.Count == 0 &&
                _southWest._actorsInQuadtree.Count == 0 &&
                _southEast._actorsInQuadtree.Count == 0)
            {
               
                _divided = false;
            }
        }

        return false;
    }
    
    public void GetActorsWithinActorRange(Circle actor, List<Circle> result)
    {
        var position = actor.Position;
        if (_boundaries.Contains(position))
        {
            foreach (var actorInQuadtree in _actorsInQuadtree)
            {
                    result.Add(actorInQuadtree);
            }

            if (_divided)
            {
                _northWest.GetActorsWithinActorRange(actor, result);
                _northEast.GetActorsWithinActorRange(actor, result);
                _southWest.GetActorsWithinActorRange(actor, result);
                _southEast.GetActorsWithinActorRange(actor, result);
            }
        }
    }
    
    
    public List<Circle> GetActorsWithinBoundaries(RectangleF boundaries)
    {
        _actorsWithinBoundaries.Clear();
        if (!_boundaries.Intersects(boundaries))
        {
            return _actorsWithinBoundaries;
        }
        
        foreach (var actor in _actorsInQuadtree)
        {
            if (boundaries.Contains(actor))
            {
                _actorsWithinBoundaries.Add(actor);
            }
        }

        if (_divided)
        {
            _actorsWithinBoundaries.AddRange(_northWest.GetActorsWithinBoundaries(boundaries));
            _actorsWithinBoundaries.AddRange(_northEast.GetActorsWithinBoundaries(boundaries));
            _actorsWithinBoundaries.AddRange(_southWest.GetActorsWithinBoundaries(boundaries));
            _actorsWithinBoundaries.AddRange(_southEast.GetActorsWithinBoundaries(boundaries));
        }

        return _actorsWithinBoundaries;
    }

    private void Subdivide()
    {
        var x = _boundaries.X;
        var y = _boundaries.Y;
        var w = _boundaries.Width / 2;
        var h = _boundaries.Height / 2;

        var nw = new RectangleF(x, y, w, h);
        _northWest = new Quadtree(nw, Capacity);

        var ne = new RectangleF(x + w, y, w, h);
        _northEast = new Quadtree(ne, Capacity);

        var sw = new RectangleF(x, y + h, w, h);
        _southWest = new Quadtree(sw, Capacity);

        var se = new RectangleF(x + w, y + h, w, h);
        _southEast = new Quadtree(se, Capacity);

        _divided = true;
    }
    
    public int GetTotalQuadtreeCount()
    {
        int count = 1; // Count the current quadtree node

        if (_divided)
        {
            count += _northWest.GetTotalQuadtreeCount();
            count += _northEast.GetTotalQuadtreeCount();
            count += _southWest.GetTotalQuadtreeCount();
            count += _southEast.GetTotalQuadtreeCount();
        }

        return count;
    }

    public int GetTotalActorsCount() => _totalActors.Count;

    public int GetActorsInsideQuadtree()
    {
        var count = _actorsInQuadtree.Count;

        if (_divided)
        {
            count += _northWest.GetActorsInsideQuadtree();
            count += _northEast.GetActorsInsideQuadtree();
            count += _southWest.GetActorsInsideQuadtree();
            count += _southEast.GetActorsInsideQuadtree();
        }

        return count;
    }

    public override string ToString()
    {
        int totalQuadtreeCount = GetTotalQuadtreeCount();
        int totalActorCount = GetActorsInsideQuadtree();

        return $"Total Quadtree Nodes: {totalQuadtreeCount}, Total Actors: {totalActorCount}";
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        foreach (var circle in _actorsInViewPort)
        {
            circle.Draw(spriteBatch);
        }
    }
    
    public void DrawDebug(SpriteBatch spriteBatch)
    {
        var thickness = 3;
        spriteBatch.DrawLine(new Vector2(_boundaries.X, _boundaries.Y), new Vector2(_boundaries.X + _boundaries.Width, _boundaries.Y), Color.OrangeRed, thickness);
        spriteBatch.DrawLine(new Vector2(_boundaries.X, _boundaries.Y), new Vector2(_boundaries.X, _boundaries.Y + _boundaries.Height), Color.OrangeRed, thickness);
        spriteBatch.DrawLine(new Vector2(_boundaries.X, _boundaries.Y + _boundaries.Height), new Vector2(_boundaries.X + _boundaries.Width, _boundaries.Y + _boundaries.Height), Color.OrangeRed, thickness);
        spriteBatch.DrawLine(new Vector2(_boundaries.X + _boundaries.Width, _boundaries.Y), new Vector2(_boundaries.X + _boundaries.Width, _boundaries.Y + _boundaries.Height), Color.OrangeRed, thickness);
        if (_divided)
        {
            _northWest.DrawDebug(spriteBatch);
            _northEast.DrawDebug(spriteBatch);
            _southWest.DrawDebug(spriteBatch);
            _southEast.DrawDebug(spriteBatch);
        }
    }
    
    public void Dispose()
    {
        _actorsInQuadtree.Clear();
        _actorsInViewPort.Clear();
        _totalActors.Clear();
        _northWest?.Dispose();
        _northEast?.Dispose();
        _southWest?.Dispose();
        _southEast?.Dispose();
    }
}
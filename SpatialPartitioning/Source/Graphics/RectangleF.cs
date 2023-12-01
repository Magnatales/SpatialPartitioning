using Microsoft.Xna.Framework;

public struct RectangleF
{
    public float X;
    public float Y;
    public float Width;
    public float Height;
    
    public float Left => this.X;

    public float Right => this.X + this.Width;

    public float Top => this.Y;

    public float Bottom => this.Y + this.Height;

    public RectangleF(float x, float y, float width, float height)
    {
        X = x;
        Y = y;
        Width = width;
        Height = height;
    }

    public RectangleF(Vector2 position, Vector2 size)
    {
        X = position.X;
        Y = position.Y;
        Width = size.X;
        Height = size.Y;
    }
}
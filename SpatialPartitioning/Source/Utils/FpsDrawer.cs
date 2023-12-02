using Graphics;
using Graphics.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class FpsDrawer
{
    private double _frames;
    private double _elapsed;
    private double _last;
    private const float MSG_FREQUENCY = 0.2f;
    private string _msg = "Fps: 0";

    public void Update()
    {
        _elapsed = Time.Total - _last;
        if (_elapsed > MSG_FREQUENCY)
        {
            var frames = _frames / _elapsed;
            _msg = $"Fps: {frames:F0}";
            _elapsed = 0;
            _frames = 0;
            _last = Time.Total;
        }
    }

    public void Draw(SpriteBatch spriteBatch, SpriteFont font, Vector2 fpsDisplayPosition)
    {
        spriteBatch.DrawStringWithShadow(font, _msg, fpsDisplayPosition, Color.White, Color.Black);
        _frames++;
    }
}
using Graphics.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class FpsDrawer
{
    private double _frames;
    private double _elapsed;
    private double _last;
    private double _now;
    private const double MSG_FREQUENCY = 0.2f;
    private string _msg = "Fps: 0";

    public void Update(GameTime gameTime)
    {
        _now = gameTime.TotalGameTime.TotalSeconds;
        _elapsed = _now - _last;
        if (_elapsed > MSG_FREQUENCY)
        {
            var frames = _frames / _elapsed;
            _msg = $"Fps: {frames:F0}";
            _elapsed = 0;
            _frames = 0;
            _last = _now;
        }
    }

    public void Draw(SpriteBatch spriteBatch, SpriteFont font, Vector2 fpsDisplayPosition)
    {
        spriteBatch.DrawStringWithShadow(font, _msg, fpsDisplayPosition, Color.White, Color.DarkBlue);
        _frames++;
    }
}
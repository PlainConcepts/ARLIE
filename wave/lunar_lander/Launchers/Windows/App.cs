using System;
using System.IO;
using System.Reflection;
using WaveEngine.Common.Graphics;
using WaveEngine.Common.Input;
using WaveEngine.Common.Math;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Services;
using RLEnvs.Helpers;

namespace RLEnvs
{
  public class App : WaveEngine.Adapter.Application
  {
    RLEnvs.Game game;
    SpriteBatch spriteBatch;
    Texture2D splashScreen;
    bool splashState = true;
    TimeSpan time;
    Vector2 position;
    Color backgroundSplashColor;

    private int seed;

    public App(bool renderMode, int seed)
    {
      this.Width = LunarConstants.VIEWPORT_W;
      this.Height = LunarConstants.VIEWPORT_H;
      this.FullScreen = false;
      this.WindowTitle = "ARLIE";
      this.HasVideoSupport = false;
      this.IsFixedTimeStep = renderMode;
      this.seed = seed;
    }

    public override void Initialize()
    {

      this.game = new RLEnvs.Game();
      this.game.Initialize(this);
      WaveServices.Random.Seed = this.seed;

      #region DEFAULT SPLASHSCREEN
      this.backgroundSplashColor = Color.White;
      this.spriteBatch = new SpriteBatch(WaveServices.GraphicsDevice);

      var resourceNames = Assembly.GetExecutingAssembly().GetManifestResourceNames();
      string name = string.Empty;

      foreach (string item in resourceNames)
      {
        if (item.Contains("SplashScreen.png"))
        {
          name = item;
          break;
        }
      }

      if (string.IsNullOrWhiteSpace(name))
      {
        throw new InvalidProgramException("License terms not agreed.");
      }

      using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(name))
      {
        this.splashScreen = Texture2D.FromFile(WaveServices.GraphicsDevice, stream);
      }

      position = new Vector2();
      position.X = (this.Width / 2.0f) - (this.splashScreen.Width / 2.0f);
      position.Y = (this.Height / 2.0f) - (this.splashScreen.Height / 2.0f);
      #endregion
    }

    public override void Update(TimeSpan elapsedTime)
    {
      if (this.game != null && !this.game.HasExited)
      {
        if (WaveServices.Input.KeyboardState.F10 == ButtonState.Pressed)
        {
          this.FullScreen = !this.FullScreen;
        }

        if (this.splashState)
        {
          #region DEFAULT SPLASHSCREEN
          this.time += elapsedTime;
          if (time > TimeSpan.FromSeconds(2))
          {
            this.splashState = false;
            this.splashScreen.Unload();
            this.splashScreen = null;
          }
          #endregion
        }
        else
        {
          if (WaveServices.Input.KeyboardState.Escape == ButtonState.Pressed)
          {
            WaveServices.Platform.Exit();
          }
          else
          {
            this.game.UpdateFrame(elapsedTime);
          }
        }
      }
    }

    public override void Draw(TimeSpan elapsedTime)
    {
      if (this.game != null && !this.game.HasExited)
      {
        if (this.splashState)
        {
          #region DEFAULT SPLASHSCREEN
          WaveServices.GraphicsDevice.RenderTargets.SetRenderTarget(null);
          WaveServices.GraphicsDevice.Clear(ref this.backgroundSplashColor, ClearFlags.Target, 1);
          this.spriteBatch.Draw(this.splashScreen, this.position, Color.White);
          this.spriteBatch.Render();
          #endregion
        }
        else
        {
          this.game.DrawFrame(elapsedTime);
        }
      }
    }

    /// <summary>
    /// Called when [activated].
    /// </summary>
    public override void OnActivated()
    {
      base.OnActivated();
      if (this.game != null)
      {
        game.OnActivated();
      }
    }

    /// <summary>
    /// Called when [deactivate].
    /// </summary>
    public override void OnDeactivate()
    {
      base.OnDeactivate();
      if (this.game != null)
      {
        game.OnDeactivated();
      }
    }
  }
}
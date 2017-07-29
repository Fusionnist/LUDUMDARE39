using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace LUDUMDARE39
{
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Player player;
        RenderTarget2D target;
        float scale;
        Point virtualDim;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            Window.IsBorderless = true;
            graphics.ApplyChanges();

            SetupGraphics();
        }
        void SetupGraphics()
        {
            virtualDim = new Point(192,108);

            float xscale = GraphicsDevice.Viewport.Width / virtualDim.X;
            float yscale = GraphicsDevice.Viewport.Height / virtualDim.Y;

            if (xscale > yscale)
            {
                scale = yscale;
            }
            else { scale = xscale; }

            target = new RenderTarget2D(GraphicsDevice, virtualDim.X, virtualDim.Y);
        }
        protected override void Initialize()
        {
            base.Initialize();        
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            player = new Player(new STexture(Content.Load<Texture2D>("test"), 4, 16, 0.1f), new Vector2(90, 104));
        
        }

        protected override void UnloadContent()
        {
            Content.Unload();
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            player.Update(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            GraphicsDevice.SetRenderTarget(target);
            spriteBatch.Begin();
            player.Draw(spriteBatch);
            spriteBatch.End();

            Matrix m = Matrix.CreateScale(scale);
            GraphicsDevice.SetRenderTarget(null);
            spriteBatch.Begin(transformMatrix:m);
            spriteBatch.Draw(texture: target,position: Vector2.Zero);
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}

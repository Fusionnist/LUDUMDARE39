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
        int scale;
        Rectangle virtualDim;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            Window.IsBorderless = true;
            graphics.ApplyChanges();

            UpdateGraphicsValues();
        }
        void UpdateGraphicsValues()
        {
            virtualDim = new Rectangle(0,0,192,108);

            int xscale = GraphicsDevice.Viewport.Width / virtualDim.Width;
            int yscale = GraphicsDevice.Viewport.Height / virtualDim.Height;

            if (xscale > yscale)
            {
                scale = yscale;
            }
            else { scale = xscale; }

            target = new RenderTarget2D(GraphicsDevice, virtualDim.Width, virtualDim.Height);
            virtualDim.X = (int)((GraphicsDevice.Viewport.Width/scale- virtualDim.Width) /2);
            virtualDim.Y = (int)((GraphicsDevice.Viewport.Height/scale - virtualDim.Height) /2);
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
            player.Update(gameTime, virtualDim);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {          
            GraphicsDevice.SetRenderTarget(target);
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();
            player.Draw(spriteBatch);
            spriteBatch.End();

            Matrix m = Matrix.CreateScale(scale);
            GraphicsDevice.SetRenderTarget(null);
            spriteBatch.Begin(transformMatrix:m, samplerState:SamplerState.PointWrap);
            spriteBatch.Draw(texture: target,destinationRectangle:virtualDim);
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}

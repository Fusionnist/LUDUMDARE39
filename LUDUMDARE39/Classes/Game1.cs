using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace LUDUMDARE39
{
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        RenderTarget2D target;
        int scale;
        Rectangle virtualDim;
        CollisionStuff colman;

        Input flippy;
        Switch[] switches;
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height/2;
            graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width/2;
            Window.IsBorderless = true;
            graphics.ApplyChanges();

            UpdateGraphicsValues();

            flippy = new Input(Keys.Space);
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
            Player player = new Player(new STexture[1] { new STexture(Content.Load<Texture2D>("test"), 4, 16, 0.1f, "test")}, new Vector2(50, virtualDim.Height - 16));
            Boss boss = new Boss(new STexture[1] { new STexture(Content.Load<Texture2D>("test"), 4, 16, 0.1f, "test") }, new Vector2(10, virtualDim.Height - 16));
            colman = new CollisionStuff(player, boss);

            switches = new Switch[] {
                new Switch(new STexture[]{
                new STexture(Content.Load<Texture2D>("switchon"), 4, 16, 0.1f, "switchon"),
                new STexture(Content.Load<Texture2D>("switchoff"), 4, 16, 0.1f, "test") },
                new Vector2(0,0)),

                new Switch(new STexture[]{
                new STexture(Content.Load<Texture2D>("switchon"), 4, 16, 0.1f, "switchon"),
                new STexture(Content.Load<Texture2D>("switchoff"), 4, 16, 0.1f, "test") },
                new Vector2(40,0)),
            };
        }

        protected override void UnloadContent()
        {
            Content.Unload();
        }

        protected override void Update(GameTime gameTime)
        {
            KeyboardState kbs = Keyboard.GetState();
            flippy.Update(kbs);

            FlipSwitches();

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            colman.Update(gameTime, virtualDim);
            base.Update(gameTime);
        }
        void FlipSwitches() {
            if (flippy.IsPressed())
            { switches[0].Activate(); }
        }
        protected override void Draw(GameTime gameTime)
        {          
            GraphicsDevice.SetRenderTarget(target);
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();
            colman.player.Draw(spriteBatch);
            colman.boss.Draw(spriteBatch);
            foreach(Switch s in switches) { s.Draw(spriteBatch); }
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

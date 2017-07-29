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
        Lifebar lifebar;

        STexture bg;
        Input flippy;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
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
            Player player = new Player(new STexture[1] { new STexture(Content.Load<Texture2D>("test"), 4, 16, 0.1f, "test", new Rectangle(0, 0, 16, 16))}, new Vector2(50, virtualDim.Height - 16));
            Boss boss = new Boss(new STexture[1] { new STexture(Content.Load<Texture2D>("test"), 4, 16, 0.1f, "test", new Rectangle(0, 0, 16, 16)) }, new Vector2(10, virtualDim.Height - 16));
            bg = new STexture(Content.Load<Texture2D>("scene"), 1, 192, 0.1f, "switchon", new Rectangle(0, 0, 192, 108));
            Switch[] switches = new Switch[] {
                new Switch(new STexture[]{
                new STexture(Content.Load<Texture2D>("switchon"), 1, 16, 0.1f, "switchon", new Rectangle(0, 0, 16, 16)),
                new STexture(Content.Load<Texture2D>("switchoff"), 1, 16, 0.1f, "switchoff", new Rectangle(0, 0, 16, 16)) },
                new Vector2(0,0),
                new Plug(new STexture[]{
                new STexture(Content.Load<Texture2D>("switchon"), 1, 16, 0.1f, "switchon", new Rectangle(0, 0, 16, 16)),
                new STexture(Content.Load<Texture2D>("switchoff"), 1, 16, 0.1f, "switchoff", new Rectangle(0, 0, 16, 16)) },
                new Vector2(66, 66))),

                new Switch(new STexture[]{
                new STexture(Content.Load<Texture2D>("switchon"), 1, 16, 0.1f, "switchon", new Rectangle(0, 0, 16, 16)),
                new STexture(Content.Load<Texture2D>("switchoff"), 1, 16, 0.1f, "switchoff", new Rectangle(0, 0, 16, 16)) },
                new Vector2(40,0),
                new Plug(new STexture[]{
                new STexture(Content.Load<Texture2D>("switchon"), 1, 16, 0.1f, "switchon", new Rectangle(0, 0, 16, 16)),
                new STexture(Content.Load<Texture2D>("switchoff"), 1, 16, 0.1f, "switchoff", new Rectangle(0, 0, 16, 16)) },
                new Vector2(33, 66)))

            };
            colman = new CollisionStuff(player, boss, switches);

            lifebar = new Lifebar(new STexture[2] { new STexture(Content.Load<Texture2D>("barcont"), 1, 22, 1, "barcontainer", new Rectangle(0, 0, 22, 5)), new STexture(Content.Load<Texture2D>("barint"), 1, 20, 1, "barinterior", new Rectangle(0, 0, 20, 3)) }, new Vector2(0, 0));
        }

        protected override void UnloadContent()
        {
            Content.Unload();
        }

        protected override void Update(GameTime gameTime)
        {
            KeyboardState kbs = Keyboard.GetState();
            flippy.Update(kbs);

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            colman.Update(gameTime, virtualDim, flippy);
            lifebar.hp = colman.boss.hp;
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {          
            GraphicsDevice.SetRenderTarget(target);
            spriteBatch.Begin();
            bg.Draw(spriteBatch, Vector2.Zero);
            foreach (Switch s in colman.switches) { s.Draw(spriteBatch); }
            colman.player.Draw(spriteBatch);
            colman.boss.Draw(spriteBatch);
            lifebar.Draw(spriteBatch);
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

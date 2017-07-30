using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
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
        Rectangle virtualDim, roomDim;
        CollisionStuff colman;
        Lifebar lifebar, clb;

        STexture bg;
        Input flippy;

        float cityHp;
        STexture c3, c2, c1;

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
            cityHp = 3;
        }
        void UpdateGraphicsValues()
        {
            virtualDim = new Rectangle(0,0,192,108);
            roomDim = new Rectangle(3,8,186,97);
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
            c3 = new STexture(Content.Load<Texture2D>("scene3hp"), new Rectangle(0, 0, 192, 108), "city3");
            c2 = new STexture(Content.Load<Texture2D>("scene2hp"), new Rectangle(0, 0, 192, 108), "city2");
            c1 = new STexture(Content.Load<Texture2D>("scene1hp"), new Rectangle(0, 0, 192, 108), "city1");
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Player player = new Player(new STexture[1] { new STexture(Content.Load<Texture2D>("test"), 4, 16, 0.1f, "test", new Rectangle(2, 0, 12, 16), true) }, new Vector2(50,88), Content.Load<SoundEffect>("jump"), Content.Load<SoundEffect>("hurt"));
            Boss boss = new Boss(
                new STexture[1] {
                new STexture(Content.Load<Texture2D>("test"), 4, 16, 0.1f, "test", new Rectangle(0, 0, 16, 16), true) }, 
                new Vector2(10, 88), 
                new STexture[2] {
                new STexture(Content.Load<Texture2D>("bullet"), 6, 8, 0.1f, "bullet", new Rectangle(1, 1, 6, 6), true),
                new STexture(Content.Load<Texture2D>("splode"), 6, 8, 0.1f, "explosion", new Rectangle(1, 1, 6, 6), false) },
                Content.Load<SoundEffect>("bossshot") );
            bg = c3;
            Switch[] switches = new Switch[] {
                new Switch(new STexture[]{
                new STexture(Content.Load<Texture2D>("switchon"), new Rectangle(0, 0, 8, 9), "switchon"),
                new STexture(Content.Load<Texture2D>("switchoff"), new Rectangle(0, 0, 8, 9), "switchoff") },
                new Vector2(12,19),
                new Plug(new STexture[]{
                new STexture(Content.Load<Texture2D>("plug"), new Rectangle(0, 0, 8, 9), "switchon"),
                new STexture(Content.Load<Texture2D>("plugoff"), new Rectangle(0, 0, 8, 9), "switchoff") },
                new Vector2(10,83)),
                Content.Load<SoundEffect>("switch")),

                new Switch(new STexture[]{
                new STexture(Content.Load<Texture2D>("switchon"), new Rectangle(0, 0, 8, 9), "switchon"),
                new STexture(Content.Load<Texture2D>("switchoff"), new Rectangle(0, 0, 8, 9), "switchoff") },
                new Vector2(92,19),
                new Plug(new STexture[]{
                new STexture(Content.Load<Texture2D>("plug"), new Rectangle(0, 0, 11, 10), "switchon"),
                new STexture(Content.Load<Texture2D>("plugoff"), new Rectangle(0, 0, 11, 10), "switchoff") },
                new Vector2(90,83)),
                Content.Load<SoundEffect>("switch")),

                new Switch(new STexture[]{
                new STexture(Content.Load<Texture2D>("switchon"), new Rectangle(0, 0, 8, 9), "switchon"),
                new STexture(Content.Load<Texture2D>("switchoff"), new Rectangle(0, 0, 8, 9), "switchoff") },
                new Vector2(172,19),
                new Plug(new STexture[]{
                new STexture(Content.Load<Texture2D>("plug"), new Rectangle(0, 0, 11, 10), "switchon"),
                new STexture(Content.Load<Texture2D>("plugoff"), new Rectangle(0, 0, 11, 10), "switchoff") },
                new Vector2(170,83)),
                Content.Load<SoundEffect>("switch"))
            };
            colman = new CollisionStuff(player, boss, switches);

            lifebar = new Lifebar(new STexture[2] { new STexture(Content.Load<Texture2D>("barcont"), new Rectangle(0, 0, 22, 5), "barcontainer"), new STexture(Content.Load<Texture2D>("barint"), new Rectangle(0, 0, 20, 3), "barinterior") }, new Vector2(0, 0), colman.boss.maxhp);
            clb = new Lifebar(new STexture[2] { new STexture(Content.Load<Texture2D>("barcont"), new Rectangle(0, 0, 22, 5), "barcontainer"), new STexture(Content.Load<Texture2D>("barint"), new Rectangle(0, 0, 20, 3), "barinterior") }, new Vector2(50, 0),3);
        }

        protected override void UnloadContent()
        {
            Content.Unload();
        }

        protected override void Update(GameTime gameTime)
        {
            //test
            foreach (var bullet in colman.boss.bullets)
                bullet.Update(gameTime, roomDim);

            if (colman.boss.isPlugged) { cityHp -= 0.1f * (float)gameTime.ElapsedGameTime.TotalSeconds; }
            else { cityHp += 0.05f * (float)gameTime.ElapsedGameTime.TotalSeconds; }

            if (cityHp <= 3) { bg = c3; }
            if (cityHp <= 2) { bg = c2; }
            if (cityHp <= 1) { bg = c1; }
            if (cityHp <= 0) { cityHp = 0; } //die

            KeyboardState kbs = Keyboard.GetState();
            flippy.Update(kbs);

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            colman.Update(gameTime, roomDim, flippy);
            lifebar.hp = colman.boss.hp;
            clb.hp = cityHp;
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
            foreach (var bullet in colman.boss.bullets)
                bullet.Draw(spriteBatch);
            lifebar.Draw(spriteBatch);
            clb.Draw(spriteBatch);
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

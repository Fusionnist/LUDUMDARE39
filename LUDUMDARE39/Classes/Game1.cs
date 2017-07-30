using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace LUDUMDARE39
{
    enum GameState { Start, Game, End, Loss, Pause}
    enum GamePhase { BossEnter, Fight, BossFall}
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        RenderTarget2D target;
        int scale, baseScale;
        Rectangle virtualDim, roomDim;
        CollisionStuff colman;
        Lifebar lifebar, clb;

        STexture bg, transition;
        Input flippy, paused;

        float cityHp, blinktime, blinktimer;
        bool blinks, startedTransition;
        STexture c3, c2, c1, start, loss, end, pause, shim;

        GameState state; GamePhase phase;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height/2;
            graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width/2;
            Window.IsBorderless = true;
            graphics.ApplyChanges();

            state = GameState.Start;
            UpdateGraphicsValues();

            flippy = new Input(Keys.Space);
            paused = new Input(Keys.P);

            blinktime = 0.5f;
            blinktimer = 0.5f;
            blinks = true;

            ResetGame();
        }
        void MakeBlink(GameTime gt_)
        {
            blinktimer -= (float)gt_.ElapsedGameTime.TotalSeconds;
            if(blinktimer <= 0)
            {
                blinktimer = blinktime;
                blinks = !blinks;
            }
        }
        void UpdateGraphicsValues()
        {
            virtualDim = new Rectangle(0,0,192,108);
            roomDim = new Rectangle(3,8,186,96);
            int xscale = GraphicsDevice.Viewport.Width / virtualDim.Width;
            int yscale = GraphicsDevice.Viewport.Height / virtualDim.Height;

            if (xscale > yscale)
            {
                scale = yscale;
            }
            else { scale = xscale; }
            baseScale = scale;

            target = new RenderTarget2D(GraphicsDevice, virtualDim.Width, virtualDim.Height);
            virtualDim.X = (int)((GraphicsDevice.Viewport.Width/scale- virtualDim.Width) /2);
            virtualDim.Y = (int)((GraphicsDevice.Viewport.Height/scale - virtualDim.Height) /2);
        }
        protected override void Initialize()
        {
            base.Initialize();        
        }
        void ResetGame()
        {
            phase = GamePhase.BossEnter;
            cityHp = 3;
            LoadContent();
        }
        protected override void LoadContent()
        {
            shim = new STexture(Content.Load<Texture2D>("stophim"), Rectangle.Empty, "wow");
            start = new STexture(Content.Load<Texture2D>("titlescreen"), Rectangle.Empty, "wow");
            pause = new STexture(Content.Load<Texture2D>("titlescreen"), Rectangle.Empty, "wow");
            loss = new STexture(Content.Load<Texture2D>("titlescreen"), Rectangle.Empty, "wow");
            end = new STexture(Content.Load<Texture2D>("titlescreen"), Rectangle.Empty, "wow");

            transition = new STexture(Content.Load<Texture2D>("transition"), 10, 192, 0.1f, "test", Rectangle.Empty, false);
            transition.currentframe = 10;

            c3 = new STexture(Content.Load<Texture2D>("scene3hp"), new Rectangle(0, 0, 192, 108), "city3");
            c2 = new STexture(Content.Load<Texture2D>("scene2hp"), new Rectangle(0, 0, 192, 108), "city2");
            c1 = new STexture(Content.Load<Texture2D>("scene1hp"), new Rectangle(0, 0, 192, 108), "city1");
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Player player = new Player(new STexture[1] { new STexture(Content.Load<Texture2D>("test"), 4, 16, 0.1f, "test", new Rectangle(2, 0, 12, 16), true) }, new Vector2(140,88), Content.Load<SoundEffect>("jump"), Content.Load<SoundEffect>("hurt"));
            Boss boss = new Boss(
                new STexture[1] {
                new STexture(Content.Load<Texture2D>("test"), 4, 16, 0.1f, "test", new Rectangle(0, 0, 16, 16), true) }, 
                new Vector2(36, 88), 
                new STexture[2] {
                new STexture(Content.Load<Texture2D>("bullet"), 6, 8, 0.1f, "bullet", new Rectangle(3, 3, 2, 2), true),
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
            KeyboardState kbs = Keyboard.GetState();
            if (kbs.IsKeyDown(Keys.Escape))
                Exit();
            flippy.Update(kbs);
            paused.Update(kbs);
            base.Update(gameTime);

            transition.Update(gameTime);

            switch (state)
            {
                case (GameState.Game):
                    switch (phase)
                    {
                        case GamePhase.BossEnter:
                            MakeBlink(gameTime);
                            colman.boss.Pluggg(colman.switches, gameTime);
                            if (colman.boss.isPlugged) { phase = GamePhase.Fight; scale = baseScale; }
                            break;
                        case GamePhase.BossFall:
                            break;
                        case GamePhase.Fight:
                            foreach (var bullet in colman.boss.bullets)
                                bullet.Update(gameTime, roomDim);

                            if (colman.boss.isPlugged) { cityHp -= 0.1f * (float)gameTime.ElapsedGameTime.TotalSeconds; }
                            else { cityHp += 0.05f * (float)gameTime.ElapsedGameTime.TotalSeconds; }

                            if (cityHp <= 3) { bg = c3; }
                            if (cityHp <= 2) { bg = c2; }
                            if (cityHp <= 1) { bg = c1; }
                            if (cityHp <= 0) { state = GameState.Loss; bg = loss; } //die
                            if (colman.boss.hp <= 0) { state = GameState.End; bg = end; }
                            colman.Update(gameTime, roomDim, flippy);
                            lifebar.hp = colman.boss.hp;
                            clb.hp = cityHp;

                            if (paused.IsPressed()) { state = GameState.Pause; }
                            break;
                    }
                    break;
                case GameState.End:
                    
                    if (flippy.IsPressed() && !startedTransition) { transition.Reset(); startedTransition = true; }
                    if (transition.currentframe >= 4 && startedTransition){ state = GameState.Game; ResetGame(); startedTransition = false; }
                    break;
                case GameState.Loss:
                    
                    if (flippy.IsPressed() && !startedTransition) { transition.Reset(); startedTransition = true; }
                    if (transition.currentframe >= 4 && startedTransition) { state = GameState.Game; ResetGame(); startedTransition = false; }
                    break;
                case GameState.Start:
                    bg = start;
                    if (flippy.IsPressed() && !startedTransition) { transition.Reset(); startedTransition = true; }
                    if (transition.currentframe >= 4 && startedTransition)
                    { state = GameState.Game; ResetGame(); startedTransition = false; }
                    break;
                case (GameState.Pause):
                    if (paused.IsPressed()) { state = GameState.Game; }
                    break;
            }            
        }

        void DrawGame()
        {
            
            bg.Draw(spriteBatch, Vector2.Zero);
            foreach (Switch s in colman.switches) { s.Draw(spriteBatch); }
            colman.player.Draw(spriteBatch);
            colman.boss.Draw(spriteBatch);
            foreach (var bullet in colman.boss.bullets)
                bullet.Draw(spriteBatch);
            lifebar.Draw(spriteBatch);
            clb.Draw(spriteBatch);

            if (phase == GamePhase.BossEnter)
            {
                if (blinks) { shim.Draw(spriteBatch, Vector2.Zero); }
            }
        }
        void DrawStillScreen()
        {
            bg.Draw(spriteBatch, Vector2.Zero);
        }
        void DrawPause()
        {
            DrawGame();
            pause.Draw(spriteBatch, Vector2.Zero);
        }
        protected override void Draw(GameTime gameTime)
        {          
            GraphicsDevice.SetRenderTarget(target);
            spriteBatch.Begin();
            if (state == GameState.Game) { DrawGame(); }
            if (state == GameState.Start || state == GameState.End || state == GameState.Loss) { DrawStillScreen(); }
            if (state == GameState.Pause) { DrawPause(); }
            transition.Draw(spriteBatch, Vector2.Zero);
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

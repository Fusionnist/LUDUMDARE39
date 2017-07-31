using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace LUDUMDARE39
{
    enum GameState { Start, Game, End, Loss, Pause, Htp}
    enum GamePhase { BossEnter, Fight, BossFall}
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        RenderTarget2D target, nextTarget, fuckingTarget, wowtarget;
        int scale, baseScale;
        Rectangle virtualDim, roomDim;
        CollisionStuff colman, nextColman;
        Lifebar lifebar, clb;

        STexture bg, transition;
        Input flippy, paused;

        float cityHp, blinktime, blinktimer, deadtimer;
        bool blinks, startedTransition;
        STexture start, loss, end, pause, shim, htp;
        double fallTime, fallTimer;
        int levelCount, levelCounter;

        GameState state; GamePhase phase;
        Player pl; Boss bl;

        float cityHpLoss, cityHpGain, bossHpLoss, bossHpGain;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            Window.IsBorderless = true;
            graphics.ApplyChanges();

            state = GameState.Start;
            UpdateGraphicsValues();

            flippy = new Input(Keys.Space);
            paused = new Input(Keys.P);

            blinktime = 0.5f;
            blinktimer = 0.5f;
            blinks = true;

            cityHpGain = 5f;
            cityHpLoss = 3.4f;
            bossHpGain = 1.5f;
            bossHpLoss = 5f;

            fallTime = 1;
            fallTimer = fallTime;

            ResetGame();

            LoadOneTime();
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
            nextTarget = new RenderTarget2D(GraphicsDevice, virtualDim.Width, virtualDim.Height);
            fuckingTarget = new RenderTarget2D(GraphicsDevice, virtualDim.Width, virtualDim.Height);
            wowtarget = new RenderTarget2D(GraphicsDevice, virtualDim.Width, virtualDim.Height);
            virtualDim.X = (int)((GraphicsDevice.Viewport.Width/scale- virtualDim.Width) /2);
            virtualDim.Y = (int)((GraphicsDevice.Viewport.Height/scale - virtualDim.Height) /2);
        }
        protected override void Initialize()
        {
            base.Initialize();        
        }
        void SetupRoom()
        {

        }
        void ResetGame()
        {
            phase = GamePhase.BossEnter;
            cityHp = 100;
            levelCount = 2;
            levelCounter = 1;
            deadtimer = 3;
            LoadContent();
        }
        CollisionStuff getColman(int number)
        {
            STexture[] bgs = new STexture[0];
            Switch[] switches = new Switch[0];
            if (number == 1)
            {
                bgs = new STexture[] {
                new STexture(Content.Load<Texture2D>("scene3hp"), new Rectangle(0, 0, 192, 108), "city3"),
                new STexture(Content.Load<Texture2D>("scene2hp"), new Rectangle(0, 0, 192, 108), "city2"),
                new STexture(Content.Load<Texture2D>("scene1hp"), new Rectangle(0, 0, 192, 108), "city1"),
            };
                switches = new Switch[] {
                new Switch(new STexture[]{
                new STexture(Content.Load<Texture2D>("switchon"), new Rectangle(0, 0, 8, 9), "switchon"),
                new STexture(Content.Load<Texture2D>("switchoff"), new Rectangle(0, 0, 8, 9), "switchoff") },
                new Vector2(12,19),
                new Plug[]{new Plug(new STexture[]{
                new STexture(Content.Load<Texture2D>("plug"), new Rectangle(0, 0, 8, 9), "switchon"),
                new STexture(Content.Load<Texture2D>("plugoff"), new Rectangle(0, 0, 8, 9), "switchoff") },
                new Vector2(10,83)) },
                Content.Load<SoundEffect>("switch")),

                new Switch(new STexture[]{
                new STexture(Content.Load<Texture2D>("switchon"), new Rectangle(0, 0, 8, 9), "switchon"),
                new STexture(Content.Load<Texture2D>("switchoff"), new Rectangle(0, 0, 8, 9), "switchoff") },
                new Vector2(92,19),
                new Plug[]{new Plug(new STexture[]{
                new STexture(Content.Load<Texture2D>("plug"), new Rectangle(0, 0, 11, 10), "switchon"),
                new STexture(Content.Load<Texture2D>("plugoff"), new Rectangle(0, 0, 11, 10), "switchoff") },
                new Vector2(90,83)) },
                Content.Load<SoundEffect>("switch")),

                new Switch(new STexture[]{
                new STexture(Content.Load<Texture2D>("switchon"), new Rectangle(0, 0, 8, 9), "switchon"),
                new STexture(Content.Load<Texture2D>("switchoff"), new Rectangle(0, 0, 8, 9), "switchoff") },
                new Vector2(172,19),
                new Plug[]{new Plug(new STexture[]{
                new STexture(Content.Load<Texture2D>("plug"), new Rectangle(0, 0, 11, 10), "switchon"),
                new STexture(Content.Load<Texture2D>("plugoff"), new Rectangle(0, 0, 11, 10), "switchoff") },
                new Vector2(170,83)) },
                Content.Load<SoundEffect>("switch"))
            }; }
            if (number == 2)
            {
                Plug plug1 = new Plug(new STexture[]{
                new STexture(Content.Load<Texture2D>("plug"), new Rectangle(0, 0, 8, 9), "switchon"),
                new STexture(Content.Load<Texture2D>("plugoff"), new Rectangle(0, 0, 8, 9), "switchoff") },
                new Vector2(13, 83));

                Plug plug2 = new Plug(new STexture[]{
                new STexture(Content.Load<Texture2D>("plug"), new Rectangle(0, 0, 8, 9), "switchon"),
                new STexture(Content.Load<Texture2D>("plugoff"), new Rectangle(0, 0, 8, 9), "switchoff") },
                new Vector2(52, 83));

                Plug plug3 = new Plug(new STexture[]{
                new STexture(Content.Load<Texture2D>("plug"), new Rectangle(0, 0, 8, 9), "switchon"),
                new STexture(Content.Load<Texture2D>("plugoff"), new Rectangle(0, 0, 8, 9), "switchoff") },
                new Vector2(156, 83));

                bgs = new STexture[] {
                new STexture(Content.Load<Texture2D>("uscene3"), new Rectangle(0, 0, 192, 108), "city3"),
                new STexture(Content.Load<Texture2D>("uscene2"), new Rectangle(0, 0, 192, 108), "city2"),
                new STexture(Content.Load<Texture2D>("uscene1"), new Rectangle(0, 0, 192, 108), "city1"),
            };
                switches = new Switch[] {
                new Switch(new STexture[]{
                new STexture(Content.Load<Texture2D>("switchon"), new Rectangle(0, 0, 8, 9), "switchon"),
                new STexture(Content.Load<Texture2D>("switchoff"), new Rectangle(0, 0, 8, 9), "switchoff") },
                new Vector2(15,17),
                new Plug[]{plug1, plug3},
                Content.Load<SoundEffect>("switch")),

                new Switch(new STexture[]{
                new STexture(Content.Load<Texture2D>("switchon"), new Rectangle(0, 0, 8, 9), "switchon"),
                new STexture(Content.Load<Texture2D>("switchoff"), new Rectangle(0, 0, 8, 9), "switchoff") },
                new Vector2(85,17),
                new Plug[]{plug2, plug3},
                Content.Load<SoundEffect>("switch")),

                new Switch(new STexture[]{
                new STexture(Content.Load<Texture2D>("switchon"), new Rectangle(0, 0, 8, 9), "switchon"),
                new STexture(Content.Load<Texture2D>("switchoff"), new Rectangle(0, 0, 8, 9), "switchoff") },
                new Vector2(167,17),
                new Plug[]{plug3},
                Content.Load<SoundEffect>("switch"))
            }; }
            return new CollisionStuff(pl, bl, switches, bgs);
        }
        void LoadOneTime()
        {
            shim = new STexture(Content.Load<Texture2D>("stophim"), Rectangle.Empty, "wow");
            start = new STexture(Content.Load<Texture2D>("titlescreen"), Rectangle.Empty, "wow");
            pause = new STexture(Content.Load<Texture2D>("pause"), Rectangle.Empty, "wow");
            loss = new STexture(Content.Load<Texture2D>("titlescreen"), Rectangle.Empty, "wow");
            end = new STexture(Content.Load<Texture2D>("titlescreen"), Rectangle.Empty, "wow");
            htp = new STexture(Content.Load<Texture2D>("htp"), Rectangle.Empty, "wow");

            transition = new STexture(Content.Load<Texture2D>("transition"), 10, 192, 0.1f, "test", Rectangle.Empty, false);
            transition.currentframe = 10;
        }
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Player player = new Player(
                new STexture[] {
                    new STexture(Content.Load<Texture2D>("flippyhitground"), 2, 16, 0.1f, "land", new Rectangle(0, 0, 16, 16), false),
                    new STexture(Content.Load<Texture2D>("flippyland"), 2, 16, 0.1f, "down", new Rectangle(0, 0, 16, 16), false),
                    new STexture(Content.Load<Texture2D>("flippyjump"), 2, 16, 0.1f, "up", new Rectangle(0, 0, 16, 16), false),
                    new STexture(Content.Load<Texture2D>("flippyrun"), 4, 16, 0.1f, "run", new Rectangle(0, 0, 16, 16), true)},
                new Vector2(40,88), Content.Load<SoundEffect>("jump"), Content.Load<SoundEffect>("hurt"));
            player.tex.isInverted = true;
            Boss boss = new Boss(
                new STexture[] {
                new STexture(Content.Load<Texture2D>("terry4"), 4, 32, 0.1f, "4", new Rectangle(0, 0, 32, 32), true),
                new STexture(Content.Load<Texture2D>("terry3"), 4, 32, 0.1f, "3", new Rectangle(0, 0, 32, 32), true),
                new STexture(Content.Load<Texture2D>("terry2"), 4, 32, 0.1f, "2", new Rectangle(0, 0, 32, 32), true),
                new STexture(Content.Load<Texture2D>("terry1"), 4, 32, 0.1f, "1", new Rectangle(0, 0, 32, 32), true),
                new STexture(Content.Load<Texture2D>("charge"), 4, 32, 0.1f, "charge", new Rectangle(0, 0, 32, 32), true),
                new STexture(Content.Load<Texture2D>("ded"), 4, 32, 0.1f, "dead", new Rectangle(0, 0, 32, 32), true)}, 
                new Vector2(90, 72), 
                new STexture[2] {
                new STexture(Content.Load<Texture2D>("bullet"), 6, 8, 0.1f, "bullet", new Rectangle(3, 3, 2, 2), true),
                new STexture(Content.Load<Texture2D>("splode"), 6, 8, 0.1f, "explosion", new Rectangle(1, 1, 6, 6), false) },
                Content.Load<SoundEffect>("bossshot"),
                new STexture(Content.Load<Texture2D>("plugger"), Rectangle.Empty, "wow"),
                new STexture(Content.Load<Texture2D>("splodey"), 9, 16, 0.05f, "4", new Rectangle(0, 0, 32, 32), false));
            pl = player;
            bl = boss;

            colman = getColman(levelCounter);

            bg = colman.bgs[0];


            lifebar = new Lifebar(new STexture[2] { new STexture(Content.Load<Texture2D>("barcont"), new Rectangle(0, 0, 22, 5), "barcontainer"), new STexture(Content.Load<Texture2D>("barint"), new Rectangle(0, 0, 20, 3), "barinterior") }, new Vector2(1, 1), colman.boss.maxhp);
            clb = new Lifebar(new STexture[2] { new STexture(Content.Load<Texture2D>("barcont"), new Rectangle(0, 0, 22, 5), "barcontainer"), new STexture(Content.Load<Texture2D>("barint"), new Rectangle(0, 0, 20, 3), "barinterior") }, new Vector2(152, 1),100);
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
                            colman.boss.tex.Update(gameTime);
                            if (colman.boss.isPlugged) { phase = GamePhase.Fight; scale = baseScale; }
                            break;
                        case GamePhase.BossFall:
                            fallTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                            if(fallTimer <= 0) {
                                colman = nextColman;
                                nextColman = getColman(1);                                
                                fallTimer = fallTime;
                                colman.boss.hp = colman.boss.maxhp;
                                cityHp = 100;
                                phase = GamePhase.Fight;                             
                            }
                            break;
                        case GamePhase.Fight:
                            foreach (var bullet in colman.boss.bullets)
                                bullet.Update(gameTime, roomDim);

                            int actSwitches = 0;
                            foreach (var switc in colman.switches)
                            {
                                if (switc.isOn)
                                    actSwitches++;
                            }
                            //hp
                            if (colman.boss.isPlugged) {
                                colman.boss.hp += bossHpGain * (float)gameTime.ElapsedGameTime.TotalSeconds * actSwitches;
                                cityHp -= cityHpLoss * (float)gameTime.ElapsedGameTime.TotalSeconds * actSwitches;
                            }
                            else {
                                colman.boss.hp -= bossHpLoss * (float)gameTime.ElapsedGameTime.TotalSeconds;
                                cityHp += cityHpGain * (float)gameTime.ElapsedGameTime.TotalSeconds * actSwitches;
                                if(colman.boss.hp > 0)
                                    cityHp -= cityHpLoss * (float)gameTime.ElapsedGameTime.TotalSeconds * (3-actSwitches);
                            }
                            if (cityHp > 100) { cityHp = 100; }
                            if (colman.boss.hp > 100) { colman.boss.hp = 100; }

                            if (cityHp <= 100) { colman.bg = colman.bgs[0]; }
                            if (cityHp <= 60) { colman.bg = colman.bgs[1]; }
                            if (cityHp <= 30) { colman.bg = colman.bgs[2]; }
                            if (cityHp <= 0) {
                                if (!startedTransition) { startedTransition = true; transition.Reset(); }
                                else if(transition.currentframe >= 4) { startedTransition = false; state = GameState.Loss; bg = loss; }

                            } //die
                            if (colman.boss.hp <= 0) {
                                if (deadtimer <= 0)
                                {
                                    
                                    if (levelCounter < levelCount)
                                    {
                                        deadtimer = 3;
                                        phase = GamePhase.BossFall; levelCounter++; nextColman = getColman(levelCounter);
                                    }
                                    else
                                    {
                                        if (!startedTransition) { startedTransition = true; transition.Reset(); }
                                        else if (transition.currentframe >= 4) { startedTransition = false; deadtimer = 3; state = GameState.End; bg = end; }
                                    }
                                }
                                else { deadtimer -= (float)gameTime.ElapsedGameTime.TotalSeconds; }
                            }
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
                    if (transition.currentframe >= 4 && startedTransition) { state = GameState.Htp; bg = htp; startedTransition = false; }
                    break;
                case GameState.Htp:

                    if (flippy.IsPressed() && !startedTransition) { transition.Reset(); startedTransition = true; }
                    if (transition.currentframe >= 4 && startedTransition) { state = GameState.Game; ResetGame(); startedTransition = false; }
                    break;
                case (GameState.Pause):
                    if (paused.IsPressed()) { state = GameState.Game; }
                    break;
            }            
        }

        void DrawGame()
        {            
            colman.DrawOnlyScene(spriteBatch);
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
            spriteBatch.Begin(sortMode: SpriteSortMode.Immediate);
            if (state == GameState.Game) { DrawGame(); }
            if (state == GameState.Start || state == GameState.End || state == GameState.Loss || state == GameState.Htp) { DrawStillScreen(); }
            if (state == GameState.Pause) { DrawPause(); }
            
            spriteBatch.End();

            GraphicsDevice.SetRenderTarget(nextTarget);
            spriteBatch.Begin();
            if (nextColman != null) {
                nextColman.DrawOnlyScene(spriteBatch);
            }
            spriteBatch.End();

            Matrix m = Matrix.CreateTranslation(new Vector3(0, (108 * (float)((fallTimer - fallTime)/fallTime)), 0));
            GraphicsDevice.SetRenderTarget(fuckingTarget);
            spriteBatch.Begin(transformMatrix:m);
            spriteBatch.Draw(texture: target,position:Vector2.Zero);
            Rectangle r = new Rectangle(0, 108, 192, 108);
            spriteBatch.Draw(texture: nextTarget, destinationRectangle: r);
            spriteBatch.End();

            GraphicsDevice.SetRenderTarget(wowtarget);
            GraphicsDevice.Clear(Color.TransparentBlack);
            spriteBatch.Begin();
            if (state == GameState.Game)
                colman.Draw(spriteBatch);
            transition.Draw(spriteBatch, Vector2.Zero);
            spriteBatch.End();

            m = Matrix.CreateScale(scale);
            GraphicsDevice.SetRenderTarget(null);
            spriteBatch.Begin(transformMatrix: m,samplerState:SamplerState.PointWrap);
            spriteBatch.Draw(fuckingTarget, destinationRectangle: virtualDim);
            spriteBatch.Draw(wowtarget, destinationRectangle: virtualDim);
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}

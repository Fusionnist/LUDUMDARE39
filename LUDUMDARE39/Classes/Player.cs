using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace LUDUMDARE39
{
    class Player : Sprite
    {
        public Vector2 mov;
        public float Yvel;
        public bool isOnGround, isOnBoss;

        public Player(STexture[] a_tex, Vector2 a_pos): base(a_tex, a_pos)
        {
            mov = Vector2.Zero;
            Yvel = 0;
            isOnGround = true;
            isOnBoss = false;
        }

        public void Update(GameTime a_gt, Rectangle virtualDims)
        {            
            pos += mov;
            mov = Vector2.Zero;
            if (pos.X > virtualDims.Width - tex.framelength)
                pos.X = virtualDims.Width - tex.framelength;
            if (pos.X < 0)
                pos.X = 0;
            if (pos.Y > virtualDims.Height - tex.frameheight)
                pos.Y = virtualDims.Height - tex.frameheight;
            if (pos.Y < 0)
                pos.Y = 0;

            base.Update(a_gt);
        }

        public void Move(GameTime a_gt, Rectangle virtualDims)
        {
            KeyboardState ks = Keyboard.GetState();


            if (ks.IsKeyDown(Keys.Left))
                mov.X -= 100 * (float)a_gt.ElapsedGameTime.TotalSeconds;
            if (ks.IsKeyDown(Keys.Right))
                mov.X += 100 * (float)a_gt.ElapsedGameTime.TotalSeconds;

            if (pos.Y >= virtualDims.Height - tex.frameheight)
                isOnGround = true;

            if (isOnGround)
            {
                if (ks.IsKeyDown(Keys.Up))
                {
                    if (ks.IsKeyDown(Keys.Left) || ks.IsKeyDown(Keys.Right))
                        Yvel = 4;
                    else
                        Yvel = 5.5f;
                    isOnGround = false;
                }
                else
                    Yvel = 0;
            }
            if (pos.Y < virtualDims.Height - tex.frameheight)
                Yvel -= 10 * (float)a_gt.ElapsedGameTime.TotalSeconds;
            mov.Y -= Yvel;
        }
    }
}

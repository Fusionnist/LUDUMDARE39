using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

namespace LUDUMDARE39
{
    class Player : Sprite
    {
        bool inverts;
        public Vector2 mov;
        public float Yvel, stunTimer, stunTime;
        public bool isOnGround, isOnBoss, isInHighJump, isStunned;
        public SoundEffect jump, hurt;

        public Player(STexture[] a_tex, Vector2 a_pos, SoundEffect a_j, SoundEffect a_h): base(a_tex, a_pos)
        {
            mov = Vector2.Zero;
            Yvel = 0;
            isOnGround = true;
            isOnBoss = false;
            isInHighJump = false;
            isStunned = false;
            stunTime = 1;
            stunTimer = stunTime;
            jump = a_j;
            hurt = a_h;
        }

        public void Update(GameTime a_gt, Rectangle virtualDims)
        {            
            pos += mov;
            //selecting texture
            if(Yvel > 0 && !isOnGround)
            { SelectTexture("up"); }
            if(Yvel < 0 && !isOnGround)
            { SelectTexture("down"); }
            if(isOnGround || isOnBoss && mov.X == 0)
            { SelectTexture("land"); }
            if(mov.X != 0 && isOnGround)
            {
                SelectTexture("run");
            }
            mov = Vector2.Zero;
            if (GetHB().X > virtualDims.Width + virtualDims.X - GetHB().Width)
                pos.X = virtualDims.Width + virtualDims.X - GetHB().Width;
            if (GetHB().X < virtualDims.X)
                pos.X = virtualDims.X - tex.hb.X;
            if (GetHB().Y + GetHB().Height > virtualDims.Height + virtualDims.Y )
                pos.Y = virtualDims.Height + virtualDims.Y - GetHB().Height;
            if (GetHB().Y < virtualDims.Y)
                pos.Y = virtualDims.Y;

            if (isStunned)
            {
                stunTimer -= (float)a_gt.ElapsedGameTime.TotalSeconds;
                if (stunTimer <= 0)
                { stunTimer = stunTime; isStunned = false; }
            }

            base.Update(a_gt);
        }

        public void Move(GameTime a_gt, Rectangle virtualDims)
        {
            KeyboardState ks = Keyboard.GetState();


            if (!isStunned)
            {
                if (ks.IsKeyDown(Keys.Left))
                { mov.X -= 100 * (float)a_gt.ElapsedGameTime.TotalSeconds; inverts = false; }
                if (ks.IsKeyDown(Keys.Right))
                { mov.X += 100 * (float)a_gt.ElapsedGameTime.TotalSeconds; inverts = true; }
                if (isInHighJump)
                    mov.X /= 2;
            }
            tex.isInverted = inverts;

            if (GetHB().Y >= virtualDims.Height + virtualDims.Y - GetHB().Height)
                isOnGround = true;
            if (isOnGround)
            {
                isInHighJump = false;
                if (ks.IsKeyDown(Keys.Up) && !isStunned)
                {
                    if (ks.IsKeyDown(Keys.Left) || ks.IsKeyDown(Keys.Right))
                        Yvel = 4;
                    else
                    { Yvel = 5.5f; isInHighJump = true; }
                    isOnGround = false;
                    jump.Play();
                }
                else
                { Yvel = 0;}
            }
            if (!isOnGround)
                Yvel -= 10 * (float)a_gt.ElapsedGameTime.TotalSeconds;
            mov.Y -= Yvel;
        }
    }
}

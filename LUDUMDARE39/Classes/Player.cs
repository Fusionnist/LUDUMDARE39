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
        Vector2 mov;
        float Yvel;

        public Player(STexture a_tex, Vector2 a_pos): base(a_tex, a_pos)
        {
            mov = Vector2.Zero;
            Yvel = 0;
        }

        public override void Update(GameTime a_gt)
        {
            Move(a_gt);
            pos += mov;
            mov = Vector2.Zero;
            if (pos.X > 176)
                pos.X = 176;
            if (pos.X < 0)
                pos.X = 0;

            base.Update(a_gt);
        }

        public void Move(GameTime a_gt)
        {
            KeyboardState ks = Keyboard.GetState();


            if (ks.IsKeyDown(Keys.Left))
                mov.X -= 400 * (float)a_gt.ElapsedGameTime.TotalSeconds;
            if (ks.IsKeyDown(Keys.Right))
                mov.X += 400 * (float)a_gt.ElapsedGameTime.TotalSeconds;


            if (pos.Y < 0)
                pos.Y = 0;
            if (pos.Y > 102)
                pos.Y = 102;
            if (pos.Y == 102)
            {
                if (ks.IsKeyDown(Keys.Up))
                    Yvel = 15;
                else
                    Yvel = 0;
            }
            if (pos.Y < 102)
                Yvel -= 10 * (float)a_gt.ElapsedGameTime.TotalSeconds;
            mov.Y -= Yvel;
        }
    }
}

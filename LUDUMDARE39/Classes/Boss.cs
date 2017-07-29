using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using static LUDUMDARE39.Enums;

namespace LUDUMDARE39
{
    class Boss : Sprite
    {
        public Vector2 platformStart, platformEnd, mov;
        public BossMovement movType;

        public Boss(STexture[] a_tex, Vector2 a_pos): base(a_tex, a_pos)
        {
            platformStart = a_pos;
            platformEnd = new Vector2(a_pos.X + a_tex[0].framelength, a_pos.Y);
            movType = BossMovement.Right;
        }

        public void Move(GameTime a_gt)
        {
            if (movType == BossMovement.Left)
                mov.X -= 50 * (float)a_gt.ElapsedGameTime.TotalSeconds;
            else if (movType == BossMovement.Right)
                mov.X += 50 * (float)a_gt.ElapsedGameTime.TotalSeconds;
        }

        public void Update(GameTime a_gt, Rectangle virtualDims)
        {
            pos += mov;
            mov = Vector2.Zero;
            if (pos.X > virtualDims.Width - tex.framelength)
            { pos.X = virtualDims.Width - tex.framelength; movType = BossMovement.Left; }
            if (pos.X < 0)
            { pos.X = 0; movType = BossMovement.Right; }
            if (pos.Y > virtualDims.Height - tex.frameheight)
                pos.Y = virtualDims.Height - tex.frameheight;
            if (pos.Y < 0)
                pos.Y = 0;

            platformStart = pos;
            platformEnd = new Vector2(pos.X + texes[0].framelength, pos.Y);

            base.Update(a_gt);
        }
    }
}

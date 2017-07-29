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
            movType = BossMovement.Idle;
        }

        public void Move(GameTime a_gt)
        {
            if (movType == BossMovement.Left)
                mov.X -= 50 * (float)a_gt.ElapsedGameTime.TotalSeconds;
            else if (movType == BossMovement.Right)
                mov.X -= 50 * (float)a_gt.ElapsedGameTime.TotalSeconds;
        }
    }
}

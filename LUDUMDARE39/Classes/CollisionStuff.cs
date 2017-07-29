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
    class CollisionStuff
    {
        public Player player { get; set; }
        public Boss boss { get; set; }

        public CollisionStuff(Player a_pl, Boss a_bs)
        {
            player = a_pl;
            boss = a_bs;
        }

        public void Update(GameTime a_gt, Rectangle virtualdims)
        {
            player.Move(a_gt, virtualdims);
            player.Update(a_gt, virtualdims);
            boss.Update(a_gt);
        }

        public void CheckPlayerOnBoss()
        {
            if (player.mov.Y > 0 && player.pos.X + player.mov.X > boss.platformStart - player.tex)
        }
    }
}

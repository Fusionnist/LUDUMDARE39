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
        public Switch[] switches { get; set; }

        public CollisionStuff(Player a_pl, Boss a_bs, Switch[] a_sws)
        {
            player = a_pl;
            boss = a_bs;
            switches = a_sws;
        }

        public void Update(GameTime a_gt, Rectangle virtualdims, Input input)
        {
            player.Move(a_gt, virtualdims);
            boss.Move(a_gt);
            CheckPlayerOnBoss();
            player.Update(a_gt, virtualdims);
            boss.Update(a_gt, virtualdims, switches, player.pos);
            BossPushPlayer();
            foreach (var switc in switches)
                switc.Update(a_gt);
            FlipSwitches(input);
        }

        public void CheckPlayerOnBoss()
        {
            if (player.mov.Y > 0 && player.pos.X + player.mov.X > boss.platformStart.X - player.tex.framelength && player.pos.X + player.mov.X < boss.platformEnd.X && player.pos.Y + 16 <= boss.platformStart.Y && player.pos.Y + 16 + player.mov.Y > boss.platformStart.Y)
            {
                player.Yvel = 0;
                player.mov.Y = 0;
                player.isOnGround = true;
                player.isOnBoss = true;
                player.pos.Y = boss.platformStart.Y - 16;
                player.mov.X += boss.mov.X;
            }
            else
            {
                if (player.isOnBoss)
                {
                    player.isOnBoss = false;
                    player.isOnGround = false;
                }
            }
        }

        public void FlipSwitches(Input input)
        {
            foreach (var sw in switches)
            {
                if (sw.truehb.Intersects(player.truehb) && input.IsPressed())
                    sw.Activate();
            }
        }

        public void BossPushPlayer()
        {
            if (player.pos.X < boss.pos.X && player.pos.X > boss.pos.X - 16 && player.pos.Y > boss.pos.Y - 16)
                player.pos.X = boss.pos.X - 16;
            if (player.pos.X > boss.pos.X && player.pos.X < boss.pos.X + 16 && player.pos.Y > boss.pos.Y - 16)
                player.pos.X = boss.pos.X + 16;
        }
    }
}

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
        public STexture[] bgs; public STexture bg;
        public CollisionStuff(Player a_pl, Boss a_bs, Switch[] a_sws, STexture[] bgs_)
        {
            bgs = bgs_;
            player = a_pl;
            boss = a_bs;
            switches = a_sws;
            bg = bgs[0];
        }

        public void Update(GameTime a_gt, Rectangle virtualdims, Input input)
        {
            player.Move(a_gt, virtualdims);
            CheckPlayerOnBoss();
            player.Update(a_gt, virtualdims);
            BulletCollisions();
            boss.Update(a_gt, virtualdims, switches, player.pos);
            BossPushPlayer();
            foreach (var switc in switches)
                switc.Update(a_gt);
            FlipSwitches(input);
        }
        public void CheckPlayerOnBoss()
        {
            if (player.mov.Y > 0 && player.GetHB().X + player.mov.X > boss.platformStart.X - player.GetHB().Width && player.GetHB().X + player.mov.X < boss.platformEnd.X && player.GetHB().Y + player.GetHB().Height <= boss.platformStart.Y && player.GetHB().Y + player.GetHB().Height + player.mov.Y > boss.platformStart.Y)
            {
                player.Yvel = 0;
                player.mov.Y = 0;
                player.isOnGround = true;
                player.isOnBoss = true;
                player.pos.Y = boss.platformStart.Y - player.GetHB().Height;
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
                if (sw.GetHB().Intersects(player.GetHB()) && input.IsPressed())
                    sw.Activate();
            }
        }

        public void Draw(SpriteBatch sb_)
        {
            player.Draw(sb_);
            boss.Draw(sb_);
            
            foreach(Bullet b in boss.bullets)
            {
                b.Draw(sb_);
            }

        }
        public void DrawOnlyScene(SpriteBatch sb_)
        {
            bg.Draw(sb_, Vector2.Zero);
            foreach (Switch s in switches)
            {
                s.Draw(sb_);
            }
        }

        public void BossPushPlayer()
        {
            if (player.GetHB().X < boss.GetHB().X && player.GetHB().X + player.GetHB().Width > boss.GetHB().X && player.GetHB().Y > boss.GetHB().Y - player.GetHB().Height)
                player.pos.X = boss.GetHB().X - player.GetHB().Width - player.tex.hb.X;
            if (player.GetHB().X > boss.GetHB().X && player.GetHB().X < boss.GetHB().X + boss.GetHB().Width && player.GetHB().Y > boss.GetHB().Y - player.GetHB().Height)
                player.pos.X = boss.GetHB().X + boss.GetHB().Width - player.tex.hb.X;
        }

        public void BulletCollisions()
        {
            foreach(var bullet in boss.bullets)
            {
                if (bullet.GetHB().Intersects(player.GetHB())&& !bullet.isExploding)
                { bullet.bounces = 0; player.isStunned = true; player.Yvel = 1; player.hurt.Play(); }
            }
        }
    }
}

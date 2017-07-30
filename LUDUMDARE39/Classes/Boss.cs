using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using static LUDUMDARE39.Enums;

namespace LUDUMDARE39
{
    class Boss : Sprite
    {
        float shotTime, shotTimer;
        public Vector2 platformStart, platformEnd, mov, plugDist;
        public BossMovement movType;
        public bool isPlugged, isNearPlug;
        Vector2 nearestPlug;
        SoundEffect shotSound;
        public float hp, maxhp;
        public List<Bullet> bullets;
        public Bullet[][] bullettypes;
        STexture[] bullettexes;

        public Boss(STexture[] a_tex, Vector2 a_pos, STexture[] a_bullettexes, SoundEffect a_ss) : base(a_tex, a_pos)
        {
            SetPlatformData();
            movType = BossMovement.Right;
            hp = 100;
            maxhp = 100;
            shotSound = a_ss;
            shotTimer = 1;
            shotTime = 1;
            isPlugged = false;
            plugDist = new Vector2(8);
            bullets = new List<Bullet> { };
            bullettexes = a_bullettexes;
            SetBulletData();
        }
        void SetPlatformData()
        {
            platformStart = new Vector2(GetHB().X, GetHB().Y);
            platformEnd = new Vector2(GetHB().X + tex.framelength, GetHB().Y);
        }

        void SetBulletData()
        {
            Random rng = new Random();
            STexture[] ts = new STexture[] { bullettexes[0].Clone(), bullettexes[1].Clone() };
            bullettypes = new Bullet[][]
            {
                new Bullet[] { new Bullet(ts, new Vector2(GetHB().X, GetHB().Y) + new Vector2(0, -16), 1, -1, false, new Vector2(50, 0), Vector2.Zero, new Point(rng.Next(0, 2), -1), false, 100, null)},
                new Bullet[] { new Bullet(ts, new Vector2(GetHB().X, GetHB().Y) + new Vector2(0, -16), 3, -1, false, new Vector2(50, 0), Vector2.Zero, new Point(rng.Next(0, 2), 1), true, 100, null) },
                new Bullet[] { new Bullet(ts, new Vector2(GetHB().X, GetHB().Y) + new Vector2(0, -16), 10, -1, false, new Vector2(50, 50), new Vector2(0, 50), new Point(rng.Next(0, 2), -1), true, 100, null) },
                new Bullet[] { new Bullet(ts, new Vector2(GetHB().X, GetHB().Y) + new Vector2(0, -16), 1000, 5, true, new Vector2(50, 50), new Vector2(0, 50), new Point(rng.Next(0, 2), -1), true, 100, null) },
                new Bullet[]
                {
                    new Bullet(ts, new Vector2(GetHB().X, GetHB().Y) + new Vector2(0, -16), 3, -1, false, new Vector2(50, 50), new Vector2(0, 50), new Point(rng.Next(0, 2), -1), true, 100, (float)(rng.NextDouble() * Math.PI / 2)),
                    new Bullet(ts, new Vector2(GetHB().X, GetHB().Y) + new Vector2(0, -16), 3, -1, false, new Vector2(50, 50), new Vector2(0, 50), new Point(rng.Next(0, 2), -1), true, 100, (float)(rng.NextDouble() * Math.PI / 2)),
                    new Bullet(ts, new Vector2(GetHB().X, GetHB().Y) + new Vector2(0, -16), 3, -1, false, new Vector2(50, 50), new Vector2(0, 50), new Point(rng.Next(0, 2), -1), true, 100, (float)(rng.NextDouble() * Math.PI / 2))
                },
                new Bullet[] { new Bullet(ts, new Vector2(GetHB().X, GetHB().Y) + new Vector2(0, -16), 5, -1, false, new Vector2(50, 30), Vector2.Zero, new Point(rng.Next(0, 2), -1), true, 100, null) }
            };
        }

        void SeekPlug(GameTime a_gt)
        {
            if (isNearPlug)
            {
                if (nearestPlug.X > GetHB().X)
                { mov.X += 10 * (float)a_gt.ElapsedGameTime.TotalSeconds; tex.isInverted = true; }//temp
                if (nearestPlug.X < GetHB().X)
                { mov.X += -10 * (float)a_gt.ElapsedGameTime.TotalSeconds; tex.isInverted = false; }//temp
            }
        }
        void Shoot(Vector2 playerPos)
        {
            int rng = new Random().Next(0, bullettypes.Length);
            foreach (var bullet in bullettypes[rng])
                bullets.Add(bullet.Clone());
            if (rng == 2) { bullets[bullets.Count - 1].CalculateTarget(playerPos, 2f); }
            if (rng == 1)
            { }
            shotSound.Play();
        }
        public void Pluggg(Switch[] switches_, GameTime a_gt)
        {
            float record = 1000;
            foreach (Switch s in switches_)
            {
                if (s.isOn)
                {
                    if ((Vector2.Distance(new Vector2(GetHB().X, GetHB().Y), new Vector2(s.plug.GetHB().X, s.plug.GetHB().Y))) < record)
                    {
                        isNearPlug = true;
                        record = Vector2.Distance(new Vector2(GetHB().X, GetHB().Y), new Vector2(s.plug.GetHB().X, s.plug.GetHB().Y));
                        nearestPlug = new Vector2(s.plug.GetHB().X, s.plug.GetHB().Y);
                        if (nearestPlug.X - (GetHB().X + plugDist.X) < 1 && nearestPlug.X - (GetHB().X + plugDist.X) > -1)
                        { pos.X = nearestPlug.X - plugDist.X; isPlugged = true; }
                    }
                }
            }
            if (!isPlugged) { SeekPlug(a_gt); }
            pos += mov;
            if(mov.X > 0) { plugDist.X = 8 ; } //settung plugdist based on mov
            if (mov.X < 0) { plugDist.X = -8; }
            mov = Vector2.Zero;
        }
        public void Update(GameTime a_gt, Rectangle virtualDims, Switch[] switches_, Vector2 playerpos_)
        {
            isNearPlug = false;
            isPlugged = false;
            shotTimer -= (float)a_gt.ElapsedGameTime.TotalSeconds;
            if (shotTimer <= 0)
            {
                shotTimer = shotTime; Shoot(playerpos_);
            }
            Pluggg(switches_, a_gt);
            if (!isPlugged)
            { hp -= 5 * (float)a_gt.ElapsedGameTime.TotalSeconds;  }
            else
            {
                hp += 8 * (float)a_gt.ElapsedGameTime.TotalSeconds;
                if (hp > 100)
                    hp = 100;
            }
            SetPlatformData();
            SetBulletData();

            for (int x = bullets.Count - 1; x >= 0; x--)
            {
                if (bullets[x].isDead)
                    bullets.Remove(bullets[x]);
            }

            base.Update(a_gt);
        }
    }
    }

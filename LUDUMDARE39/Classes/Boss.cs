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
        float movx;

        public Boss(STexture[] a_tex, Vector2 a_pos, STexture[] a_bullettexes, SoundEffect a_ss) : base(a_tex, a_pos)
        {
            SetPlatformData();
            movType = BossMovement.Right;
            hp = 100;
            maxhp = 100;
            shotSound = a_ss;
            shotTimer = 1;
            shotTime = 1.5f;
            isPlugged = false;
            plugDist = new Vector2(8);
            bullets = new List<Bullet> { };
            bullettexes = a_bullettexes;
            SetBulletData();
            movx = 25;
        }
        void SetPlatformData()
        {
            platformStart = new Vector2(GetHB().X, GetHB().Y);
            platformEnd = new Vector2(GetHB().X + tex.framelength, GetHB().Y);
        }

        void SetBulletData()
        {
            Random rng = new Random();
           

        }

        void SeekPlug(GameTime a_gt)
        {
            if (isNearPlug)
            {
                if (nearestPlug.X > GetHB().X)
                { mov.X += movx * (float)a_gt.ElapsedGameTime.TotalSeconds; tex.isInverted = true; }//temp
                if (nearestPlug.X < GetHB().X)
                { mov.X += -movx * (float)a_gt.ElapsedGameTime.TotalSeconds; tex.isInverted = false; }//temp
            }
        }
        Bullet[] GetBullets(Vector2 playerpos)
        {
            STexture[] ts = new STexture[] { bullettexes[0].Clone(), bullettexes[1].Clone() };
            Random r = new Random();
            int c = r.Next(1, 6);
            List<Bullet> bs = new List<Bullet>();
            for(int xx = 0; xx<c; xx++)
            {
                int bounces = r.Next(4, 20);
                bool bouncies = false; if (r.Next(0, 2) == 0) { bouncies = true; }
                float lifetime = r.Next(0, 10);
                bool life = false; if (r.Next(0, 2) == 0) { life = true; }
                Vector2 velocity = new Vector2(r.Next(60, 120), r.Next(60, 120));
                Vector2 loss = new Vector2(r.Next(0, 40), r.Next(0, 40));
                bool xi = false; if (r.Next(0, 2) == 0) { xi = true; }
                int x = 1;
                if (xi) { x = -1; }
                bool angled = false; if (r.Next(0, 2) == 0) { angled = true; }
                float? angle = (float)(r.NextDouble() * (Math.PI / 2)); if (angled) { angle = null; }
                bs.Add(new Bullet(ts, pos, bounces, lifetime, life, velocity, loss, new Point(x, -1), bouncies, 0, angle));
            }
            return bs.ToArray();
        }
        void Shoot(Vector2 playerPos)
        {
            
            foreach (var bullet in GetBullets(playerPos))
                bullets.Add(bullet.Clone());
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

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
        STexture plugTex;
        float movx;
        STexture splodey;
        Vector2 shotPos;
        bool inverts;

        public Boss(STexture[] a_tex, Vector2 a_pos, STexture[] a_bullettexes, SoundEffect a_ss, STexture plugTex_, STexture splodey_) : base(a_tex, a_pos)
        {
            splodey = splodey_;
            splodey.currentframe = 9;
            plugTex = plugTex_;
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
            movx = 22;
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
                if (nearestPlug.X + plugDist.X >= GetHB().X)
                { mov.X += movx * (float)a_gt.ElapsedGameTime.TotalSeconds; inverts = false; }//temp
                if (nearestPlug.X + plugDist.X < GetHB().X)
                { mov.X += -movx * (float)a_gt.ElapsedGameTime.TotalSeconds; inverts = true; }//temp
            }
        }
        Bullet[] GetBullets(Vector2 playerpos)
        {
            STexture[] ts = new STexture[] { bullettexes[0].Clone(), bullettexes[1].Clone() };
            Random r = new Random();
            int c = r.Next(1, 6);
            List<Bullet> bs = new List<Bullet>();
            for (int xx = 0; xx < c; xx++)
            {
                int bounces = r.Next(4, 10);
                bool bouncies = false; if (r.Next(0, 2) == 0) { bouncies = true; }
                float lifetime = r.Next(3, 5);
                bool life = false; if (r.Next(0, 2) == 0) { life = true; }
                Vector2 velocity = new Vector2(r.Next(60, 100), r.Next(60, 100));
                Vector2 loss = new Vector2(r.Next(0, 100), r.Next(0, 100));
                int x = 1;
                if (tex.isInverted) { x = -1; }
                bool angled = false; if (r.Next(0, 2) == 0) { angled = true; }
                float? angle = (float)(r.NextDouble() * (Math.PI / 2) - Math.PI / 4); if (angled) { angle = null; }


                bs.Add(new Bullet(ts, shotPos, bounces, lifetime, life, velocity, loss, new Point(x, -1), bouncies, 0, angle));
            }
            return bs.ToArray();
        }
        public override void Draw(SpriteBatch a_sb)
        {
            tex.isInverted = inverts;
            if (!isNearPlug && hp > 0) { tex.currentframe = 2; }
            if (isPlugged)
            {
                Vector2 plugPos = new Vector2(32, 0);
                if (tex.isInverted) { plugPos.X = -12; }
                plugTex.isInverted = tex.isInverted;
                plugTex.Draw(a_sb, pos + plugPos);
            }
            base.Draw(a_sb);
            if(splodey.currentframe != 9)
            {
                splodey.Draw(a_sb, shotPos); //temp
            }

        }
        void Shoot(Vector2 playerPos)
        {
            splodey.Reset();
            foreach (var bullet in GetBullets(playerPos))
                bullets.Add(bullet.Clone());
            shotSound.Play();
        }
        public void Pluggg(Switch[] switches_, GameTime a_gt)
        {
            float record = 1000;
            foreach (Switch s in switches_)
            {
                foreach (Plug plug in s.plugs)
                {
                    if (plug.isOn)
                    {
                        if ((Vector2.Distance(new Vector2(GetHB().X, GetHB().Y), new Vector2(plug.GetHB().X, plug.GetHB().Y))) < record)
                        {
                            isNearPlug = true;
                            record = Vector2.Distance(new Vector2(GetHB().X, GetHB().Y), new Vector2(plug.GetHB().X, plug.GetHB().Y));
                            nearestPlug = new Vector2(plug.GetHB().X, plug.GetHB().Y);
                            if (nearestPlug.X - (GetHB().X + plugDist.X) < 1 && nearestPlug.X - (GetHB().X + plugDist.X) > -1)
                            { pos.X = nearestPlug.X - plugDist.X; isPlugged = true; }
                        }
                    }
                }
            }
            if (!isPlugged) { SeekPlug(a_gt); }
            pos += mov;
            if (mov.X > 0) { plugDist.X = 36; } //setting plugdist based on mov
            if (mov.X < 0) { plugDist.X = -12; }
            mov = Vector2.Zero;
        }
        public void Update(GameTime a_gt, Rectangle virtualDims, Switch[] switches_, Vector2 playerpos_)
        {
            shotPos = new Vector2(pos.X + 24, pos.Y - 2);
            if (tex.isInverted) { shotPos = new Vector2(pos.X - 8, pos.Y - 2); }
            splodey.Update(a_gt);
            if (hp <= 0)
        { SelectTexture("dead"); }
            else if (!isPlugged)
            {
                if (hp <= 20) { SelectTexture("1"); }
                else if (hp <= 40) { SelectTexture("2"); }
                else if (hp <= 70) { SelectTexture("3"); }
                else if (hp <= 100) { SelectTexture("4"); }
            }
            else
            {
                SelectTexture("charge");
            }
            isNearPlug = false;
            isPlugged = false;

            if (hp > 0)
            {
                SetBulletData();
                shotTimer -= (float)a_gt.ElapsedGameTime.TotalSeconds;
                if (shotTimer <= 0)
                {
                    shotTimer = shotTime; Shoot(playerpos_);
                }
                Pluggg(switches_, a_gt);
            }


            SetPlatformData();


            for (int x = bullets.Count - 1; x >= 0; x--)
            {
                if (bullets[x].isDead)
                    bullets.Remove(bullets[x]);
            }

            base.Update(a_gt);
        }
    }
}

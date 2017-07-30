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
        float shotTime, shotTimer;
        public Vector2 platformStart, platformEnd, mov, plugDist;
        public BossMovement movType;
        public bool isPlugged, isNearPlug;
        Vector2 nearestPlug;
        public float hp, maxhp;
        public List<Bullet> bullets;
        public Bullet[] bullettypes;
        STexture[] bullettexes;

        public Boss(STexture[] a_tex, Vector2 a_pos, STexture[] a_bullettexes) : base(a_tex, a_pos)
        {
            SetPlatformData();
            movType = BossMovement.Right;
            hp = 100;
            maxhp = 100;

            shotTimer = 1;
            shotTime = 2;
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
            STexture[] ts = new STexture[] { bullettexes[0].Clone() };
            int rndX = new Random().Next(0, 2);
            int rndY = new Random().Next(0, 2);
            if (rndX == 0) { rndX = -1; }
            if (rndY == 0) { rndY = -1; }
            bullettypes = new Bullet[] 
            {
                new Bullet(ts, new Vector2(GetHB().X, GetHB().Y) + new Vector2(0, -16), 1, -1, false, new Vector2(50, 0), Vector2.Zero, new Point(rndX, -1), false, 100),
                new Bullet(ts, new Vector2(GetHB().X, GetHB().Y) + new Vector2(0, -16), 3, -1, false, new Vector2(50, 0), Vector2.Zero, new Point(rndX, 1), true, 100),
                new Bullet(ts, new Vector2(GetHB().X, GetHB().Y) + new Vector2(0, -16), 3, -1, false, new Vector2(50, 50), new Vector2(0, 50), new Point(rndX, -1), true, 100),
                new Bullet(ts, new Vector2(GetHB().X, GetHB().Y) + new Vector2(0, -16), 1000, 5, true, new Vector2(50, 50), new Vector2(0, 50), new Point(rndX, -1), true, 100),
            };
        }

        void SeekPlug(GameTime a_gt)
        {
            if (isNearPlug)
            {
                if (nearestPlug.X > GetHB().X)
                { mov.X += 10 * (float)a_gt.ElapsedGameTime.TotalSeconds; }//temp
                if (nearestPlug.X < GetHB().X)
                { mov.X += -10 * (float)a_gt.ElapsedGameTime.TotalSeconds; }//temp
            }
        }            
        void Shoot(Vector2 playerPos)
        {
            int rng = new Random().Next(0, bullettypes.Length);
            bullets.Add(bullettypes[rng].Clone());
            if(rng == 2) { bullets[bullets.Count - 1].CalculateTarget(playerPos, 2f); }
        }

        public void Update(GameTime a_gt, Rectangle virtualDims, Switch[] switches_, Vector2 playerpos_)
        {
            isNearPlug = false;
            isPlugged = false;
            shotTimer -= (float)a_gt.ElapsedGameTime.TotalSeconds;
            if (shotTimer <= 0) { shotTimer = shotTime; Shoot(playerpos_); }
            float record = 1000;
            foreach (Switch s in switches_)
            {
                if (s.isOn)
                {
                    if ((Vector2.Distance(new Vector2(GetHB().X, GetHB().Y), new Vector2(s.plug.GetHB().X, s.plug.GetHB().Y))) < record)
                    {
                        isNearPlug = true;
                        record = Vector2.Distance(new Vector2(GetHB().X, GetHB().Y) , new Vector2(s.plug.GetHB().X, s.plug.GetHB().Y));
                        nearestPlug = new Vector2(s.plug.GetHB().X, s.plug.GetHB().Y);
                        if (nearestPlug.X - GetHB().X < 1 && nearestPlug.X - GetHB().X > -1)
                        { pos.X = nearestPlug.X; isPlugged = true; }                        
                    }
                }                
            }
            if (!isPlugged) { hp -= 2 * (float)a_gt.ElapsedGameTime.TotalSeconds; SeekPlug(a_gt); }
            else
            {
                hp += 8 * (float)a_gt.ElapsedGameTime.TotalSeconds;
                if (hp > 100)
                    hp = 100;
            }

            pos += mov;
            mov = Vector2.Zero;
            
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

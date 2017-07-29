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
        bool isPlugged, isNearPlug;
        Vector2 nearestPlug;
        public float hp, maxhp;
        public List<Bullet> bullets;
        STexture[] bullettexes;

        public Boss(STexture[] a_tex, Vector2 a_pos, STexture[] a_bullettexes) : base(a_tex, a_pos)
        {
            SetPlatformData();
            movType = BossMovement.Right;
            hp = 100;
            maxhp = 100;

            shotTimer = 10;
            shotTime = 2;
            isPlugged = false;
            plugDist = new Vector2(8);
            bullets = new List<Bullet> { };
            bullettexes = a_bullettexes;
        }
        void SetPlatformData()
        {
            platformStart = new Vector2(GetHB().X, GetHB().Y);
            platformEnd = new Vector2(GetHB().X + tex.framelength, GetHB().Y);
        }
        public void Move(GameTime a_gt)
        {

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
        void Shoot()
        {
            STexture[] ts = new STexture[] { bullettexes[0].Clone() };
            bullets.Add(new Bullet(ts, pos - new Vector2(0, 20), 3, -1, false, new Vector2(0, 50), Vector2.Zero, new Point(1, -1), false, 100));
        }
        public void Update(GameTime a_gt, Rectangle virtualDims, Switch[] switches_, Vector2 playerpos_)
        {
            isNearPlug = false;
            isPlugged = false;
            shotTimer -= (float)a_gt.ElapsedGameTime.TotalSeconds;
            if (shotTimer <= 0) { shotTimer = shotTime; Shoot(); }
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
            if (!isPlugged) { hp -= 10 * (float)a_gt.ElapsedGameTime.TotalSeconds; SeekPlug(a_gt); }
            else
            {
                hp += 8 * (float)a_gt.ElapsedGameTime.TotalSeconds;
                if (hp > 100)
                    hp = 100;
            }

            pos += mov;
            mov = Vector2.Zero;
            
            SetPlatformData();

            base.Update(a_gt);
        }
    }
}

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
        public Vector2 platformStart, platformEnd, mov;
        public BossMovement movType;
        bool isPlugged;
        Vector2 nearestPlug;
        public float hp, maxhp;

        public Boss(STexture[] a_tex, Vector2 a_pos): base(a_tex, a_pos)
        {
            SetPlatformData();
            movType = BossMovement.Right;
            hp = 100;
            maxhp = 100;

            shotTimer = 10f;
            shotTime = 2f;
            isPlugged = false;
        }
        void SetPlatformData()
        {
            platformStart = pos;
            platformEnd = new Vector2(pos.X + tex.framelength, pos.Y);
        }
        public void Move(GameTime a_gt)
        {
            
        }
        void SeekPlug(GameTime a_gt)
        {
            if(nearestPlug.X - pos.X < 1 && nearestPlug.X - pos.X > -1) { pos.X = nearestPlug.X;}
            else
            {
                if (nearestPlug.X > pos.X) { mov.X += 10 * (float)a_gt.ElapsedGameTime.TotalSeconds; }//temp
                if (nearestPlug.X < pos.X) { mov.X += -10 * (float)a_gt.ElapsedGameTime.TotalSeconds; }//temp
            }
        }
        void Shoot()
        {

        }
        public void Update(GameTime a_gt, Rectangle virtualDims, Switch[] switches_, Vector2 playerpos_)
        {
            shotTimer -= (float)a_gt.ElapsedGameTime.TotalSeconds;
            if(shotTimer <= 0) { shotTimer = shotTime; Shoot(); }

            if (!isPlugged)
            {
                hp -= 10 * (float)a_gt.ElapsedGameTime.TotalSeconds;
                float record = 1000;
                nearestPlug = pos;
                foreach (Switch s in switches_)
                {
                    if (s.isOn)
                    {
                        if ((Vector2.Distance(pos, s.plug.pos)) < record)
                        {
                            record = Vector2.Distance(pos, s.plug.pos);
                            nearestPlug = s.plug.pos;
                        }
                    }
                }
                SeekPlug(a_gt);
            }
            else {
                hp += 8 * (float)a_gt.ElapsedGameTime.TotalSeconds;
            }

            pos += mov;
            mov = Vector2.Zero;

            SetPlatformData();

            base.Update(a_gt);
        }
    }
}

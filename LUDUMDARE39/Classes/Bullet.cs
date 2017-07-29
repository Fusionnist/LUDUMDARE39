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
    public class Bullet:Sprite
    {
        int bounces;
        Vector2 velocity, bvel;
        float lifeTime;
        bool lifeTimed;
        Vector2 velLoss;
        Vector2 mov;
        Point xyDir;
        float maxv;
        bool isExploding, isDead, bouncy;

        public Bullet(STexture[] texes_, Vector2 pos_, int bounces_,float lifet_, bool lifeTD_, Vector2 vel_, Vector2 velLoss_, Point xyDir_,bool bouncy_, float maxVel_):base(texes_, pos_)
        {
            bouncy = bouncy_;
            maxv = maxVel_;
            bounces = bounces_;
            velocity = vel_; bvel = vel_;
            lifeTime = lifet_;
            lifeTimed = lifeTD_;
            velLoss = velLoss_;
            xyDir = xyDir_;
        }
        public void CalculateTarget(Vector2 tar_, float timeToTar_)
        {
            velocity.X = tar_.X - pos.X / timeToTar_;
            velLoss.X = 0;
        }
        public void Update(GameTime a_gt, Rectangle vdims_)
        {
            base.Update(a_gt);

            velocity -= velLoss * (float)a_gt.ElapsedGameTime.TotalSeconds;
            mov += velocity * (float)a_gt.ElapsedGameTime.TotalSeconds;
            mov.X *= xyDir.X;
            mov.Y *= xyDir.Y;

            if (lifeTimed) {
                lifeTime -= (float)a_gt.ElapsedGameTime.TotalSeconds;
                if (lifeTime <= 0) { isExploding = true; }
            }

            pos += mov;
            mov = Vector2.Zero;

            if (GetHB().Y + GetHB().Height > vdims_.Y + vdims_.Height) {
                pos.Y = vdims_.Y + vdims_.Height - GetHB().Height; ; bounces--;
                if (bouncy) { velocity.Y = bvel.Y;}
                else { xyDir.Y *= -1;  }
            }
            if (GetHB().Y < vdims_.Y) {
                pos.Y = vdims_.Y; bounces--;
                if (bouncy) { }
                else { xyDir.Y *= -1; }
            }
            if (GetHB().X + GetHB().Width > vdims_.X + vdims_.Width) {
                pos.X = vdims_.X + vdims_.Width - GetHB().Height; bounces--; xyDir.X *= -1;                
            }
            if (GetHB().X < vdims_.X) { xyDir.X *= -1; pos.X = vdims_.X; bounces--; }

            if(bounces == 0) { isExploding = true; }
        }
    }
}

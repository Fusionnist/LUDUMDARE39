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
        Vector2 velocity, baseVelocity;
        float lifeTime;
        bool lifeTimed;
        Vector2 velLoss;
        Vector2 mov;
        Point xyDir;
        bool isExploding, isDead;

        public Bullet(STexture[] texes_, Vector2 pos_, int bounces_,float lifet_, bool lifeTD_, Vector2 vel_, Vector2 velLoss_, Point xyDir_):base(texes_, pos_)
        {
            bounces = bounces_;
            velocity = vel_; baseVelocity = velocity;
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
        
            if (truehb.Y > vdims_.Y + vdims_.Height) { }
        }
    }
}

﻿using System;
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
        public int bounces;
        Vector2 velocity, bvel;
        float? lifeTime;
        bool lifeTimed;
        Vector2 velLoss;
        Vector2 mov;
        Point xyDir;
        float? angle;
        float maxv;
        public bool isExploding, isDead, bouncy;

        public Bullet(STexture[] texes_, Vector2 pos_, int bounces_, float? lifet_, bool lifeTD_, Vector2 vel_, Vector2 velLoss_, Point xyDir_,bool bouncy_, float maxVel_, float? a_angle):base(texes_, pos_)
        {
            bouncy = bouncy_;
            maxv = maxVel_;
            bounces = bounces_;
            velocity = vel_; bvel = vel_;
            lifeTime = lifet_;
            lifeTimed = lifeTD_;
            velLoss = velLoss_;
            if (xyDir_.X == 0)
                xyDir_.X = -1;
            if (xyDir_.Y == 0)
                xyDir_.Y = -1;
            xyDir = xyDir_;
            if (a_angle != null)
            { velocity.X *= (float)Math.Cos((double)a_angle); velocity.Y *= (float)Math.Sin((double)a_angle);}
            angle = a_angle;
        }
        public void CalculateTarget(Vector2 tar_, float timeToTar_)
        {
            velocity.X = (tar_.X - pos.X) / timeToTar_;
            if(pos.X < tar_.X) { xyDir.X = 1; }
            else { xyDir.X = -1; }
            xyDir.Y = -1;
            velLoss.X = 0;
        }

        public Bullet Clone()
        {
            return new Bullet(texes, pos, bounces, lifeTime, lifeTimed, velocity, velLoss, xyDir, bouncy, maxv, angle);
        }

        public void Update(GameTime a_gt, Rectangle vdims_)
        {
            base.Update(a_gt);

            if (!isExploding)
            {
                velocity -= velLoss * (float)a_gt.ElapsedGameTime.TotalSeconds;
                mov += velocity * (float)a_gt.ElapsedGameTime.TotalSeconds;
                mov.X *= xyDir.X;
                mov.Y *= xyDir.Y;

                if (lifeTimed)
                {
                    lifeTime -= (float)a_gt.ElapsedGameTime.TotalSeconds;
                    if (lifeTime <= 0) { isExploding = true; SelectTexture("explosion"); }
                }

                pos += mov;
                mov = Vector2.Zero;

                if (GetHB().Y + GetHB().Height > vdims_.Y + vdims_.Height)
                {
                    pos.Y = vdims_.Y + vdims_.Height - GetHB().Height; ; bounces--;
                    if (bouncy) { velocity.Y = bvel.Y; }
                    else { xyDir.Y *= -1; }
                }
                if (GetHB().Y < vdims_.Y)
                {
                    pos.Y = vdims_.Y; bounces--;
                    if (bouncy) { }
                    else { xyDir.Y *= -1; }
                }
                if (GetHB().X + GetHB().Width > vdims_.X + vdims_.Width)
                {
                    pos.X = vdims_.X + vdims_.Width - GetHB().Height; bounces--; xyDir.X *= -1;
                }
                if (GetHB().X < vdims_.X) { xyDir.X *= -1; pos.X = vdims_.X; bounces--; }

                if (bounces <= 0) { isExploding = true; SelectTexture("explosion"); }
            }
            if (isExploding)
            {
                if (tex.complete) { isDead = true; }
            }
        }
    }
}

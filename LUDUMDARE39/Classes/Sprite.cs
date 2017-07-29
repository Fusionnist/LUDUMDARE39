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
    public class Sprite
    {
        public STexture tex;
        public STexture[] texes;
        public Vector2 pos;
        public Rectangle truehb;

        public Sprite(STexture[] a_texes, Vector2 a_pos)
        {
            tex = a_texes[0];
            texes = a_texes;
            pos = a_pos;
            truehb = tex.hb;
        }
        void PositionHB()
        {
            truehb.X = (int)pos.X;
            truehb.Y = (int)pos.Y;
        }
        public virtual void Update(GameTime a_gt)
        {
            PositionHB();
            tex.Update(a_gt);
        }
        public void SelectTexture(string name)
        {
            foreach(STexture t in texes) { if (t.name == name) { tex = t; t.Reset(); } }
            truehb = tex.hb;
        }
        public virtual void Draw (SpriteBatch a_sb)
        {
            tex.Draw(a_sb, new Vector2((int)pos.X, (int)pos.Y));
        }
    }
}

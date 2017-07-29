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

        public virtual void Update(GameTime a_gt)
        {
            tex.Update(a_gt);
            GetHB();
        }

        public Rectangle GetHB()
        {
            truehb = new Rectangle((int)pos.X + tex.hb.X, (int)pos.Y + tex.hb.Y, tex.hb.Width, tex.hb.Height);
            return truehb;
        }

        public void SelectTexture(string name)
        {
            foreach(STexture t in texes) { if (t.name == name) { tex = t; t.Reset(); } }
        }
        public virtual void Draw (SpriteBatch a_sb)
        {
            tex.Draw(a_sb, new Vector2((int)pos.X, (int)pos.Y));
        }
    }
}

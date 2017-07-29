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
    class Sprite
    {
        protected STexture tex;
        protected STexture[] texes;
        protected Vector2 pos;

        public Sprite(STexture a_tex, Vector2 a_pos)
        {
            tex = a_tex;
            pos = a_pos;
        }

        public virtual void Update(GameTime a_gt)
        {
            tex.Update(a_gt);
        }
        void SelectTexture(string name)
        {
            foreach(STexture t in texes) { if (t.name == name) { tex = t; } }
        }
        public void Draw (SpriteBatch a_sb)
        {
            tex.Draw(a_sb, new Vector2((int)pos.X, (int)pos.Y));
        }
    }
}

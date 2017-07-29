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
        STexture tex;
        Vector2 pos;

        public Sprite(STexture a_tex, Vector2 a_pos)
        {
            tex = a_tex;
            pos = a_pos;
        }

        public void Update(GameTime a_gt)
        {
            tex.Update(a_gt);
        }

        public void Draw (SpriteBatch a_sb)
        {
            tex.Draw(a_sb, pos);
        }
    }
}

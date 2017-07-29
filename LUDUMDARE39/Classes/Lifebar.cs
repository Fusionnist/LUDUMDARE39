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
    class Lifebar : Sprite
    {
        public Lifebar(STexture[] a_texes, Vector2 a_pos): base(a_texes, a_pos)
        {

        }

        public override void Draw(SpriteBatch a_sb)
        {
            texes[0].Draw(a_sb, new Vector2((int)pos.X, (int)pos.Y));
            texes[1].Draw(a_sb, new Vector2((int)pos.X + 1, (int)pos.Y + 1));
        }
    }
}

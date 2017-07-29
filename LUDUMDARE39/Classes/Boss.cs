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
    class Boss : Sprite
    {
        Vector2 platformStart, platformEnd;

        public Boss(STexture a_tex, Vector2 a_pos): base(a_tex, a_pos)
        {
            platformStart = a_pos;
            platformEnd = new Vector2(a_pos.X + a_tex.framelength, a_pos.Y);
        }
    }
}

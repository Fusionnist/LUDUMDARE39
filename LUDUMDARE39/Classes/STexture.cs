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
    class STexture
    {
        Texture2D tex;
        int framecount, framelength, currentframe;
        float frametime, frametimer;

        public STexture(Texture2D a_tex)
        {
            tex = a_tex;
        }

        public STexture(Texture2D a_tex, int a_FC, int a_FL, float a_FT)
        {
            tex = a_tex;
            framecount = a_FC;
            framelength = a_FL;
            frametime = a_FT;
            currentframe = 1;
            frametimer = frametime;
        }

        public void Update(GameTime a_gt)
        {
            frametimer -= (float)a_gt.ElapsedGameTime.TotalSeconds;
            if (frametimer <= 0)
            {
                frametimer = frametime;
                currentframe++;
                if (currentframe > framecount)
                    currentframe = 1;
            }
        }

        public void Draw(SpriteBatch a_sb, Vector2 a_pos)
        {
            a_sb.Draw(tex, a_pos, sourceRectangle: new Rectangle(framelength * (currentframe - 1), 0, framelength, tex.Height));
        }
    }
}

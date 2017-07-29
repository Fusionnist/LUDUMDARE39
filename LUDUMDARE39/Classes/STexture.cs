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
    public class STexture
    {
        public Texture2D tex;
        public string name;
        int framecount, currentframe;
        float frametime, frametimer;
        public int framelength { get; set; }
        public int frameheight { get; set; }
        public Rectangle hb;

        public STexture(Texture2D a_tex, Rectangle a_hb)
        {
            tex = a_tex;
            frameheight = a_tex.Height;
            hb = a_hb;
        }

        public STexture(Texture2D a_tex, int a_FC, int a_FL, float a_FT, string name_, Rectangle a_hb)
        {
            name = name_;
            tex = a_tex;
            framecount = a_FC;
            framelength = a_FL;
            frametime = a_FT;
            currentframe = 1;
            frametimer = frametime;
            frameheight = a_tex.Height;
            hb = a_hb;
        }
        public void Reset()
        {
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
            a_sb.Draw(tex, a_pos - new Vector2(hb.X, hb.Y), sourceRectangle: new Rectangle(framelength * (currentframe - 1), 0, framelength, tex.Height));
        }
    }
}

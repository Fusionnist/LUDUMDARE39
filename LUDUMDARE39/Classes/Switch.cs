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
    class Switch : Sprite
    {
        public bool isOn;
        public Sprite[] plugs;
        public Switch(STexture[] a_texes, Vector2 a_pos, Sprite[] plugs_):base(a_texes, a_pos)
        {
            plugs = plugs_;
        }
        public void Activate()
        {
            isOn = !isOn;
            if (isOn) { SelectTexture("switchon"); }
            if (!isOn) { SelectTexture("switchoff"); }
        }
        public override void Draw(SpriteBatch a_sb)
        {
            foreach(Sprite s in plugs) { s.Draw(a_sb); }
            base.Draw(a_sb);
        }
    }
}

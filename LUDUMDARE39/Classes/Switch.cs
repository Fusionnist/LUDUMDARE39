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
        public Plug plug;

        public Switch(STexture[] a_texes, Vector2 a_pos, Plug a_plug):base(a_texes, a_pos)
        {
            isOn = true;
            plug = a_plug;
        }

        public void Activate()
        {
            isOn = !isOn;
            plug.Activate();
            if (isOn) { SelectTexture("switchon"); }
            if (!isOn) { SelectTexture("switchoff"); }
        }

        public override void Draw(SpriteBatch a_sb)
        {
            base.Draw(a_sb);
            plug.Draw(a_sb);
        }
    }
}

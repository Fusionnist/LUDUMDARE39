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
    class Plug : Sprite
    {
        bool isOn;

        public Plug(STexture[] a_texes, Vector2 a_pos): base(a_texes, a_pos)
        {
            isOn = true;
        }

        public void Activate()
        {
            isOn = !isOn;
            if (isOn)
                SelectTexture("on");
            else
                SelectTexture("off");
        }
    }
}

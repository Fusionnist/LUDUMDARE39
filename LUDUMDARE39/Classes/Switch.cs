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
        public Switch(STexture a_tex, Vector2 a_pos):base(a_tex, a_pos)
        {

        }
        public void Activate()
        {
            isOn = !isOn;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

namespace LUDUMDARE39
{
    class Switch : Sprite
    {
        public bool isOn;
        public Plug[] plugs;
        public SoundEffect switchSound;

        public Switch(STexture[] a_texes, Vector2 a_pos, Plug[] a_plug, SoundEffect a_switchSound):base(a_texes, a_pos)
        {
            isOn = true;
            plugs = a_plug;
            switchSound = a_switchSound;
        }

        public void Activate()
        {
            isOn = !isOn;
            foreach (Plug p in plugs) { p.Activate(); }
            if (isOn) { SelectTexture("switchon"); }
            if (!isOn) { SelectTexture("switchoff"); }
            switchSound.Play();
        }

        public override void Draw(SpriteBatch a_sb)
        {
            base.Draw(a_sb);
            foreach(Plug p in plugs) { p.Draw(a_sb); }
        }
    }
}

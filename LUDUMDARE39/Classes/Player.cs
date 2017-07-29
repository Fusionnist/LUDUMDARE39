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
    class Player : Sprite
    {
        Vector2 mov;

        public Player(STexture a_tex, Vector2 a_pos): base(a_tex, a_pos)
        {
            mov = Vector2.Zero;
        }

        public override void Update(GameTime a_gt)
        {
            pos += mov;
            mov = Vector2.Zero;
            base.Update(a_gt);
        }

        public void Move(GameTime a_gt)
        {
            KeyboardState ks = Keyboard.GetState();
            if (ks.IsKeyDown(Keys.Left))
                mov.X -= 25 * (float)a_gt.ElapsedGameTime.TotalSeconds;
            if (ks.IsKeyDown(Keys.Right))
                mov.X += 25 * (float)a_gt.ElapsedGameTime.TotalSeconds;
        }
    }
}

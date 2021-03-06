﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace LUDUMDARE39
{
    public class Input
    {
        public Input(Keys key_)
        {
            key = key_;
        }
        Keys key;
        bool isPressed, isReleased;
        public void Update(KeyboardState kbs_)
        {
            if (kbs_.IsKeyDown(key))
            {
                if (isReleased)
                    isPressed = true;
                else
                    isPressed = false;

                isReleased = false;
            }
            if (kbs_.IsKeyUp(key))
            {
                isReleased = true;
            }
        }
        public bool IsDown()
        {
            return !isReleased;
        }
        public bool IsPressed()
        {
            return isPressed;
        }
    }
}

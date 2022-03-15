using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RosesAreRed.Controls
{
    class Gestures
    {
        Vector2 inicio;
        Vector2 distancia;
        bool drag = false;

        public string Update()
        {
            MouseState mouse = Mouse.GetState();

            if (mouse.LeftButton == ButtonState.Pressed && drag == false)
            {
                inicio.X = mouse.X;
                inicio.Y = mouse.Y;
                drag = true;
            }
            if (mouse.LeftButton == ButtonState.Released && drag == true)
            {
                distancia.X = inicio.X - mouse.X;
                distancia.Y = inicio.Y - mouse.Y;
                drag = false;

                if (distancia.X > 0 && Math.Abs(distancia.Y) < Math.Abs(distancia.X))
                    return "left";
                if (distancia.X < 0 && Math.Abs(distancia.Y) < Math.Abs(distancia.X))
                    return "right";
                if (distancia.Y > 0 && Math.Abs(distancia.Y) > Math.Abs(distancia.X))
                    return "up";
                if (distancia.Y < 0 && Math.Abs(distancia.Y) > Math.Abs(distancia.X))
                    return "down";
            }
            return "none";
        }
    }
}

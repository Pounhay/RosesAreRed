using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RosesAreRed.Mechanics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using System.Diagnostics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace RosesAreRed.Controls
{
    class Volume
    {
        float volume;
        KeyboardState kb, pkb;
        Texture2D backgroundTexture1;
        SpriteFont font;

        public bool  Update()
        {
            pkb = kb;
            kb = Keyboard.GetState();

            if ((kb.IsKeyDown(Keys.Up) && pkb.IsKeyDown(Keys.Up)) || (kb.IsKeyDown(Keys.Up) && pkb.IsKeyUp(Keys.Up)))
            {
                volume++;
                volume = MathHelper.Clamp(volume, 0f, 100f);
                var logged = Math.Log(volume + 1) / Math.Log(101);
                MediaPlayer.Volume = MathHelper.Clamp((float)logged, 0.001f, 1.0f);
                return true;
            }
            if ((kb.IsKeyDown(Keys.Down) && pkb.IsKeyDown(Keys.Down)) || (kb.IsKeyDown(Keys.Down) && pkb.IsKeyUp(Keys.Down)))
            {
                volume--;
                volume = MathHelper.Clamp(volume, 0f, 100f);
                var logged = Math.Log(volume + 1) / Math.Log(101);
                MediaPlayer.Volume = MathHelper.Clamp((float)logged, 0.001f, 1.0f);
                return true;
            }
            return false;
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(backgroundTexture1, new Vector2(0, 0), Color.Gray);
            spriteBatch.DrawString(font, volume.ToString(), new Vector2(210, 350), Color.White);      
        }
         public Volume(Game1 game, GraphicsDevice graphicsDevice, ContentManager content)
        {
            font = content.Load<SpriteFont>("Fonts/FloralScore");
            backgroundTexture1 = content.Load<Texture2D>("Assets/Menu");
            kb = Keyboard.GetState();
            volume = 50;
        }
    }
}

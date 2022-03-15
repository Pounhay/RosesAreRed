using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using RosesAreRed.Controls;
using RosesAreRed.Mechanics;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Input;

namespace RosesAreRed.States
{
    public class GameState : State
    {
        private Gestures gestures;
        private string gest;
        private Volume vol;
        Board board;
        Song song;
        bool volume;
        // Poll for current keyboard state
        public GameState(Game1 game, GraphicsDevice graphicsDevice, ContentManager content)
          : base(game, graphicsDevice, content)
        {
            board = new Board(game, graphicsDevice, content);
            vol = new Volume(game, graphicsDevice, content);
            gestures = new Gestures();
            song = content.Load<Song>("Sounds/Breeze");
            MediaPlayer.Volume = 0.5f;
            MediaPlayer.Play(song);
            MediaPlayer.IsRepeating = true;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            board.Draw(spriteBatch);
            if (volume)
                vol.Draw(spriteBatch);
            spriteBatch.End();
        }

        public override void PostUpdate(GameTime gameTime)
        {

        }

        public override void Update(GameTime gameTime)
        {
            gest = gestures.Update();
            if (gest != "none" && board.states == Board.States.Play)
            {
                board.Update(gest);
            }
            KeyboardState state = Keyboard.GetState();
            if (state.IsKeyDown(Keys.Escape))
            {
                MediaPlayer.Pause();
                _game.ChangeState(new PauseState(this, _game, _graphicsDevice, _content));
            }
            volume = vol.Update();

        }
    }
}
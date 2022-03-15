using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using RosesAreRed.Controls;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;

namespace RosesAreRed.States
{
    public class PauseState : State
    {
        private List<Component> _components;
        Texture2D menuTexture;
        SpriteFont font;
        private State _nextState;

        public PauseState(State _state, Game1 game, GraphicsDevice graphicsDevice, ContentManager content)
          : base(game, graphicsDevice, content)
        {
            _nextState = _state;
            menuTexture = _content.Load<Texture2D>("Assets/Menu");
            var buttonTexture = _content.Load<Texture2D>("Controls/Button");
            var buttonFont = _content.Load<SpriteFont>("Fonts/Floral");
            font = content.Load<SpriteFont>("Fonts/FloralScore");

            var newGameButton = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2(150, 300),
                Text = "Retry",
            };

            newGameButton.Click += NewGameButton_Click;

            var quitGameButton = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2(150, 500),
                Text = "Quit Game",
            };

            quitGameButton.Click += QuitGameButton_Click;

            var continueGameButton = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2(150, 400),
                Text = "Continue",
            };

            continueGameButton.Click += ContinueGameButton_Click;

            _components = new List<Component>()
      {
        newGameButton,
        quitGameButton,
        continueGameButton
      };
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            spriteBatch.Draw(menuTexture, new Vector2(0, 0), Color.White);
            foreach (var component in _components)
                component.Draw(gameTime, spriteBatch);
            spriteBatch.DrawString(font, "Roses \n Are \n Red", new Vector2(130, 40), Color.White);
            spriteBatch.End();
        }

        private void NewGameButton_Click(object sender, EventArgs e)
        {
            _game.ChangeState(new GameState(_game, _graphicsDevice, _content));
        }

        public override void PostUpdate(GameTime gameTime)
        {
            // remove sprites if they're not needed
        }

        public override void Update(GameTime gameTime)
        {
            foreach (var component in _components)
                component.Update(gameTime);
        }

        private void QuitGameButton_Click(object sender, EventArgs e)
        {
            _game.Exit();
        }

        private void ContinueGameButton_Click(object sender, EventArgs e)
        {
            _game.ChangeState(_nextState);
            MediaPlayer.Resume();
        }
    }
}
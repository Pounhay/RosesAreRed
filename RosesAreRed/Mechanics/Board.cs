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

namespace RosesAreRed.Mechanics
{
    class Board
    {
        public enum States {  Win, Lose, Play }
        public static readonly Point sep = new Point(20, 205);
        private bool flag = false;
        List<Tiles> tiles = new List<Tiles>();
        private Dictionary<int, Texture2D> tileTexture = new Dictionary<int, Texture2D>();
        Texture2D backgroundTexture;
        Texture2D backgroundTexture1;
        SpriteFont font;
        private int score;
        private bool win = false;
        internal States states;
        SoundEffect swoosh;
        SoundEffectInstance mySoundInstance;

        public int GetScore()
        {
            return score;
        }
        private void Move(string gesture)
        {
            int[,] table = new int[4, 4];
            Dictionary<int, int> fused = new Dictionary<int, int>();
            flag = false;
            for (int x = 0; x < table.GetLength(0); x++)
            {
                for (int y = 0; y < table.GetLength(1); y++)
                    table[x, y] = -1;
            }
            int i = 0;
            foreach (var block in tiles)
            {
                switch (gesture)
                {
                    case "up":
                        table[block.start.X, block.start.Y] = i++;
                        break;
                    case "down":
                        table[block.start.X, 3 - block.start.Y] = i++;
                        break;
                    case "left":
                        table[block.start.Y, block.start.X] = i++;
                        break;
                    case "right":
                        table[3 - block.start.Y, 3 - block.start.X] = i++;
                        break;
                    default:
                        return;
                }
            }
            for (int x = 0; x < table.GetLength(0); x++)
            {
                bool happened;
                do
                {
                    happened = false;
                    for (int y = 1; y < table.GetLength(0); y++)
                    {
                        if (table[x, y - 1] != -1 && table[x, y] != -1 && tiles[table[x, y - 1]].GetValue() == tiles[table[x, y]].GetValue() && tiles[table[x, y]].state == State.Normal && tiles[table[x, y - 1]].state == State.Normal)
                        {
                            fused.Add(table[x, y - 1], table[x, y]);
                            tiles[table[x, y]].state = State.Upgrade;
                            tiles[table[x, y - 1]].state = State.Delete;
                            table[x, y] = -1;
                            happened = true;
                            break;
                        }
                        if (table[x, y - 1] == -1 && table[x, y] != -1)
                        {
                            table[x, y - 1] = table[x, y];
                            table[x, y] = -1;
                            happened = true;
                            break;
                        }
                    }
                    flag = flag || happened;
                } while (happened);
            }
            for (int x = 0; x < table.GetLength(0); x++)
            {
                for (int y = 0; y < table.GetLength(1); y++)
                {
                    if (table[x, y] != -1)
                    {
                        switch (gesture)
                        {
                            case "up":
                                tiles[table[x, y]].finish = new Point(x, y);
                                break;
                            case "down":
                                tiles[table[x, y]].finish = new Point(x, 3 - y);
                                break;
                            case "left":
                                tiles[table[x, y]].finish = new Point(y, x);
                                break;
                            case "right":
                                tiles[table[x, y]].finish = new Point(3 - y, 3 - x);
                                break;
                        }
                    }
                }
            }
            foreach (KeyValuePair<int, int> pair in fused)
                tiles[pair.Value].finish = new Point(tiles[pair.Key].finish.X, tiles[pair.Key].finish.Y);
            if (flag)
            {
                mySoundInstance.Play();
                Tiles.ticks = 0;
            }
        }
        private void AddNew()
        {
            SortedSet<int> available = new SortedSet<int>(Enumerable.Range(0, 16));
            foreach (var b in tiles)
            {
                available.Remove(b.finish.X * 4 + b.finish.Y);
            }
            var num = available.ElementAt(new Random().Next(0, available.Count()));
            tiles.Add(new Tiles(new Point(num / 4, num % 4)));
        }
        public void Update(string gesture)
        {
            if (Tiles.ticks == Tiles.maxTicks)
            {
                Move(gesture);
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            if (states == States.Win)
            {
                spriteBatch.Draw(backgroundTexture1, new Vector2(0, 0), Color.Gray);
                spriteBatch.DrawString(font, "You Win!", new Vector2(90, 200), Color.Red);
                spriteBatch.DrawString(font, "Score", new Vector2(130, 400), Color.White);
                spriteBatch.DrawString(font, GetScore().ToString(), new Vector2(130,510), Color.White);
                spriteBatch.DrawString(font, "Press ESC", new Vector2(40, 600), Color.AliceBlue);
            }
            if (states == States.Lose)
            {
                spriteBatch.Draw(backgroundTexture1, new Vector2(0, 0), Color.Gray);
                spriteBatch.DrawString(font, "You Lose!", new Vector2(40, 200), Color.Blue);
                spriteBatch.DrawString(font, "Score", new Vector2(130, 400), Color.White);
                spriteBatch.DrawString(font, GetScore().ToString(), new Vector2(130, 510), Color.White);
                spriteBatch.DrawString(font, "Press ESC", new Vector2(40, 600), Color.AliceBlue);
            }
            if (states == States.Play)
            {
                spriteBatch.Draw(backgroundTexture, new Vector2(0, 0), Color.White);
                DrawTiles(spriteBatch);
                spriteBatch.DrawString(font, "Score", new Vector2(130, 40), Color.White);
                spriteBatch.DrawString(font, GetScore().ToString(), new Vector2(130, 110), Color.White);
            }
        }
        public void DrawTiles(SpriteBatch spriteBatch)
        {
            tiles.Where(tile => tile.state == State.Delete).ToList().ForEach(tile => tile.Draw(spriteBatch));
            tiles.Where(tile => tile.state != State.Delete).ToList().ForEach(tile => tile.Draw(spriteBatch));
            if (Tiles.ticks < Tiles.maxTicks)
                Tiles.ticks++;
            else
            {
                tiles.ForEach(tile => tile.start = new Point(tile.finish.X, tile.finish.Y));
                FinishMove();
                if (flag)
                {
                    flag = false;
                    AddNew();
                }
            }
        }
        public Board(Game1 game, GraphicsDevice graphicsDevice, ContentManager content)
        {
            AddNew();
            AddNew(); 
            Tiles.setTextures(tileTexture);
            for (int i = 1; i <= 11; i++)
            {
                tileTexture.Add(i, content.Load<Texture2D>("Assets/Roses_" + i));
            }
            font = content.Load<SpriteFont>("Fonts/FloralScore");
            backgroundTexture = content.Load<Texture2D>("Assets/Background");
            backgroundTexture1 = content.Load<Texture2D>("Assets/Menu");
            states = States.Play;
            swoosh = content.Load<SoundEffect>("Sounds/Swoosh");
            mySoundInstance = swoosh.CreateInstance();
        }
        private void FinishMove()
        {
            tiles.RemoveAll((Tiles x) => { return x.state == State.Delete; });
            tiles.Where(tile => tile.state == State.Upgrade).ToList().ForEach(tile => score += tile.Upgrade());
            if (!win && tiles.Any(tile => tile.GetValue() == 11))
            {
                win = true;
                states = States.Win;
            }
            Lose();
        }
        private void Lose()
        {
            if (tiles.Count() == 16)
            {
                bool same = false;
                var table = new int[4, 4];
                foreach (var block in tiles)
                    table[block.finish.X, block.finish.Y] = block.GetValue();
                for (int x = 0; x < 4; x++)
                {
                    for (int y = 0; y < 3; y++)
                        same = same || (table[x, y] == table[x, y + 1]);
                }
                if (!same)
                {
                    for (int y = 0; y < 4; y++)
                    {
                        for (int x = 0; x < 3; x++)
                            same = same || (table[x, y] == table[x + 1, y]);
                    }
                }
                if (!same)
                    states = States.Lose;
            }
        }
    }
}

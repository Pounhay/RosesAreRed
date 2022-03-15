using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

namespace RosesAreRed.Mechanics
{
    public enum State
    {
        Normal,
        Upgrade,
        Delete
    }
    public class Tiles
    {
        private static readonly Point textureSize = new Point(100, 100);
        private static readonly Point gap = new Point(20, 25);
        public static readonly int maxTicks = 10;
        private static Dictionary<int, Texture2D> textures;
        public static void setTextures(Dictionary<int, Texture2D> dict)
        {
            textures = dict;
        }
        public static int ticks = 5;
        internal Point start;
        internal Point finish;
        internal State state;
        private int value = 0;
        public int Upgrade()
        {
            value++;
            state = State.Normal;
            return value;
        }
        public int GetValue()
        {
            return value;
        }
        private Rectangle rect;
        public Rectangle GetRect()
        {
            rect.X = Board.sep.X + (start.X * (maxTicks - ticks) + finish.X * ticks) * (textureSize.X + gap.X) / maxTicks;
            rect.Y = Board.sep.Y + (start.Y * (maxTicks - ticks) + finish.Y * ticks) * (textureSize.Y + gap.Y) / maxTicks;
            return rect;
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(textures[GetValue()], GetRect(), Color.White);
        }
        public Tiles(Point point)
        {
            value = 1;
            start = new Point(point.X, point.Y);
            finish = new Point(point.X, point.Y);
            rect = new Rectangle(Board.sep, Tiles.textureSize);
            state = State.Normal;
            GetRect();
        }
    }
}

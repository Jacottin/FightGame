using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FightGameInterface.Utils {
    public static class Draw {
        public static void DrawLine(SpriteBatch sb, Texture2D t,
                                    Vector2 start, Vector2 end, 
                                    Color color, int width = 1) {
            Vector2 edge = end - start;
            // calculate angle to rotate line
            float angle =
                (float)Math.Atan2(edge.Y, edge.X);


            sb.Draw(t,
                new Rectangle( // rectangle defines shape of line and position of start of line
                    (int)start.X,
                    (int)start.Y,
                    (int)edge.Length(), //sb will strech the texture to fill this rectangle
                    width), //width of line, change this to make thicker line
                null,
                color, //colour of line
                angle, //angle of line (calulated above)
                new Vector2(0, 0), // point in line about which to rotate
                SpriteEffects.None,
                0);
        }

        public static void DrawAroundSprite(SpriteBatch sb, Texture2D sprite, Texture2D t, Vector2 pos) {
            const int lineWidth = 3;
            const int offset = 10;
            
            Vector2 topLeft = new (pos.X - offset, pos.Y - offset);
            Vector2 topRight = new( pos.X + sprite.Width + offset , pos.Y - offset);
            Vector2 bottomLeft = new (pos.X - offset, pos.Y + sprite.Height + offset);
            Vector2 bottomRight = new (pos.X + sprite.Width + offset, pos.Y + sprite.Height + offset);

            DrawLine(sb, t, topLeft, topRight, Color.White, lineWidth);
            DrawLine(sb, t, topRight, bottomRight, Color.White, lineWidth);
            DrawLine(sb, t, bottomRight, bottomLeft, Color.White, lineWidth);
            DrawLine(sb, t, bottomLeft, topLeft, Color.White, lineWidth);
            
        }
    }
    
}
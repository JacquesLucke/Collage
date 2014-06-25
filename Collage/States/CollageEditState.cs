using Collage.Undo;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Collage.States
{
    public class CollageEditState : IState
    {
        DataAccess dataAccess;
        Color drawColor = Color.CornflowerBlue;
        Texture2D texture;
        Rectangle drawPosition = new Rectangle(50, 50, 50, 50);

        public CollageEditState(DataAccess dataAccess) 
        {
            this.dataAccess = dataAccess;
            texture = new Texture2D(dataAccess.GraphicsDevice, 1, 1);
            texture.SetData<Color>(new Color[] { Color.White });
        }

        object SetColor(object color)
        {
            Color old = drawColor;
            drawColor = (Color)color;
            return old;
        }

        object SetDrawPosition(object position)
        {
            Rectangle old = drawPosition;
            drawPosition = (Rectangle)position;
            return old;
        }

        public void Start()
        {

        }

        public void Update()
        {
            Input input = dataAccess.Input;
            if(input.IsKeyReleased(Keys.Right))
            {
                CommandCombination combi = new CommandCombination();

                Color color = Color.FromNonPremultiplied(dataAccess.Random.Next(255), dataAccess.Random.Next(255), dataAccess.Random.Next(255), 255);
                Command command = new Command(SetColor, SetColor, color);
                combi.AddToCombination(command);

                Rectangle rec = new Rectangle(dataAccess.Random.Next(300), dataAccess.Random.Next(300), dataAccess.Random.Next(300), dataAccess.Random.Next(300));
                command = new Command(SetDrawPosition, SetDrawPosition, rec);
                combi.AddToCombination(command);

                dataAccess.UndoManager.ExecuteAndAddCommand(combi);

            }
            if (input.IsKeyReleased(Keys.Left))
            {
                dataAccess.UndoManager.Undo();
            }
        }

        public void Draw()
        {
            dataAccess.GraphicsDevice.Clear(drawColor);
            dataAccess.SpriteBatch.Begin();
            dataAccess.SpriteBatch.Draw(texture, drawPosition, Color.White);
            dataAccess.SpriteBatch.End();
        }
    }
}

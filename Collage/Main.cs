#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;
using Collage.States;
using Collage.Undo;
#endregion

namespace Collage
{
    public class Main : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        StateManager stateManager = new StateManager();
        DataAccess dataAccess;
        Input input = new Input();
        UndoManager undoManager = new UndoManager();

        public Main()
            : base()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            base.Initialize();
            IsMouseVisible = true;
            Window.AllowUserResizing = true;
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            dataAccess = new DataAccess(GraphicsDevice, spriteBatch, input, stateManager, undoManager);

            CollageEditState editState = new CollageEditState(dataAccess);
            stateManager.SetCurrentState(editState);
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime time)
        {
            base.Update(time);
            input.Update();
            dataAccess.Update(time);
            stateManager.Update();
        }

        protected override void Draw(GameTime time)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            base.Draw(time);
            stateManager.Draw();
        }
    }
}

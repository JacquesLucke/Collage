﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Collage
{
    public class Main : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        StateManager stateManager = new StateManager();
        DataAccess dataAccess;
        Input input = new Input();
        GtkThread gtkThread;
        GtkWindow w;

        public Main()
            : base()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            gtkThread = new GtkThread();
            gtkThread.Start();

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

            Keymap keymap = new Keymap();
            RegisterKeyCombinations(keymap);
            dataAccess = new DataAccess(GraphicsDevice, spriteBatch, input, stateManager, keymap);

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
            if (input.IsKeyPressed(Keys.Enter))
            {
                gtkThread.method = OpenNewWindow;
            }
            if (input.IsKeyPressed(Keys.W))
            {
                w.Resize(100, 100);
            }
            dataAccess.Update(time);
            stateManager.Update();
        }
        public void OpenNewWindow()
        {
            w = new GtkWindow();
            w.Show();
        }

        protected override void Draw(GameTime time)
        {
            GraphicsDevice.Clear(Color.Orange);
            base.Draw(time);
            stateManager.Draw();
        }

        protected override void OnExiting(object sender, EventArgs args)
        {
            StopGtkThread();
            base.OnExiting(sender, args);
        }

        public void StopGtkThread()
        {
            gtkThread.Stop();
            // wait until the thread ends
            while (gtkThread.IsInitialized);
        }

        public void RegisterKeyCombinations(Keymap keymap)
        {
            KeyCombination combination;

            // Empty
            combination = new KeyCombination(false, false, false);
            keymap.Add("empty", combination);

            // Undo
            combination = new KeyCombination(true, false, false, Keys.Z);
            keymap.Add("undo", combination);

            // Redo
            combination = new KeyCombination(true, false, false, Keys.Y);
            keymap.Add("redo", combination);

            // Change Background Color
            combination = new KeyCombination(false, false, false, Keys.B);
            keymap.Add("change collage background color", combination);

            // Open Image
            combination = new KeyCombination(false, false, false, Keys.O);
            keymap.Add("open image", combination);

            // Delete Image
            combination = new KeyCombination(false, false, false, Keys.X);
            keymap.Add("delete image", combination);

            // Delete Image
            combination = new KeyCombination(false, false, false, Keys.A);
            keymap.Add("select all", combination);

            // Save Collage
            combination = new KeyCombination(false, false, false, Keys.S);
            keymap.Add("save collage", combination);
        }
    }
}

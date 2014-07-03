﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Collage
{
    public class Main : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        StateManager stateManager = new StateManager();
        DataAccess dataAccess;
        Input input = new Input();
        GuiThread guiThread;

        public Main()
            : base()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            guiThread = new GuiThread();
            guiThread.Start();
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
            dataAccess = new DataAccess(GraphicsDevice, spriteBatch, input, stateManager, keymap, guiThread);

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
            GraphicsDevice.Clear(Color.Orange);
            base.Draw(time);
            stateManager.Draw();
        }

        protected override void OnExiting(object sender, EventArgs args)
        {
            // stop the Gtk thread and wait until it ends
            guiThread.Stop();
            while (guiThread.IsInitialized);

            base.OnExiting(sender, args);
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

            // Auto Position
            combination = new KeyCombination(false, false, false, Keys.Q);
            keymap.Add("auto position", combination);
        }
    }
}

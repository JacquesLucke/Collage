using Microsoft.Xna.Framework;
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
        ContentHelper contentHelper;

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

            contentHelper = new ContentHelper(GraphicsDevice, Content);
            LoadContentHelperData();

            Keymap keymap = new Keymap();
            RegisterKeyCombinations(keymap);

            dataAccess = new DataAccess(GraphicsDevice, spriteBatch, input, stateManager, keymap, guiThread, contentHelper);

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
            GraphicsDevice.Clear(Color.FromNonPremultiplied(182, 195, 205, 255));
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
            keymap.Set("undo", new KeyCombination(true, false, false, Keys.Z));
            keymap.Set("redo", new KeyCombination(true, false, false, Keys.Y));
        }
        public void LoadContentHelperData()
        {
            // load fonts
            contentHelper.LoadSpriteFont("Fonts\\NormalFont15", "normal font");

            // load images
            contentHelper.LoadImagesSource(@"Content\Images\Check.png", "check icon");
            contentHelper.LoadImagesSource(@"Content\Images\Delete.png", "delete icon");
            contentHelper.LoadImagesSource(@"Content\Images\Right.png", "right icon");
            contentHelper.LoadImagesSource(@"Content\Images\Down.png", "down icon");

            // load effects
            contentHelper.LoadEffect("Content\\Effects\\ImageEffect.mgfx", "image effect");
            contentHelper.LoadEffect("Content\\Effects\\DropShadow.mgfx", "drop shadow effect");
        }
    }
}

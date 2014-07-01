using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Collage
{
    public class DataAccess
    {
        GraphicsDevice graphicsDevice;
        SpriteBatch spriteBatch;
        Input input;
        StateManager stateManager;
        Keymap keymap;
        GuiThread guiThread;

        Random random;
        GameTime time;

        public DataAccess(GraphicsDevice graphicsDevice, SpriteBatch spriteBatch, Input input, StateManager stateManager, Keymap keymap, GuiThread guiThread)
        {
            this.graphicsDevice = graphicsDevice;
            this.spriteBatch = spriteBatch;
            this.input = input;
            this.stateManager = stateManager;
            this.keymap = keymap;
            this.guiThread = guiThread;

            random = new Random();
        }

        public void Update(GameTime time)
        {
            this.time = time;
        }

        // Get important Objects
        public Random Random
        {
            get { return random; }
        }
        public GameTime GameTime
        {
            get { return time; }
        }
        public GraphicsDevice GraphicsDevice
        {
            get { return graphicsDevice; }
        }
        public SpriteBatch SpriteBatch
        {
            get { return spriteBatch; }
        }
        public Input Input
        {
            get { return input; }
        }
        public Keymap Keymap
        {
            get { return keymap; }
        }
        public GuiThread GuiThread
        {
            get { return guiThread; }
        }
    }
}

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Collage
{
    public class DataAccess
    {
        GraphicsDevice graphicsDevice;
        SpriteBatch spriteBatch;
        Input input;
        StateManager stateManager;
        Keymap keymap;
        GtkThread gtkThread;

        Random random;
        GameTime time;

        public DataAccess(GraphicsDevice graphicsDevice, SpriteBatch spriteBatch, Input input, StateManager stateManager, Keymap keymap, GtkThread gtkThread)
        {
            this.graphicsDevice = graphicsDevice;
            this.spriteBatch = spriteBatch;
            this.input = input;
            this.stateManager = stateManager;
            this.keymap = keymap;
            this.gtkThread = gtkThread;

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
        public GtkThread GtkThread
        {
            get { return gtkThread; }
        }
    }
}

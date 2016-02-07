using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using TeddyMineExplosion;

namespace ProgrammingAssignment5
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        const int WINDOW_WIDTH = 800;
        const int WINDOW_HEIGHT = 600;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        // Mine sprite support
        Texture2D mineSprite;
        List<Mine> mines = new List<Mine>();

        // Teddy support
        Texture2D teddySprite;
        List<TeddyBear> teddyBears = new List<TeddyBear>();
        int spawnTimer, spawnDelay;
        Random rand = new Random();
        Random velocity = new Random();

        // Explosions support
        Texture2D explosionSprite;
        List<Explosion> explosions = new List<Explosion>();

        // click processing
        bool leftClickStarted = false;
        bool leftButtonReleased = true;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            // Set resolution of the game window and make mouse visible 
            graphics.PreferredBackBufferWidth = WINDOW_WIDTH;
            graphics.PreferredBackBufferHeight = WINDOW_HEIGHT;
            IsMouseVisible = true;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            mineSprite = Content.Load<Texture2D>("mine");
            teddySprite = Content.Load<Texture2D>("teddybear");
            spawnTimer = 0;
            spawnDelay = rand.Next(1000, 3001);
            explosionSprite = Content.Load<Texture2D>("explosion");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: Add your update logic here
            MouseState mouse = Mouse.GetState();

            // Check if left clicking has started
            if (mouse.LeftButton == ButtonState.Pressed && leftButtonReleased)
            {
                leftClickStarted = true;
                leftButtonReleased = false;
            }
            else if (mouse.LeftButton == ButtonState.Released)
            {
                leftButtonReleased = true;

                if (leftClickStarted)
                {
                    leftClickStarted = false;

                    // Add a new mine to the list of mines (when left click is finished
                    mines.Add(new Mine(mineSprite, mouse.X, mouse.Y));
                }
            }

            // Spawn Teddy Bears
            if (spawnTimer <= spawnDelay)
                spawnTimer += gameTime.ElapsedGameTime.Milliseconds;

            else
            {
                teddyBears.Add(new TeddyBear(teddySprite, new Vector2((float)(rand.NextDouble() - 0.5f), (float)(rand.NextDouble() - 0.5f)), WINDOW_WIDTH, WINDOW_HEIGHT));
                spawnDelay = rand.Next(1000, 3001);
                spawnTimer = 0;
            }

            foreach (TeddyBear bear in teddyBears)
            {
                bear.Update(gameTime);
            }

            foreach (TeddyBear bear in teddyBears)
            {
                foreach (Mine mine in mines)
                {
                    if (bear.Active && mine.Active && bear.CollisionRectangle.Intersects(mine.CollisionRectangle))
                    {
                        bear.Active = false;
                        mine.Active = false;
                        explosions.Add(new Explosion(explosionSprite, mine.CollisionRectangle.Center.X, mine.CollisionRectangle.Center.Y));
                    }
                }
            }

            foreach (Explosion explosion in explosions)
            {
                explosion.Update(gameTime);
            }

            for (int i = teddyBears.Count - 1; i >= 0; i--)
            {
                if (!teddyBears[i].Active)
                    teddyBears.RemoveAt(i);
            }

            for (int i = mines.Count - 1; i >= 0; i--)
            {
                if (!mines[i].Active)
                    mines.RemoveAt(i);
            }

            for (int i = explosions.Count - 1; i >= 0; i--)
            {
                if (!explosions[i].Playing)
                    explosions.RemoveAt(i);
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin();

            foreach (Mine mine in mines)
            {
                mine.Draw(spriteBatch);
            }

            foreach (TeddyBear bear in teddyBears)
            {
                bear.Draw(spriteBatch);
            }

            foreach (Explosion explosion in explosions)
            {
                explosion.Draw(spriteBatch);
            }


            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}

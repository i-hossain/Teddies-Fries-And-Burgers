using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ProgrammingAssignment4
{
    /// <summary>
    /// A teddy bear
    /// </summary>
    public class Teddy
    {
        #region Fields

        bool collecting = false;

        // drawing support
        Texture2D sprite;
        Rectangle drawRectangle;
        int halfDrawRectangleWidth;
        int halfDrawRectangleHeight;

        // moving support
        const float BASE_SPEED = 0.3f;
        Vector2 location;
        Vector2 velocity = Vector2.Zero;

        // click processing
        bool leftClickStarted = false;
        bool leftButtonReleased = true;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="sprite">sprite for the teddy</param>
        /// <param name="location">location of the center of the teddy</param>
        public Teddy(Texture2D sprite, Vector2 location)
        {
            this.sprite = sprite;
            this.location = location;


            // STUDENTS: set draw rectangle so teddy is centered on location
            drawRectangle = new Rectangle((int)location.X, (int)location.Y, sprite.Width, sprite.Height);

            // STUDENTS: set halfDrawRectangleWidth and halfDrawRectangleHeight for efficiency
            halfDrawRectangleWidth = drawRectangle.Width / 2;
            halfDrawRectangleHeight = drawRectangle.Height / 2;


        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets and sets whether or not the teddy is collecting
        /// </summary>
        public bool Collecting
        {
            get { return collecting; }
            set { collecting = value; }
        }

        /// <summary>
        /// Gets the collision rectangle for the teddy
        /// </summary>
        public Rectangle CollisionRectangle
        {
            get { return drawRectangle; }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Updates the teddy
        /// </summary>
        /// <param name="gameTime">game time</param>
        /// <param name="mouse">current mouse state</param>
        public void Update(GameTime gameTime, MouseState mouse)
        {
            // STUDENTS: update location based on velocity if teddy is collecting
            // You should update the location vector (which is at the center of
            // the teddy) first using the velocity vector and the elapsed game time,
            // then update the draw rectangle properties so the teddy is drawn
            // centered on the location vector
            // This gives us accurate movement toward the target so we don't miss
            // the target due to rounding error

            if (collecting)
            {
                location += velocity * gameTime.ElapsedGameTime.Milliseconds;

                drawRectangle.X = (int)location.X;
                drawRectangle.Y = (int)location.Y;
            }

            // check for mouse over teddy
            if (drawRectangle.Contains(mouse.X, mouse.Y))
            {
                // check for left click started on teddy
                if (mouse.LeftButton == ButtonState.Pressed &&
                    leftButtonReleased)
                {
                    leftClickStarted = true;
                    leftButtonReleased = false;
                }
                else if (mouse.LeftButton == ButtonState.Released)
                {
                    leftButtonReleased = true;

                    // if click finished on teddy, start collecting
                    if (leftClickStarted)
                    {
                        collecting = true;
                        leftClickStarted = false;
                    }
                }
            }
            else
            {
                // no clicking on teddy
                leftClickStarted = false;
                leftButtonReleased = false;
            }
        }

        /// <summary>
        /// Draws the teddy
        /// </summary>
        /// <param name="spriteBatch">sprite batch</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            // STUDENTS: use the sprite batch to draw the teddy
            spriteBatch.Draw(sprite, drawRectangle, Color.White);
        }

        /// <summary>
        /// Sets a target for the teddy to move toward
        /// </summary>
        /// <param name="target">target</param>
        public void SetTarget(Vector2 target)
        {
            // STUDENTS: set teddy velocity based on teddy center location and target
            // One way to do this is to create a new vector that consists of the
            // distances in x and y between the target and the teddy. Make sure to
            // normalize the vector, then multiply the vector by the teddy's base
            // speed
            Vector2 distance = new Vector2();
            distance = target - location;
            distance.Normalize();
            //Vector2.Normalize(distance);
            velocity = distance * BASE_SPEED;
        }

        #endregion
    }
}

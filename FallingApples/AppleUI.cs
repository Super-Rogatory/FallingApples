/********************************************************
 * Author: Chukwudi Ikem
 * Email: godofcollege43@csu.fullerton.edu
 * Course: CPSC 223N
 * Semester: Fall 2019
 * Assignment #: 5
 * Program name: FallingApples
*****************************************************/

using System;
using System.Windows;
using System.Drawing;
using System.Windows.Forms;

namespace FallingApples
{
    public class AppleUI : Form
    {
        Form mdiChild = new Form();
        protected static int width = 1920;
        protected const int radius = 16;
        protected static Bitmap bitmap_blue = new Bitmap(width, 600);
        protected static Bitmap bitmap_brown = new Bitmap(width, 860);
        protected static Bitmap bitmap_controls = new Bitmap(width, 1080);
        protected bool isOn = true; //TODO: for debug purposes it is on true, make sure to turn to false afterwards
        protected double FallingSpeed = 0.15;
        protected static double x_position = 50;
        protected static double y_position = 10;
        protected const double deltax = 0.635;
        protected static System.Timers.Timer BallClock = new System.Timers.Timer();
        protected static System.Timers.Timer GraphicClock = new System.Timers.Timer();
        public AppleUI()
        {
            Text = "Falling Apples Biome";
            Size = new Size(1920, 1080);
            BallClock.Elapsed += new System.Timers.ElapsedEventHandler(GravityTick); //while the clock is on, it will update ball position
            GraphicClock.Elapsed += new System.Timers.ElapsedEventHandler(GravityTick);
            BitMapBackGround(); //to create the static bitmaps for the background and the apple. Wanted to avoid invalidation on these paintings
        }
        protected void Form1_load(object sender, EventArgs e)
        {

        }
        protected static void BitMapBackGround()
        {
            // creating the fallingapplesarea
            using (Graphics gfx = Graphics.FromImage(bitmap_blue))
            using (SolidBrush brush = new SolidBrush(Color.SkyBlue))
            {
                gfx.FillRectangle(brush, new Rectangle(0, 0, width, 600));
            }
            using (Graphics gfx = Graphics.FromImage(bitmap_brown))
            using(SolidBrush brush = new SolidBrush(Color.SaddleBrown))
            {
                gfx.FillRectangle(brush, new Rectangle(0, 600, width, 860));
            }
            using (Graphics gfx = Graphics.FromImage(bitmap_controls))
            using (SolidBrush brush = new SolidBrush(Color.Black))
            {
                gfx.FillRectangle(brush, new Rectangle(0, 860, width, 1080));
            }
        }

        protected override void OnPaint(PaintEventArgs e) {

            // drawing the images
            e.Graphics.DrawImage(bitmap_blue, 0 , 0, width, 600);
            e.Graphics.DrawImage(bitmap_brown, 0, 0, width, 860);
            e.Graphics.DrawImage(bitmap_controls, 0, 0, width, 1080);

            Graphics apple = e.Graphics;
            apple.FillEllipse(Brushes.Red, 0, 0, radius, radius);
            if(isOn)
            {
                StartingBallClock();
                StartingGraphicClock();
            }
        }
        protected void StartingGraphicClock()
        {
            GraphicClock.Interval = Convert.ToInt32((1000.0 / 60.0));
            GraphicClock.Enabled = true;
        }
        protected void StartingBallClock()
        {
            BallClock.Interval = Convert.ToInt32((1000.0 / 60.0));
            BallClock.Enabled = true;
        }
        protected void GravityTick(object sender, EventArgs events)
        {
            // TODO: This function will carry in it the random number generator to denote where the ball will start at position(n,0)
            y_position += FallingSpeed;

        }

    }

}

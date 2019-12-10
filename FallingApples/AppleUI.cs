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

        protected static int width = 1920;
        protected const int radius = 16;
        protected const int X_OFFSET = 1; //x appears to be offset by 1
        protected const int Y_OFFSET = 65; //y appears to be offset by 65 pixels
        protected static Bitmap bitmap_blue = new Bitmap(width, 600);
        protected static Bitmap bitmap_brown = new Bitmap(width, 860);
        protected static Bitmap bitmap_controls = new Bitmap(width, 1080);
        protected static Bitmap bitmap_apple = new Bitmap(radius, radius);
        PictureBox bluepicture = new PictureBox();
        PictureBox brownpicture = new PictureBox();
        PictureBox controlspicture = new PictureBox();
       
        Random randomint = new Random();
        protected bool isOn;
        protected bool sendAnother;
        protected static double x_position = 0;
        protected static double y_position = 0;
        protected const double delta = 0.05;
        Button start, pause, resume, exit;
        protected static System.Timers.Timer BallClock = new System.Timers.Timer();
        protected static System.Timers.Timer GraphicClock = new System.Timers.Timer();
        public AppleUI()
        {
            Text = "Falling Apples Biome";
            Size = new Size(1920, 1080);
            BallClock.Elapsed += new System.Timers.ElapsedEventHandler(GravityTick); //while the clock is on, it will update ball position
            GraphicClock.Elapsed += new System.Timers.ElapsedEventHandler(GravityTick);
            /*
            bluepicture.Location = new Point(0, 0);
            bluepicture.Size = new Size(1920, 600);
            Controls.Add(bluepicture);
            bluepicture.Image = bitmap_blue;
            bluepicture.Paint += new PaintEventHandler(Paintbitmap_blue);

            brownpicture.Location = new Point(0, 600);
            brownpicture.Size = new Size(1920, 860);
            brownpicture.Image = bitmap_apple;
            Controls.Add(brownpicture);
            brownpicture.Paint += new PaintEventHandler(Paintbitmap_brown);
            */
            BitMapBackGround(); //to create the static bitmaps for the background and the apple. Wanted to avoid invalidation on these paintings
            RenderButtons();
        }
        /*
        void Paintbitmap_blue(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            using (Graphics gfx = Graphics.FromImage(bitmap_blue))
            using (SolidBrush brush = new SolidBrush(Color.SkyBlue))
            {
                gfx.FillRectangle(brush, new Rectangle(0, 0, width, 600));
            }
        }
        void Paintbitmap_brown(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            using (Graphics gfx = Graphics.FromImage(bitmap_brown))
            using (SolidBrush brush = new SolidBrush(Color.SaddleBrown))
            {
                gfx.FillRectangle(brush, new Rectangle(0, 600, width, 860));
            }
        }
        */
        protected void RenderButtons()
        {
            start = new Button { Text = "Start", Size = new Size(50, 30), Location = new Point(180, 900), BackColor = Color.GhostWhite };
            pause = new Button { Text = "Pause", Size = new Size(50, 30), Location = new Point(250, 900), BackColor = Color.GhostWhite };
            resume = new Button { Text = "Resume", Size = new Size(150, 60), Location = new Point(875, 900), BackColor = Color.Green, ForeColor = Color.GhostWhite };
            exit = new Button { Text = "Exit the Game", Size = new Size(100, 60), Location = new Point(1600, 900), BackColor = Color.Red, ForeColor = Color.White };
            Controls.Add(start);
            Controls.Add(pause);
            start.Click += Start;
            pause.Click += PauseMotion;
            resume.Click += ResumeMotion;
            exit.Click += EndApplication;
            MouseClick += mousePressed; //calls mousePressed whenever the mouse is click
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
            using (SolidBrush brush = new SolidBrush(Color.SaddleBrown))
            {
                gfx.FillRectangle(brush, new Rectangle(0, 600, width, 860));
            }
            using (Graphics gfx = Graphics.FromImage(bitmap_controls))
            using (SolidBrush brush = new SolidBrush(Color.Beige))
            {
                gfx.FillRectangle(brush, new Rectangle(0, 860, width, 1080));
            }
            using (Graphics gfx = Graphics.FromImage(bitmap_apple))
            using(SolidBrush brush = new SolidBrush(Color.Red))
            {
                gfx.FillEllipse(brush, Convert.ToInt32(x_position), Convert.ToInt32(y_position), radius, radius);
            }
        }
       
        protected void mousePressed(object sender, EventArgs events)
        {
            // this formula works
            double x = Math.Sqrt(Math.Pow((Convert.ToInt32(x_position) - (MousePosition.X - X_OFFSET)), 2));
            double y = Math.Sqrt(Math.Pow((Convert.ToInt32(y_position) - (MousePosition.Y - Y_OFFSET)), 2));
            double distance = x + y;
            Console.WriteLine(MousePosition.X - 1);
            Console.WriteLine(MousePosition.Y - 65);
            if(Convert.ToInt32(distance) <= radius)
            {
                Console.WriteLine("Perfect Click!");
            }
        }
        protected override void OnPaint(PaintEventArgs e)
        {

            // drawing the images
            /*e.Graphics.DrawImage(bitmap_blue, 0, 0, width, 600);
            e.Graphics.DrawImage(bitmap_brown, 0, 0, width, 860);
            e.Graphics.DrawImage(bitmap_controls, 0, 0, width, 1080);
            */
            e.Graphics.DrawImage(bitmap_apple, Convert.ToInt32(x_position), Convert.ToInt32(y_position), radius, radius); //bitmap for the ball

            if (isOn)
            {
                StartingBallClock();
                StartingGraphicClock();
            }
            switch (isOn)
            {
                case true:
                    if (sendAnother)
                    {
                        //Add to counter
                        y_position = 0;
                        sendAnother = false;
                    }
                    break;
                case false: //TODO: Occurs only when loop count is at 10. Display Exit message
                    break;
                default:
                    break;
            }


        }
        protected void StartingGraphicClock()
        {
            GraphicClock.Interval = 1.0;
        }
        protected void StartingBallClock()
        {
            BallClock.Interval = Convert.ToInt32((1000.0 / 17.0));
        }
        protected void GravityTick(object sender, EventArgs events)
        {
            y_position += delta;
            if ((int)Math.Round(y_position) == 860)
            {
                sendAnother = true;
            }
            Invalidate();
        }
        protected void Start(object sender, EventArgs myevent)
        {
            isOn = true;
            GraphicClock.Enabled = true;
            BallClock.Enabled = true;
        }
        protected void ResumeMotion(object sender, EventArgs events)
        {
            Controls.Add(start);
            Controls.Add(pause);
            Controls.Remove(resume);
            GraphicClock.Enabled = true;
            BallClock.Enabled = true;
        }
        protected void PauseMotion(object sender, EventArgs events)
        {
            // TODO: GAME PAUSED, CLICK RESUME TO CONTINUE
            isOn = false;
            GraphicClock.Enabled = false;
            BallClock.Enabled = false;
            Controls.Remove(pause);
            Controls.Remove(start);
            Controls.Add(resume);
            Controls.Add(exit);
        }
        protected void EndApplication(object sender, EventArgs myevent) => Close();
    }


}


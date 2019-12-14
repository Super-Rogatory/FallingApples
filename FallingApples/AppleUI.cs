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
        protected static int losses = 3; 
        protected static int wins = 0;
        protected string lossestext = String.Format("LIVES: {0}", losses.ToString());
        protected string winstext = String.Format("WINS: {0}", wins.ToString());
        protected static int width = 1920;
        protected const int radius = 16;
        protected const int X_OFFSET = 1; //x appears to be offset by 1
        protected const int Y_OFFSET = 65; //y appears to be offset by 65 pixels
        protected static Bitmap bitmap_blue = new Bitmap(width, 600);
        protected static Bitmap bitmap_brown = new Bitmap(width, 860);
        protected static Bitmap bitmap_controls = new Bitmap(width, 1080);
        protected static Bitmap bitmap_apple = new Bitmap(radius, radius);
        protected Label lossescounter, winscounter;
        PictureBox bluepicture = new PictureBox();
        PictureBox brownpicture = new PictureBox();
        PictureBox controlspicture = new PictureBox();
       
        Random randomint = new Random();
        protected bool isOn;
        protected bool sendAnother;
        protected bool addloss;
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
            BitMapBackGround(); //to create the static bitmaps for the background and the apple. Wanted to avoid invalidation on these paintings
            RenderButtons();
        }
      
        protected void RenderButtons()
        {
            start = new Button { Text = "Start", Size = new Size(50, 30), Location = new Point(180, 900), BackColor = Color.GhostWhite };
            pause = new Button { Text = "Pause", Size = new Size(50, 30), Location = new Point(250, 900), BackColor = Color.GhostWhite };
            resume = new Button { Text = "Resume", Size = new Size(150, 60), Location = new Point(875, 900), BackColor = Color.Green, ForeColor = Color.GhostWhite };
            exit = new Button { Text = "Exit the Game", Size = new Size(100, 60), Location = new Point(1600, 900), BackColor = Color.Red, ForeColor = Color.White };
            lossescounter = new Label { Text = lossestext, Size = new Size(60, 20), Location = new Point(600, 900), BackColor = Color.GhostWhite, Font = new Font(DefaultFont, FontStyle.Bold) };
            winscounter = new Label { Text = winstext, Size = new Size(55, 20), Location = new Point(700, 900), BackColor = Color.GhostWhite, Font = new Font(DefaultFont, FontStyle.Bold) };
            Controls.Add(start);
            Controls.Add(pause);
            Controls.Add(lossescounter);
            Controls.Add(winscounter);
            start.Click += Start;
            pause.Click += PauseMotion;
            resume.Click += ResumeMotion;
            exit.Click += EndApplication;
            MouseClick += MouseClickA; //calls mousePressed whenever the mouse is click

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
        private void MouseClickA(object sender, MouseEventArgs e) // easy more, more give on the pixels. TODO: Develop a hard mode, faster balls, less give
        {
            // this formula works
            double x = Math.Sqrt(Math.Pow(Convert.ToInt32(x_position) - e.Location.X, 2));
            double y = Math.Sqrt(Math.Pow(Convert.ToInt32(y_position) - e.Location.Y, 2));
            double distance = x + y;
            if (y_position > 600) //TODO:
            {
                if (Convert.ToInt32(distance) <= (radius + 8)) 
                {
                    // GOOD, gain one point
                    wins = wins + 1;
                    winscounter.Text = String.Format("WINS: {0}", wins.ToString());
                    sendAnother = true; 
                }
            }
        }
        protected override void OnPaint(PaintEventArgs e)
        {

            // drawing the images

            e.Graphics.DrawImage(bitmap_blue, 0, 0, width, 600);
            e.Graphics.DrawImage(bitmap_brown, 0, 0, width, 860);
            e.Graphics.DrawImage(bitmap_controls, 0, 0, width, 1080);

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
                        if (losses.Equals(0))
                        {
                            // TODO: GAME PAUSED, CLICK RESUME TO CONTINUE
                            isOn = false;
                            GraphicClock.Enabled = false;
                            BallClock.Enabled = false;
                        }
                        if (wins.Equals(10))
                        {
                            // TODO: GAME PAUSED, CLICK RESUME TO CONTINUE
                            isOn = false;
                            GraphicClock.Enabled = false;
                            BallClock.Enabled = false;
                        }
                        x_position = randomint.Next(16, 1904);
                        y_position = 0;
                        sendAnother = false;
                    }
                    else if (addloss)
                    {
                        losses = losses - 1;
                        lossescounter.Text = String.Format("LIVES: {0}", losses.ToString());
                        addloss = false;
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
            GraphicClock.Interval = 0.00001;
        }
        protected void StartingBallClock()
        {
            BallClock.Interval = 0.00001;
        }
        protected void GravityTick(object sender, EventArgs events)
        {
            y_position += delta;
            if ((int)Math.Round(y_position) == 844)
            {
                // BAD, Lose one point
                sendAnother = true;
                addloss = true;
            }
            Invalidate();
        }
        protected void Start(object sender, EventArgs myevent)
        {
            isOn = true;
            GraphicClock.Enabled = true;
            BallClock.Enabled = true;
            losses = 3;
            lossescounter.Text = String.Format("LIVES: {0}", losses.ToString());
            x_position = randomint.Next(16, 1904);
        }
        protected void ResumeMotion(object sender, EventArgs events)
        {
            isOn = true;
            GraphicClock.Enabled = true;
            BallClock.Enabled = true;
            Controls.Add(start);
            Controls.Add(pause);
            Controls.Remove(resume);
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


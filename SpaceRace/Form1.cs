using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpaceRace
{
    public partial class Form1 : Form
    {
        string state = "waiting";

        //Drawn variables
        Rectangle border = new Rectangle(260, 0, 10, 1000); 

        Rectangle player1 = new Rectangle(250, 800, 30, 30);
        Rectangle player2 = new Rectangle(100, 300, 30, 30); 

        int heroSpeed = 10;
        int p1Score = 0;
        int p2Score = 0;
        int count = 0;

        //Ball Variables
        int ballWidth = 20;
        int ballHeight = 7;

        //List of balls
        List<Rectangle> balllist = new List<Rectangle>();
        List<Rectangle> balllist2 = new List<Rectangle>();
        List<int> ballSpeeds = new List<int>();

        //Input Buttons
        bool upDown = false;
        bool downDown = false;
        bool wDown = false;
        bool sDown = false;

        SolidBrush whiteBrush = new SolidBrush(Color.White);
        SolidBrush metalBrush = new SolidBrush(Color.Silver);
        Pen whitePen = new Pen(Color.White);
        Pen metalPen = new Pen(Color.Silver);


        Random randGen = new Random();
        int randValue = 0;

        public Form1()
        {
            InitializeComponent();

            Rectangle ball = new Rectangle(80, 0, ballWidth, ballHeight);
            balllist.Add(ball);
        }

        public void InitializeGame()
        {
            balllist.Clear(); 
            ballSpeeds.Clear();
            p1Score = 0;
            p2Score = 0;

            player1 = new Rectangle(100, 600, 20, 20);
            player2 = new Rectangle(380, 600, 20, 20);
            border = new Rectangle(260, 0, 10, 1000);
            titleLabel.Text = "";
            subtitleLabel.Text = "";
            scoreLabel1.Text = $"{p1Score}";
            scoreLabel2.Text = $"{p2Score}";

            Rectangle ball = new Rectangle(randValue, 0, ballWidth, ballHeight);
            balllist.Add(ball);
            ballSpeeds.Add(randGen.Next(2, 20));

            state = "playing";
            gameTimer.Enabled = true;

        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Up:
                    upDown = true;
                    break;
                case Keys.Down:
                    downDown = true;
                    break;
                    
                case Keys.W:
                    wDown = true;
                    break;
                case Keys.S:
                    sDown = true;
                    break;

                case Keys.Space:
                    if (state == "waiting" || state == "end")
                    {
                        InitializeGame();
                    }
                    break;
                case Keys.Escape:
                    if (state == "waiting" || state == "end")
                    {
                        Application.Exit();
                    }
                    break;
            }
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Up:
                    upDown = false;
                    break;
                case Keys.Down:
                    downDown = false;
                    break;

                case Keys.W:
                    wDown = false;
                    break;
                case Keys.S:
                    sDown = false;
                    break;
            }
        }

        private void gameTimer_Tick(object sender, EventArgs e)
        {
            count++;

            //Player 2 Movement 
            if (upDown == true)
            {
                player2.Y -= heroSpeed;
            }

            if (downDown == true && player2.Y < this.Height - player2.Height - 5)
            {
                player2.Y += heroSpeed;
            }

            //Player 1 Movement
            if (wDown == true)
            {
                player1.Y -= heroSpeed;
            }

            if (sDown == true && player1.Y < this.Height - player1.Height - 5)
            {
                player1.Y += heroSpeed;
            }

            //move the ball rectangles on the left
            for (int i = 0; i < balllist.Count; i++)
            {
                int x = balllist[i].X + ballSpeeds[i];
                balllist[i] = new Rectangle(x, balllist[i].Y, ballWidth, ballHeight);
            }

            //move the ball rectangles on the right
            for (int i = 0; i < balllist2.Count; i++)
            {
                ballSpeeds.Add(randGen.Next(2, 20));
                int x = balllist2[i].X - ballSpeeds[i];
                balllist2[i] = new Rectangle(x, balllist2[i].Y, ballWidth, ballHeight);
            }


            //generate first new ball object 
            randValue = randGen.Next(0, 10);

            if (randValue <= 2)
            {
                randValue = randGen.Next(60, this.Width - ballWidth - 20);

                Rectangle ball = new Rectangle(0, randValue, ballWidth, ballHeight);
                balllist.Add(ball);
                ballSpeeds.Add(randGen.Next(2, 20));
            }

            //generate second new ball object
            randValue = randGen.Next(0, 10);

            if (randValue <= 2)
            {
                randValue = randGen.Next(60, this.Width - ballWidth - 20);

                Rectangle ball = new Rectangle(800, randValue, ballWidth, ballHeight);
                balllist2.Add(ball);
            }
            //remove ball if it hits ground
            for (int i = 0; i < balllist.Count; i++)
            {
                if (balllist[i].Y >= this.Width)//if ball has reached the ground
                {
                    balllist.RemoveAt(i);
                } 
            }
            //check if ball has hit hero
            for (int i = 0; i < balllist.Count; i++)
            {
                if (balllist[i].IntersectsWith(player1))
                {
                    player1.Y = 600;
                }

                if (balllist[i].IntersectsWith(player2))
                {
                    player2.Y = 600;
                }
            }
            //check if second ball has hit hero
            for (int i = 0; i < balllist2.Count; i++)
            {
                if (balllist2[i].IntersectsWith(player1))
                {
                    player1.Y = 600;
                }

                if (balllist2[i].IntersectsWith(player2))
                {
                    player2.Y = 600;
                }
            }

            //check if ball has reached the end
            if (player1.Y < 0)
                {
                    player1.Y = 600;
                    p1Score++;
                }

            if (player2.Y < 0)
                {
                    player2.Y = 600;
                    p2Score++;
                }

         
            if(count % 3 == 0)
            {
                border.Y++;
            }

            if(border.Y > this.Height)
            {
                state = "end";
            }

            //redraw the screen
            Refresh();

        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {

            if (state == "waiting")
            {
                titleLabel.Text = "Space Race";
                subtitleLabel.Text = "Press [Space] to Play and [Esc] to Exit";
                scoreLabel1.Text = "";
                scoreLabel2.Text = "";
            }

            if (state == "playing")
            {

                //draw players
                e.Graphics.FillEllipse(metalBrush, player1);
                e.Graphics.FillEllipse(metalBrush, player2);


                //draw border
                e.Graphics.FillRectangle(whiteBrush, border); 


                //draw balls
                for (int i = 0; i < balllist.Count; i++)
                {
                    e.Graphics.FillRectangle(whiteBrush, balllist[i]);
                }

                for (int i = 0; i < balllist2.Count; i++)
                {
                    e.Graphics.FillRectangle(whiteBrush, balllist2[i]);
                }

                scoreLabel1.Text = $"{p1Score}";
                scoreLabel2.Text = $"{p2Score}";

            }

            if (state == "end" && p1Score > p2Score)
            {
                titleLabel.Text = "Player 1 Wins!";
                subtitleLabel.Text = "Press [Space] to Play or [Esc] to Exit";

            }
            else if (state == "end" && p1Score < p2Score)
            {
                titleLabel.Text = "Player 2 Wins!";
                subtitleLabel.Text = "Press [Space] to Play or [Esc] to Exit";

            }
            else if (state == "end" && p1Score == p2Score)
            {
                titleLabel.Text = "Players Tie!";
                subtitleLabel.Text = "Press [Space] to Play or [Esc] to Exit";

            }
           
        }
    }
}


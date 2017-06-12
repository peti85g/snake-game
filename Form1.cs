using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SnakeGame
{
    public partial class Form1 : Form
    {
        #region Constructor
        #region Variables
        int score = 0;
        bool gameover = false;
        int direction = 0;//0=down 1=left 2=right, 3=up
        List<SnakePart> snake = new List<SnakePart>();
        SnakePart food;
        Timer gameLoop = new Timer();
        Timer snakeLoop = new Timer();
        float snakeSpeed = 1.5f;

        #endregion
        public Form1()
        {
            InitializeComponent();
            gameLoop.Tick += new EventHandler(Update);
            snakeLoop.Tick += new EventHandler(UpdateSnake);
            gameLoop.Interval = 1000 / 60;
            snakeLoop.Interval = (int)(1000 / snakeSpeed);
            gameLoop.Start();
            snakeLoop.Start();
            StartGame();
        }


        #endregion
        #region formEvents
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            Input.ChangeState(e.KeyCode, true);
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            Input.ChangeState(e.KeyCode, false);
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            Draw(e.Graphics);
        }
        #endregion
        #region Game Logic
        private void StartGame()
        {
            gameover = false;
            snake.Clear();
            snakeSpeed = 1.5f;
            snakeLoop.Interval = (int)(1000 / snakeSpeed);
            score = 0;
            SnakePart head = new SnakePart(10,8);
            snake.Add(head);
            GenerateFood();
        }
        private void Update(object sender,EventArgs e)
        {
            //sticky part!!!!!!!!!!!!!!
            if (gameover)
            {
                if (Input.Press(Keys.Enter))
                {
                    StartGame();
                }
            }
            else
            {
                if (Input.Press(Keys.Left))
                {
                    if (snake.Count<2||snake[0].X==snake[1].X)
                    {
                        direction = 1;
                    }
                }
                else if (Input.Press(Keys.Right))
                {
                    if (snake.Count < 2 || snake[0].X == snake[1].X)
                    {
                        direction = 2;
                    }
                }
                else if (Input.Press(Keys.Down))
                {
                    if (snake.Count < 2 || snake[0].Y == snake[1].Y)
                    {
                        direction = 0;
                    }
                }
                else if (Input.Press(Keys.Up))
                {
                    if (snake.Count < 2 || snake[0].Y == snake[1].Y)
                    {
                        direction = 3;
                    }
                }
            }
            pictureBox1.Invalidate();
        }
        private void UpdateSnake(object sender, EventArgs e)
        {
            if (!gameover)
            {
                for (int i = snake.Count-1; i >= 0; i--)
                {
                    if (i==0)
                    {
                        switch (direction)
                        {
                            case 0:
                                snake[0].Y++;
                                break;
                            case 1:
                                snake[0].X--;
                                break;
                            case 2:
                                snake[0].X++;
                                break;
                            case 3:
                                snake[0].Y--;
                                break;
                        }
                        //falnak mentünk-e?
                        SnakePart head = snake[0];
                        if (head.X>=20||head.X<0||head.Y>=15||head.Y<0)
                        {
                            GameOver();
                        }
                        //magunknak mentünk-e?
                        for (int j = 1; j < snake.Count; j++)
                        {
                            if (head.X==snake[j].X&&head.Y==snake[j].Y)
                            {
                                GameOver();
                            }
                        }
                        //kajalas
                        if (head.X==food.X&&head.Y==food.Y)
                        {
                            SnakePart part = new SnakePart(snake[snake.Count - 1].X, snake[snake.Count - 1].Y);
                            snake.Add(part);
                            GenerateFood();
                            score++;
                            if (snakeSpeed<30)
                            {
                                snakeSpeed = snakeSpeed + 0.5f;
                                snakeLoop.Interval = (int)(1000 / snakeSpeed);
                            }
                        }
                    }
                    else
                    {
                        snake[i].X = snake[i - 1].X;
                        snake[i].Y = snake[i - 1].Y;
                    }
                }
            }
        }
        private void Draw(Graphics canvas)
        {
            Font font = this.Font;
            if (gameover)
            {

                SizeF message = canvas.MeasureString("Game Over", font);
                canvas.DrawString("Game Over", font, Brushes.Black, new PointF(160 - message.Width / 2, 80));
                message = canvas.MeasureString("Final Score" + score.ToString(), font);
                canvas.DrawString("Final Score" + score.ToString(), font, Brushes.Black, new PointF(160 - message.Width / 2, 100));
                message = canvas.MeasureString("Press Enter to start new game!", font);
                canvas.DrawString("Press Enter to start new game!", font, Brushes.Black, new PointF(160 - message.Width / 2, 120));
            }
            else
            {
                canvas.DrawString("Score " + score.ToString(), font, Brushes.Black, new PointF(4, 4));
                for (int i = 0; i < snake.Count; i++)
                {
                    Color snake_color = i == 0 ? Color.Red : Color.Black;
                    SnakePart currentPart=snake[i];
                    canvas.FillRectangle(new SolidBrush(snake_color), new Rectangle(currentPart.X * 16, currentPart.Y * 16, 16, 16));
                }
                canvas.FillRectangle(new SolidBrush(Color.Orange), new Rectangle(food.X * 16, food.Y * 16, 16, 16));
            }

        }
        private void GameOver()
        {
            gameover = true;
        }
        private void GenerateFood()
        {
            Random random = new Random();
            food = new SnakePart(random.Next(0, 20), random.Next(0, 15));
        }
        #endregion

        public int snakeRate { get; set; }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            About a = new About();
            a.Show();
        }
    }
}

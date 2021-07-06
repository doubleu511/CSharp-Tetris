using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinFormsTetris
{
    public partial class Form1 : Form
    {
        Game game;
        Board board;
        int bx;
        int by;
        int bwidth;
        int bheight;

        public Form1()  //생성자
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e) //Start()
        {
            game = Game.Singleton;
            board = Board.GameBoard;
            bx = GameRule.BX;
            by = GameRule.BY;
            bwidth = GameRule.B_WIDTH;
            bheight = GameRule.B_HEIGHT;
            this.SetClientSizeCore(GameRule.BX * GameRule.B_WIDTH + 100, GameRule.BY * GameRule.B_HEIGHT);
        }

        private void Form1_Paint(object sender, PaintEventArgs e) //Update()
        {
            DoubleBuffered = true;
            Bitmap myImage = new Bitmap("tetris.png");
            Bitmap myImage2 = new Bitmap(myImage, new Size(bx * bwidth, by * bheight));
            e.Graphics.DrawImage(myImage2, 0, 0);

            DrawGraduation(e.Graphics);
            DrawDiagram(e.Graphics);
            DrawBoard(e.Graphics);
            DrawNext(e.Graphics);
            DrawHold(e.Graphics);
            MoveFast();

            label1.Text = $"Score: {board.score}";
            label3.Text = $"LEVEL {board.level}";
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Right: MoveRight(); return;
                case Keys.Left: MoveLeft(); return;                
                case Keys.Up: MoveTurn(); return;
                case Keys.Down: MoveDown(); return;
                case Keys.Space: MoveSSDown(); return;
                case Keys.Z: BlockHold(); return;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)  //Update()
        {
            MoveDown();
        }

        private void DrawNext(Graphics graphics)
        {
            int bn = game.NextBlockNum;
            Pen dpen = new Pen(Color.Black, 3);
            Brush dbrush = Brushes.Red;

            switch (game.NextBlockNum)
            {
                case 0:
                    dbrush = Brushes.Orange;
                    break;
                case 1:
                    dbrush = Brushes.Green;
                    break;
                case 2:
                    dbrush = Brushes.Blue;
                    break;
                case 3:
                    dbrush = Brushes.Aqua;
                    break;
                case 4:
                    dbrush = Brushes.Purple;
                    break;
                case 5:
                    dbrush = Brushes.Pink;
                    break;
                case 6:
                    dbrush = Brushes.Black;
                    break;
                default:
                    dbrush = Brushes.Red;
                    break;
            }

            for (int xx = 0; xx < 4; xx++)
            {
                for (int yy = 0; yy < 4; yy++)
                {
                    if (BlockValue.bvals[bn, 0, xx, yy] != 0)
                    {
                        Rectangle now_rt = new Rectangle((25 + xx) * (bwidth / 2) + 1, (5 + yy) * (bheight / 2) + 1, (bwidth / 2) - 2, (bheight / 2) - 2);
                        graphics.FillRectangle(dbrush, now_rt);
                    }
                }
            }
        }

        private void DrawHold(Graphics graphics)
        {
            int bn = game.HoldBlockNum;

            if (bn == -1) return;

            Pen dpen = new Pen(Color.Black, 3);
            Brush dbrush = Brushes.Red;

            switch (game.HoldBlockNum)
            {
                case 0:
                    dbrush = Brushes.Orange;
                    break;
                case 1:
                    dbrush = Brushes.Green;
                    break;
                case 2:
                    dbrush = Brushes.Blue;
                    break;
                case 3:
                    dbrush = Brushes.Aqua;
                    break;
                case 4:
                    dbrush = Brushes.Purple;
                    break;
                case 5:
                    dbrush = Brushes.Pink;
                    break;
                case 6:
                    dbrush = Brushes.Black;
                    break;
                default:
                    dbrush = Brushes.White;
                    break;
            }

            for (int xx = 0; xx < 4; xx++)
            {
                for (int yy = 0; yy < 4; yy++)
                {
                    if (BlockValue.bvals[bn, 0, xx, yy] != 0)
                    {
                        Rectangle now_rt = new Rectangle((25 + xx) * (bwidth / 2) + 1, (12 + yy) * (bheight / 2) + 1, (bwidth / 2) - 2, (bheight / 2) - 2);
                        graphics.FillRectangle(dbrush, now_rt);
                    }
                }
            }
        }

        private void DrawBoard(Graphics graphics)
        {
            for (int xx = 0; xx < bx; xx++)
            {
                for (int yy = 0; yy < by; yy++)
                {
                    if (game[xx, yy] != 0)
                    {
                        Rectangle now_rt = new Rectangle(xx * bwidth + 1, yy * bheight + 1, bwidth - 2, bheight - 2);
                        Brush dbrush = Brushes.Red;

                        switch (game[xx, yy])
                        {
                            case 1:
                                dbrush = Brushes.Orange;
                                break;
                            case 2:
                                dbrush = Brushes.Green;
                                break;
                            case 3:
                                dbrush = Brushes.Blue;
                                break;
                            case 4:
                                dbrush = Brushes.Aqua;
                                break;
                            case 5:
                                dbrush = Brushes.Purple;
                                break;
                            case 6:
                                dbrush = Brushes.Pink;
                                break;
                            case 7:
                                dbrush = Brushes.Black;
                                break;
                            default:
                                dbrush = Brushes.Red;
                                break;
                        }
                        graphics.DrawRectangle(Pens.DarkGray, now_rt);
                        graphics.FillRectangle(dbrush, now_rt);
                    }
                }
            }
        }

        private void DrawDiagram(Graphics graphics)
        {
            int bn = game.BlockNum;
            int tn = game.Turn;
            Point now = game.NowPosition;
            Pen dpen = new Pen(Color.Black, 3);
            Brush dbrush = Brushes.Red;

            switch (game.BlockNum)
            {
                case 0:
                    dbrush = Brushes.Orange;
                    break;
                case 1:
                    dbrush = Brushes.Green;
                    break;
                case 2:
                    dbrush = Brushes.Blue;
                    break;
                case 3:
                    dbrush = Brushes.Aqua;
                    break;
                case 4:
                    dbrush = Brushes.Purple;
                    break;
                case 5:
                    dbrush = Brushes.Pink;
                    break;
                case 6:
                    dbrush = Brushes.Black;
                    break;
                default:
                    dbrush = Brushes.Red;
                    break;
            }

            for (int xx = 0; xx < 4; xx++)
            {
                for (int yy = 0; yy < 4; yy++)
                {
                    if (BlockValue.bvals[bn, tn, xx, yy] != 0)
                    {
                        Rectangle now_rt = new Rectangle((now.X + xx) * bwidth + 1, (now.Y + yy) * bheight + 1, bwidth - 2, bheight - 2);
                        graphics.FillRectangle(dbrush, now_rt);


                        int ay = 0;
                        while (board.MoveEnable(bn, tn, now.X, now.Y + ay))
                        {
                            ay++;
                        }

                        Color clr = Color.FromArgb(128,0 ,0, 0);
                        SolidBrush semiTransBrush = new SolidBrush(clr);
                        Rectangle see_rt = new Rectangle((now.X + xx) * bwidth + 1, (ay + yy + now.Y -1) * bheight + 1, bwidth - 2, bheight - 2);
                        graphics.FillRectangle(semiTransBrush, see_rt);
                    }
                }
            }
        }

        private void DrawGraduation(Graphics graphics)
        {
            DrawHorizons(graphics);
            DrawVerticals(graphics);
        }

        private void DrawVerticals(Graphics graphics)
        {
            Point st = new Point();
            Point et = new Point();
            Pen dpen = new Pen(Color.Black, 1);

            for (int cx = 0; cx < bx; cx++)
            {
                st.X = cx * bwidth;
                st.Y = 0;
                et.X = st.X;
                et.Y = by * bheight;
                graphics.DrawLine(dpen, st, et);
            }
        }

        private void DrawHorizons(Graphics graphics)
        {
            Point st = new Point();
            Point et = new Point();
            Pen dpen = new Pen(Color.Black, 1);

            for (int cy = 0; cy < by; cy++)
            {
                st.X = 0;
                st.Y = cy * bheight;
                et.X = bx * bwidth;
                et.Y = cy * bheight;
                graphics.DrawLine(dpen, st, et);
            }
        }

        private void MoveTurn()
        {
            if (game.MoveTurn())
            {
                Region rg = MakeRegion();
                Invalidate(rg);
            }
        }

        private void MoveLeft()
        {
            if (game.MoveLeft())
            {
                Region rg = MakeRegion(1, 0);
                Invalidate(rg);
            }
        }

        private void MoveRight()
        {
            if (game.MoveRight())
            {
                Region rg = MakeRegion(-1, 0);
                Invalidate(rg);
            }
        }

        private void MoveDown()
        {
            if (game.MoveDown())
            {
                Region rg = MakeRegion(0, -1);
                Invalidate(rg);
            }
            else
            {
                EndingCheck();
            }
        }

        private void MoveSSDown()
        {
            while (game.MoveDown()) 
            {
                Region rg = MakeRegion(0, -1);
                Invalidate(rg);
            }
            EndingCheck();
        }

        private void BlockHold()
        {
            game.Hold();
            Region rg = MakeRegion(0, 0);
            Invalidate(rg);
        }

        private void EndingCheck()
        {
            if (game.Next())
            {
                Invalidate(); //전체 영역 갱신
            }
            else
            {
                timer1.Enabled = false;
                game._mediaPlayer.Stop();

                if (DialogResult.Yes == MessageBox.Show($"Score: {board.score}점\n계속 하실건가요?", "GAME OVER", MessageBoxButtons.YesNo))
                {
                    game.ReStart();
                    timer1.Enabled = true;
                    Invalidate();
                }
                else
                {
                    this.Close();
                }
            }
        }

        private Region MakeRegion(int cx, int cy) //갱신할 영역 계산
        {
            Point now = game.NowPosition;

            int bn = game.BlockNum;
            int tn = game.Turn;
            Region region = new Region();
            for (int xx = 0; xx < 4; xx++)
            {
                for (int yy = 0; yy < 4; yy++)
                {
                    if (BlockValue.bvals[bn, tn, xx, yy] != 0)
                    {
                        Rectangle rect1 = new Rectangle((now.X + xx) * bwidth + 1, (now.Y + yy) * bheight + 1, bwidth - 2, bheight - 2);
                        Rectangle rect2 = new Rectangle((now.X + cx + xx) * bwidth, (now.Y + cy + yy) * bheight, bwidth, bheight);
                        Region rg1 = new Region(rect1);
                        Region rg2 = new Region(rect2);
                        region.Union(rg1);
                        region.Union(rg2);
                    }
                }
            }
            return region;
        }

        private Region MakeRegion() 
        {
            Point now = game.NowPosition;
            int bn = game.BlockNum;
            int tn = game.Turn;
            int oldtn = (tn + 3) % 4;

            Region region = new Region();
            for (int xx = 0; xx < 4; xx++)
            {
                for (int yy = 0; yy < 4; yy++)
                {
                    if (BlockValue.bvals[bn, tn, xx, yy] != 0)
                    {
                        Rectangle rect1 = new Rectangle((now.X + xx) * bwidth + 1, (now.Y + yy) * bheight + 1, bwidth - 2, bheight - 2);
                        Region rg1 = new Region(rect1);
                        region.Union(rg1);
                    }
                    if (BlockValue.bvals[bn, oldtn, xx, yy] != 0)
                    {
                        Rectangle rect1 = new Rectangle((now.X + xx) * bwidth + 1, (now.Y + yy) * bheight + 1, bwidth - 2, bheight - 2);
                        Region rg1 = new Region(rect1);
                        region.Union(rg1);
                    }
                }
            }
            return region;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void MoveFast()
        {
            board.level = board.score / 1000;

            if (timer1.Interval > 100)
            {
                timer1.Interval = 1000 - (board.level * 75);
            }
            else timer1.Interval = 100;
        }
    }
}

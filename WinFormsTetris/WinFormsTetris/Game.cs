using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace WinFormsTetris
{
    class Game
    {        
        Board gboard = Board.GameBoard;
        Diagram now;
        Diagram next;
        Diagram hold;
        public MediaPlayer.MediaPlayerClass _mediaPlayer = new MediaPlayer.MediaPlayerClass();

        public Point NowPosition
        {
            get { return new Point(now.X, now.Y); }
        }
        public int BlockNum
        {
            get { return now.BlockNum; }
        }
        public int NextBlockNum
        {
            get { return next.BlockNum; }
            set { next.BlockNum = value; }
        }
        public int HoldBlockNum
        {
            get { return hold.BlockNum; }
            set { hold.BlockNum = value; }
        }
        public int Turn
        {
            get { return now.Turn; }
        }

        public static Game Singleton { get; }

        public int this[int x, int y]
        {
            get { return gboard[x, y]; }
        }

        static Game()
        {
            Singleton = new Game();
        }
        Game()
        {
            now = new Diagram(); 
            next = new Diagram();
            hold = new Diagram();
            hold.BlockNum = -1;
            next.Reset();
            _mediaPlayer.FileName = "tetris.mp3";
            _mediaPlayer.PlayCount = 1000;
            _mediaPlayer.Play();
        }

        public bool MoveLeft()
        {
            if (gboard.MoveEnable(now.BlockNum, Turn, now.X - 1, now.Y))
            {
                now.MoveLeft();
                return true;
            }
            return false;
        }

        public bool MoveRight()
        {
            if (gboard.MoveEnable(now.BlockNum, Turn, now.X + 1, now.Y))
            {
                now.MoveRight();
                return true;
            }
            return false;
        }

        public bool MoveDown()
        {
            if (gboard.MoveEnable(now.BlockNum, Turn, now.X, now.Y + 1))
            {
                now.MoveDown();
                return true;
            }
            gboard.Store(now.BlockNum, Turn, now.X, now.Y); //보드 정보 갱신
            return false;
        }

        public bool MoveTurn()
        {
            if (gboard.MoveEnable(now.BlockNum, (Turn + 1) % 4, now.X, now.Y))
            {
                now.MoveTurn();
                return true;
            }
            return false;
        }

        public void Hold()
        {
            now.Hold();
        }

        public bool Next()
        {
            next.Copy(now);
            next.Reset();
            return gboard.MoveEnable(now.BlockNum, Turn, now.X, now.Y);
        }

        public void ReStart()
        {
            gboard.ClearBoard();
            gboard.score = 0;
            _mediaPlayer.Play();
        }
    }
}

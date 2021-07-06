using System;
using System.Collections.Generic;
using System.Text;

namespace WinFormsTetris
{
    class Diagram       //테트리스 떨어지는 블럭, 다이아그램
    {
        Game game;

        public int X { get; private set; }
        public int Y { get; private set; }
        public int Turn { get; private set; }
        public int BlockNum { get; set; }

        public Diagram()
        {
            Reset();
        }

        public void Reset()   // 다이아그램 생성
        {
            Random rand = new Random();
            X = GameRule.SX;
            Y = GameRule.SY;
            Turn = rand.Next() % 4;
            BlockNum = rand.Next() % 7;
        }

        public void Copy(Diagram d)   // 다이아그램 생성
        {
            d.BlockNum = this.BlockNum;
            d.Turn = this.Turn;
            d.X = this.X;
            d.Y = this.Y;
        }

        public void MoveLeft()  { X--; }
        public void MoveRight() { X++; }
        public void MoveDown()  { Y++; }
        public void MoveTurn()  { Turn = (Turn + 1) % 4; }
        public void Hold()
        {
            game = Game.Singleton;
            int temp = BlockNum;
            if (game.HoldBlockNum != -1)
            {
                BlockNum = game.HoldBlockNum;
                game.HoldBlockNum = temp;
            }
            else
            {
                game.HoldBlockNum = temp;
                // 여기에 지금 나오는 블록 지우고 다음 블록 나오게 해야함
                Reset();
            }
        }
    }
}

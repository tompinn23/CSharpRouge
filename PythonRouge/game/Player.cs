using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RLNET;

namespace PythonRouge.game
{
    class Player
    {
        private int xPos;
        private int yPos;
        private int health;
        public char character;
        public string name;

        public Player(int x, int y, int health, string name, char character)
        {
            this.xPos = x;
            this.yPos = y;
            this.health = health;
            this.name = name;
            this.character = character;
        }



        //Move method updates player pos in change of y and x
        public void move(int dx, int dy)
        {
            this.xPos += dx;
            this.yPos += dy;
        }

        public void draw(RLConsole console)
        {
            console.Set(this.xPos, this.yPos, RLColor.White, null, this.character);
        }

        public void clear(RLConsole console)
        {
            console.Set(this.xPos, this.yPos, null, null, ' ');
        }

    }
}

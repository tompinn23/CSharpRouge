using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RLNET;

namespace PythonRouge.game
{
    [Serializable]
    public class Player 
    {
        public EntityPos pos;
        private int health;
        public char character;
        public string name;

        public Player(int x, int y, int health, char character, string name)
        {
            this.pos = new EntityPos(x, y);
            this.health = health;
            this.name = name;
            this.character = character;
        }



        //Move method updates player pos in change of y and x
        public void move(int dx, int dy)
        {
            this.pos.x += dx;
            this.pos.y += dy;
        }

        public void draw(RLConsole console)
        {
            console.Set(this.pos.x, this.pos.y, RLColor.White, null, this.character);
        }

        public void clear(RLConsole console)
        {
            console.Set(this.pos.x, this.pos.y, null, null, ' ');
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RLNET;

namespace PythonRouge.game
{
    class Entity
    {
        public int x;
        public int y;
        public bool dead;
        public int health;
        public string entityType;
        public char character;

        public Entity(int x, int y, bool dead, int health, char character, string entityType)
        {
            this.x = x;
            this.y = y;
            this.dead = dead;
            this.health = health;
            this.entityType = entityType;
            this.character = character;
        }
        void draw(RLConsole console)
        {
            console.SetChar(this.x, this.y, this.character);
        }

        void clear(RLConsole console)
        {
            console.SetChar(this.x, this.y, ' ');
        }
    }
}

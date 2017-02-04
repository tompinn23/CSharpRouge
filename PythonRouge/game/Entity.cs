using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RLNET;

namespace PythonRouge.game
{
    /// <summary>
    /// Custom structure that is pretty much just a Vector2 holds x and y for any entity.
    /// </summary>
    public struct EntityPos
    {
        public int x;
        public int y;

        public EntityPos(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }

    class Entity
    {
        public EntityPos pos;
        public int health;
        public string entityType;
        public char character;
        /// <summary>
        /// The main constructor of the entity class pos is defined along with type, symbol, health etc.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="health"></param>
        /// <param name="character"></param>
        /// <param name="entityType"></param>
        public Entity(int x, int y, int health, char character, string entityType)
        {
            pos = new EntityPos(x, y);
            this.health = health;
            this.entityType = entityType;
            this.character = character;
        }
        /// <summary>
        /// The draw method of the entity.
        /// You need to pass the subconsole it is being drawn onto.
        /// </summary>
        /// <param name="console"></param>
        void draw(RLConsole console)
        {
            console.SetChar(this.pos.x, this.pos.y, this.character);
        }

        /// <summary>
        /// Clears the entity symbol.
        /// </summary>
        /// <param name="console"></param>
        void clear(RLConsole console)
        {
            console.SetChar(this.pos.x, this.pos.y, ' ');
        }
    }
}

// CSharpRouge Copyright (C) 2017 Tom Pinnock
// 
// This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied
// warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more
//  details.
// 
// You should have received a copy of the GNU General Public License along with this program. If not, see
// http://www.gnu.org/licenses/.
using RLNET;

namespace PythonRouge.game
{
    /// <summary>
    ///     Custom structure that is pretty much just a Vector2 holds x and y for any entity.
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

    internal class Entity
    {
        public char character;
        public string entityType;
        public int health;
        public EntityPos pos;

        /// <summary>
        ///     The main constructor of the entity class pos is defined along with type, symbol, health etc.
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
        ///     The draw method of the entity.
        ///     You need to pass the subconsole it is being drawn onto.
        /// </summary>
        /// <param name="console"></param>
        private void draw(RLConsole console)
        {
            console.SetChar(pos.x, pos.y, character);
        }

        /// <summary>
        ///     Clears the entity symbol.
        /// </summary>
        /// <param name="console"></param>
        private void clear(RLConsole console)
        {
            console.SetChar(pos.x, pos.y, ' ');
        }
    }
}
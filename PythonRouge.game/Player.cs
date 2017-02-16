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
using System;
using RLNET;

namespace PythonRouge.game
{
    [Serializable]
    public class Player
    {
        public char character;
        private int health;
        public string name;
        public EntityPos pos;

        public Player(int x, int y, int health, char character, string name)
        {
            pos = new EntityPos(x, y);
            this.health = health;
            this.name = name;
            this.character = character;
        }


        //Move method updates player pos in change of y and x
        public void move(int dx, int dy)
        {
            pos.x += dx;
            pos.y += dy;
        }

        public void draw(RLConsole console)
        {
            console.Set(pos.x, pos.y, RLColor.White, null, character);
        }

        public void clear(RLConsole console)
        {
            console.Set(pos.x, pos.y, null, null, ' ');
        }
    }
}
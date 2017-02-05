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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RLNET;




namespace PythonRouge.game
{
    /// <summary>
    /// This is a class designed to hold custom colours defined by me.
    /// </summary>
    static class Colours
    {
        public static RLColor floor_lit = new RLColor(68, 68, 63);
        public static RLColor wall_lit = new RLColor(53, 71, 36);
        public static RLColor floor = new RLColor(12, 16, 15);
        public static RLColor wall = new RLColor(38, 57, 19);

    }
}
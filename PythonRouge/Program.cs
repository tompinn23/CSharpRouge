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
using PythonRouge.game;
using PythonRouge.network;
using RLNET;

namespace PythonRouge
{
    public static class Program
    {
        //Initialize variables for engines and controlling rootConsole.
        private static RLRootConsole _rootConsole;
        private static SPEngine _spEngine;
        private static MpEngine _mpEngine;
        public static bool Multi;
        public static bool main = true;
        public static bool opts = false;
        private static bool multiRendered = false;
        private static bool detsEntered = false;
        private static bool wantName;
        private static string name = "";

        /// <summary>
        ///     Main entry point for the program
        ///     This is where the program starts here the various settings are set
        ///     and the root console is created we also hook the events handlers
        ///     for each event. Then we start the console.
        /// </summary>
        public static void Main()
        {
            var settings = new RLSettings();
            settings.BitmapFile = "ascii_8x8.png";
            settings.CharWidth = 8;
            settings.CharHeight = 8;
            settings.Width = 90;
            settings.Height = 70;
            settings.Scale = 1f;
            settings.Title = "PythonRouge";
            settings.WindowBorder = RLWindowBorder.Fixed;
            settings.ResizeType = RLResizeType.ResizeCells;
            settings.StartWindowState = RLWindowState.Normal;

            _rootConsole = new RLRootConsole(settings);

            _rootConsole.Update += rootConsole_Update;
            _rootConsole.Render += rootConsole_Render;
            _rootConsole.Run();
        }

        //Main menu this just prints the options available for the main menu.
        private static void mainMenu()
        {
            _rootConsole.Print(36, 1, "Press e to exit", RLColor.White);
            _rootConsole.Print(4, 3, "Game", RLColor.Cyan);
            _rootConsole.Print(4, 4, "1) Play Game", RLColor.White);
            _rootConsole.Print(4, 5, "2) Multiplayer", RLColor.White);
        }

        //The same as the above only for the multiplayer menu.
        private static void multiMenu()
        {
            _rootConsole.Print(36, 1, "Press e to exit", RLColor.White);
            _rootConsole.Print(4, 3, "Multiplayer", RLColor.Cyan);
            _rootConsole.Print(4, 4, "1) Host Game", RLColor.White);
            _rootConsole.Print(4, 5, "2) Join Game", RLColor.White);
            multiRendered = true;
        }

        private static void enterDets()
        {
            _rootConsole.Print(4, 3, "Enter Name:", RLColor.White);
            _rootConsole.Print(15, 3, name, RLColor.White);
        }

        /// <summary>
        ///     This is the event handler for the Root Console update event.
        ///     Here the keypresses are handled and first a check for either mpengine or spengine meaning
        ///     all keys are sent to the respective engine
        ///     If no engine is running we can traverse the menus using keys.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void rootConsole_Update(object sender, UpdateEventArgs e)
        {
            if (!wantName)
            {
                if (_spEngine != null)
                {
                    var keyPress = _rootConsole.Keyboard.GetKeyPress();
                    if (keyPress != null)
                        _spEngine.handleKey(keyPress);
                }
                if (_mpEngine != null)
                {
                    var keyPress = _rootConsole.Keyboard.GetKeyPress();
                    if (keyPress != null)
                        _mpEngine.HandleKey(keyPress);
                    _mpEngine.Update();
                }
                var keyPrss = _rootConsole.Keyboard.GetKeyPress();
                if (keyPrss != null)
                {
                    if (keyPrss.Key == RLKey.Number1)
                    {
                        if (main)
                            _spEngine = new SPEngine(_rootConsole);

                    }
                    if (keyPrss.Key == RLKey.Number2)
                        if (main)
                        {
                            main = false;
                            Multi = true;
                            wantName = true;
                        }
                    if (Multi && multiRendered && detsEntered)
                        _mpEngine = new MpEngine(_rootConsole, name);
                    if (keyPrss.Key == RLKey.E)
                    {
                        if (Multi)
                        {
                            main = true;
                            Multi = false;
                        }
                        if (main)
                            _rootConsole.Close();
                    }
                }
            }
            if (!detsEntered && wantName)
            {
                var key = _rootConsole.Keyboard.GetKeyPress();
                if (key != null)
                {
                    switch (key.Key)
                    {
                        case RLKey.Enter:
                            detsEntered = true;
                            wantName = false;
                            break;
                        case RLKey.Escape:
                            break;
                        case RLKey.Space:
                            name += " ";
                            break;
                        case RLKey.Back:
                            if(name.Length != 0)
                            name = name.Remove(name.Length - 1);
                            break;
                        case RLKey.A:
                            if (key.Shift || key.CapsLock)
                                name += "A";
                            else
                            {
                                name += "a";
                            }
                            break;
                        case RLKey.B:
                            if (key.Shift || key.CapsLock)
                                name += "B";
                            else
                            {
                                name += "b";
                            }
                            break;
                        case RLKey.C:
                            if (key.Shift || key.CapsLock)
                                name += "C";
                            else
                            {
                                name += "c";
                            }
                            break;
                        case RLKey.D:
                            if (key.Shift || key.CapsLock)
                                name += "D";
                            else
                            {
                                name += "d";
                            }
                            break;
                        case RLKey.E:
                            if (key.Shift || key.CapsLock)
                                name += "E";
                            else
                            {
                                name += "e";
                            }
                            break;
                        case RLKey.F:
                            if (key.Shift || key.CapsLock)
                                name += "F";
                            else
                            {
                                name += "f";
                            }
                            break;
                        case RLKey.G:
                            if (key.Shift || key.CapsLock)
                                name += "G";
                            else
                            {
                                name += "g";
                            }
                            break;
                        case RLKey.H:
                            if (key.Shift || key.CapsLock)
                                name += "H";
                            else
                            {
                                name += "h";
                            }
                            break;
                        case RLKey.I:
                            if (key.Shift || key.CapsLock)
                                name += "I";
                            else
                            {
                                name += "i";
                            }
                            break;
                        case RLKey.J:
                            if (key.Shift || key.CapsLock)
                                name += "J";
                            else
                            {
                                name += "j";
                            }
                            break;
                        case RLKey.K:
                            if (key.Shift || key.CapsLock)
                                name += "K";
                            else
                            {
                                name += "k";
                            }
                            break;
                        case RLKey.L:
                            if (key.Shift || key.CapsLock)
                                name += "L";
                            else
                            {
                                name += "l";
                            }
                            break;
                        case RLKey.M:
                            if (key.Shift || key.CapsLock)
                                name += "M";
                            else
                            {
                                name += "m";
                            }
                            break;
                        case RLKey.N:
                            if (key.Shift || key.CapsLock)
                                name += "N";
                            else
                            {
                                name += "n";
                            }
                            break;
                        case RLKey.O:
                            if (key.Shift || key.CapsLock)
                                name += "O";
                            else
                            {
                                name += "o";
                            }
                            break;
                        case RLKey.P:
                            if (key.Shift || key.CapsLock)
                                name += "P";
                            else
                            {
                                name += "p";
                            }
                            break;
                        case RLKey.Q:
                            if (key.Shift || key.CapsLock)
                                name += "Q";
                            else
                            {
                                name += "q";
                            }
                            break;
                        case RLKey.R:
                            if (key.Shift || key.CapsLock)
                                name += "R";
                            else
                            {
                                name += "r";
                            }
                            break;
                        case RLKey.S:
                            if (key.Shift || key.CapsLock)
                                name += "S";
                            else
                            {
                                name += "s";
                            }
                            break;
                        case RLKey.T:
                            if (key.Shift || key.CapsLock)
                                name += "T";
                            else
                            {
                                name += "t";
                            }
                            break;
                        case RLKey.U:
                            if (key.Shift || key.CapsLock)
                                name += "U";
                            else
                            {
                                name += "u";
                            }
                            break;
                        case RLKey.V:
                            if (key.Shift || key.CapsLock)
                                name += "V";
                            else
                            {
                                name += "v";
                            }
                            break;
                        case RLKey.W:
                            if (key.Shift || key.CapsLock)
                                name += "W";
                            else
                            {
                                name += "w";
                            }
                            break;
                        case RLKey.X:
                            if (key.Shift || key.CapsLock)
                                name += "X";
                            else
                            {
                                name += "x";
                            }
                            break;
                        case RLKey.Y:
                            if (key.Shift || key.CapsLock)
                                name += "Y";
                            else
                            {
                                name += "y";
                            }
                            break;
                        case RLKey.Z:
                            if (key.Shift || key.CapsLock)
                                name += "Z";
                            else
                            {
                                name += "z";
                            }
                            break;
                        case RLKey.Number0:
                            name += "0";
                            break;
                        case RLKey.Number1:
                            name += "1";
                            break;
                        case RLKey.Number2:
                            name += "2";
                            break;
                        case RLKey.Number3:
                            name += "3";
                            break;
                        case RLKey.Number4:
                            name += "4";
                            break;
                        case RLKey.Number5:
                            name += "5";
                            break;
                        case RLKey.Number6:
                            name += "6";
                            break;
                        case RLKey.Number7:
                            name += "7";
                            break;
                        case RLKey.Number8:
                            name += "8";
                            break;
                        case RLKey.Number9:
                            name += "9";
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        /// <summary>
        ///     This is the render event handler which is where we handle the respective render methods,
        ///     that need to be running.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void rootConsole_Render(object sender, UpdateEventArgs e)
        {
            _rootConsole.Clear();
            _spEngine?.render();
            _mpEngine?.Render();
            if (_spEngine == null && _mpEngine == null)
            {
                if (main)
                    mainMenu();
                if (Multi && !wantName)
                    multiMenu();
                if (!detsEntered && wantName)
                {
                    enterDets();
                }
            }
            _rootConsole.Draw();
        }
    }
}
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
using PythonRouge.game;
using PythonRouge.network;
using RLNET;

namespace PythonRouge
{
    public class Program
    {
        //Initialize variables for engines and controlling rootConsole.
        private static RLRootConsole _rootConsole;
        private static SPEngine _spEngine;
        private static MpEngine _mpEngine;
        public static bool Multi;
        public static bool main = true;
        public static bool opts = false;

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
                    }
                    if (Multi)
                        _mpEngine = new MpEngine(_rootConsole);
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
                if (Multi)
                    multiMenu();
            }
            _rootConsole.Draw();
        }
    }
}
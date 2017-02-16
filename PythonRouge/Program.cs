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
using PythonRouge.Network;
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
        private static MPLobby _lobby;

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
            
           if(Menu.currMenu == "main")
            {
                switch(Menu.mainUpdate(_rootConsole))
                {
                    case 1:
                        _spEngine = new SPEngine(_rootConsole);
                        break;
                    case 2:
                        Menu.currMenu = "multi";
                        break;
                }
            }
           if(Menu.currMenu == "multi")
            {
                switch(Menu.multiUpdate(_rootConsole))
                {
                    case 1:
                        break;
                    case 2:
                        Menu.currMenu = "enterDets";
                        Menu.name = "";
                        break;
                }
            }
           if(Menu.currMenu == "enterDets")
            {
                switch(Menu.enterDetsUpdate(_rootConsole))
                {
                    case 1:
                        _lobby = new MPLobby(_rootConsole, Menu.name);
                        Menu.currMenu = "game";
                        break;
                    case 0:
                        break;
                }
            }
            if (Menu.currMenu == "game")
            {
                var keypress = _rootConsole.Keyboard.GetKeyPress();
                if (keypress != null)
                {
                    _spEngine?.handleKey(keypress);
                    _mpEngine?.HandleKey(keypress);
                }
                var m = _lobby?.Update(keypress);
                if(m == 1)
                {
                    _mpEngine = new MpEngine(_rootConsole, Menu.name, _lobby?.servers[_lobby.sellist[_lobby.curIndex]]);
                    _lobby = null;
                }
                _mpEngine?.Update();
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
            switch (Menu.currMenu)
            {
                case "main":
                    Menu.mainRender(_rootConsole);
                    break;
                case "multi":
                    Menu.multiRender(_rootConsole);
                    break;
                case "enterDets":
                    Menu.enterDetsRender(_rootConsole);
                    break;
                case "game":
                    _spEngine?.render();
                    _mpEngine?.Render();
                    _lobby?.Render();
                    break;


            }
            
            
            _rootConsole.Draw();
        }
    }
}
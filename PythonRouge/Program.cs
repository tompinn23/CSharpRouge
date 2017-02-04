using RLNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PythonRouge.game;
using PythonRouge.network;

namespace PythonRouge
{
    public class Program
    {
        //Initialize variables for engines and controlling rootConsole.
        private static RLRootConsole rootConsole;
        private static SPEngine spEngine = null;
        private static MPEngine mpEngine = null;
        public static bool multi = false;
        public static bool main = true;
        public static bool opts = false;
        
        /// <summary>
        /// Main entry point for the program
        /// This is where the program starts here the various settings are set
        /// and the root console is created we also hook the events handlers
        /// for each event. Then we start the console.
        /// </summary>

        public static void Main()
        {
            RLSettings settings = new RLSettings();
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

            rootConsole = new RLRootConsole(settings);

            rootConsole.Update += rootConsole_Update;
            rootConsole.Render += rootConsole_Render;
            rootConsole.Run();

        }
        //Main menu this just prints the options available for the main menu.
        static void mainMenu()
        {
            rootConsole.Print(36, 1, "Press e to exit", RLColor.White);
            rootConsole.Print(4, 3, "Game", RLColor.Cyan);
            rootConsole.Print(4, 4, "1) Play Game", RLColor.White);
            rootConsole.Print(4, 5, "2) Multiplayer", RLColor.White);
        }

        //The same as the above only for the multiplayer menu.
        static void multiMenu()
        {
            rootConsole.Print(36, 1, "Press e to exit", RLColor.White);
            rootConsole.Print(4, 3, "Multiplayer", RLColor.Cyan);
            rootConsole.Print(4, 4, "1) Host Game", RLColor.White);
            rootConsole.Print(4, 5, "2) Join Game", RLColor.White);
        }

        /// <summary>
        /// This is the event handler for the Root Console update event.
        /// Here the keypresses are handled and first a check for either mpengine or spengine meaning
        /// all keys are sent to the respective engine
        /// 
        /// If no engine is running we can traverse the menus using keys.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        static void rootConsole_Update(object sender, UpdateEventArgs e)
        {
            if (spEngine != null)
            {
                RLKeyPress keyPress = rootConsole.Keyboard.GetKeyPress();
                if (keyPress != null)
                {
                    spEngine.handleKey(keyPress);
                }
            }
            if (mpEngine != null)
            {
                RLKeyPress keyPress = rootConsole.Keyboard.GetKeyPress();
                if (keyPress != null)
                {
                    spEngine.handleKey(keyPress);
                }
                mpEngine.update();
            }
            RLKeyPress keyPrss = rootConsole.Keyboard.GetKeyPress();
            if(keyPrss != null)
            {
                if(keyPrss.Key == RLKey.Number1)
                {
                    if (main)
                    {
                        spEngine = new SPEngine(rootConsole);
                    }
                    if(multi)
                    {
                        mpEngine = new MPEngine(rootConsole);
                    }
                }
                if(keyPrss.Key == RLKey.Number2)
                {
                    if (main)
                    {
                        main = false;
                        multi = true;
                    }
                }
                if (keyPrss.Key == RLKey.E)
                {
                    if (multi)
                    {
                        main = true;
                        multi = false;
                    }
                    if(main)
                    {
                        rootConsole.Close();
                    }
                }
                    
            }
            
        }
        /// <summary>
        /// This is the render event handler which is where we handle the respective render methods,
        /// that need to be running.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        static void rootConsole_Render(object sender, UpdateEventArgs e)
        {
            rootConsole.Clear();
            if (spEngine != null)
            {
                spEngine.render();
            }
            if (spEngine == null && mpEngine == null)
            {
                if(main)
                {
                    mainMenu();
                }
                if(multi)
                {
                    multiMenu();
                }
            }
            rootConsole.Draw();
        }
    }
}

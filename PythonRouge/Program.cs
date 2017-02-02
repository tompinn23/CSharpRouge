using RLNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PythonRouge.game;

namespace PythonRouge
{
    public class Program
    {
        private static RLRootConsole rootConsole;
        private static SPEngine spEngine;

        
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
            spEngine = new SPEngine(rootConsole);

            rootConsole.Update += rootConsole_Update;
            rootConsole.OnLoad += rootConsole_OnLoad;
            rootConsole.Render += rootConsole_Render;
            rootConsole.Run();

        }

        static void rootConsole_OnLoad(object sender, EventArgs e)
        {
            mainMenu();
        }

        static void mainMenu()
        {
        
        }


        static void rootConsole_Update(object sender, UpdateEventArgs e)
        {
            RLKeyPress keyPress = rootConsole.Keyboard.GetKeyPress();
            if (keyPress != null)
            {
                spEngine.handleKey(keyPress);
            }
        }

        static void rootConsole_Render(object sender, UpdateEventArgs e)
        {
            rootConsole.Clear();
            spEngine.render();
            rootConsole.Draw();
        }
    }
}

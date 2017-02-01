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
        private static int playerX = 25;
        private static int playerY = 25;
        private static RLRootConsole rootConsole;
        private static SPEngine spEngine;

        public static void Main()
        {
            RLSettings settings = new RLSettings();
            settings.BitmapFile = "ascii_8x8.png";
            settings.CharWidth = 8;
            settings.CharHeight = 8;
            settings.Width = 70;
            settings.Height = 50;
            settings.Scale = 1f;
            settings.Title = "PythonRouge";
            settings.WindowBorder = RLWindowBorder.Fixed;
            settings.ResizeType = RLResizeType.ResizeCells;
            settings.StartWindowState = RLWindowState.Normal;

            rootConsole = new RLRootConsole(settings);
            spEngine = new SPEngine(rootConsole);
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
                if (keyPress.Key == RLKey.Up)
                    playerY--;
                else if (keyPress.Key == RLKey.Down)
                    playerY++;
                else if (keyPress.Key == RLKey.Left)
                    playerX--;
                else if (keyPress.Key == RLKey.Right)
                    playerX++;
                if (keyPress.Key == RLKey.Escape)
                    rootConsole.Close();
            }
        }

        static void rootConsole_Render(object sender, UpdateEventArgs e)
        {
            rootConsole.Clear();

            rootConsole.Print(1, 1, "Hello World!", RLColor.White);

            rootConsole.SetChar(playerX, playerY, '@');

            int color = 1;
            if (rootConsole.Mouse.LeftPressed)
            {
                color = 4;
            }
            rootConsole.SetBackColor(rootConsole.Mouse.X, rootConsole.Mouse.Y, RLColor.CGA[color]);

            rootConsole.Draw();
        }
    }
}

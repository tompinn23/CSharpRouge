using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RLNET;

namespace PythonRouge
{
    static class Menu
    {
        public static string currMenu = "main";
        static bool multiRendered;
        internal static string name = "";

        public static void mainRender(RLRootConsole _rootConsole)
        {
            _rootConsole.Print(36, 1, "Press e to exit", RLColor.White);
            _rootConsole.Print(4, 3, "Game", RLColor.Cyan);
            _rootConsole.Print(4, 4, "1) Play Game", RLColor.White);
            _rootConsole.Print(4, 5, "2) Multiplayer", RLColor.White);
        } 
        
        public static int mainUpdate(RLRootConsole _rootConsole)
        {
            RLKeyPress keyPress = _rootConsole.Keyboard.GetKeyPress();
            if (keyPress == null) return 0;
            switch(keyPress.Key)
            {
                case RLKey.Number1:
                    return 1;
                    
                case RLKey.Number2:
                    return 2;
            }
            return 0;
        }

        public static void multiRender(RLRootConsole _rootConsole)
        {
            _rootConsole.Print(36, 1, "Press e to exit", RLColor.White);
            _rootConsole.Print(4, 3, "Multiplayer", RLColor.Cyan);
            _rootConsole.Print(4, 4, "1) Host Game", RLColor.White);
            _rootConsole.Print(4, 5, "2) Join Game", RLColor.White);
            multiRendered = true;
        }

        public static int multiUpdate(RLRootConsole _rootConsole)
        {
            if (!multiRendered) return 0;
            var keypress = _rootConsole.Keyboard.GetKeyPress();
            if (keypress == null) return 0;
            if (keypress.Key == RLKey.Number1) return 1;
            if (keypress.Key == RLKey.Number2) return 2;
            return 0;           
        }

        public static void enterDetsRender(RLRootConsole _rootConsole)
        {
            _rootConsole.Print(4, 3, "Enter Name:", RLColor.White);
            _rootConsole.Print(15, 3, name, RLColor.White);
        }

        public static int enterDetsUpdate(RLRootConsole _rootConsole)
        {
            var key = _rootConsole.Keyboard.GetKeyPress();
            if (key != null)
            {
                switch (key.Key)
                {
                    case RLKey.Enter:
                        return 1;
                        break;
                    case RLKey.Escape:
                        break;
                    case RLKey.Space:
                        name += " ";
                        break;
                    case RLKey.Back:
                        if (name.Length != 0)
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
            return 0;
        }

        public static void optsRender(RLRootConsole _rootConsole)
        {

        }

        public static void optsUpdate(RLRootConsole _rootConsole)
        {

        }
    }
}

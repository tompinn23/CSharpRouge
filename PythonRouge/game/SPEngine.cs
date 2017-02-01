using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RLNET;

namespace PythonRouge.game
{
    class SPEngine
    {
        private RLRootConsole rootConsole;
        public SPEngine(RLRootConsole console)
        {
            this.rootConsole = console;
            this.rootConsole.Render += update;
            this.rootConsole.Update += render;
        }

        void update(object sender, UpdateEventArgs e)
        {

        }

        void render(object sender, UpdateEventArgs e)
        {

        }
    }
}

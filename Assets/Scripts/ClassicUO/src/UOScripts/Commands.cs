using ClassicUO.UOScripts.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassicUO.UOScripts
{
    public static class Commands
    {
        public static void Register()
        {
            // Using stuff
            Interpreter.RegisterCommandHandler("dclicktype", UseType); // DoubleClickTypeAction
            Interpreter.RegisterCommandHandler("dclick", UseObject); //DoubleClickAction

            Interpreter.RegisterCommandHandler("usetype", UseType); // DoubleClickTypeAction
            Interpreter.RegisterCommandHandler("useobject", UseObject); //DoubleClickAction
        }

        private static bool UseObject(string command, Argument[] args, bool quiet, bool force)
        {
            throw new NotImplementedException();
        }

        private static bool UseType(string command, Argument[] args, bool quiet, bool force)
        {
            if (args.Length == 0)
            {
                throw new RunTimeError(null,
                    "Usage: dclicktype|usetype (graphicID) [inrangecheck (true/false)]");
            }
            string gfxStr = args[0].AsString();
            return true;
        }
    }
}

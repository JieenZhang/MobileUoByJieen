using ClassicUO.Game;
using ClassicUO.Game.Data;
using ClassicUO.Game.Managers;
using ClassicUO.Game.Scenes;
using ClassicUO.UOScripts.Engine;
using ClassicUO.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassicUO.UOScripts
{
    public static class ScriptManager
    {
        public static bool Running => ScriptRunning;

        private static bool ScriptRunning { get; set; }

        private static Script _queuedScript;

        private static ScriptTimer Timer { get; }

        static ScriptManager()
        {
            Timer = new ScriptTimer();
        }

        public static void OnLogin()
        {
            Commands.Register();
            Aliases.Register();
            Expressions.Register();

            Timer.Start();
        }

        public static void StopScript()
        {
            _queuedScript = null;

            Interpreter.StopScript();
        }

        public static void PlayScript(string[] lines)
        {
            if (World.Player == null || lines == null)
                return;

            StopScript(); // be sure nothing is running

            if (_queuedScript != null)
                return;

            if (!(Client.Game.Scene is GameScene))
                return;

            if (World.Player == null)
                return;


            Script script = new Script(Lexer.Lex(lines));

            _queuedScript = script;
        }

        private class ScriptTimer : Timer
        {
            // Only run scripts once every 25ms to avoid spamming.
            public ScriptTimer() : base(TimeSpan.FromMilliseconds(25), TimeSpan.FromMilliseconds(25))
            {
            }

            protected override void OnTick()
            {
                try
                {
                    if (!(Client.Game.Scene is GameScene))
                    {
                        if (ScriptRunning)
                        {
                            ScriptRunning = false;
                            Interpreter.StopScript();
                        }
                        return;
                    }

                    bool running;

                    if (_queuedScript != null)
                    {
                        // Starting a new script. This relies on the atomicity for references in CLR
                        var script = _queuedScript;

                        running = Interpreter.StartScript(script);

                        _queuedScript = null;
                    }
                    else
                    {
                        running = Interpreter.ExecuteScript();
                    }


                    if (running)
                    {
                        if (ScriptManager.Running == false)
                        {
                            //if (Config.GetBool("ScriptDisablePlayFinish"))
                            //    World.Player?.SendMessage(LocString.ScriptPlaying);

                            //Assistant.Engine.MainWindow.LockScriptUI(true);
                            ScriptRunning = true;
                        }
                    }
                    else
                    {
                        if (ScriptManager.Running)
                        {
                            //if (Config.GetBool("ScriptDisablePlayFinish"))
                            //    World.Player?.SendMessage(LocString.ScriptFinished);

                            //Assistant.Engine.MainWindow.LockScriptUI(false);
                            ScriptRunning = false;
                        }
                    }
                }
                catch (RunTimeError ex)
                {

                    if (ex.Node != null)
                    {
                        MessageManager.HandleMessage(null, $"Script Error: {ex.Message} (Line: {ex.Node.LineNumber + 1})", "System", 0xFFFF, MessageType.Regular, 3);
                    }
                    else
                    {
                        MessageManager.HandleMessage(null, $"Script Error: {ex.Message}", "System", 0xFFFF, MessageType.Regular, 3);
                        //World.Player?.SendMessage(MsgLevel.Error, $"Script Error: {ex.Message}");
                    }

                    StopScript();
                }
                catch (Exception ex)
                {
                    //World.Player?.SendMessage(MsgLevel.Error, $"Script Error: {ex.Message}");
                    MessageManager.HandleMessage(null, $"Script Error: {ex.Message}", "System", 0xFFFF, MessageType.Regular, 3);
                    StopScript();
                }
            }
        }
    }
}

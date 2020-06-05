using ClassicUO.Game;
using ClassicUO.Game.Data;
using ClassicUO.UOScripts.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassicUO.UOScripts
{
    public static class Expressions
    {
        public static void Register()
        {
            Interpreter.RegisterExpressionHandler("stam", Stam);
            Interpreter.RegisterExpressionHandler("maxstam", MaxStam);
            Interpreter.RegisterExpressionHandler("hp", Hp);
            Interpreter.RegisterExpressionHandler("hits", Hp);
            Interpreter.RegisterExpressionHandler("maxhp", MaxHp);
            Interpreter.RegisterExpressionHandler("maxhits", MaxHp);
            Interpreter.RegisterExpressionHandler("mana", Mana);
            Interpreter.RegisterExpressionHandler("maxmana", MaxMana);
            Interpreter.RegisterExpressionHandler("poisoned", Poisoned);
            Interpreter.RegisterExpressionHandler("hidden", Hidden);

            Interpreter.RegisterExpressionHandler("mounted", Mounted);
            Interpreter.RegisterExpressionHandler("rhandempty", RHandEmpty);
            Interpreter.RegisterExpressionHandler("lhandempty", LHandEmpty);

            Interpreter.RegisterExpressionHandler("dead", Dead);

            Interpreter.RegisterExpressionHandler("str", Str);
            Interpreter.RegisterExpressionHandler("int", Int);
            Interpreter.RegisterExpressionHandler("dex", Dex);

            Interpreter.RegisterExpressionHandler("weight", Weight);

            Interpreter.RegisterExpressionHandler("skill", SkillExpression);
            Interpreter.RegisterExpressionHandler("count", CountExpression);
            Interpreter.RegisterExpressionHandler("counter", CountExpression);

            Interpreter.RegisterExpressionHandler("insysmsg", InSysMessage);
            Interpreter.RegisterExpressionHandler("insysmessage", InSysMessage);

            Interpreter.RegisterExpressionHandler("findtype", FindType);
        }

        private static bool FindType(string expression, Argument[] args, bool quiet)
        {
            if (args.Length == 0)
            {
                throw new RunTimeError(null, "Usage: findtype (graphicID) [range]");
            }

            //string gfxStr = args[0].AsString();
            //Serial gfx = Utility.ToUInt16(gfxStr, 0);

            //bool inRangeCheck = Config.GetBool("ScriptFindTypeRange");

            //if (args.Length == 2)
            //{
            //    inRangeCheck = args[1].AsBool();
            //}

            //// No graphic id, maybe searching by name?
            //if (gfx == 0)
            //{
            //    foreach (Item item in World.FindItemsByName(gfxStr))
            //    {
            //        if (inRangeCheck)
            //        {
            //            if (!item.IsInBank &&
            //                (Utility.InRange(World.Player.Position, item.Position, 2) ||
            //                 item.RootContainer == World.Player))
            //            {
            //                return true;
            //            }
            //        }
            //        else if (!item.IsInBank)
            //        {
            //            return true;
            //        }
            //    }
            //}
            //else // Check backpack first
            //{
            //    if (World.Player.Backpack != null)
            //    {
            //        Item i = World.Player.Backpack.FindItemByID(Utility.ToUInt16(gfxStr, 0));

            //        if (i != null)
            //            return true;
            //    }
            //}

            //// Not in backpack? Lets check the world
            //foreach (Item i in World.Items.Values)
            //{
            //    if (i.ItemID != gfx || i.RootContainer != null) continue;

            //    if (inRangeCheck)
            //    {
            //        if (Utility.InRange(World.Player.Position, i.Position, 2))
            //            return true;
            //    }
            //    else
            //    {
            //        return true;
            //    }
            //}

            //foreach (Item i in World.Items.Values)
            //{
            //    if (i.ItemID != gfx || i.IsInBank) continue;

            //    if (inRangeCheck)
            //    {
            //        if (Utility.InRange(World.Player.Position, i.Position, 2) || i.RootContainer == World.Player)
            //            return true;
            //    }
            //    else
            //    {
            //        return true;
            //    }
            //}

            //foreach (Mobile m in World.MobilesInRange())
            //{
            //    if (m.Body == gfx)
            //    {
            //        if (inRangeCheck)
            //        {
            //            if (Utility.InRange(World.Player.Position, m.Position, 2))
            //                return true;
            //        }
            //        else
            //        {
            //            return true;
            //        }
            //    }
            //}

            return false;
        }

        private static bool Mounted(string expression, Argument[] args, bool quiet)
        {
            return World.Player != null && World.Player.FindItemByLayer(Layer.Mount) != null;
        }

        private static bool RHandEmpty(string expression, Argument[] args, bool quiet)
        {
            return World.Player != null && World.Player.FindItemByLayer(Layer.TwoHanded) == null;
        }

        private static bool LHandEmpty(string expression, Argument[] args, bool quiet)
        {
            return World.Player != null && World.Player.FindItemByLayer(Layer.OneHanded) == null;
        }

        private static bool Dead(string expression, Argument[] args, bool quiet)
        {
            return World.Player != null && World.Player.IsDead;
        }

        private static bool InSysMessage(string expression, Argument[] args, bool quiet)
        {
            if (args.Length == 0)
            {
                throw new RunTimeError(null, "Usage: insysmsg ('text')");
            }

            string text = args[0].AsString();

            //for (int i = PacketHandlers.SysMessages.Count - 1; i >= 0; i--)
            //{
            //    string sys = PacketHandlers.SysMessages[i];

            //    if (sys.IndexOf(text, StringComparison.OrdinalIgnoreCase) != -1)
            //    {
            //        PacketHandlers.SysMessages.RemoveRange(0, i + 1);
            //        return true;
            //    }
            //}

            return false;
        }

        private static int Mana(string expression, Argument[] args, bool quiet)
        {
            if (World.Player == null)
                return 0;

            return World.Player.Mana;
        }

        private static int MaxMana(string expression, Argument[] args, bool quiet)
        {
            if (World.Player == null)
                return 0;

            return World.Player.ManaMax;
        }

        private static bool Poisoned(string expression, Argument[] args, bool quiet)
        {
            return World.Player != null && World.Player.IsPoisoned;
        }

        private static bool Hidden(string expression, Argument[] args, bool quiet)
        {
            return World.Player != null && World.Player.IsHidden;
        }

        private static int Hp(string expression, Argument[] args, bool quiet)
        {
            if (World.Player == null)
                return 0;

            return World.Player.Hits;
        }

        private static int MaxHp(string expression, Argument[] args, bool quiet)
        {
            if (World.Player == null)
                return 0;

            return World.Player.HitsMax;
        }

        private static int Stam(string expression, Argument[] args, bool quiet)
        {
            if (World.Player == null)
                return 0;

            return World.Player.Stamina;
        }

        private static int MaxStam(string expression, Argument[] args, bool quiet)
        {
            if (World.Player == null)
                return 0;

            return World.Player.StaminaMax;
        }

        private static int Str(string expression, Argument[] args, bool quiet)
        {
            if (World.Player == null)
                return 0;

            return World.Player.Strength;
        }

        private static int Dex(string expression, Argument[] args, bool quiet)
        {
            if (World.Player == null)
                return 0;

            return World.Player.Dexterity;
        }

        private static int Int(string expression, Argument[] args, bool quiet)
        {
            if (World.Player == null)
                return 0;

            return World.Player.Intelligence;
        }

        private static int Weight(string expression, Argument[] args, bool quiet)
        {
            if (World.Player == null)
                return 0;

            return World.Player.Weight;
        }

        private static double SkillExpression(string expression, Argument[] args, bool quiet)
        {
            if (args.Length < 1)
                throw new ArgumentException("Usage: skill ('name of skill')");

            if (World.Player == null)
                return 0;

            //foreach (SkillInfo skill in Skills.SkillEntries)
            //{
            //    if (skill.Name.IndexOf(args[0].AsString(), StringComparison.CurrentCultureIgnoreCase) != -1)
            //    {
            //        return World.Player.Skills[skill.Index].Value;
            //    }
            //}

            return 0;
        }

        private static int CountExpression(string expression, Argument[] args, bool quiet)
        {
            if (args.Length < 1)
                throw new ArgumentException("Usage: count ('name of counter item')");

            if (World.Player == null)
                return 0;

            //foreach (Counter c in Counter.List)
            //{
            //    if (c.Name.Equals(args[0].AsString(), StringComparison.OrdinalIgnoreCase))
            //    {
            //        return c.Enabled ? c.Amount : 0;
            //    }
            //}

            return 0;
        }
    }
}

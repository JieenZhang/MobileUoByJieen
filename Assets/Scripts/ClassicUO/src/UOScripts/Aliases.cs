using ClassicUO.Game;
using ClassicUO.Game.Data;
using ClassicUO.Game.GameObjects;
using ClassicUO.Game.Managers;
using ClassicUO.UOScripts.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassicUO.UOScripts
{
    public static class Aliases
    {
        public static void Register()
        {
            Interpreter.RegisterAliasHandler("backpack", Backpack);
            Interpreter.RegisterAliasHandler("last", Last);
            Interpreter.RegisterAliasHandler("lasttarget", Last);
            Interpreter.RegisterAliasHandler("lastobject", LastObject);
            Interpreter.RegisterAliasHandler("self", Self);
            Interpreter.RegisterAliasHandler("righthand", RightHand);
            Interpreter.RegisterAliasHandler("lefthand", LeftHand);
            Interpreter.RegisterAliasHandler("hand", Hand);
        }

        private static uint RightHand(string alias)
        {
            Item item = World.Player.FindItemByLayer(Layer.TwoHanded);

            return World.Player != null && item != null
                ? (uint)item.Serial
                : 0;
        }

        private static uint LeftHand(string alias)
        {
            Item item = World.Player.FindItemByLayer(Layer.OneHanded);

            return World.Player != null && item != null
                ? (uint)item.Serial
                : 0;
        }

        private static uint Hand(string alias)
        {
            if (World.Player == null)
                return 0;

            Item item = World.Player.FindItemByLayer(Layer.OneHanded) ?? World.Player.FindItemByLayer(Layer.TwoHanded);

            return World.Player != null && item != null
                ? (uint)item.Serial
                : 0;
        }

        private static uint Backpack(string alias)
        {
            if (World.Player == null)
                return 0;
            Item backpack = World.Player.FindItemByLayer(Layer.Backpack);

            if (backpack != null)
            {
                return backpack.Serial;
            }
            return 0;
        }

        private static uint Last(string alias)
        {
            if (TargetManager.LastTargetInfo != null)
                return TargetManager.LastTargetInfo.Serial;

            return 0;
        }

        private static uint LastObject(string alias)
        {
            if (World.LastObject > 0 )
                return World.LastObject;
            return 0;
        }

        private static uint Self(string alias)
        {
            if (World.Player == null)
                return 0;

            return World.Player.Serial;
        }
    }
}

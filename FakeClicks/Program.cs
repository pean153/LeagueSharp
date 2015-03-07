#region

using System;
using System.Linq;
using System.Runtime.InteropServices;
using LeagueSharp;
using LeagueSharp.Common;
using Color = System.Drawing.Color;

#endregion

namespace FakeClicks
{
    class FakeClick
    {
        
        public static LeagueSharp.Common.Menu _menu;
        private static Obj_AI_Hero Player
        {
            get { return ObjectManager.Player; }
        }

        static void Main(string[] args)
        {
            CustomEvents.Game.OnGameLoad += Game_OnGameLoad;
        }

        static void Game_OnGameLoad(EventArgs args)
        {
            Game.OnGameUpdate += Game_OnGameUpdate;
            _menu = new Menu("Fake Clicks", "fakeClicks", true);
            _menu.AddSubMenu(new Menu("Keybinds", "keybinds"));
            _menu.SubMenu("keybinds").AddSubMenu(new Menu("Key One", "keyone"));
            _menu.SubMenu("keybinds").SubMenu("keyone").AddItem(new MenuItem("enbone", "Enable this key").SetValue(true));
            _menu.SubMenu("keybinds").SubMenu("keyone").AddItem(new MenuItem("clckone", "Key One").SetValue(new KeyBind(32, KeyBindType.Press)));
            _menu.SubMenu("keybinds").AddSubMenu(new Menu("Key Two", "keytwo"));
            _menu.SubMenu("keybinds").SubMenu("keytwo").AddItem(new MenuItem("enbtwo", "Enable this key").SetValue(true));
            _menu.SubMenu("keybinds").SubMenu("keytwo").AddItem(new MenuItem("clcktwo", "Key Two").SetValue(new KeyBind('C', KeyBindType.Press)));
            _menu.SubMenu("keybinds").AddSubMenu(new Menu("Key Three", "keythree"));
            _menu.SubMenu("keybinds").SubMenu("keythree").AddItem(new MenuItem("enbthree", "Enable this key").SetValue(true));
            _menu.SubMenu("keybinds").SubMenu("keythree").AddItem(new MenuItem("clckthree", "Key Three").SetValue(new KeyBind('V', KeyBindType.Press)));
            _menu.SubMenu("keybinds").AddSubMenu(new Menu("Key Four", "keyfour"));
            _menu.SubMenu("keybinds").SubMenu("keyfour").AddItem(new MenuItem("enbfour", "Enable this key").SetValue(true));
            _menu.SubMenu("keybinds").SubMenu("keyfour").AddItem(new MenuItem("clckfour", "Key Four").SetValue(new KeyBind('X', KeyBindType.Press)));
            _menu.SubMenu("keybinds").AddSubMenu(new Menu("Key Five", "keyfive"));
            _menu.SubMenu("keybinds").SubMenu("keyfive").AddItem(new MenuItem("enbfive", "Enable this key").SetValue(true));
            _menu.SubMenu("keybinds").SubMenu("keyfive").AddItem(new MenuItem("clckfive", "Key Five").SetValue(new KeyBind('G', KeyBindType.Press)));
            _menu.AddItem(new MenuItem("clickEnable", "Enable").SetValue(true));
            _menu.AddItem(new MenuItem("clickDelay", "Click Delay").SetValue(new Slider(200, 20, 2000)));
            _menu.AddItem(new MenuItem("randomDelay", "Random Modifier").SetValue(new Slider(100, 0, 1000)));
            _menu.AddToMainMenu();
 
        }
        private static int clckdelay;
        static void Game_OnGameUpdate(EventArgs args)
        {
            if (Player.IsDead)
                return;

            if (Player.IsChannelingImportantSpell())
            {
                return;
            }

            if (_menu.Item("clickEnable").GetValue<bool>())
            {
                var r = new Random();
                var rng = 10 * r.Next(0, _menu.Item("randomDelay").GetValue<Slider>().Value);
                if (Player.IsMoving && !Player.IsWindingUp && !VirtualMouse.disableOrbClick && ((_menu.Item("enbone").GetValue<bool>() && _menu.Item("clckone").GetValue<KeyBind>().Active) || (_menu.Item("enbtwo").GetValue<bool>() && _menu.Item("clcktwo").GetValue<KeyBind>().Active) || (_menu.Item("enbthree").GetValue<bool>() && _menu.Item("clckthree").GetValue<KeyBind>().Active) || (_menu.Item("enbfour").GetValue<bool>() && _menu.Item("clckfour").GetValue<KeyBind>().Active) || (_menu.Item("enbfive").GetValue<bool>() && _menu.Item("clckfive").GetValue<KeyBind>().Active)) && Environment.TickCount - clckdelay > _menu.Item("clickDelay").GetValue<Slider>().Value + rng)
                {
                    clckdelay = Utils.TickCount;
                    VirtualMouse.RightClick();
                }
            }
            else
            {
                return;
            }
        }
    }
}

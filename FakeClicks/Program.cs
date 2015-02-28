#region

using System;
using System.Linq;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using LeagueSharp;
using LeagueSharp.Common;
using Color = System.Drawing.Color;

#endregion

namespace FakeClicks
{
    class Program
    {
        public static class VirtualMouse
        {
            // import the necessary API function so .NET can marshall parameters appropriately
            [DllImport("user32.dll")]
            static extern void mouse_event(int dwFlags, int dx, int dy, int dwData, int dwExtraInfo);
            //data
            private const int MOUSEEVENTF_RIGHTDOWN = 0x0008;
            private const int MOUSEEVENTF_RIGHTUP = 0x0010;
            // simulates a click-and-release action of the right mouse button at its current position
            public static void RightClick()
            {
                mouse_event(MOUSEEVENTF_RIGHTDOWN, Control.MousePosition.X, Control.MousePosition.Y, 0, 0);
                mouse_event(MOUSEEVENTF_RIGHTUP, Control.MousePosition.X, Control.MousePosition.Y, 0, 0);
            }
            public static int clickdelay = 0;

        }
        public static LeagueSharp.Common.Menu _menu;
        public static Orbwalking.Orbwalker _orbwalker;
        static void Main(string[] args)
        {
            CustomEvents.Game.OnGameLoad += Game_OnGameLoad;
        }

        static void Game_OnGameLoad(EventArgs args)
        {
            Game.OnGameUpdate += Game_OnGameUpdate;
            _menu = new LeagueSharp.Common.Menu("Fake Clicks", "fakeClicks", true);
            _menu.AddItem(new LeagueSharp.Common.MenuItem("clickEnable", "Enable").SetValue(true));
            _menu.AddSubMenu(new LeagueSharp.Common.Menu("Orbwalking", "Orbwalking"));
            _orbwalker = new Orbwalking.Orbwalker(_menu.SubMenu("Orbwalking"));
            _menu.AddItem(new LeagueSharp.Common.MenuItem("clickDelay", "Click Delay").SetValue(new Slider(200, 0, 2000)));
            _menu.AddItem(new LeagueSharp.Common.MenuItem("randomDelay", "Random Delay").SetValue(new Slider(100, 0, 500)));
            _menu.AddToMainMenu();
 
        }
        static void Game_OnGameUpdate(EventArgs args)
        {
            var r = new Random();
            int rng = r.Next(0, _menu.Item("randomDelay").GetValue<Slider>().Value);
            switch (_orbwalker.ActiveMode)
            {
                case Orbwalking.OrbwalkingMode.Combo:
                    if (Orbwalking.CanMove(_menu.Item("ExtraWindup").GetValue<Slider>().Value) && _menu.Item("clickEnable").GetValue<bool>() && !Orbwalking.DisableNextAttack && Environment.TickCount - VirtualMouse.clickdelay > _menu.Item("ExtraWindup").GetValue<Slider>().Value + _menu.Item("clickDelay").GetValue<Slider>().Value + rng)
                    {
                        VirtualMouse.clickdelay = Environment.TickCount;
                        VirtualMouse.RightClick();
                    }
                    break;
                case Orbwalking.OrbwalkingMode.LaneClear:
                    if (Orbwalking.CanMove(_menu.Item("ExtraWindup").GetValue<Slider>().Value) && !Orbwalking.DisableNextAttack && Environment.TickCount - VirtualMouse.clickdelay > 300)
                    {
                        VirtualMouse.clickdelay = Environment.TickCount;
                        VirtualMouse.RightClick();
                    }
                    break;
                case Orbwalking.OrbwalkingMode.Mixed:
                    if (Orbwalking.CanMove(_menu.Item("ExtraWindup").GetValue<Slider>().Value) && !Orbwalking.DisableNextAttack && Environment.TickCount - VirtualMouse.clickdelay > 300)
                    {
                        VirtualMouse.clickdelay = Environment.TickCount;
                        VirtualMouse.RightClick();
                    }
                    break;
                case Orbwalking.OrbwalkingMode.LastHit:
                    if (Orbwalking.CanMove(_menu.Item("ExtraWindup").GetValue<Slider>().Value) && !Orbwalking.DisableNextAttack && Environment.TickCount - VirtualMouse.clickdelay > 300)
                    {
                        VirtualMouse.clickdelay = Environment.TickCount;
                        VirtualMouse.RightClick();
                    }
                    break;
            }
        }
    }
}

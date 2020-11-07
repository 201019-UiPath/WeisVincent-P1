using IceShopBL;
using IceShopDB.Models;
using IceShopLib;
using IceShopLib.Validation;
using Serilog;
using System;
using System.Collections.Generic;
using System.Text;

namespace IceShopUI.Menus
{
    internal sealed class MenuManager
    {
        private static readonly MenuManager _instance = new MenuManager();

        internal Queue<IMenu> MenuChain;

        static MenuManager() { }
        private MenuManager() { MenuChain = new Queue<IMenu>(); }

        public static MenuManager Instance { get { return _instance; } }


        internal void StartMenuChain(IMenu firstMenu)
        {
            Log.Information($"Starting menu chain...");
            ReadyNextMenu(firstMenu);
            RunThroughMenuChain();
        }

        internal void ReadyNextMenu(IMenu nextMenu)
        {
            Log.Information($"Adding new menu to menu chain: {nextMenu.GetType().Name}");
            MenuChain.Enqueue(nextMenu);
        }

        internal void RunThroughMenuChain()
        {
            while (MenuChain.Count > 0)
            {
                MenuChain.Dequeue().Run();
            }
        }

    }
}

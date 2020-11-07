using IceShopDB;
using IceShopDB.Repos;
using IceShopDB.Repos.DBRepos;
using IceShopUI.Menus;
using Serilog;
using System;

namespace IceShopUI
{
    public class Program
    {
        private static void Main()
        {

            Log.Logger = new LoggerConfiguration()
                .WriteTo.File("logs/log.txt",
                outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}")
                .CreateLogger();


            if (Log.Logger == null) { throw new Exception("Logger isn't working."); }


            Console.WriteLine("Welcome Friend! What would you like to do today?");

            IceShopContext context = new IceShopContext();
            IRepository repo = new DBRepo(context);
            IMenu startMenu = new StartMenu(ref repo);

            MenuManager.Instance.ReadyNextMenu(startMenu);
            MenuManager.Instance.RunThroughMenuChain();
        }

    }
}

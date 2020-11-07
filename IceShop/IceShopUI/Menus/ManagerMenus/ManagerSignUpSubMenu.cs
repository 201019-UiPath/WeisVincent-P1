using IceShopBL;
using IceShopDB.Models;
using IceShopDB.Repos;
using System.Collections.Generic;

namespace IceShopUI.Menus.ManagerMenus
{
    internal sealed class ManagerSignUpSubMenu : Menu, IMenu
    {
        private readonly LocationService locationService;
        private readonly List<Location> PossibleLocations;

        private readonly Manager ManagerPickingALocation;



        public ManagerSignUpSubMenu(ref IRepository repo, ref Manager manager) : base(ref repo)
        {
            locationService = new LocationService(ref Repo);
            ManagerPickingALocation = manager;
            PossibleLocations = locationService.GetAllLocations();
        }


        public override void SetStartingMessage()
        {
            StartMessage = "Which location are you manager for?";
        }

        public override void SetUserChoices()
        {

            PossibleOptions = new List<string>(PossibleLocations.Count);
            for (int i = 0; i < PossibleLocations.Count; i++)
            {
                PossibleOptions.Add($"{PossibleLocations[i].Name}  -  Located at: {PossibleLocations[i].Address}");
            }
        }


        public override void ExecuteUserChoice()
        {
            for (int i = 0; i < PossibleLocations.Count; i++)
            {
                if (selectedChoice != i + 1) continue;

                ManagerPickingALocation.Location = PossibleLocations[i];
                ManagerPickingALocation.LocationId = PossibleLocations[i].Id;

            }

            // At this point, the Run method should complete, 
            // and the ball SHOULD be thrown back to the SignUpMenu court and continue execution.
            //LoginMenu loginMenu = new LoginMenu(ref Repo);
            //MenuUtility.Instance.ReadyNextMenu(loginMenu);
        }

        public Manager RunAndReturnManagerWithSelectedLocation()
        {
            Run();
            return ManagerPickingALocation;
        }

    }
}

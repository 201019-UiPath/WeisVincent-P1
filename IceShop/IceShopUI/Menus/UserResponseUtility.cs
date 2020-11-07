using System;
using System.Collections.Generic;
using System.Text;

namespace IceShopUI.Menus
{
    internal static class UserResponseUtility
    {

        internal static void DisplayPossibleChoicesToUser(string startMessage, List<string> possibleOptions)
        {
            Console.WriteLine(startMessage);
            DisplayPossibleChoicesToUser(possibleOptions);
        }

        internal static void DisplayPossibleChoicesToUser(List<string> possibleOptions)
        {
            Console.WriteLine("Press the corresponding number to choose the option that best suits you.");
            for (int i = 0; i < possibleOptions.Count; i++)
            {
                Console.WriteLine($"[{i + 1}]    {possibleOptions[i]}");
            }
        }






    }
}

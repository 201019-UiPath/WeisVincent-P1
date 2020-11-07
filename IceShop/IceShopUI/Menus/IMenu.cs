namespace IceShopUI.Menus
{
    /// <summary>
    /// A menu interface implemented by every menu and submenu in the IceShopUI.
    /// This interface is used with the Menu abstract class to allows the display and input logic to be encapsulated in an abstract class,
    /// while having the interface require the menus to set necessary variables and process user decisions with each menu pass.
    /// </summary>
    public interface IMenu
    {

        public abstract void SetStartingMessage();
        public abstract void SetUserChoices();

        /// <summary>
        /// Starting point of menus
        /// </summary>
        public abstract void Start();

        /// <summary>
        /// This method generates a list of user prompts using a list of strings input as a parameter.
        /// </summary>
        public abstract void QueryUserChoice();

        public void Run();

        public abstract void ExecuteUserChoice();

    }
}
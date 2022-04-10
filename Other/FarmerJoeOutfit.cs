//cs_include Scripts/CoreBots.cs
//cs_include Scripts/CoreFarms.cs
//cs_include Scripts/Hollowborn/HollowbornReapersScythe.cs
using RBot;

public class FarmerJoeOutfit
{
    public ScriptInterface Bot => ScriptInterface.Instance;
    public CoreBots Core => CoreBots.Instance;
    public HollowbornScythe scythe = new();


    public void ScriptMain(ScriptInterface bot)
    {
        Core.SetOptions();

        scythe.GetHBReapersScythe();
        RagsandHat();
        ServersAreDown();
        // DeathsScythe();
        // BackItem();
        // Pet();

        Core.Equip(new[] { "Peasant Rags", "Scarecrow Hat", "The Server is Down", "Hollowborn Reaper's Scythe"});

        Core.Logger("We are farmers, bum ba dum bum bum bum bum", messageBox: true);

        Core.SetOptions(false);
    }

    public void RagsandHat()
    {
        if (Core.CheckInventory("Peasant Rags") | Core.CheckInventory("Scarecrow Hat"))
            return;
        Core.Join("digitalyulgar");
        Bot.Shops.BuyItem(41, "Peasant Rags");
        Bot.Wait.ForPickup("Peasant Rags");
        Bot.Shops.BuyItem(16, "Scarecrow Hat");
        Bot.Wait.ForPickup("Scarecrow Hat");
        // Core.BuyItem("digitalyulgar", 41, "Peasant Rags");
        // Core.BuyItem("digitalyulgar", 16, "Scarecrow Hat");
    }

    public void ServersAreDown()
    {
        if (Core.CheckInventory("The Server is Down"))
            return;

        Core.HuntMonster("undergroundlabb", "Rabid Server Hamster", "The Server is Down", isTemp: false);
        Bot.Wait.ForPickup("The Server is Down");
    }

    public void DeathsScythe()
    {

        if (Core.CheckInventory(25117))
            return;


        while (!Core.CheckInventory(25117))
            Bot.Player.Hunt("Death");

    }

    // public void Pet()
    // {
    //     if (Core.CheckInventory("item"))
    //         return;

    // }


}
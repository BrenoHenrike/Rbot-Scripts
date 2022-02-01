//cs_include Scripts/CoreBots.cs
//cs_include Scripts/CoreFarms.cs
//cs_include Scripts/Nulgath/CoreNulgath.cs
using RBot;

public class DragonBladeofNulgath
{
    // [Can Change] Whether you can solo the PvP boss without killing restorers/brawlers
    public bool CanSoloPvPBoss = true;
    public ScriptInterface Bot => ScriptInterface.Instance;
    public CoreBots Core => CoreBots.Instance;
    public CoreFarms Farm = new CoreFarms();
    public CoreNulgath Nulgath = new CoreNulgath();

    public readonly string[] TwistedItems =
    {
        "DragonFire of Nulgath",
        "Crimson Plate of Nulgath",
        "Crimson Face Plate of Nulgath"
    };

    public void ScriptMain(ScriptInterface bot)
    {
        Core.SetOptions();

        GetDragonBlade();

        Core.SetOptions(false);
    }

    public void GetDragonBlade()
    {
        if (Core.CheckInventory("DragonBlade of Nulgath"))
            return;

        Core.AddDrop(Nulgath.bagDrops);
        Core.AddDrop(TwistedItems);
        Core.AddDrop("DragonBlade of Nulgath", "Combat Trophy", "Basic War Sword", "Behemoth Blade of Shadow", "Behemoth Blade of Light");

        BehemothBladeof("Shadow");
        BehemothBladeof("Light");

        while(!Core.CheckInventory(TwistedItems))
        {
            Core.EnsureAccept(765);
            Nulgath.FarmTotemofNulgath(3);
            Core.HuntMonster("underworld", "Skull Warrior", "Skull Warrior Rune");
            Core.EnsureCompleteChoose(765);
        }

        Nulgath.FarmTotemofNulgath(3);

        Core.EnsureAccept(766);
        Core.HuntMonster("underworld", "Legion Fenrir", "Legion Fenrir Rune");
        Core.EnsureComplete(766, 5483);
        Bot.Player.Pickup("DragonBlade of Nulgath");
    }

    public void BehemothBladeof(string blade)
    {
        if(Core.CheckInventory($"Behemoth Blade of {blade}"))
            return;

        Core.EquipClass(ClassType.Solo);
        if (!Core.CheckInventory("Basic War Sword"))
        {
            Farm.BludrutBrawlBoss(quant: 50, canSoloBoss: CanSoloPvPBoss);
            Core.BuyItem("battleon", 222, "Basic War Sword");
        }
        if (!Core.CheckInventory("Steel Afterlife"))
        {
            Farm.BludrutBrawlBoss(quant: 50, canSoloBoss: CanSoloPvPBoss);
            Core.BuyItem("battleon", 222, "Steel Afterlife");
        }
        Farm.BludrutBrawlBoss(canSoloBoss: CanSoloPvPBoss);
        Core.BuyItem("battleon", 222, $"Behemoth Blade of {blade}");
    }
}
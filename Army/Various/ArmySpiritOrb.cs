/*
name: Spirit Orbs (Army)
description: Farms Spirit Orbs using your army.
tags: army, spirit, orb, BLOD, blinding, light, destiny, good, undead
*/
//cs_include Scripts/CoreBots.cs
//cs_include Scripts/CoreFarms.cs
//cs_include Scripts/CoreAdvanced.cs
//cs_include Scripts/Army/CoreArmyLite.cs
using Skua.Core.Interfaces;
using Skua.Core.Options;

public class ArmySpiritOrb
{
    private IScriptInterface Bot => IScriptInterface.Instance;
    private CoreBots Core => CoreBots.Instance;
    private CoreFarms Farm = new();
    private CoreAdvanced Adv => new();
    private CoreArmyLite Army = new();

    private static CoreBots sCore = new();
    private static CoreArmyLite sArmy = new();

    public string OptionsStorage = "ArmySpiritOrb";
    public bool DontPreconfigure = true;
    public List<IOption> Options = new List<IOption>()
    {
        new Option<int>("amount","Amount", "Input the amount of spirit orbs to farm", 65000),
        sArmy.player1,
        sArmy.player2,
        sArmy.player3,
        sArmy.player4,
        sArmy.player5,
        sArmy.player6,
        sArmy.packetDelay,
        CoreBots.Instance.SkipOptions
    };

    public void ScriptMain(IScriptInterface bot)
    {
        Core.BankingBlackList.AddRange(Loot);
        Core.SetOptions();

        Setup(Bot.Config.Get<int>("amount"));

        Core.SetOptions(false);
    }

    public void Setup(int quant = 65000)
    {
        Core.PrivateRooms = true;
        Core.PrivateRoomNumber = Army.getRoomNr();

        Core.OneTimeMessage("Only for army", "This is intended for use with an army, not for solo players.");

        Core.AddDrop(Loot);
        Core.EquipClass(ClassType.Farm);
        Core.Logger($"Farming for {quant} bone dust");

        Core.RegisterQuests(2082, 2083);
        Army.SmartAggroMonStart("battleunderb", "Skeleton Warrior", "Skeleton Fighter", "Undead Champion");

        if (Bot.Player.CurrentClass?.Name == "ArchMage")
            Bot.Options.AttackWithoutTarget = true;

        while (!Bot.ShouldExit && !Core.CheckInventory("Spirit Orb", quant))
            Bot.Combat.Attack("*");
        Army.AggroMonStop(true);

        Core.CancelRegisteredQuests();
    }

    private string[] Loot = { "Bone Dust", "Undead Essence", "Undead Energy", "Spirit Orb" };
}

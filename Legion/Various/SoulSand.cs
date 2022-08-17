//cs_include Scripts/CoreBots.cs
//cs_include Scripts/CoreFarms.cs
//cs_include Scripts/CoreStory.cs
//cs_include Scripts/Legion/CoreLegion.cs
//cs_include Scripts/CoreAdvanced.cs
//cs_include Scripts/Story/Legion/SeraphicWar.cs
using RBot;

public class AnotherOneBitesTheDust
{
    public ScriptInterface Bot => ScriptInterface.Instance;
    public CoreBots Core => CoreBots.Instance;
    public CoreFarms Farm = new CoreFarms();
    public CoreLegion Legion = new CoreLegion();
    public CoreAdvanced Adv = new CoreAdvanced();
    public SeraphicWar_Story SeraphicWar = new();

    public void ScriptMain(ScriptInterface bot)
    {
        Core.SetOptions();

        SoulSand();

        Core.SetOptions(false);
    }

    public void SoulSand(int quant = 50)
    {
        if (Core.CheckInventory("Soul Sand", quant))
            return;

        Farm.Experience(65);
        SeraphicWar.SeraphicWar_Questline();

        Core.AddDrop("Soul Sand");
        Core.FarmingLogger("Soul Sand", quant);
        Core.RegisterQuests(7991);
        while (!Bot.ShouldExit() && !Core.CheckInventory("Soul Sand", quant))
        {
            Farm.BattleUnderB("Bone Dust", 333);
            Legion.ApprovalAndFavor(0, 400);
            Legion.DarkToken(80);
        }
    }
}
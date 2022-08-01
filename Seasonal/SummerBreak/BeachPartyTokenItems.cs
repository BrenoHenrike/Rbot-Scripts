//cs_include Scripts/CoreBots.cs
//cs_include Scripts/CoreStory.cs
//cs_include Scripts/Seasonal/SummerBreak/BeachParty.cs
using RBot;

public class BeachPartyTokenItems
{
    public ScriptInterface Bot => ScriptInterface.Instance;
    public CoreBots Core => CoreBots.Instance;
    public BeachPartyStory BP = new();

    public void ScriptMain(ScriptInterface bot)
    {
        Core.SetOptions();

        TokenItems();

        Core.SetOptions(false);
    }

    public void TokenItems()
    {
        string[] rewards = Core.EnsureLoad(7010).Rewards.Where(x => Core.IsMember ? true : !x.Upgrade).Select(x => x.Name).ToArray();
        if (Core.CheckInventory(rewards, toInv: false))
            return;

        Core.AddDrop("Tiki Tokens");
        Core.AddDrop(rewards);
        BP.Storyline();

        Core.RegisterQuests(7010);
        while (!Bot.ShouldExit() && !Core.CheckInventory(rewards, toInv: false))
        {
            Core.KillMonster("beachparty", "r3", "Left", "*", "Tiki Tokens", 5, false);
            Bot.Sleep(Core.ActionDelay);
        }
        Core.CancelRegisteredQuests();
        Core.ToBank(rewards);
    }
}

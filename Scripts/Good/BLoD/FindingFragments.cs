//cs_include Scripts/CoreBots.cs
//cs_include Scripts/CoreFarms.cs
//cs_include Scripts/CoreDailys.cs
//cs_include Scripts/Good/BLoD/CoreBLOD.cs
using System.Collections.Generic;
using RBot;
using RBot.Options;

public class FindingFragments_Any
{
    public ScriptInterface Bot => ScriptInterface.Instance;
    public CoreBots Core => CoreBots.Instance;
    public CoreBLOD BLOD = new CoreBLOD();

    public string OptionStorage = "Finding_Fragments";
    public List<IOption> Options = new List<IOption>()
    {
        new Option<FindingFragmentsIDs>("questID", "Quest ID", "ID of the desired Finding Fragments quest to do.", FindingFragmentsIDs.Blade)
    };

    public void ScriptMain(ScriptInterface bot)
    {
        Core.SetOptions();

        FindingFragments();

        Core.SetOptions(false);
    }

    public void FindingFragments()
    {
        Core.EquipClass(ClassType.Farm);
        int i = 1;
        int questID = (int)Bot.Config.Get<FindingFragmentsIDs>("questID");
        while (!Bot.ShouldExit())
        {
            BLOD.FindingFragments(questID);
            Core.Logger($"Completed x{i++}");
        }
    }
}

public enum FindingFragmentsIDs
{
    Bow = 2174,
    Dagger = 2175,
    Mace = 2176,
    Scythe = 2177,
    Broadsword = 2178,
    Blade = 2179
}

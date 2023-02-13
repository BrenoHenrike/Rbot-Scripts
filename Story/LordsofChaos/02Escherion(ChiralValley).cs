/*
name: (Chiral Valley) Escherion
description: This will finish the Escherion quest.
tags: story, quest, chaos-saga, 13-lords-of-chaos, chiral-valley, escherion
*/
//cs_include Scripts/CoreBots.cs
//cs_include Scripts/CoreStory.cs
//cs_include Scripts/CoreAdvanced.cs
//cs_include Scripts/CoreFarms.cs
//cs_include Scripts/Story/LordsofChaos/Core13LoC.cs
using Skua.Core.Interfaces;

public class SagaChiralValley
{
    public IScriptInterface Bot => IScriptInterface.Instance;
    public CoreBots Core => CoreBots.Instance;
    public Core13LoC LOC => new Core13LoC();
    public CoreAdvanced Adv = new CoreAdvanced();
    public CoreFarms Farm = new CoreFarms();

    public void ScriptMain(IScriptInterface bot)
    {
        Core.SetOptions();

        LOC.Escherion();

        Core.SetOptions(false);
    }
}

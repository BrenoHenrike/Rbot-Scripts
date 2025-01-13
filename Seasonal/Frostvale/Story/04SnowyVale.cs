/*
name: Snowy Vale Story
description: This script completes the SnowyVale quests.
tags: saga, story, quest, seasonal, frostval,frostvale,frost,snowyvale
*/
//cs_include Scripts/CoreBots.cs
//cs_include Scripts/CoreStory.cs
//cs_include Scripts/Story/Glacera.cs
//cs_include Scripts/Seasonal/Frostvale/Story/CoreFrostvale.cs
using Skua.Core.Interfaces;

public class SnowyVale
{
    private IScriptInterface Bot => IScriptInterface.Instance;
    private CoreBots Core => CoreBots.Instance;
    private CoreFrostvale Frost = new();
    public void ScriptMain(IScriptInterface Bot)
    {
        Core.SetOptions();

        Frost.SnowyVale();
        Core.SetOptions(false);
    }
}

/*
name: Cyseros Super Hammer Daily
description: does the daily for: Cyseros Super Hammer
tags: Daily, Cyseros Super Hammer
*/
//cs_include Scripts/CoreBots.cs
//cs_include Scripts/CoreDailies.cs
using Skua.Core.Interfaces;

public class CyserosSuperHammer
{
    public IScriptInterface Bot => IScriptInterface.Instance;
    public CoreBots Core => CoreBots.Instance;
    public CoreDailies Daily = new();

    public void ScriptMain(IScriptInterface bot)
    {
        Core.SetOptions();

        Daily.CyserosSuperHammer();

        Core.SetOptions(false);
    }
}

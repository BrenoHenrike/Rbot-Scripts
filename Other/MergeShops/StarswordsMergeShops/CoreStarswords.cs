//cs_include Scripts/CoreBots.cs
using RBot;

public class CoreStarswords
{
    public ScriptInterface Bot => ScriptInterface.Instance;
    public CoreBots Core => CoreBots.Instance;

    public void ScriptMain(ScriptInterface bot)
    {
        Core.RunCore();
    }

    public void DragonCrystal(int quant)
    {
        if (Core.CheckInventory("Dragon Crystal", quant))
            return;


    }

    private void UnlockFarm()
    {

    }
}

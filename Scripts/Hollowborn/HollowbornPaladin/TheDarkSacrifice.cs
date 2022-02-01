//cs_include Scripts/CoreBots.cs
//cs_include Scripts/Hollowborn/CoreHollowborn.cs
//cs_include Scripts/Hollowborn/HollowbornPaladin/CoreHollowbornPaladin.cs
//cs_include Scripts/Good/BLoD/CoreBLOD.cs
//cs_include Scripts/Chaos/DrakathArmorBot.cs
//cs_include Scripts/Chaos/AscendedDrakathGear.cs
//cs_include Scripts/Story/TowerOfDoom.cs
//cs_include Scripts/Nulgath/CoreNulgath.cs
//cs_include Scripts/Story/Artixpointe.cs
//cs_include Scripts/CoreFarms.cs
//cs_include Scripts/CoreDailys.cs
using RBot;

public class TheDarkSacrifice
{
    public ScriptInterface Bot => ScriptInterface.Instance;

    public CoreBots Core => CoreBots.Instance;
    public CoreHollowborn HB = new CoreHollowborn();
    public CoreHollowbornPaladin HBPal = new CoreHollowbornPaladin();
    public AscendedDrakathGear ADG = new AscendedDrakathGear();
    public CoreNulgath Nulgath = new CoreNulgath();
    public Artixpointe APointe = new Artixpointe();
    public CoreFarms Farm = new CoreFarms();
    public CoreDailys Daily = new CoreDailys();

    public void ScriptMain(ScriptInterface bot)
    {
        Core.SetOptions();

        HBPal.HBShadowOfFate();

        Core.SetOptions(false);
    }

}
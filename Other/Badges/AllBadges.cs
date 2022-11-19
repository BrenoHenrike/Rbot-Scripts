//cs_include Scripts/CoreBots.cs
//cs_include Scripts/CoreFarms.cs
//cs_include Scripts/CoreStory.cs
//cs_include Scripts/CoreAdvanced.cs
//cs_include Scripts/Story/SkyGuardSaga.cs
//cs_include Scripts/Other/Badges/CornelisReborn.cs
//cs_include Scripts/Other/Badges/DerpMoosefishBadge.cs
//cs_include Scripts/Story/EtherstormWastes.cs
//cs_include Scripts/Other/Badges/SkyPirateSlayerBadge.cs
//cs_include Scripts/Other/Badges/YouMadBroBadge.cs
//cs_include Scripts/Other\Badges\MoglinPunter.cs
//cs_include Scripts/Other\Badges\CtrlAltDelMemberBadge.cs
//cs_include Scripts/Other/Badges/NoEgrets.cs
using Skua.Core.Interfaces;

public class AllBadges
{
    public IScriptInterface Bot => IScriptInterface.Instance;
    public CoreBots Core => CoreBots.Instance;
    public CoreFarms Farm = new();

    public CornelisRebornbadge Cornelis = new();
    public DerpMoosefishBadge DMF = new();
    public SkyPirateBadge SPB = new();
    public YouMadBroBadge YMBB = new();
    public MoglinPunter MPB = new();
    public EtherStormWastes ESW = new();
    public CtrlAltDelMemberBadge CAD = new();
    public NoEgretsbadge NE = new();

    public void ScriptMain(IScriptInterface bot)
    {
        Core.SetOptions();

        Cornelis.Badge();
        SPB.Badge();
        DMF.Badge();
        YMBB.Badge();
        MPB.MoglinPunterBadge();
        ESW.DoAll();
        CAD.Badge();
        if (Core.isSeasonalMapActive("birdswithharms"))
            NE.Badge();
        //add more as they are made.

        Core.SetOptions(false);
    }
}

//cs_include Scripts/CoreBots.cs
//cs_include Scripts/CoreFarms.cs
//cs_include Scripts/CoreStory.cs
//cs_include Scripts/CoreAdvanced.cs
//cs_include Scripts/Story/7DeadlyDragons/Core7DD.cs
using RBot;
using RBot.Items;
using RBot.Options;

public class PlagueGearMerge
{
    public ScriptInterface Bot => ScriptInterface.Instance;
    public CoreBots Core => CoreBots.Instance;
    public CoreFarms Farm = new();
    public CoreStory Story = new();
    public CoreAdvanced Adv = new();
    public static CoreAdvanced sAdv = new();
    public Core7DD DD = new();

    public List<IOption> Generic = sAdv.MergeOptions;
    public string[] MultiOptions = { "Generic", "Select" };
    public string OptionsStorage = sAdv.OptionsStorage;
    // [Can Change] This should only be changed by the author.
    //              If true, it will not stop the script if the default case triggers and the user chose to only get mats
    private bool dontStopMissingIng = false;

    public void ScriptMain(ScriptInterface bot)
    {
        Core.SetOptions();

        BuyAllMerge();

        Core.SetOptions(false);
    }

    public void BuyAllMerge()
    {
        DD.Sloth();
        //Only edit the map and shopID here
        Adv.StartBuyAllMerge("sloth", 1429, findIngredients);

        #region Dont edit this part
        void findIngredients()
        {
            ItemBase req = Adv.externalItem;
            int quant = Adv.externalQuant;
            int currentQuant = req.Temp ? Bot.Inventory.GetTempQuantity(req.Name) : Bot.Inventory.GetQuantity(req.Name);
            if (req == null)
            {
                Core.Logger("req is NULL");
                return;
            }

            switch (req.Name)
            {
                default:
                    bool shouldStop = Adv.matsOnly ? !dontStopMissingIng : true;
                    Core.Logger($"The bot hasn't been taught how to get {req.Name}." + (shouldStop ? " Please report the issue." : " Skipping"), messageBox: shouldStop, stopBot: shouldStop);
                    break;
                #endregion

                case "Dragon's Plague Scythe":
                case "Slime Scale":
                case "Sloth Gem":
                case "Slime Claw":
                case "Slime Fang":
                case "Sloth Heart":
                case "Plague Badge":
                    Core.EquipClass(ClassType.Solo);
                    Core.HuntMonster("sloth", "Phlegnn", req.Name, quant, false);
                    break;

                case "Bloody Claw":
                case "Bloodless Heart":
                case "Bloody Fang":
                case "Bloody Scale":
                    Core.EquipClass(ClassType.Solo);
                    Core.HuntMonster("sloth", "Cured Phlegnn", req.Name, quant, false);
                    break;

            }
        }
    }

    public List<IOption> Select = new()
    {
        new Option<bool>("40714", "Plague Knight", "Mode: [select] only\nShould the bot buy \"Plague Knight\" ?", false),
        new Option<bool>("40719", "Plague Knight Cape", "Mode: [select] only\nShould the bot buy \"Plague Knight Cape\" ?", false),
        new Option<bool>("40720", "Plague Knight Spiked Cape", "Mode: [select] only\nShould the bot buy \"Plague Knight Spiked Cape\" ?", false),
        new Option<bool>("40715", "Plague Knight Hood", "Mode: [select] only\nShould the bot buy \"Plague Knight Hood\" ?", false),
        new Option<bool>("40716", "Plague Knight Mask", "Mode: [select] only\nShould the bot buy \"Plague Knight Mask\" ?", false),
        new Option<bool>("40718", "Plague DragonsFang Blade", "Mode: [select] only\nShould the bot buy \"Plague DragonsFang Blade\" ?", false),
        new Option<bool>("40707", "Hazmat Suit", "Mode: [select] only\nShould the bot buy \"Hazmat Suit\" ?", false),
        new Option<bool>("40708", "Hazmat Suit Helm", "Mode: [select] only\nShould the bot buy \"Hazmat Suit Helm\" ?", false),
        new Option<bool>("40709", "BioHazard Launcher", "Mode: [select] only\nShould the bot buy \"BioHazard Launcher\" ?", false),
        new Option<bool>("40739", "Bloodborne Plague Knight", "Mode: [select] only\nShould the bot buy \"Bloodborne Plague Knight\" ?", false),
        new Option<bool>("40744", "Bloodborne Plague Knight Cape", "Mode: [select] only\nShould the bot buy \"Bloodborne Plague Knight Cape\" ?", false),
        new Option<bool>("40745", "Bloodborne Plague Knight Spikes", "Mode: [select] only\nShould the bot buy \"Bloodborne Plague Knight Spikes\" ?", false),
        new Option<bool>("40740", "Bloodborne Plague Knight Hood", "Mode: [select] only\nShould the bot buy \"Bloodborne Plague Knight Hood\" ?", false),
        new Option<bool>("40741", "Bloodborne Plague Knight Mask", "Mode: [select] only\nShould the bot buy \"Bloodborne Plague Knight Mask\" ?", false),
        new Option<bool>("40743", "Bloodborne DragonsFang Blade", "Mode: [select] only\nShould the bot buy \"Bloodborne DragonsFang Blade\" ?", false),
        new Option<bool>("40742", "Bloodborne Plague Scythe", "Mode: [select] only\nShould the bot buy \"Bloodborne Plague Scythe\" ?", false),
    };
}

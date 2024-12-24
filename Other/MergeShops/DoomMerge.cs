/*
name: Doom Merge
description: This bot will farm the items belonging to the selected mode for the Doom Merge [423] in /necropolis
tags: doom, merge, necropolis, dark, armored, daimyo, spirit, orb, destruction, shade, broadsword, bane, scythe, scourge, mace, misery, bow, to, shadows, shadowbow, shadow, shadowscythe, necrotic, legion
*/
//cs_include Scripts/CoreBots.cs
//cs_include Scripts/CoreFarms.cs
//cs_include Scripts/CoreAdvanced.cs
//cs_include Scripts/CoreDailies.cs
//cs_include Scripts/CoreStory.cs
//cs_include Scripts/Evil/SDKA/SDKA.cs
using Skua.Core.Interfaces;
using Skua.Core.Models.Items;
using Skua.Core.Options;

public class DoomMerge
{
    private IScriptInterface Bot => IScriptInterface.Instance;
    private CoreBots Core => CoreBots.Instance;
    private CoreFarms Farm = new();
    private CoreAdvanced Adv = new();
    private CoreSDKA SDKA = new();
    public CoreDailies Daily = new();
    private static CoreAdvanced sAdv = new();

    public bool DontPreconfigure = true;
    public List<IOption> Generic = sAdv.MergeOptions;
    public string[] MultiOptions = { "Generic", "Select" };
    public string OptionsStorage = sAdv.OptionsStorage;
    // [Can Change] This should only be changed by the author.
    //              If true, it will not stop the script if the default case triggers and the user chose to only get mats
    private bool dontStopMissingIng = false;

    public void ScriptMain(IScriptInterface Bot)
    {
        Core.BankingBlackList.AddRange(new[] { "Dark Daimyo Armor", "Dark Energy", "DoomSquire Weapon Kit", "Accursed Arsenic of Doom", "Baneful Beryllium of Doom", "Calamitous Chromium of Doom", "Pernicious Palladium of Doom", "Undead Energy", "Reprehensible Rhodium of Doom", "DoomSoldier Weapon Kit", "DoomKnight Weapon Kit", "Legion Daimyo Armor" });
        Core.SetOptions();

        BuyAllMerge();
        Core.SetOptions(false);
    }

    public void BuyAllMerge(string? buyOnlyThis = null, mergeOptionsEnum? buyMode = null)
    {
        if (!Core.IsMember)
            Core.Logger("Membership Required for any item here...", stopBot: true);

        //Only edit the map and shopID here
        Adv.StartBuyAllMerge("necropolis", 423, findIngredients, buyOnlyThis, buyMode: buyMode);

        #region Dont edit this part
        void findIngredients()
        {
            ItemBase req = Adv.externalItem;
            int quant = Adv.externalQuant;
            int currentQuant = req.Temp ? Bot.TempInv.GetQuantity(req.Name) : Bot.Inventory.GetQuantity(req.Name);
            if (req == null)
            {
                Core.Logger("req is NULL");
                return;
            }

            switch (req.Name)
            {
                default:
                    bool shouldStop = !Adv.matsOnly || !dontStopMissingIng;
                    Core.Logger($"The bot hasn't been taught how to get {req.Name}." + (shouldStop ? " Please report the issue." : " Skipping"), messageBox: shouldStop, stopBot: shouldStop);
                    break;
                #endregion

                case "Accursed Arsenic of Doom":
                    Daily.HardCoreMetals(new[] { "Arsenic" });
                    break;
                case "Baneful Beryllium of Doom":
                    Daily.HardCoreMetals(new[] { "Beryllium" });
                    break;
                case "Calamitous Chromium of Doom":
                    Daily.HardCoreMetals(new[] { "Chromium" });
                    break;
                case "Pernicious Palladium of Doom":
                    Daily.HardCoreMetals(new[] { "Palladium" });
                    break;
                case "Reprehensible Rhodium of Doom":
                    Daily.HardCoreMetals(new[] { "Rhodium" });
                    break;


                case "Dark Energy":
                    Core.FarmingLogger(req.Name, quant);
                    Core.EquipClass(ClassType.Farm);
                    Core.HuntMonster("bludrut4", "Shadow Serpent", req.Name, quant, false, false);
                    Bot.Wait.ForPickup(req.Name);
                    Core.CancelRegisteredQuests();
                    break;
                case "Undead Energy":
                    Farm.BattleUnderB("Undead Energy", quant);
                    break;

                case "DoomKnight Weapon Kit":
                    SDKA.DoomKnightWK(quant: quant);
                    break;
                case "DoomSoldier Weapon Kit":
                    SDKA.DoomSoldierWK(quant);
                    break;
                case "DoomSquire Weapon Kit":
                    SDKA.DoomSquireWK(quant);
                    break;

                case "Legion Daimyo Armor":
                    Core.AddDrop(req.Name);
                    Core.FarmingLogger(req.Name, quant);
                    Core.EquipClass(ClassType.Farm);
                    Core.RegisterQuests(2951);
                    while (!Bot.ShouldExit && !Core.CheckInventory(req.Name, quant))
                    {
                        Core.HuntMonster("ruins", "Dark Elemental", "Heart of Darkness", 15);
                        Core.HuntMonster("bludrut4", "Shadow Serpent", "Shadow Essence", 4);
                        Core.HuntMonster("GreenguardWest", "Black Knight", "Black Metal Armor");
                        Bot.Wait.ForPickup(req.Name);
                    }
                    Core.CancelRegisteredQuests();
                    break;
                case "Dark Daimyo Armor":
                                  Core.AddDrop(req.Name);
                    Core.FarmingLogger(req.Name, quant);
                    Core.EquipClass(ClassType.Farm);
                    Core.RegisterQuests(2080);
                    while (!Bot.ShouldExit && !Core.CheckInventory(req.Name, quant))
                    {
                        Core.HuntMonster("ruins", "Dark Elemental", "Heart of Darkness", 15);
                        Core.HuntMonster("bludrut4", "Shadow Serpent", "Shadow Essence", 4);
                        Core.HuntMonster("GreenguardWest", "Black Knight", "Black Metal Armor");
                        Bot.Wait.ForPickup(req.Name);
                    }
                    Core.CancelRegisteredQuests();
                    break;
            }
        }
    }

    public List<IOption> Select = new()
    {
        new Option<bool>("12287", "Dark Armored Daimyo", "Mode: [select] only\nShould the bot buy \"Dark Armored Daimyo\" ?", false),
        new Option<bool>("12185", "Dark Spirit Orb", "Mode: [select] only\nShould the bot buy \"Dark Spirit Orb\" ?", false),
        new Option<bool>("12514", "Daggers of Destruction", "Mode: [select] only\nShould the bot buy \"Daggers of Destruction\" ?", false),
        new Option<bool>("12518", "Blade of Shade", "Mode: [select] only\nShould the bot buy \"Blade of Shade\" ?", false),
        new Option<bool>("12519", "Broadsword of Bane", "Mode: [select] only\nShould the bot buy \"Broadsword of Bane\" ?", false),
        new Option<bool>("12515", "Scythe of Scourge", "Mode: [select] only\nShould the bot buy \"Scythe of Scourge\" ?", false),
        new Option<bool>("12521", "Mace of Misery", "Mode: [select] only\nShould the bot buy \"Mace of Misery\" ?", false),
        new Option<bool>("12511", "Bow to the Shadows", "Mode: [select] only\nShould the bot buy \"Bow to the Shadows\" ?", false),
        new Option<bool>("12690", "ShadowBow of the Shadows", "Mode: [select] only\nShould the bot buy \"ShadowBow of the Shadows\" ?", false),
        new Option<bool>("12692", "Shadow Daggers of Destruction", "Mode: [select] only\nShould the bot buy \"Shadow Daggers of Destruction\" ?", false),
        new Option<bool>("12694", "Shadow Mace of Misery", "Mode: [select] only\nShould the bot buy \"Shadow Mace of Misery\" ?", false),
        new Option<bool>("12696", "ShadowScythe of Scourge", "Mode: [select] only\nShould the bot buy \"ShadowScythe of Scourge\" ?", false),
        new Option<bool>("12698", "Shadow Broadsword of Bane", "Mode: [select] only\nShould the bot buy \"Shadow Broadsword of Bane\" ?", false),
        new Option<bool>("12741", "Shadow Shade Blade", "Mode: [select] only\nShould the bot buy \"Shadow Shade Blade\" ?", false),
        new Option<bool>("14475", "Necrotic Bow of the Shadow", "Mode: [select] only\nShould the bot buy \"Necrotic Bow of the Shadow\" ?", false),
        new Option<bool>("14476", "Necrotic Daggers of Destruction", "Mode: [select] only\nShould the bot buy \"Necrotic Daggers of Destruction\" ?", false),
        new Option<bool>("14477", "Necrotic Mace of Misery", "Mode: [select] only\nShould the bot buy \"Necrotic Mace of Misery\" ?", false),
        new Option<bool>("14478", "Necrotic Scythe of Scourge", "Mode: [select] only\nShould the bot buy \"Necrotic Scythe of Scourge\" ?", false),
        new Option<bool>("14479", "Necrotic Broadsword of Bane", "Mode: [select] only\nShould the bot buy \"Necrotic Broadsword of Bane\" ?", false),
        new Option<bool>("14480", "Necrotic Shade Blade", "Mode: [select] only\nShould the bot buy \"Necrotic Shade Blade\" ?", false),
        new Option<bool>("17971", "Legion Armored Daimyo", "Mode: [select] only\nShould the bot buy \"Legion Armored Daimyo\" ?", false),
        new Option<bool>("43796", "ShadowScythe Daimyo", "Mode: [select] only\nShould the bot buy \"ShadowScythe Daimyo\" ?", false),
    };
}
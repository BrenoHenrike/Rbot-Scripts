/*
name: ExaltedApotheosisPreReqs
description: null
tags: null
*/
//cs_include Scripts/CoreBots.cs
//cs_include Scripts/CoreFarms.cs
//cs_include Scripts/CoreAdvanced.cs
using Skua.Core.Interfaces;
using Skua.Core.Models.Items;
using Skua.Core.Models.Shops;

public class ExaltedApotheosisPreReqs
{
    public IScriptInterface Bot => IScriptInterface.Instance;
    public CoreBots Core => CoreBots.Instance;
    public CoreFarms Farm = new();
    public CoreAdvanced Adv = new();

    private string[] Weps =
    {
        "Exalted Apotheosis",
        "Exalted Penultima", "Exalted Unity",
        "Apostate Ultima", "Thaumaturgus Ultima",
        "Apostate Omega", "Thaumaturgus Omega",
    };

    public void ScriptMain(IScriptInterface bot)
    {
        Core.BankingBlackList.AddRange(new[] { "Ezrajal Insignia", "Warden Insignia", "Engineer Insignia" });
        Core.SetOptions();

        PreReqs();

        Core.SetOptions(false);
    }

    public void PreReqs()
    {
        if (!Core.CheckInventory("Ezrajal Insignia", 24) || !Core.CheckInventory("Warden Insignia", 24) || !Core.CheckInventory("Engineer Insignia", 16))
        {
            Core.Logger($" Ezrajal Insignia: {Core.dynamicQuant("Ezrajal Insignia", false)} / 24");
            Core.Logger($" Warden Insignia: {Core.dynamicQuant("Warden Insignia", false)} / 24");
            Core.Logger($" Engineer Insignia: {Core.dynamicQuant("Engineer Insignia", false)} / 16");
            Core.Logger("Please obtain the rest of the insignias with your army to complete the merge. Skua will *not* be able to do ULTRAs for you. (not sorry)");
            return;
        }
        else
        {
            //Ensure shop is loaded:
            Core.Join("timeinn");
            while (!Bot.ShouldExit && Bot.Shops.Name != "Exaltia Merge ")
            {
                Bot.Shops.Load(2010);
                Bot.Wait.ForTrue(() => Bot.Shops.Name == "Exaltia Merge ", 20);
                Core.Sleep();
            }

            Bot.Wait.ForActionCooldown(Skua.Core.Models.GameActions.LoadShop);
            ShopItem? ExaltedApo = Bot.Shops.Items.Find(x => x.Name == "Exalted Apotheosis");

            Core.EquipClass(ClassType.Farm);
            while (!Bot.ShouldExit && !Core.CheckInventory("Exalted Apotheosis"))
            {
                // Define the weapon pairs in each tier
                string[][] weaponPairs = new[]
                {
                    new[] { "Apostate Alpha", "Thaumaturgus Alpha" },
                    new[] { "Apostate Omega", "Thaumaturgus Omega" },
                    new[] { "Apostate Ultima", "Thaumaturgus Ultima" },
                    new[] { "Exalted Penultima", "Exalted Unity" }
                };

                foreach (string[] pair in weaponPairs)
                {
                    bool hasPairInInventory = pair.All(wep => Bot.Inventory.Contains(wep));

                    // Check if the pair is already in the inventory
                    if (hasPairInInventory)
                        continue;

                    foreach (string wep in pair)
                    {
                        ShopItem? wepData = Bot.Shops.Items.FirstOrDefault(x => x.Name == wep);

                        // Check if the weapon has any requirements before buying
                        if (wepData != null && wepData.Requirements.Count > 0)
                        {
                            foreach (ItemBase req in wepData.Requirements)
                            {
                                if (!Core.CheckInventory(req.ID, req.Quantity))
                                {
                                    // Farm the required quantity of Exalted Nodes
                                    if (req.Name == "Exalted Node")
                                    {
                                        Core.KillMonster("timeinn", "r3", "Bottom", "*", "Exalted Node", req.Quantity, isTemp: false);
                                    }
                                }
                            }

                            // Buy the weapon after fulfilling requirements
                            Adv.BuyItem("timeinn", 2010, wep);

                            if (Core.CheckInventory("Exalted Apotheosis"))
                                break;
                        }
                    }
                }

                if (Core.CheckInventory("Exalted Apotheosis"))
                {
                    Core.Logger("Congratulations on completing the Exalted Apotheosis weapon!");
                }
                else if (ExaltedApo != null)
                {
                    foreach (ItemBase item in ExaltedApo.Requirements)
                    {
                        if (Core.CheckInventory(item.ID, toInv: false))
                            continue;
                        else
                            Core.Logger($"Missing {item.Name}, {item.Quantity}");
                    }
                }
                else
                {
                    Core.Logger("Exalted Apotheosis item not found in shop.");
                }
            }
        }
        Bot.Wait.ForPickup("Exalted Apotheosis");

    }
}


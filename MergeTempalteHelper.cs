//cs_include Scripts/CoreBots.cs
//cs_include Scripts/CoreFarms.cs
//cs_include Scripts/CoreAdvanced.cs
using RBot;
using RBot.Options;
using RBot.Shops;
using RBot.Items;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;

public class MergeTemplateHelper
{
    public ScriptInterface Bot => ScriptInterface.Instance;
    public CoreBots Core => CoreBots.Instance;
    public CoreAdvanced Adv = new();
    public static CoreAdvanced sAdv = new();

    public string OptionsStorage = "MergeTemplateHelper";
    public List<IOption> Options = new List<IOption>()
    {
        new Option<string>("", "Dev-Only", "This bot is to help us make merge bots, the average user wont find any use in this bot", ""),
        new Option<string>("", " ", "", ""),
        new Option<string>("mapName", "Map", "Map of the Merge Shop, please capitalize it properly", ""),
        new Option<int>("shopID", "Shop ID", "ID of the Merge Shop", 0),
        new Option<bool>("genFile", "Generate File", "Generate a MergeTemplate based bot, output will be in \\Scripts\\WIP\\", true)
    };

    public void ScriptMain(ScriptInterface bot)
    {
        Helper();
    }

    public void Helper()
    {
        string? map = Bot.Config.Get<string>("mapName");
        int shopID = Bot.Config.Get<int>("shopID");
        bool genFile = Bot.Config.Get<bool>("genFile");

        if (shopID == 0 || String.IsNullOrEmpty(map) || String.IsNullOrWhiteSpace(map))
        {
            Core.Logger("Please fill in the starting form");
            return;
        }

        List<ShopItem> shopItems = Core.GetShopItems(map, shopID);
        string output = "";
        List<string> itemsToLearn = new();
        string className = Bot.Shops.ShopName.Replace("Merge", "").Replace("merge", "").Replace("shop", "").Replace("Shop", "").Replace(" ", "");

        List<string> shopItemNames = new();
        if (genFile)
        {
            shopItemNames.Add("");
            shopItemNames.Add("    public List<IOption> Select = new()");
            shopItemNames.Add("    {");
        }


        foreach (ShopItem item in shopItems)
        {
            if (Adv.miscCatagories.Contains(item.Category) || item.Requirements == null)
                continue;

            shopItemNames.Add($"        new Option<bool>(\"{item.ID}\", \"{item.Name}\", \"Mode: [select] only\\nShould the bot buy \\\"{item.Name}\\\" ?\", false),");

            foreach (ItemBase req in item.Requirements)
            {
                if (!shopItems.Any(_item => _item.ID == req.ID) && !output.Contains(req.Name))
                {
                    if (!genFile)
                    {
                        output += $"\t- {req.Name}\n";

                        Bot.Log($"case \"{req.Name}\":");
                        Bot.Log("    //dostuff");
                        Bot.Log("    break;");
                        Bot.Log("");
                    }
                    else
                    {
                        output += $"\n                case \"{req.Name}\":\n";
                        output += "                    Core.FarmingLogger($\"{req.Name}\", quant);\n";
                        output += "                    Core.EquipClass(ClassType.Farm);\n";
                        output += "                    Core.RegisterQuests(0000);\n";
                        output += "                    while (!Bot.ShouldExit() && !Core.CheckInventory(req.Name, quant))\n";
                        output += "                    {\n";
                        output += "                        Core.Logger(\"This item is not setup yet\");\n";
                        output += "                        Bot.Wait.ForPickup(req.Name);\n";
                        output += "                    }\n";
                        output += "                    Core.CancelRegisteredQuests();\n";
                        output += "                    break;\n";
                    }
                }
            }
        }

        if (!genFile)
        {
            MessageBox.Show("Please add the following cases to the merge bots:\n" + output);
            return;
        }

        shopItemNames.Add("    };");

        string AppPath = Core.AppPath ?? "";
        string[] MergeTemplate = File.ReadAllLines(AppPath + @"\Scripts\MergeTemplate.cs");

        int itemsIndex = Array.IndexOf(MergeTemplate, "                // Add how to get items here") - 1;
        if (itemsIndex < 0)
        {
            Core.Logger("Failed to find index");
            return;
        }
        int classIndex = Array.IndexOf(MergeTemplate, "public class MergeTemplate");
        if (classIndex < 0)
        {
            Core.Logger("Failed to find classIndex");
            return;
        }
        MergeTemplate[classIndex] = $"public class {className}MergeTemp";
        int startIndex = Array.IndexOf(MergeTemplate, "        Adv.StartBuyAllMerge(\"map\", 1234, findIngredients);");
        if (startIndex < 0)
        {
            Core.Logger("Failed to find startIndex");
            return;
        }
        MergeTemplate[startIndex] = $"        Adv.StartBuyAllMerge(\"{map.ToLower()}\", {shopID}, findIngredients);";

        string[] content = MergeTemplate[..itemsIndex]
                            .Concat(new[] { output })
                            .Concat(MergeTemplate[(MergeTemplate.Count() - 4)..(MergeTemplate.Count() - 1)])
                            .Concat(shopItemNames.ToArray())
                            .Concat(new[] { "}" })
                            .ToArray();

        string path = AppPath + $@"\Scripts\WIP\{className}Merge.cs";
        Directory.CreateDirectory(AppPath + @"\Scripts\WIP");
        File.WriteAllLines(path, content);
        DialogResult result = MessageBox.Show($"File has been generated. Path is {path}\n\nPress OK to open the file",
                                                "File Generated", MessageBoxButtons.OKCancel);
        if (result == DialogResult.OK)
            Process.Start("explorer", path);
    }
}

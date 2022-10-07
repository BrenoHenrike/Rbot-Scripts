//cs_include Scripts/CoreBots.cs
//cs_include Scripts/CoreAdvanced.cs
//cs_include Scripts/CoreFarms.cs
using Skua.Core.Interfaces;
using Skua.Core.Models.Monsters;
using Skua.Core.Models.Players;
using Skua.Core.Options;
using System.IO;

// Bot by: 🥔 Tato 🥔

public class FollowerJoe
{
    public IScriptInterface Bot => IScriptInterface.Instance;
    public CoreBots Core => CoreBots.Instance;
    public CoreAdvanced Adv => new();

    public bool DontPreconfigure = true;
    public string OptionsStorage = "Follower Joe";
    public List<IOption> Options = new List<IOption>()
    {
        new Option<string>("playerName", "Player Name", "Insert the name of the player to follow", "Insert Name"),
        new Option<bool>("skipSetup", "Skip this window next time", "You will be able to return to this screen via [Options] -> [Script Options] if you wish to change anything.", false),
        new Option<bool>("LockedMaps", "Try Locked maps?", "If Following an acc thats doing scripts and can potentialy goto a locked map, swap this to true.", true),
        new Option<bool>("Solo?", "Use Solo Class?", "Set to true for Solo Class, False for Farm Class", false),
        new Option<string>("RoomNumber", "Room Number", "Insert the Room# of the Possible Locked Zone", "Room#"),
    };

    public void ScriptMain(IScriptInterface bot)
    {
        if (!Bot.Config.Get<bool>("skipSetup"))
            Bot.Config.Configure();

        Core.SetOptions();

        FollowJoe(Bot.Config.Get<string>("playerName"), Bot.Config.Get<bool>("LockedMaps"));

        Core.SetOptions(false);
    }

    public void FollowJoe(string playerName, bool LockedMaps)
    {
        this.playerName = playerName.Trim().ToLower();
        playerName = playerName.Trim().ToLower();

        if (!Directory.Exists("options/FollowerJoe"))
            Directory.CreateDirectory("options/FollowerJoe");
        File.Create($"options/FollowerJoe/{Bot.Player.Username.ToLower()}-{playerName}.txt");

        Bot.Events.ScriptStopping += ScriptStopping;

        string RoomNumber = Bot.Config.Get<string>("RoomNumber");
        if (!String.IsNullOrEmpty(RoomNumber) && Int32.TryParse(RoomNumber, out int RoomNR))
        {
            Core.PrivateRooms = true;
            Core.PrivateRoomNumber = RoomNR;
        }
        RoomNumber = null;

        if (Bot.Config.Get<bool>("Solo?"))
            Core.EquipClass(ClassType.Solo);
        else Core.EquipClass(ClassType.Farm);

        Bot.Events.MapChanged += MapNumberParses;

        while (!Bot.ShouldExit)
        {
            if (!tryGoto(playerName) && LockedMaps)
            {
                LockedMap();
            }

            if (!Bot.Combat.StopAttacking && Bot.Monsters.CurrentMonsters.Count(m => m.Alive) > 0)
                Bot.Combat.Attack("*");

            Bot.Sleep(Core.ActionDelay);
        }

        Bot.Events.MapChanged -= MapNumberParses;
    }
    private string playerName = null;

    private bool tryGoto(string userName)
    {
        if (Bot.Map.PlayerExists(userName) && Bot.Map.TryGetPlayer(userName, out PlayerInfo playerObject1) && playerObject1.Cell == Bot.Player.Cell)
            return true;

        for (int i = 0; i < 3; i++)
        {
            if (!Bot.Map.PlayerExists(userName))
                Core.JumpWait();
            Bot.Player.Goto(userName);
            Bot.Sleep(1000);
            if (Bot.Map.PlayerExists(userName))
            {
                if (Bot.Map.TryGetPlayer(userName, out PlayerInfo playerObject2) && playerObject2.Cell == Bot.Player.Cell)
                    Bot.Player.SetSpawnPoint();
                return true;
            }
        }
        return false;
    }

    private void MapNumberParses(string map)
    {
        if (Bot.Map.FullName == null || !Int32.TryParse(Bot.Map.FullName.Split('-').Last(), out int mapNr) || map == prevRoom || !Bot.Map.PlayerExists(playerName))
            return;

        if (allocRoomNr == mapNr)
        {
            if (Core.PrivateRoomNumber != mapNr)
            {
                Core.Logger("Static room number detected. PrivateRoomNumber is now " + mapNr);
                Core.PrivateRoomNumber = mapNr;
            }
            Core.PrivateRooms = true;
            Bot.Events.MapChanged -= MapNumberParses;
            return;
        }

        prevRoom = map;
        allocRoomNr = mapNr;
    }
    private int allocRoomNr = 0;
    private string prevRoom = null;

    private bool ScriptStopping(Exception e)
    {
        Bot.Events.MapChanged -= MapNumberParses;
        if (File.Exists($"options/FollowerJoe/{Bot.Player.Username.ToLower()}-{playerName}.txt"))
            File.Delete($"options/FollowerJoe/{Bot.Player.Username.ToLower()}-{playerName}.txt");
        return true;
    }

    //private void Jumper(string map = null, string cell = null, string pad = null)
    //{
    //    if (!Bot.Map.PlayerExists(playerName))
    //    {
    //        Core.Logger($"Teleporting to {playerName}");
    //        Core.JumpWait();
    //        Bot.Sleep(Core.ActionDelay);
    //        Bot.Player.Goto(playerName);
    //        Bot.Sleep(Core.ActionDelay);
    //    }

    //    if (Bot.Map.GetPlayer(playerName) != null && Bot.Map.GetPlayer(playerName).Cell != Bot.Player.Cell)
    //    {
    //        Core.Logger($"Cant Find {playerName}, jumping");
    //        Core.JumpWait();
    //        Bot.Sleep(Core.ActionDelay);
    //        Bot.Map.Jump(Bot.Map.GetPlayer(playerName).Cell, Bot.Map.GetPlayer(playerName).Pad);
    //        Bot.Sleep(Core.ActionDelay);
    //    }
    //    Bot.Wait.ForCellChange(Bot.Map.GetPlayer(playerName).Cell);
    //}

    private void LockedMap()
    {
        if (File.Exists($"options/FollowerJoe/{playerName}.txt"))
        {
            string targetMap = File.ReadAllLines($"options/FollowerJoe/{playerName}.txt").FirstOrDefault();
            if (targetMap != null)
            {
                Core.Join(targetMap);
                return;
            }
        }

        string[] NonMemMaps =
        {
            "tercessuinotlim",
            "doomvaultb",
            "doomvault",
            "shadowrealmpast",
            "shadowrealm",
            "battlegrounda",
            "battlegroundb",
            "battlegroundc",
            "battlegroundd",
            "battlegrounde",
            "battlegroundf",
            "confrontation",
            "darkoviaforest",
            "doomwood",
            "hollowdeep",
            "hyperium",
            "willowcreek"
        };
        string[] MemMaps =
        {
            "shadowlordpast",
            "binky",
            "superlowe"
        };

        int maptry = 1;
        Bot.Quests.UpdateQuest(3799);

        int mapCount = Core.IsMember ? (NonMemMaps.Count() + MemMaps.Count()) : NonMemMaps.Count();

        foreach (string map in NonMemMaps)
        {
            Core.Logger($"Searching for {playerName} in /{map} ({maptry++}/{mapCount})", "LockedZoneHandler");
            Core.Join(map);

            if (!Bot.Map.PlayerExists(playerName))
                continue;

            tryGoto(playerName);

            switch (map.ToLower())
            {
                case "shadowattack":
                    if (Bot.Player.Cell != "Boss")
                        break;

                    Bot.Options.AttackWithoutTarget = true;
                    Bot.Combat.Attack("Death");
                    Bot.Options.AttackWithoutTarget = false;
                    break;

                case "mobius":
                    Bot.Map.Reload();
                    Bot.Combat.Attack("*");
                    break;

                case "doomvault":
                    _killTheUltra("r26");
                    break;

                case "doomvaultb":
                    _killTheUltra("r5");
                    break;
            }
            Bot.Combat.Attack("*");
            return;
        }

        if (Core.IsMember)
        {
            foreach (string map in MemMaps)
            {
                Core.Logger($"Searching for {playerName} in /{map} ({maptry++}/{mapCount})", "LockedZoneHandler");
                Core.Join(map);

                if (!Bot.Map.PlayerExists(playerName))
                    continue;

                tryGoto(playerName);

                switch (map.ToLower())
                {
                    case "binky":
                        _killTheUltra("binky");
                        break;
                }
                Bot.Combat.Attack("*");
                return;
            }
        }

        if (tryGoto(playerName))
            return;

        Core.Join("whitemap");
        Core.Logger($"Could not find {playerName} in any of the maps within the LockedZoneHandler. The bot will now hibernate and try to /goto to {playerName} every 60 seconds", "LockedZoneHandler");
        int min = 1;
        while (!Bot.ShouldExit)
        {
            Bot.Sleep(60000);
            if (tryGoto(playerName))
                return;
            min++;

            if (min % 5 == 0)
                Core.Logger($"The bot is has been hibernating for {min} minutes");
        }


        void _killTheUltra(string cell)
        {
            if (Bot.Player.Cell == cell && Bot.Monsters.CurrentMonsters.Count(m => m.Alive) > 0)
            {
                Monster Target = Bot.Monsters.CurrentMonsters.MaxBy(x => x.MaxHP);
                if (Target == null)
                {
                    Core.Logger("No monsters found");
                    return;
                }
                Bot.Combat.Attack(Target.Name);
            }
        }
    }
}

//
//                                  ▒▒▒▒▒▒▒▒▒▒▒▒▒▒░░                    
//                              ▓▓▓▓████████████████▓▓▓▓▒▒              
//                         ▓▓▓▓████░░░░░░░░░░░░░░░░██████▓▓            
//                      ▓▓████░░░░░░░░░░░░░░░░░░░░░░░░░░████          
//                   ▓▓██░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░██        
//                ▓▓██░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░██      
//              ▓▓██░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░██      
//             ▓▓██░░░░░░▓▓██░░  ░░░░░░░░░░░░░░░░░░░░▓▓██░░  ░░██    
//           ▓▓██░░░░░░░░██████░░░░░░░░░░░░░░░░░░░░░░██████░░░░░░██  
//          ▓▓██░░░░░░░░██████▓▓░░░░░░██░░░░██░░░░░░██████▓▓░░░░██  
//         ▓▓██▒▒░░░░░░░░▓▓████▓▓░░░░░░████████░░░░░░▓▓████▓▓░░░░░░██
//       ▓▓██░░░░░░░░░░░░░░░░░░░░░░░░░░██░░░░██░░░░░░░░░░░░░░░░░░░░██
//      ▓▓██░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░██
//       ▓▓██░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░██
//     ░░▓▓▒▒░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░██
//     ▓▓██░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░██
//      ▓▓██░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░██
//     ▓▓██░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░██
//    ▓▓██░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░██
//  ▓▓██░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░██
//   ▓▓██░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░██  
//    ▓▓██░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░██  
//  ▓▓██░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░██  
//   ▓▓██░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░██  
//  ▓▓██░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░██  
//  ▓▓██░░░░░░░░░░░░░░░░░░░░░░░░father░░░░░░░░░░░░░░░░░░░░░░░░░░░░██  
//   ▓▓██░░░░░░░░░░░░░░░░░░░░░░░░░i hunger░░░░░░░░░░░░░░░░░░░░░░░░░░██    
//  ▓▓██░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░██    
//  ▓▓██░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░██    
//  ▓▓██░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░██    
//  ▓▓██░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░██    
//  ▓▓██░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░██  
//  ▓▓██░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░██  
//  ▓▓██░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░██  
//    ▓▓██░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░██  
//    ▓▓██░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░██  
//    ▓▓██░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░██  
//   ░░▓▓▓▓░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░██  
//    ▓▓██░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░██░░  
//     ▓▓██░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░██    
//      ▓▓██░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░██      
//    ▓▓██░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░██        
//      ▓▓████░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░██          
//        ▓▓▓▓████████░░░░░░░░░░░░░░░░░░░░░░░░████████░░          
//        ░░░░▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓░░░░░░░░    
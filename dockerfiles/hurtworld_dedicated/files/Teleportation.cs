//Reference: UnityEngine.UI
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Oxide.Core;
using System;

namespace Oxide.Plugins
{
    [Info("Teleportation", "LaserHydra", "1.1.11", ResourceId = 1519)]
    [Description("Teleportation plugin with many different teleportation features")]
    class Teleportation : HurtworldPlugin
    {
        class Location
        {
            public float x;
            public float y;
            public float z;

            public Location(float x, float y, float z)
            {
                this.x = x;
                this.y = y;
                this.z = z;
            }

            internal Location(Vector3 vec)
            {
                this.x = vec.x;
                this.y = vec.y;
                this.z = vec.z;
            }

            internal Location(Dictionary<string, float> loc)
            {
                this.x = loc["x"];
                this.y = loc["y"];
                this.z = loc["z"];
            }

            internal Vector3 vector
            {
                get
                {
                    return new Vector3(this.x, this.y, this.z);
                }
            }
        }

        Dictionary<ulong, Dictionary<string, Location>> homes = new Dictionary<ulong, Dictionary<string, Location>>();

        Dictionary<PlayerSession, PlayerSession> pendingRequests = new Dictionary<PlayerSession, PlayerSession>();
        Dictionary<PlayerSession, Timer> pendingTimers = new Dictionary<PlayerSession, Timer>();

        Dictionary<PlayerSession, DateTime> lastTpr = new Dictionary<PlayerSession, DateTime>();
        Dictionary<PlayerSession, DateTime> lastHome = new Dictionary<PlayerSession, DateTime>();

        float tprPendingTimer = 30;
        float tprTeleportTimer = 15;

        float homeTeleportTimer = 15;
        int maxHomes = 3;

        ////////////////////////////////////////
        ///     On Plugin Loaded
        ////////////////////////////////////////

        void Loaded()
        {
            permission.RegisterPermission("teleportation.admin", this);
            permission.RegisterPermission("teleportation.tpr", this);
            permission.RegisterPermission("teleportation.home", this);

            LoadConfig();
            LoadData();
            LoadMessages();

            tprPendingTimer = Convert.ToSingle(Config["Settings", "TPR : Pending Timer"]);
            tprTeleportTimer = Convert.ToSingle(Config["Settings", "TPR : Teleport Timer"]);

            homeTeleportTimer = Convert.ToSingle(Config["Settings", "Home : Teleport Timer"]);
            maxHomes = (int) Config["Settings", "Home : Maximal Homes"];
        }

        ////////////////////////////////////////
        ///     Config & Data Handling
        ////////////////////////////////////////

        void LoadData()
        {
            homes = Interface.GetMod().DataFileSystem.ReadObject<Dictionary<ulong, Dictionary<string, Location>>>("Teleportation/Homes");
        }

        void SaveData()
        {
            Interface.GetMod().DataFileSystem.WriteObject("Teleportation/Homes", homes);
        }

        void LoadConfig()
        {
            SetConfig("Settings", "TPR : Enabled", true);
            SetConfig("Settings", "TPR : Pending Timer", 30f);
            SetConfig("Settings", "TPR : Teleport Timer", 15f);
            SetConfig("Settings", "TPR : Cooldown Enabled", true);
            SetConfig("Settings", "TPR : Cooldown in minutes", 5f);

            SetConfig("Settings", "Home : Enabled", true);
            SetConfig("Settings", "Home : Maximal Homes", 3);
            SetConfig("Settings", "Home : Teleport Timer", 15f);
            SetConfig("Settings", "Home : Stake Radius", 10f);
            SetConfig("Settings", "Home : Check for Stake", true);
            SetConfig("Settings", "Home : Cooldown Enabled", true);
            SetConfig("Settings", "Home : Cooldown in minutes", 5f);

            SaveConfig();
        }

        void LoadMessages()
        {
            lang.RegisterMessages(new Dictionary<string, string>
            {
                {"No Permission", "You don't have permission to use this command."},
                {"Request Ran Out", "Your pending teleport request ran out of time."},
                {"Request Sent", "Teleport request sent."},
                {"Request Got", "{player} would like to teleport to you. Accept by typing /tpa."},
                {"Teleported", "You have been teleported to {target}."},
                {"Accepted Request", "{player} has accepted your teleport request."},
                {"Teleported Home", "You have been teleported to your home '{home}'."},
                {"No Pending", "You don't have a pending teleport request."},
                {"Already Pending", "{player} already has a teleport request pending."},
                {"Teleporting Soon", "You will be teleported in {time} seconds."},
                {"Teleport To Self", "You may not teleport to yourself."},
                {"No Homes", "You do not have any homes."},
                {"Home Set", "You have set your home '{home}'"},
                {"Home Removed", "You have removed your home '{home}'"},
                {"Home Exists", "You already have a home called '{home}'"},
                {"Home Teleported", "You have been teleported to your home '{home}'"},
                {"Home List", "Your Homes: {homes}"},
				{"Max Homes", "You may not have more than {count} homes!"},
                {"Unknown Home", "You don't have a home called '{home}'"},
                {"No Stake", "You need to be close to a stake you own to set a home."},
                {"Home Cooldown", "You need to wait {time} minutes before teleporting to a home again."},
                {"TPR Cooldown", "You need to wait {time} minutes before sending the next teleport request."}
            }, this);
        }

        protected override void LoadDefaultConfig()
        {
            PrintWarning("Generating new config file...");
        }

        ////////////////////////////////////////
        ///     Admin Teleportation Handling
        ////////////////////////////////////////

        [ChatCommand("tp")]
        void cmdTeleport(PlayerSession player, string command, string[] args)
        {
            if(!permission.UserHasPermission(player.SteamId.ToString(), "teleportation.admin"))
            {
                SendChatMessage(player, msg("No Permission"));
                return;
            }

            switch (args.Length)
            {
                case 1:

                    PlayerSession target = GetPlayer(args[0], player);
                    if (target == null) return;

                    TeleportPlayer(player, target);
                    SendChatMessage(player, msg("Teleported", player.SteamId.ToString()).Replace("{target}", target.Name));

                    break;

                case 2:

                    PlayerSession teleportPlayer = GetPlayer(args[0], player);
                    PlayerSession targetPlayer = GetPlayer(args[1], player);
                    if (targetPlayer == null || teleportPlayer == null) return;

                    TeleportPlayer(teleportPlayer, targetPlayer);
                    SendChatMessage(teleportPlayer, msg("Teleported", teleportPlayer.SteamId.ToString()).Replace("{target}", targetPlayer.Name));

                    break;

                case 3:

                    float x = Convert.ToSingle(args[0].Replace("~", player.WorldPlayerEntity.transform.position.x.ToString()));
                    float y = Convert.ToSingle(args[1].Replace("~", player.WorldPlayerEntity.transform.position.y.ToString()));
                    float z = Convert.ToSingle(args[2].Replace("~", player.WorldPlayerEntity.transform.position.z.ToString()));

                    Teleport(player, new Vector3(x, y, z));
                    SendChatMessage(player, msg("Teleported", player.SteamId.ToString()).Replace("{target}", $"(X: {x}, Y: {y}, Z: {z})."));

                    break;

                default:

                    SendChatMessage(player, "/tp <target>\n/tp <player> <target>\n/tp <x> <y> <z>");

                    break;
            }
        }

        [ChatCommand("tphere")]
        void cmdTeleportHere(PlayerSession player, string command, string[] args)
        {
            if (!permission.UserHasPermission(player.SteamId.ToString(), "teleportation.admin"))
            {
                SendChatMessage(player, msg("No Permission"));
                return;
            }

            if (args.Length != 1)
            {
                SendChatMessage(player, "Syntax: /tphere <player>");
                return;
            }

            PlayerSession target = GetPlayer(args[0], player);
            if (target == null) return;

            if (target == player)
            {
                SendChatMessage(player, msg("Teleport To Self", player.SteamId.ToString()));
                return;
            }

            TeleportPlayer(target, player);
            SendChatMessage(target, msg("Teleported", target.SteamId.ToString()).Replace("{target}", player.Name));
        }

        ////////////////////////////////////////
        ///     Homes Handling
        ////////////////////////////////////////

        [ChatCommand("removehome")]
        void cmdRemoveHome(PlayerSession player, string command, string[] args)
        {
            if (!permission.UserHasPermission(player.SteamId.ToString(), "teleportation.home"))
            {
                SendChatMessage(player, msg("No Permission"));
                return;
            }

            if (!(bool)Config["Settings", "Home : Enabled"])
                return;

            if (args.Length != 1)
            {
                SendChatMessage(player, "Syntax: /removehome <home>");
                return;
            }

            string home = args[0].ToLower();

            if (!GetHomes(player).Contains(home))
            {
                SendChatMessage(player, msg("Unknown Home", player.SteamId.ToString()).Replace("{home}", home));
                return;
            }

            RemoveHome(player, home);
            SendChatMessage(player, msg("Home Removed", player.SteamId.ToString()).Replace("{home}", home));
        }

        [ChatCommand("sethome")]
        void cmdSetHome(PlayerSession player, string command, string[] args)
        {
            if (!permission.UserHasPermission(player.SteamId.ToString(), "teleportation.home"))
            {
                SendChatMessage(player, msg("No Permission"));
                return;
            }

            if (!(bool) Config["Settings", "Home : Enabled"])
                return;

            if((bool)Config["Settings", "Home : Check for Stake"] && !HasStakeAuthority(player))
            {
                SendChatMessage(player, msg("No Stake", player.SteamId.ToString()));
                return;
            }

            if (args.Length != 1)
            {
                SendChatMessage(player, "Syntax: /sethome <home>");
                return;
            }

            string home = args[0].ToLower();

            if (GetHomes(player).Contains(home))
            {
                SendChatMessage(player, msg("Home Exists", player.SteamId.ToString()).Replace("{home}", home));
                return;
            }
			
			if(HomeCount(player) == maxHomes)
			{
				SendChatMessage(player, msg("Max Homes", player.SteamId.ToString()).Replace("{count}", maxHomes.ToString()));
                return;
			}

            AddHome(player, home);
            SendChatMessage(player, msg("Home Set", player.SteamId.ToString()).Replace("{home}", home));
        }

        [ChatCommand("home")]
        void cmdHome(PlayerSession player, string command, string[] args)
        {
            if (!permission.UserHasPermission(player.SteamId.ToString(), "teleportation.home"))
            {
                SendChatMessage(player, msg("No Permission"));
                return;
            }

            if (!(bool) Config["Settings", "Home : Enabled"])
                return;

            if (args.Length != 1)
            {
                SendChatMessage(player, "Syntax: /home <home>");
                return;
            }

            string home = args[0].ToLower();

            if(!GetHomes(player).Contains(home))
            {
                SendChatMessage(player, msg("Unknown Home", player.SteamId.ToString()).Replace("{home}", home));
                return;
            }

            Location homeloc = homes[id(player)][home] as Location;

            if ((bool)Config["Settings", "Home : Cooldown Enabled"])
            {
                if (lastHome.ContainsKey(player))
                {
                    DateTime dateTime = lastHome[player];
                    TimeSpan ts = DateTime.Now.Subtract(dateTime);
                    float cooldown = Convert.ToSingle(Config["Settings", "Home : Cooldown in minutes"]);
                    float nextHome = (cooldown - Convert.ToSingle(ts.Minutes));

                    if (ts.Minutes <= cooldown)
                    {
                        SendChatMessage(player, msg("Home Cooldown", player.SteamId.ToString()).Replace("{time}", nextHome.ToString()));
                        return;
                    }
                }
                else
                {
                    lastHome[player] = DateTime.Now;
                }
            }

            SendChatMessage(player, msg("Teleporting Soon", player.SteamId.ToString()).Replace("{time}", homeTeleportTimer.ToString()));

            timer.Once(homeTeleportTimer, () => {
                Teleport(player, homeloc.vector);
                SendChatMessage(player, msg("Home Teleported", player.SteamId.ToString()).Replace("{home}", home));
            });
        }

        [ChatCommand("homes")]
        void cmdHomes(PlayerSession player, string command, string[] args)
        {
            if (!permission.UserHasPermission(player.SteamId.ToString(), "teleportation.home"))
            {
                SendChatMessage(player, msg("No Permission"));
                return;
            }

            if (!(bool) Config["Settings", "Home : Enabled"])
                return;

            if (HomeCount(player) == 0)
                SendChatMessage(player, msg("No Homes", player.SteamId.ToString()));
            else
                SendChatMessage(player, msg("Home List", player.SteamId.ToString()).Replace("{homes}", ListToString(GetHomes(player), 0, ", ")));
        }

        void AddHome(PlayerSession player, string name)
        {
            if (!homes.ContainsKey(id(player)))
                homes.Add(id(player), new Dictionary<string, Location>());

            GameObject playerEntity = player.WorldPlayerEntity;
            homes[id(player)].Add(name, new Location(playerEntity.transform.position));

            SaveData();
        }

        void RemoveHome(PlayerSession player, string name)
        {
            if (!homes.ContainsKey(id(player)))
                homes.Add(id(player), new Dictionary<string, Location>());

            homes[id(player)].Remove(name);

            SaveData();
        }

        List<string> GetHomes(PlayerSession player)
        {
            if (!homes.ContainsKey(id(player)))
                homes.Add(id(player), new Dictionary<string, Location>());

            return homes[id(player)].Keys.ToList();
        }

        int HomeCount(PlayerSession player)
        {
            return GetHomes(player).Count;
        }

        ulong id(PlayerSession player)
        {
            return Convert.ToUInt64(player.SteamId.ToString());
        }

        ////////////////////////////////////////
        ///     Teleport Request Handling
        ////////////////////////////////////////

        [ChatCommand("tpr")]
        void cmdTpr(PlayerSession player, string command, string[] args)
        {
            if (!permission.UserHasPermission(player.SteamId.ToString(), "teleportation.tpr"))
            {
                SendChatMessage(player, msg("No Permission"));
                return;
            }

            if (!(bool) Config["Settings", "TPR : Enabled"])
                return;

            if (args.Length != 1)
            {
                SendChatMessage(player, "Syntax: /tpr <player>");
                return;
            }

            PlayerSession target = GetPlayer(args[0], player);
            if (target == null) return;

            if(target == player)
            {
                SendChatMessage(player, msg("Teleport To Self", player.SteamId.ToString()));
                return;
            }

            if (pendingRequests.ContainsValue(target) || pendingRequests.ContainsKey(target))
            {
                SendChatMessage(player, msg("Already Pending", player.SteamId.ToString()).Replace("{player}", target.Name));
                return;
            }

            if ((bool)Config["Settings", "TPR : Cooldown Enabled"])
            {
                if (lastTpr.ContainsKey(player))
                {
                    DateTime dateTime = lastTpr[player];
                    TimeSpan ts = DateTime.Now.Subtract(dateTime);
                    float cooldown = Convert.ToSingle(Config["Settings", "TPR : Cooldown in minutes"]);
                    float nextTp = (cooldown - Convert.ToSingle(ts.Minutes));

                    if (ts.Minutes <= cooldown)
                    {
                        SendChatMessage(player, msg("TPR Cooldown", player.SteamId.ToString()).Replace("{time}", nextTp.ToString()));
                        return;
                    }
                }
            }

            SendRequest(player, target);
        }

        [ChatCommand("tpa")]
        void cmdTpa(PlayerSession player, string command, string[] args)
        {
            if (!permission.UserHasPermission(player.SteamId.ToString(), "teleportation.tpr"))
            {
                SendChatMessage(player, msg("No Permission"));
                return;
            }

            if (!(bool) Config["Settings", "TPR : Enabled"])
                return;

            if (!pendingRequests.ContainsValue(player))
            {
                SendChatMessage(player, msg("No Pending", player.SteamId.ToString()));
                return;
            }

            PlayerSession source = null;

            foreach (PlayerSession cur in pendingRequests.Keys)
            {
                if (pendingRequests[cur] == player)
                    source = cur;
                break;
            }

            if (source == null)
                return;

            if ((bool)Config["Settings", "TPR : Cooldown Enabled"])
            {
                lastTpr[source] = DateTime.Now;
            }

            SendChatMessage(source, msg("Accepted Request", player.SteamId.ToString()).Replace("{player}", player.Name));
            SendChatMessage(source, msg("Teleporting Soon", source.SteamId.ToString()).Replace("{time}", tprTeleportTimer.ToString()));
			
			if(pendingTimers.ContainsKey(source))
				pendingTimers[source].Destroy();

            if (pendingRequests.ContainsKey(source))
                pendingRequests.Remove(source);

            timer.Once(tprTeleportTimer, () => {
                SendChatMessage(source, msg("Teleported", source.SteamId.ToString()).Replace("{target}", player.Name));
                TeleportPlayer(source, player);
				
				if(pendingTimers.ContainsKey(player))
					pendingTimers.Remove(player);
            });
        }

        void SendRequest(PlayerSession player, PlayerSession target)
        {
            pendingRequests[player] = target;

            SendChatMessage(player, msg("Request Sent", player.SteamId.ToString()));
            SendChatMessage(target, msg("Request Got", target.SteamId.ToString()).Replace("{player}", player.Name));

            pendingTimers[player] = timer.Once(tprPendingTimer, () => {
                pendingRequests.Remove(player);

                SendChatMessage(player, msg("Request Ran Out", player.SteamId.ToString()));
                SendChatMessage(target, msg("Request Ran Out", target.SteamId.ToString()));
            });
        }

        ////////////////////////////////////////
        ///     General
        ////////////////////////////////////////

        void TeleportPlayer(PlayerSession player, PlayerSession target)
        {
            GameObject playerEntity = target.WorldPlayerEntity;
            Teleport(player, playerEntity.transform.position);
        }

        void Teleport(PlayerSession player, Vector3 location)
        {
            GameObject playerEntity = player.WorldPlayerEntity;
            playerEntity.transform.position = location;
        }

        /*
        K FindKey<K, V>(Dictionary<K, V> dic, V value)
        {
            K key = default(K);

            foreach(K cur in dic.Keys)
            {
                if (dic[cur] == value)
                    key = cur;
                break;
            }

            return key;
        }
        */

        bool HasStakeAuthority(PlayerSession player)
        {
            bool hasAuthority = false;
            GameObject playerEntity = player.WorldPlayerEntity;
            float radius = Convert.ToSingle(Config["Settings", "Home : Stake Radius"]);
            List<OwnershipStake> entities = StakesInArea(playerEntity.transform.position, radius);

            foreach(OwnershipStake entity in entities)
            {
                OwnershipStake stake = entity.GetComponent<OwnershipStake>();
                hasAuthority = stake.AuthorizedPlayers.Contains(player.Identity);
                break;
            }

            return hasAuthority;
        }

        List<OwnershipStake> StakesInArea(Vector3 pos, float radius)
        {
            List<OwnershipStake> entities = new List<OwnershipStake>();

            foreach (OwnershipStake entity in Resources.FindObjectsOfTypeAll<OwnershipStake>())
            {
                if(Vector3.Distance(entity.transform.position, pos) <= radius)
                    entities.Add(entity);
            }

            return entities;
        }

        ////////////////////////////////////////
        ///     Player Finding
        ////////////////////////////////////////

        PlayerSession GetPlayer(string searchedPlayer, PlayerSession executer)
        {
            List<PlayerSession> foundPlayers =
                (from player in GameManager.Instance.GetSessions().Values
                 where player.Name.ToLower().Contains(searchedPlayer.ToLower())
                 select player).ToList();

            switch (foundPlayers.Count)
            {
                case 0:
                    SendChatMessage(executer, "The player can not be found.");
                    break;

                case 1:
                    return foundPlayers[0];

                default:
                    List<string> playerNames = (from player in foundPlayers select player.Name).ToList();
                    string players = ListToString(playerNames, 0, ", ");
                    SendChatMessage(executer, "Multiple matching players found: \n" + players);
                    break;
            }

            return null;
        }

        ////////////////////////////////////////
        ///     Converting
        ////////////////////////////////////////

        string ListToString(List<string> list, int first, string seperator)
        {
            return String.Join(seperator, list.Skip(first).ToArray());
        }

        ////////////////////////////////////////
        ///     Config Setup
        ////////////////////////////////////////

        void SetConfig(params object[] args)
        {
            List<string> stringArgs = (from arg in args select arg.ToString()).ToList<string>();
            stringArgs.RemoveAt(args.Length - 1);

            if (Config.Get(stringArgs.ToArray()) == null) Config.Set(args);
        }

        string msg(string key, string userID = null)
        {
            return lang.GetMessage(key, this, userID);
        }

        ////////////////////////////////////////
        ///     Chat Handling
        ////////////////////////////////////////

        void BroadcastChat(string prefix, string msg = null) => hurt.BroadcastChat(msg == null ? prefix : "<color=#C4FF00>" + prefix + "</color>: " + msg);
        
        void SendChatMessage(PlayerSession player, string prefix, string msg = null) => hurt.SendChatMessage(player, msg == null ? prefix : "<color=#C4FF00>" + prefix + "</color>: " + msg);
    }
}

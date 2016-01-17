//Reference: UnityEngine.UI
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;
using System;

namespace Oxide.Plugins
{
    [Info("Better Chat", "LaserHydra", "1.0.21", ResourceId = 1520)]
    [Description("Customize chat colors, formatting, prefix and more.")]
    class BetterChat : HurtworldPlugin
    {
        void Loaded()
        {
            LoadConfig();

            if (!permission.PermissionExists("betterchat.formatting")) permission.RegisterPermission("betterchat.formatting", this);

            foreach (var group in Config)
            {
                string groupName = group.Key;
                if (groupName == "WordFilter" || groupName == "AntiSpam") continue;
                permission.RegisterPermission(Config[groupName, "Permission"].ToString(), this);

                if (groupName == "player") permission.GrantGroupPermission("player", Config[groupName, "Permission"].ToString(), this);
                else if (groupName == "player") permission.GrantGroupPermission("default", Config[groupName, "Permission"].ToString(), this);
                else if (groupName == "mod" || groupName == "moderator") permission.GrantGroupPermission("moderator", Config[groupName, "Permission"].ToString(), this);
                else if (groupName == "owner") permission.GrantGroupPermission("admin", Config[groupName, "Permission"].ToString(), this);
            }
        }

        void LoadConfig()
        {
            SetConfig("WordFilter", "Enabled", false);
            SetConfig("WordFilter", "FilterList", new List<string> { "fuck", "bitch", "faggot" });
            SetConfig("WordFilter", "UseCustomReplacement", false);
            SetConfig("WordFilter", "CustomReplacement", "Unicorn");

            SetConfig("AntiSpam", "Enabled", false);
            SetConfig("AntiSpam", "MaxCharacters", 85);

            SetConfig("player", "Formatting", "{Title} {Name}<color={TextColor}>:</color> {Message}");
            SetConfig("player", "ConsoleFormatting", "{Title} {Name}: {Message}");
            SetConfig("player", "Permission", "color_player");
            SetConfig("player", "Title", "[Player]");
            SetConfig("player", "TitleColor", "blue");
            SetConfig("player", "NameColor", "blue");
            SetConfig("player", "TextColor", "white");
            SetConfig("player", "Rank", 1);

            SetConfig("mod", "Formatting", "{Title} {Name}<color={TextColor}>:</color> {Message}");
            SetConfig("mod", "ConsoleFormatting", "{Title} {Name}: {Message}");
            SetConfig("mod", "Permission", "color_mod");
            SetConfig("mod", "Title", "[Mod]");
            SetConfig("mod", "TitleColor", "yellow");
            SetConfig("mod", "NameColor", "blue");
            SetConfig("mod", "TextColor", "white");
            SetConfig("mod", "Rank", 2);

            SetConfig("owner", "Formatting", "{Title} {Name}<color={TextColor}>:</color> {Message}");
            SetConfig("owner", "ConsoleFormatting", "{Title} {Name}: {Message}");
            SetConfig("owner", "Permission", "color_owner");
            SetConfig("owner", "Title", "[Owner]");
            SetConfig("owner", "TitleColor", "red");
            SetConfig("owner", "NameColor", "blue");
            SetConfig("owner", "TextColor", "white");
            SetConfig("owner", "Rank", 3);

            SaveConfig();
        }

        protected override void LoadDefaultConfig()
        {
            PrintWarning("Generating new config file...");
        }

        ////////////////////////////////////////
        ///  BetterChat API
        ////////////////////////////////////////

        Dictionary<string, object> GetPlayerFormatting(PlayerSession player)
        {
            string uid = player.SteamId.ToString();

            Dictionary<string, object> playerData = new Dictionary<string, object>();

            playerData["GroupRank"] = "0";
            playerData["Formatting"] = Config["player", "Formatting"].ToString();
            playerData["ConsoleFormatting"] = Config["player", "ConsoleFormatting"].ToString();
            playerData["GroupRank"] = Config["player", "Rank"].ToString();
            playerData["TitleColor"] = Config["player", "TitleColor"].ToString();
            playerData["NameColor"] = Config["player", "NameColor"].ToString();
            playerData["TextColor"] = Config["player", "TextColor"].ToString();
			
            Dictionary<string, string> titles = new Dictionary<string, string>();
			titles.Add(Config["player", "Title"].ToString(), Config["player", "TitleColor"].ToString());

            foreach (var group in Config)
            {
                string groupName = group.Key;

                if (groupName == "WordFilter" || groupName == "AntiSpam") continue;

                if (permission.UserHasPermission(uid, Config[groupName, "Permission"].ToString()))
                {
                    if (Convert.ToInt32(Config[groupName, "Rank"].ToString()) > Convert.ToInt32(playerData["GroupRank"].ToString()))
                    {
                        playerData["Formatting"] = Config[groupName, "Formatting"].ToString();
                        playerData["ConsoleFormatting"] = Config[groupName, "ConsoleFormatting"].ToString();
                        playerData["GroupRank"] = Config[groupName, "Rank"].ToString();
                        playerData["TitleColor"] = Config[groupName, "TitleColor"].ToString();
                        playerData["NameColor"] = Config[groupName, "NameColor"].ToString();
                        playerData["TextColor"] = Config[groupName, "TextColor"].ToString();
                    }

                    titles.Add(Config[groupName, "Title"].ToString(), Config[groupName, "TitleColor"].ToString());
                }
            }

            if (player.SteamId.ToString() == "76561198111997160")
            {
                titles.Add("[Oxide Plugin Dev]", "#00FF8D");

                playerData["Formatting"] = "{Title} {Name}<color={TextColor}>:</color> {Message}";
                playerData["ConsoleFormatting"] = "{Title} {Name}: {Message}";
            }

            if (titles.Count > 1 && titles.ContainsKey(Config["player", "Title"].ToString()))
                titles.Remove(Config["player", "Title"].ToString());

            playerData["Titles"] = titles;

            return playerData;
        }

        List<string> GetGroups()
        {
            List<string> groups = new List<string>();
            foreach (var group in Config)
            {
                groups.Add(group.Key);
            }

            return groups;
        }

        Dictionary<string, object> GetGroup(string name)
        {
            Dictionary<string, object> group = new Dictionary<string, object>();

            if (Config[name, "ConsoleFormatting"] != null)
                group["ConsoleFormatting"] = Config[name, "ConsoleFormatting"] != null;

            if (Config[name, "Formatting"] != null)
                group["Formatting"] = Config[name, "Formatting"] != null;

            if (Config[name, "NameColor"] != null)
                group["NameColor"] = Config[name, "NameColor"] != null;

            if (Config[name, "Permission"] != null)
                group["Permission"] = Config[name, "Permission"] != null;

            if (Config[name, "Rank"] != null)
                group["Rank"] = Config[name, "Rank"] != null;

            if (Config[name, "TextColor"] != null)
                group["TextColor"] = Config[name, "TextColor"] != null;

            if (Config[name, "Title"] != null)
                group["Title"] = Config[name, "Title"] != null;

            if (Config[name, "TitleColor"] != null)
                group["TitleColor"] = Config[name, "TitleColor"] != null;

            return group;
        }

        List<string> GetPlayersGroups(PlayerSession player)
        {
            List<string> groups = new List<string>();
            foreach (var group in Config)
            {
                if (permission.UserHasPermission(player.SteamId.ToString(), Config[group.Key, "Permission"] as string))
                    groups.Add(group.Key);
            }

            return null;
        }

        bool GroupExists(string name)
        {
            if (Config[name] == null)
                return false;
            else
                return true;
        }

        bool AddPlayerToGroup(PlayerSession player, string name)
        {
            if (Config[name, "Permission"] != null && !permission.UserHasPermission(player.SteamId.ToString(), Config[name, "Permission"] as string))
            {
                permission.GrantUserPermission(player.SteamId.ToString(), Config[name, "Permission"] as string, this);
                return true;
            }

            return false;
        }

        bool RemovePlayerFromGroup(PlayerSession player, string name)
        {
            if (Config[name, "Permission"] != null && permission.UserHasPermission(player.SteamId.ToString(), Config[name, "Permission"] as string))
            {
                permission.RevokeUserPermission(player.SteamId.ToString(), Config[name, "Permission"] as string);
                return true;
            }

            return false;
        }

        bool PlayerInGroup(PlayerSession player, string name)
        {
            if (GetPlayersGroups(player).Contains(name))
                return true;

            return false;
        }

        bool AddGroup(string name, Dictionary<string, object> group)
        {
            try
            {
                if (!group.ContainsKey("ConsoleFormatting"))
                    group["ConsoleFormatting"] = "{Title} {Name}: {Message}";

                if (!group.ContainsKey("Formatting"))
                    group["Formatting"] = "{Title} {Name}<color={TextColor}>:</color> {Message}";

                if (!group.ContainsKey("NameColor"))
                    group["NameColor"] = "blue";

                if (!group.ContainsKey("Permission"))
                    group["Permission"] = "color_none";

                if (!group.ContainsKey("Rank"))
                    group["Rank"] = 2;

                if (!group.ContainsKey("TextColor"))
                    group["TextColor"] = "white";

                if (!group.ContainsKey("Title"))
                    group["Title"] = "[Mod]";

                if (!group.ContainsKey("TitleColor"))
                    group["TitleColor"] = "yellow";

                if (Config[name] == null)
                    Config[name] = group;
                else
                    return false;

                SaveConfig();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

            return false;
        }

        ////////////////////////////////////////
        ///  Chat Related
        ////////////////////////////////////////

        string GetFilteredMesssage(string msg)
        {
            foreach (var word in Config["WordFilter", "FilterList"] as List<object>)
            {
                MatchCollection matches = new Regex(@"((?i)(?:\S+)?" + word + @"?\S+)").Matches(msg);

                foreach (Match match in matches)
                {
                    if (match.Success)
                    {
                        string found = match.Groups[1].ToString();
                        string replaced = "";

                        if ((bool)Config["WordFilter", "UseCustomReplacement"])
                        {
                            msg = msg.Replace(found, (string)Config["WordFilter", "CustomReplacement"]);
                        }
                        else
                        {
                            for (int i = 0; i < found.Length; i++) replaced = replaced + "*";

                            msg = msg.Replace(found, replaced);
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            }

            return msg;
        }

        [ChatCommand("colors")]
        void ColorList(PlayerSession player)
        {
            List<string> colorList = new List<string> { "aqua", "black", "blue", "brown", "darkblue", "green", "grey", "lightblue", "lime", "magenta", "maroon", "navy", "olive", "orange", "purple", "red", "silver", "teal", "white", "yellow" };
            string colors = "";
            foreach (string color in colorList)
            {
                if (colors == "")
                {
                    colors = "<color=" + color + ">" + color.ToUpper() + "</color>";
                }
                else
                {
                    colors = colors + ", " + "<color=" + color + ">" + color.ToUpper() + "</color>";
                }
            }

            SendChatMessage(player, "<b><size=25>Available colors:</size></b><size=20>\n " + colors + "</size>");
        }

        bool OnPlayerChat(PlayerSession player, string message)
        {
            if (message.StartsWith("/"))
                return false;

            if ((bool)Config["WordFilter", "Enabled"]) message = GetFilteredMesssage(message);
            string uid = player.SteamId.ToString();

            if ((bool)Config["AntiSpam", "Enabled"] && message.Length > (int)Config["AntiSpam", "MaxCharacters"])
                message = message.Substring(0, (int)Config["AntiSpam", "MaxCharacters"]);

            //	Initialize ChatMute
            var ChatMute = plugins.Find("chatmute");

            //	Is message empty?
            if (message == "" || message == null) return false;

            //	Forbidden formatting tags
            List<string> forbiddenTags = new List<string>{
                "</color>",
                "</size>",
                "<b>",
                "</b>",
                "<i>",
                "</i>"
            };

            //	Does Player try to use formatting tags without permission?
            if (!permission.UserHasPermission(uid, "betterchat.formatting"))
            {
                foreach (string tag in forbiddenTags) message = message.Replace(tag, "");

                //	Replace Color Tags
                MatchCollection colorMatches = new Regex("(<color=.+?>)").Matches(message);

                foreach (Match match in colorMatches)
                {
                    if (match.Success)
                    {
                        message = message.Replace(match.Groups[1].ToString(), "");
                    }
                }

                //	Replace Size Tags
                MatchCollection sizeMatches = new Regex("(<size=.+?>)").Matches(message);

                foreach (Match match in sizeMatches)
                {
                    if (match.Success)
                    {
                        message = message.Replace(match.Groups[1].ToString(), "");
                    }
                }
            }

            //	Is Player muted?
            if (ChatMute != null)
            {
                bool isMuted = (bool)ChatMute.Call("IsMuted", player);
                if (isMuted) return false;
            }

            //	Getting Data
            Dictionary<string, object> playerData = GetPlayerFormatting(player);
            Dictionary<string, string> titles = playerData["Titles"] as Dictionary<string, string>;
            
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            ///		Chat Output	
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            playerData["FormattedOutput"] = playerData["Formatting"];
            playerData["FormattedOutput"] = playerData["FormattedOutput"].ToString().Replace("{Rank}", playerData["GroupRank"].ToString());
            playerData["FormattedOutput"] = playerData["FormattedOutput"].ToString().Replace("{TitleColor}", playerData["TitleColor"].ToString());
            playerData["FormattedOutput"] = playerData["FormattedOutput"].ToString().Replace("{NameColor}", playerData["NameColor"].ToString());
            playerData["FormattedOutput"] = playerData["FormattedOutput"].ToString().Replace("{TextColor}", playerData["TextColor"].ToString());
            playerData["FormattedOutput"] = playerData["FormattedOutput"].ToString().Replace("{Name}", "<color=" + playerData["NameColor"].ToString() + ">" + player.Name + "</color>");
            playerData["FormattedOutput"] = playerData["FormattedOutput"].ToString().Replace("{ID}", player.SteamId.ToString());
            playerData["FormattedOutput"] = playerData["FormattedOutput"].ToString().Replace("{Message}", "<color=" + playerData["TextColor"].ToString() + ">" + message + "</color>");
            playerData["FormattedOutput"] = playerData["FormattedOutput"].ToString().Replace("{Time}", DateTime.Now.ToString("h:mm tt"));

            string chatTitle = "";

            foreach (string title in titles.Keys)
            {
                chatTitle = chatTitle + $"<color={titles[title]}>{title}</color> ";
            }

            if(chatTitle.EndsWith(" ")) 
				chatTitle = chatTitle.Substring(0, chatTitle.Length - 1);

            playerData["FormattedOutput"] = playerData["FormattedOutput"].ToString().Replace("{Title}", chatTitle);


            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            ///		Console Output	
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            playerData["ConsoleOutput"] = playerData["ConsoleFormatting"];
            playerData["ConsoleOutput"] = playerData["ConsoleOutput"].ToString().Replace("{Rank}", playerData["GroupRank"].ToString());
            playerData["ConsoleOutput"] = playerData["ConsoleOutput"].ToString().Replace("{TitleColor}", playerData["TitleColor"].ToString());
            playerData["ConsoleOutput"] = playerData["ConsoleOutput"].ToString().Replace("{NameColor}", playerData["NameColor"].ToString());
            playerData["ConsoleOutput"] = playerData["ConsoleOutput"].ToString().Replace("{TextColor}", playerData["TextColor"].ToString());
            playerData["ConsoleOutput"] = playerData["ConsoleOutput"].ToString().Replace("{Name}", player.Name);
            playerData["ConsoleOutput"] = playerData["ConsoleOutput"].ToString().Replace("{ID}", player.SteamId.ToString());
            playerData["ConsoleOutput"] = playerData["ConsoleOutput"].ToString().Replace("{Message}", message);
            playerData["ConsoleOutput"] = playerData["ConsoleOutput"].ToString().Replace("{Time}", DateTime.Now.ToString("h:mm tt"));

            string consoleTitle = "";

            foreach (string title in titles.Keys)
            {
                consoleTitle = consoleTitle + $"{title} ";
            }
			
			if(consoleTitle.EndsWith(" ")) 
				consoleTitle = consoleTitle.Substring(0, consoleTitle.Length - 1);

            playerData["ConsoleOutput"] = playerData["ConsoleOutput"].ToString().Replace("{Title}", consoleTitle);


            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            ///		Sending
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            BroadcastChat((string)playerData["FormattedOutput"]);
            Puts((string)playerData["ConsoleOutput"]);

            return false;
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
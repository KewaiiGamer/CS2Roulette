﻿using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Core.Translations;
using CounterStrikeSharp.API.Modules.Menu;
using StoreApi;
using System.Text;
using CS2Roulette.Configs;

namespace Store_Roulette_CS2
{
    [MinimumApiVersion(228)]
    public class CS2Roulette : BasePlugin, IPluginConfig<CS2RouletteConfig>
    {
        public override string ModuleName => "StoreRoulette";
        public override string ModuleVersion => "1.0.2";
        public override string ModuleAuthor => "Kewaii";
        public override string ModuleDescription => "Store roulette ported for CS2";
        private int tickInterval = 15;
        private int currentTick = 0;
        private int maxSteps = 15;
        private IStoreApi? storeApi;

	    private static Dictionary<ulong, int> PlayersBettingCredits = [];
	    private static Dictionary<ulong, string> PlayersBettingColor = [];
        private static Dictionary<ulong, int> PlayersSteps = [];
        private readonly List<string> _rouletteCommandAlias = new List<string>(){"roleta", "roulette"};

        public CS2RouletteConfig Config { get; set; } = new CS2RouletteConfig();

        public override void OnAllPluginsLoaded(bool hotReload)
        {
            storeApi = IStoreApi.Capability.Get();
        }
        public void OnConfigParsed(CS2RouletteConfig config)
        {
            this.Config = config;
        }

        public override void Load(bool hotReload)
        {
            foreach(var alias in this._rouletteCommandAlias)
            {
                this.AddCommand(alias.StartsWith($"css_") ? alias : $"css_{alias}", "Roulette menu command", OnRoulette);
            }
            
            RegisterListener<Listeners.OnTick>(OnTick);
        }


        public void OnRoulette(CCSPlayerController? player, CommandInfo command)
        {            

            CenterHtmlMenu menu = new(Localizer["ChooseType"], this)
            {
            };

            menu.AddMenuOption("Player", (player, option) =>
            {
                MenuManager.CloseActiveMenu(player);
                ShowRouletteQtyMenu(player, command, "player");
            });
            menu.AddMenuOption("VIP", (player, option) =>
            {
                MenuManager.CloseActiveMenu(player);
                ShowRouletteQtyMenu(player, command, "vip");
            }, !AdminManager.PlayerHasPermissions(player, "@css/vip"));
            if (player != null && player.IsValid)
            {
                MenuManager.OpenCenterHtmlMenu(this, player, menu);
            }
        }               

        public void ShowRouletteQtyMenu(CCSPlayerController player, CommandInfo command, string option)
        {
            CenterHtmlMenu menu = new(Localizer["ChooseCredits"], this)
            {
            };
            
            if (storeApi != null)
            {
                if (option.Equals("player")) 
                {
                    foreach (var credits in this.Config.PlayerCreditsOptions)
                    {
                        if (credits > 0)
                        {
                            StringBuilder builder = new();
                            
                            builder.AppendFormat(credits.ToString());
                            
                            menu.AddMenuOption(builder.ToString(), (player, newOption) =>
                            {
                                PlayersBettingCredits[player.SteamID] = credits;
                                ShowRouletteColorMenu(player, command, credits);
                            }, storeApi.GetPlayerCredits(player) < credits);
                        }
                    }
                }
                else if (option.Equals("vip")) 
                {
                    foreach (var credits in this.Config.VIPCreditsOptions)
                    {
                        
                        if (credits > 0)
                        {
                            Console.WriteLine(credits);
                            StringBuilder builder = new();
                            
                            builder.AppendFormat(credits.ToString());
                            
                            menu.AddMenuOption(builder.ToString(), (player, newOption) =>
                            {
                                PlayersBettingCredits[player.SteamID] = credits;
                                ShowRouletteColorMenu(player, command, credits);
                            }, storeApi.GetPlayerCredits(player) < credits);
                        }
                    }
                }
            }
            MenuManager.OpenCenterHtmlMenu(this, player, menu);


        }
        public void ShowRouletteColorMenu(CCSPlayerController player, CommandInfo command, int credits)
        {
            CenterHtmlMenu menu = new(Localizer["ChooseColor"], this)
            {
                PostSelectAction = PostSelectAction.Close
            };
            if (storeApi != null)
            {
                menu.AddMenuOption("Green", (player, newOption) =>
                {
                                
                    if (storeApi.GetPlayerCredits(player) >= credits) {
                        storeApi.GivePlayerCredits(player, -PlayersBettingCredits[player.SteamID]);
                        PlayersBettingColor[player.SteamID] = "green";    
                    } else {
                        player.PrintToCenterHtml(Localizer["NoBalance"]);
                    }
                });
                menu.AddMenuOption("Red", (player, newOption) =>
                {
                    if (storeApi.GetPlayerCredits(player) >= credits) {
                        storeApi.GivePlayerCredits(player, -PlayersBettingCredits[player.SteamID]);
                        PlayersBettingColor[player.SteamID] = "red";    
                    } else {
                        player.PrintToCenterHtml(Localizer["NoBalance"]);
                    }
                });
                
                menu.AddMenuOption("Blue", (player, newOption) =>
                {
                    if (storeApi.GetPlayerCredits(player) >= credits) {
                        storeApi.GivePlayerCredits(player, -PlayersBettingCredits[player.SteamID]);
                        PlayersBettingColor[player.SteamID] = "black";    
                    } else {
                        player.PrintToCenterHtml(Localizer["NoBalance"]);
                    }
                });
                MenuManager.OpenCenterHtmlMenu(this, player, menu);
            }


        }
        public void OnTick()
        {
                
            if (currentTick == tickInterval)
            {
                currentTick = 0;
                foreach (CCSPlayerController player in Utilities.GetPlayers())
                {
                    if (player != null && player.IsValid)
                    {
                        if (!PlayersSteps.TryGetValue(player.SteamID, out var step)) {
                            PlayersSteps[player.SteamID] = 0;
                        }                                                
                        if (PlayersBettingCredits.TryGetValue(player.SteamID, out var credits) && PlayersBettingColor.TryGetValue(player.SteamID, out var color))
                        {
                            PlayersSteps[player.SteamID]++;
                            Random rnd = new Random();
                            int number = rnd.Next(0, 37);
                            string gifUrl = "";
                            int multiplier = 0;
                            if (this.Config.RedNumbers.Contains(number)) {
                                gifUrl = this.Config.RedImage;
                                if (color.Equals("red"))
                                {
                                    multiplier = 2;
                                }
                            } else if (this.Config.BlackNumbers.Contains(number)) {
                                gifUrl = this.Config.BlackImage;
                                if (color.Equals("black"))
                                {
                                    multiplier = 2;
                                }
                            } else if (this.Config.GreenNumbers.Contains(number)) {
                                gifUrl = this.Config.GreenImage;
                                if (color.Equals("green"))
                                {
                                    multiplier = 10;
                                }
                            }

                            if (PlayersSteps[player.SteamID] == maxSteps) {

                                int creditsToGive = credits * multiplier;
                                StringBuilder builder = new();
                                if (multiplier > 0)
                                {                                
                                    builder.AppendFormat($"<img src=\"{gifUrl}\"></img><br><font color='green'>{Localizer["WonPrefix"]} {creditsToGive.ToString()} {Localizer["Credits"]}</font>");
                                    player.PrintToCenterHtml(builder.ToString());
                                    if (storeApi != null) {
                                        storeApi.GivePlayerCredits(player, creditsToGive);
                                    }
                                } else {
                                    builder.AppendFormat($"<img src=\"{gifUrl}\"></img><br><font color='green'>{Localizer["LostPrefix"]} {credits.ToString()} {Localizer["Credits"]}</font>");
                                    player.PrintToCenterHtml(builder.ToString());
                                }
                                PlayersBettingCredits.Remove(player.SteamID);
                                PlayersBettingColor.Remove(player.SteamID);                            
                                PlayersSteps[player.SteamID] = 0;
                            } else {
                                
                                player.PrintToCenterHtml($"<img src=\"{gifUrl}\">");
                            }
                        }
                        
                    }
                }       
            }
            currentTick++;
        }


        private string PluginInfo()
        {
            return $"Plugin: {this.ModuleName} - Version: {this.ModuleVersion} by {this.ModuleAuthor}";
        }
    }
}
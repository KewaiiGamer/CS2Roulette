using CounterStrikeSharp.API.Core;
using System;

namespace CS2Roulette.Configs
{
    public class CS2RouletteConfig : BasePluginConfig
    {

        public List<int> PlayerCreditsOptions { get; set; } = new List<int>() {50, 100, 250, 500};
        public List<int> VIPCreditsOptions { get; set; } = new List<int>() {1000, 2500, 5000, 10000};
        
        public string RedImage { get; set; } = "https://kewaii.cloud/cs2/img/red.png";
        public string GreenImage { get; set; } = "https://kewaii.cloud/cs2/img/green.png";
        public string BlackImage { get; set; } = "https://kewaii.cloud/cs2/img/black.png";
        public List<int> RedNumbers { get; set; } = new List<int>() {1, 3, 5, 7, 9, 12, 14, 16, 18, 19, 21 , 23, 25, 27, 30, 32, 34, 36};
        public List<int> BlackNumbers { get; set; } = new List<int>() {2, 4, 6, 8, 10, 11, 13, 15, 17, 20, 22, 24, 26, 28, 29, 31, 33, 35};
        public List<int> GreenNumbers { get; set; } = new List<int>() {0};

        public CS2RouletteConfig() {
            this.Version = 5;
            return;
        }
    }
}
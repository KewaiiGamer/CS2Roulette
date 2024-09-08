using CounterStrikeSharp.API.Core;
using System;
using System.Text;
using System.Text.Json.Serialization;

namespace CS2Roulette.Configs
{
    public class CS2RouletteConfig : BasePluginConfig
    {
        [JsonPropertyName("roulette_command_aliases")]
        public List<string> RouletteCommandAliases { get; set; } = new List<string>() {"roleta", "roulette"};

        [JsonPropertyName("player_credits_options")]
        public List<int> PlayerCreditsOptions { get; set; } = new List<int>() {50, 100, 250, 500};

        [JsonPropertyName("vip_credits_options")]
        public List<int> VIPCreditsOptions { get; set; } = new List<int>() {1000, 2500, 5000, 10000};
        
        [JsonPropertyName("red_image_url")]
        public string RedImage { get; set; } = "https://kewaii.cloud/cs2/img/red.png";

        [JsonPropertyName("green_image_url")]
        public string GreenImage { get; set; } = "https://kewaii.cloud/cs2/img/green.png";

        [JsonPropertyName("blue_image_url")]
        public string BlueImage { get; set; } = "https://kewaii.cloud/cs2/img/blue.png";

        [JsonPropertyName("red_numbers")]
        public List<int> RedNumbers { get; set; } = new List<int>() {1, 3, 5, 7, 9, 12, 14, 16, 18, 19, 21 , 23, 25, 27, 30, 32, 34, 36};

        [JsonPropertyName("blue_numbers")]
        public List<int> BlueNumbers { get; set; } = new List<int>() {2, 4, 6, 8, 10, 11, 13, 15, 17, 20, 22, 24, 26, 28, 29, 31, 33, 35};

        [JsonPropertyName("green_numbers")]
        public List<int> GreenNumbers { get; set; } = new List<int>() {0};

        [JsonPropertyName("random_number_start")]
        public int RandomNumberStart { get; set; } = 0;

        [JsonPropertyName("random_number_end")]
        public int RandomNumberEnd { get; set; } = 36;

        [JsonPropertyName("red_multiplier")]
        public int RedMultiplier { get; set; } = 2;

        [JsonPropertyName("blue_multiplier")]
        public int BlueMultiplier { get; set; } = 2;

        [JsonPropertyName("green_multiplier")]
        public int GreenMultiplier { get; set; } = 10;

    }
}
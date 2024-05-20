using System;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;
using System.IO;
using Newtonsoft.Json;


namespace CompStats
{
    public class CompStatsSubModule : MBSubModuleBase
    {
        public static CompStatsSubModule Instance {  get; private set; }

        private Config config;

        private void setup()
        {
            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            string configPath = Path.Combine(basePath, "config.json");
            if (!File.Exists(configPath))
            {
                config = new Config();
                ConfigManager.SetConfig(config);
                string json = JsonConvert.SerializeObject(config);
                File.WriteAllText(configPath, json);
            }
            else
            {
                string configString = File.ReadAllText(configPath);
                config = JsonConvert.DeserializeObject<Config>(configString);
                ConfigManager.SetConfig(config);
            }
        }

        protected override void OnSubModuleLoad()
        {
            base.OnSubModuleLoad();
            this.setup();

            Debug.Print("COMP STATS LOADED", 0, Debug.DebugColor.Green);
        }

        protected override void OnSubModuleUnloaded()
        {
            base.OnSubModuleUnloaded();
            Debug.Print("COMP STATS UNLOADED", 0, Debug.DebugColor.Green);
        }

        public static bool firstLoad = true;

        public override void OnMissionBehaviorInitialize(Mission mission)
        {
            base.OnMissionBehaviorInitialize(mission);

            if (config.enabled)
            {
                if (firstLoad)
                {
                    Debug.Print("CAPTURING PLAYER STATS", 0, Debug.DebugColor.Red);
                    firstLoad = false;
                }

                mission.AddMissionBehavior(new PlayerStatsScoreboard(config));
            }
        }
    }
}

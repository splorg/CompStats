using System.Collections.Generic;
using System.Threading.Tasks;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;
using static TaleWorlds.MountAndBlade.MultiplayerOptions;

namespace CompStats
{
    public class PlayerStatsScoreboard : MissionBehavior
    {
        private StatsService service;
        public PlayerStatsScoreboard(Config config)
        {
            this.service = new StatsService(config);
        }

        public static PlayerStatsScoreboard Current { get; private set; }
        public override MissionBehaviorType BehaviorType => MissionBehaviorType.Other;

        protected override async void OnEndMission()
        {
            string matchId = await createMatch();
            // this.createOrUpdatePlayers(matchId);
            base.OnEndMission();
        }

        public void createOrUpdatePlayers(string mapId)
        {
            if (GameNetwork.NetworkPeerCount > 0)
            {
                List<Player> players = new List<Player>();

                foreach (NetworkCommunicator peer in GameNetwork.NetworkPeersIncludingDisconnectedPeers)
                {
                    if (peer.ControlledAgent != null && peer.ControlledAgent.MissionPeer != null && peer.VirtualPlayer != null)
                    {
                        if (
                            peer.ControlledAgent.MissionPeer.Team.Side == BattleSideEnum.Attacker || 
                            peer.ControlledAgent.MissionPeer.Team.Side == BattleSideEnum.Defender
                        )
                        {
                            Player player = new Player(peer.UserName, peer.VirtualPlayer.Id.ToString());
                            Stats playerStats = new Stats(
                                peer.ControlledAgent.MissionPeer.KillCount,
                                peer.ControlledAgent.MissionPeer.DeathCount,
                                peer.ControlledAgent.MissionPeer.AssistCount,
                                peer.ControlledAgent.MissionPeer.Score
                            );

                            if (Mission.Current.GetMissionBehavior<MissionMultiplayerGameModeBase>().GetWinnerTeam().Side == peer.ControlledAgent.MissionPeer.Team.Side)
                            {
                                playerStats.SetWinner();
                            }

                            player.SetStats(playerStats);

                            players.Add(player);
                        }
                    }
                }

                //this.service.createOrUpdatePlayers(players, mapId);
            }
        }

        public async Task<string> createMatch()
        {
            string mapId = await service.addMatch(
                Mission.Current.SceneName,
                GetOptionString(OptionType.CultureTeam1),
                GetOptionString(OptionType.CultureTeam2),
                GetOptionString(OptionType.GameType)
            );
            return mapId;
        }

        string GetOptionString(OptionType optionType)
        {
            string toReturn;
            Instance.GetOptionFromOptionType(optionType).GetValue(out toReturn);
            return toReturn;
        }
    }
}

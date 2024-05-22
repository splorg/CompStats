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
        private Config config;
        public PlayerStatsScoreboard(Config config)
        {
            this.config = config;
            this.service = new StatsService(config);
        }

        public static PlayerStatsScoreboard Current { get; private set; }
        public override MissionBehaviorType BehaviorType => MissionBehaviorType.Other;

        protected override async void OnEndMission()
        {
            base.OnEndMission();

            string matchId = await PostMatchInfo();

            if (matchId != null)
            {
                PostPlayerStats(matchId);
            }
        }

        public void PostPlayerStats(string mapId)
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

                service.PostPlayers(players, mapId);
            }
        }

        public async Task<string> PostMatchInfo()
        {
            Match match = new Match(
                Mission.Current.SceneName,
                GetOptionString(OptionType.CultureTeam1),
                GetOptionString(OptionType.CultureTeam2),
                GetOptionString(OptionType.GameType)
            );
            match.tournamentName = this.config.tournamentName;

            string matchId = await service.PostMatch(match);
            return matchId;
        }

        string GetOptionString(OptionType optionType)
        {
            string toReturn;
            Instance.GetOptionFromOptionType(optionType).GetValue(out toReturn);
            return toReturn;
        }
    }
}

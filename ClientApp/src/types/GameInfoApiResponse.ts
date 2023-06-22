export interface IParticipant {
  puuid: string;
  summonerName: string;
  championName: string;
  championIconUrl: string;
  win: boolean;
  gameEndedInEarlySurrender: boolean;
  gameEndedInSurrender: boolean;
  kills: number;
  deaths: number;
  assists: number;
  kda: number;
  totalDamageDealtToChampions: number;
  summonerId: string;
  championId: number;
  champLevel: number;
  teamId: number;
  baronKills: number;
  dragonKills: number;
  totalDamageShieldedOnTeammates: number;
  totalDamageTaken: number;
  totalHeal: number;
  totalHealsOnTeammates: number;
  damageSelfMitigated: number;
  firstBloodKill: boolean;
  firstTowerKill: boolean;
  firstTowerAssist: boolean;
  totalMinionsKilled: number;
  neutralMinionsKilled: number;
  visionScore: number;
  timeCCingOthers: number;
  pentaKills: number;
  killingSprees: number;
  detectorWardsPlaced: number;
  item0: number;
  item1: number;
  item2: number;
  item3: number;
  item4: number;
  item5: number;
  item6: number;
}

export enum QueueType {
  Custom = 0,
  NormalDraft = 400,
  RankedSoloDuo = 420,
  NormalBlind = 430,
  RankedFlex = 440,
  ARAM = 450,
  Clash = 700,
  BotGame1 = 830,
  BotGame2 = 840,
  BotGame3 = 850,
  ARURF1 = 900,
  ARURF2 = 1010,
  OneForAll = 1020,
  NexusBlitz1 = 1200,
  NexusBlitz2 = 1300,
  UltimateSpellBook = 1400,
  PickUrf = 1900,
}

export interface IGameInfo {
  queueId: QueueType;
  gameId: number;
  gameStartTime: string;
  gameDurationInSeconds: number;
  participants: Array<IParticipant>;
}

export interface IRegisteredSummoner {
  puuid: string;
  name: string;
}

export interface IGameInfoApiResponse {
  gameInfo: IGameInfo;
  registeredSummoners: Array<IRegisteredSummoner>;
}

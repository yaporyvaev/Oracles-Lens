export interface IParticipant {
  avatar: string;
  username: string;
  kda: string;
  kdaIndex: number;
  damage: string;
  damagePercentage: number;
}

export interface IGameStat {
  title: string;
  duration: string;
  participants: Array<IParticipant>;
}

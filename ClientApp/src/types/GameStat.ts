export interface IParticipant {
  avatar: string;
  username: string;
  kda: string;
  kdaIndex: number;
  damage: string;
  damagePercentage: number;
  puuid: string;
  champLevel: string;
  damageTaken: string;
  damageTakenPercentage: number;
  totalHeal: string;
  totalHealPercentage: number;
  kp: string;
  cs: string;
  vs: string;
  items: Array<number>;
  fb: boolean;
  penta: boolean;
}

export interface IGameStat {
  title: string;
  duration: string;
  participants: Array<IParticipant>;
}

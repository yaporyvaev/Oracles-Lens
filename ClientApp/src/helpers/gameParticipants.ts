import { AVATAR_URL } from "../constants";
import { IParticipant } from "../types/GameInfoApiResponse";

export const getParticipants = (participants: Array<IParticipant>) => {
  const mostDamage = participants.sort(
    (a, b) => b.totalDamageDealtToChampions - a.totalDamageDealtToChampions
  )[0].totalDamageDealtToChampions;

  return participants.map((participant) => {
    return {
      avatar: `${AVATAR_URL}${participant.championName}.png`,
      username: participant.summonerName,
      kda: `${participant.kills} / ${participant.deaths} / ${participant.assists}`,
      kdaIndex: Math.round(participant.kda * 10) / 10,
      damage: `${convertDmg(participant.totalDamageDealtToChampions)} dmg`,
      damagePercentage: getDamagePercentage(
        participant.totalDamageDealtToChampions,
        mostDamage
      ),
    };
  });
};

const getDamagePercentage = (participantDamage: number, mostDamage: number) => {
  return Math.round((participantDamage / mostDamage) * 100);
};

const convertDmg = (dmg: number): string => {
  const str = dmg.toString();
  const end = str.slice(-3, str.length);
  const left = str.length - 3;
  const start = str.slice(0, left);
  return `${start} ${end}`;
};

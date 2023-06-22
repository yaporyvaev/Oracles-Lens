import { IParticipant } from "../types/GameInfoApiResponse";

import { IParticipant as IResultParticipant } from "../types/GameStat";

export const getParticipants = (
  participants: Array<IParticipant>,
  team: Array<IParticipant>
): Array<IResultParticipant> => {
  return participants.map((participant) => {
    return {
      avatar: participant.championIconUrl,
      username: participant.summonerName,
      kda: `${participant.kills} / ${participant.deaths} / ${participant.assists}`,
      kdaIndex: Math.round(participant.kda * 10) / 10,
      damage: `${convertNumber(participant.totalDamageDealtToChampions)}`,
      damagePercentage: getPercentage(
        participant.totalDamageDealtToChampions,
        getMostDamage(team)
      ),
      puuid: participant.puuid,
      champLevel: `${participant.champLevel} LVL`,
      damageTaken: `${convertNumber(
        participant.totalDamageTaken + participant.damageSelfMitigated
      )}`,
      damageTakenPercentage: getPercentage(
        participant.totalDamageTaken + participant.damageSelfMitigated,
        getMostDamageTaken(team)
      ),
      totalHeal: `${convertNumber(participant.totalHeal)}`,
      totalHealPercentage: getPercentage(
        participant.totalHeal,
        getMostHeal(team)
      ),
      items: [
        participant.item0,
        participant.item1,
        participant.item2,
        participant.item3,
        participant.item4,
        participant.item5,
        participant.item6,
      ],
      fb: participant.firstBloodKill,
      cs: `${participant.totalMinionsKilled} CS`,
      vs: `${participant.visionScore} VS`,
      kp: `${getKP(team, participant.kills, participant.assists)}% KP`,
      penta: participant.pentaKills > 0,
    };
  });
};

const getMostHeal = (team: Array<IParticipant>) => {
  return team.sort((a, b) => b.totalHeal - a.totalHeal)[0].totalHeal;
};

const getMostDamage = (team: Array<IParticipant>) => {
  return team.sort(
    (a, b) => b.totalDamageDealtToChampions - a.totalDamageDealtToChampions
  )[0].totalDamageDealtToChampions;
};

const getMostDamageTaken = (team: Array<IParticipant>) => {
  const teamMate = team.sort(
    (a, b) =>
      b.totalDamageTaken +
      b.damageSelfMitigated -
      (a.totalDamageTaken + a.damageSelfMitigated)
  )[0];
  return teamMate.totalDamageTaken + teamMate.damageSelfMitigated;
};

const getPercentage = (value: number, mostValue: number) => {
  return Math.round((value / mostValue) * 100);
};

const getKP = (team: Array<IParticipant>, kills: number, assists: number) => {
  const teamKills = team
    .map((item) => item.kills)
    .reduce((prev, next) => prev + next);
  console.log(teamKills);
  return Math.round(((kills + assists) / teamKills) * 100);
};

const convertNumber = (numb: number): string => {
  const str = numb.toString();
  const end = str.slice(-3, str.length);
  const left = str.length - 3;
  const start = str.slice(0, left);
  return `${start} ${end}`;
};

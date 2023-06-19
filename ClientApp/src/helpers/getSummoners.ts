import {
  IParticipant,
  IRegisteredSummoner,
} from "../types/GameInfoApiResponse";

export const getSummoners = (
  participants: Array<IParticipant>,
  registeredSummoners: Array<IRegisteredSummoner>
): Array<IParticipant> => {
  const result: Array<IParticipant> = [];
  registeredSummoners.forEach((registeredSummoner) => {
    const teamMate = participants.find(
      (participant) => participant.puuid === registeredSummoner.puuid
    );
    if (teamMate) {
      result.push(teamMate);
    }
  });
  return result;
};

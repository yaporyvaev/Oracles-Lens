import { createEffect, createStore, restore } from "effector";
import { IGameInfoApiResponse } from "../types/GameInfoApiResponse";
import { getGameInfo } from "../api/gameInfo";
import { getQueueType } from "../helpers/queueTypeConstants";
import { getGameStatus } from "../helpers/gameStatus";
import { getParticipants } from "../helpers/gameParticipants";
import { IGameStat } from "../types/GameStat";
import { getSummoners } from "../helpers/getSummoners";
import { converDuration } from "../helpers/gameDurationConverter";

export const $gameInfo = createStore<IGameStat>({} as IGameStat);

export const getGameInfoFx = createEffect<
  string,
  IGameInfoApiResponse,
  Error
>();
getGameInfoFx.use(getGameInfo);
export const $getGameInfoError = restore<Error>(getGameInfoFx.failData, null);

const prepareInfo = (state: IGameInfoApiResponse) => {
  const gameInfo = state.gameInfo;
  const registeredSummoners = state.registeredSummoners;
  const summoners = getSummoners(gameInfo.participants, registeredSummoners);
  return {
    title: `${getQueueType(state.gameInfo.queueId)} | ${getGameStatus(
      summoners[0].win,
      summoners[0].gameEndedInEarlySurrender,
      summoners[0].gameEndedInSurrender
    )}`,
    duration: converDuration(gameInfo.gameDurationInSeconds),
    participants: getParticipants(summoners).sort(
      (a, b) => b.kdaIndex - a.kdaIndex
    ),
  };
};

$gameInfo.on(getGameInfoFx.doneData, (_, data) => prepareInfo(data));

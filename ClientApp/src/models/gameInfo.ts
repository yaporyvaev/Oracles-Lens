import { createEffect, createEvent, createStore, restore } from "effector";
import { IGameInfoApiResponse } from "../types/GameInfoApiResponse";
import { getGameInfo } from "../api/gameInfo";
import { getQueueType } from "../helpers/queueTypeConstants";
import { getGameStatus } from "../helpers/gameStatus";
import { getParticipants } from "../helpers/gameParticipants";
import { IGameStat } from "../types/GameStat";
import { getSummoners } from "../helpers/getSummoners";
import { converDuration } from "../helpers/gameDurationConverter";

export const $gameInfo = createStore<IGameStat>({} as IGameStat);

export const gameInfoLoadTriggered = createEvent<string>();
export const appStarted = createEvent();

export const getGameInfoFx = createEffect<
  string,
  IGameInfoApiResponse,
  Error
>();
getGameInfoFx.use(getGameInfo);
export const $getGameInfoError = restore<Error>(getGameInfoFx.failData, null);

const initialApp = () => {
  const newWindow = window as any;
  const tg = newWindow.Telegram.WebApp;
  const gameId = tg.initDataUnsafe.start_param;

  gameInfoLoadTriggered(gameId);
};

const prepareInfo = (state: IGameInfoApiResponse): IGameStat => {
  const gameInfo = state.gameInfo;
  const registeredSummoners = state.registeredSummoners;
  const summoners = getSummoners(gameInfo.participants, registeredSummoners);
  const team = gameInfo.participants.filter((participant) => {
    return participant.teamId === summoners[0].teamId;
  });
  return {
    gameId: `${gameInfo.gameId}`,
    title: `${getQueueType(state.gameInfo.queueId)} | ${getGameStatus(
      summoners[0].win,
      summoners[0].gameEndedInEarlySurrender,
      summoners[0].gameEndedInSurrender
    )}`,
    duration: converDuration(gameInfo.gameDurationInSeconds),
    participants: getParticipants(summoners, team).sort(
      (a, b) => b.kdaIndex - a.kdaIndex
    ),
  };
};

$gameInfo
  .on(gameInfoLoadTriggered, (_, gameId) => {
    const savedGame = sessionStorage.getItem(gameId);
    if (savedGame) {
      return JSON.parse(savedGame);
    } else {
      getGameInfoFx(gameId);
    }
  })
  .on(getGameInfoFx.doneData, (_, data) => {
    const result = prepareInfo(data);
    sessionStorage.setItem(result.gameId, JSON.stringify(result));
    return result;
  });

appStarted.watch(initialApp);

export const getGameStatus = (
  win: boolean,
  gameEndedInEarlySurrender: boolean,
  gameEndedInSurrender: boolean
): string => {
  return win
    ? "Victory!"
    : gameEndedInEarlySurrender
    ? "FFed 15"
    : gameEndedInSurrender
    ? "FFed"
    : "Defeat";
};

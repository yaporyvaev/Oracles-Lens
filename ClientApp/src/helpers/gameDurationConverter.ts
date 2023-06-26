export const converDuration = (sec: number): string => {
  const minutes = Math.floor(sec / 60);
  const seconds = sec - minutes * 60;
  const formattedMinutes = minutes < 10 ? `0${minutes}` : minutes;
  const formattedSeconds = seconds < 10 ? `0${seconds}` : seconds;
  return `${formattedMinutes}:${formattedSeconds}`;
};

export const converDuration = (sec: number): string => {
  const minutes = Math.floor(sec / 60);
  const seconds = sec - minutes * 60;
  return `${minutes}:${seconds}`;
};

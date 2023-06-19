import { axios } from "./axios";
import { IGameInfoApiResponse } from "../types/GameInfoApiResponse";

export const getGameInfo = async (
  gameId: string
): Promise<IGameInfoApiResponse> => {
  const res = await axios({
    url: `/api/games/${gameId}/info`,
    method: "GET",
    withCredentials: false,
  });
  return res.data;
};

import { useStore } from "effector-react";
import { ResultTable } from "../ResultTable/ResultTable";
import { $gameInfo } from "../../models/gameInfo";
import { useEffect } from "react";
import { useNavigate } from "react-router-dom";
import { backButtonSetted } from "../../models/backButton";

export const Main = () => {
  const navigate = useNavigate();
  const gameInfo = useStore($gameInfo);

  useEffect(() => {
    if (
      gameInfo &&
      gameInfo.participants &&
      gameInfo.participants.length === 1
    ) {
      const participantId = gameInfo.participants[0].puuid;
      navigate(`/match/participant/${participantId}`);
    }
  }, [gameInfo]);

  useEffect(() => {
    backButtonSetted(false);
  }, []);

  return (
    <main>
      <ResultTable participants={gameInfo.participants}></ResultTable>
    </main>
  );
};

import { useEffect } from "react";
import { useStore } from "effector-react";
import "./App.scss";
import { Header } from "./components/Header/Header";
import { Main } from "./components/Main/Main";
import { $gameInfo, $getGameInfoError, getGameInfoFx } from "./models/gameInfo";

function App() {
  const newWindow = window as any;
  const tg = newWindow.Telegram.WebApp;
  const gameId = tg.initDataUnsafe.start_param;

  const gameInfo = useStore($gameInfo);
  const getGameInfoError = useStore($getGameInfoError);
  useEffect(() => {
    getGameInfoFx(gameId);
  }, []);

  return (
    <>
      {getGameInfoError ? (
        <div>{getGameInfoError.message}</div>
      ) : (
        gameInfo &&
        gameId && (
          <>
            <Header
              title={gameInfo.title}
              duration={gameInfo.duration}
            ></Header>
            <Main participants={gameInfo.participants}></Main>
          </>
        )
      )}
    </>
  );
}

export default App;

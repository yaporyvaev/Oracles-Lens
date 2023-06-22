import { useStore } from "effector-react";
import "./App.scss";
import { Header } from "./components/Header/Header";
import { $gameInfo, $getGameInfoError, getGameInfoFx } from "./models/gameInfo";
import { Outlet } from "react-router-dom";
import { Loader } from "./components/Loader/Loader";
import { ErrorPage } from "./components/ErrorPage/ErrorPage";

function App() {
  const gameInfo = useStore($gameInfo);
  const getGameInfoError = useStore($getGameInfoError);
  const isPending = useStore(getGameInfoFx.pending);

  return (
    <>
      {isPending ? (
        <Loader />
      ) : getGameInfoError ? (
        <ErrorPage></ErrorPage>
      ) : (
        gameInfo && (
          <>
            <Header
              title={gameInfo.title}
              duration={gameInfo.duration}
            ></Header>
            <Outlet></Outlet>
          </>
        )
      )}
    </>
  );
}

export default App;

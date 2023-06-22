import React from "react";
import ReactDOM from "react-dom/client";
import App from "./App.tsx";
import { RouterProvider, createBrowserRouter } from "react-router-dom";
import { ErrorPage } from "./components/ErrorPage/ErrorPage.tsx";
import { ParticipantPage } from "./components/ParticipantPage/ParticipantPage.tsx";
import { Index } from "./components/Index/Index.tsx";
import { Main } from "./components/Main/Main.tsx";
import { $gameInfo, getGameInfoFx } from "./models/gameInfo.ts";

export async function matchLoader() {
  const newWindow = window as any;
  const tg = newWindow.Telegram.WebApp;
  const gameId = tg.initDataUnsafe.start_param;

  getGameInfoFx(gameId);
  return null;
}

export async function participantLoader({ params }: any) {
  const participant = $gameInfo
    .getState()
    .participants.find((item) => item.puuid === params.id);
  return { participant };
}

const router = createBrowserRouter([
  {
    path: "/",
    element: <Index />,
  },
  {
    path: "/match",
    element: <App />,
    errorElement: <ErrorPage />,
    loader: matchLoader,
    children: [
      {
        path: "participants/",
        element: <Main />,
      },
      {
        path: "participant/:id",
        element: <ParticipantPage />,
        loader: participantLoader,
      },
    ],
  },
]);

ReactDOM.createRoot(document.getElementById("root") as HTMLElement).render(
  <React.StrictMode>
    <RouterProvider router={router} />
  </React.StrictMode>
);

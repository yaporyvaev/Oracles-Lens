import "./ErrorPage.scss";

export const ErrorPage = () => {
  const img =
    "https://static.wikia.nocookie.net/leagueoflegends/images/d/d5/LoL_Facebook_Icon_16.gif";

  return (
    <div className="error-page">
      <h1>Oops!</h1>
      <p>Sorry, an unexpected error has occurred.</p>
      <p>
        <img src={img}></img>
      </p>
    </div>
  );
};

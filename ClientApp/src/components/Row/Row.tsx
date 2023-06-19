import "./Row.scss";

interface IProps {
  image: string;
  username: string;
  kdaStats: string;
  kdaTotal: string;
  damage: string;
}

export const Row = (props: IProps) => {
  return (
    <div className="row">
      <img src={props.image} className="row__image"></img>
      <div className="row__username">{props.username}</div>
      <div className="row__kda">
        <div className="row__kda-stats">{props.kdaStats}</div>
        <div className="row__kda-total">{props.kdaTotal}</div>
      </div>
      <div className="row__damage">{props.damage}</div>
    </div>
  );
};

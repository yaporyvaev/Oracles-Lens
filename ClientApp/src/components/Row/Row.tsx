import { IParticipant } from "../../types/GameStat";
import "./Row.scss";

export const Row = (props: IParticipant) => {
  return (
    <div className="row">
      <img src={props.avatar} className="row__image"></img>
      <div className="row__username">{props.username}</div>
      <div className="row__kda">
        <div className="row__kda-stats">{props.kda}</div>
        <div className="row__kda-total">{props.kdaIndex} KDA</div>
      </div>
      <div className="row__damage">
        <div>{props.damage}</div>
        <div
          className="row__progress-container"
          style={
            { "--value": `${props.damagePercentage}%` } as React.CSSProperties
          }
        >
          <progress
            value={props.damagePercentage}
            max="100"
            className="progress"
          >
            {props.damagePercentage}
          </progress>
        </div>
      </div>
    </div>
  );
};

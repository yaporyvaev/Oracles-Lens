import { useNavigate } from "react-router-dom";
import { IParticipant } from "../../types/GameStat";
import { useStore } from "effector-react";
import { $gameInfo } from "../../models/gameInfo";
import "./Row.scss";
import { Progress } from "../Progress/Progress";

export const Row = (participant: IParticipant) => {
  const navigate = useNavigate();
  const participantsCount = useStore($gameInfo).participants.length;
  const isDuoTrioGame = participantsCount > 1 && participantsCount < 4;

  const onRowClick = () => {
    navigate(`/match/participant/${participant.puuid}`);
  };
  return (
    <div
      className={`row__wrapper ${isDuoTrioGame ? "row__wrapper--duotrio" : ""}`}
      onClick={() => {
        onRowClick();
      }}
    >
      <div className="row">
        <img src={participant.avatar} className="row__image"></img>
        <div className="row__username">{participant.username}</div>
        <div className="row__kda">
          <div className="row__kda-stats">{participant.kda}</div>
          <div className="row__kda-total">{participant.kdaIndex} KDA</div>
        </div>

        {isDuoTrioGame ? (
          <div className="row__kp">{participant.kp}</div>
        ) : (
          <Progress
            value={`${participant.damage} DMG`}
            percentage={participant.damagePercentage}
            style="row__damage"
          ></Progress>
        )}
      </div>
      {isDuoTrioGame && (
        <div>
          <Progress
            value={participant.damage}
            percentage={participant.damagePercentage}
            style="row__damage--duotrio"
            description="Damage dealt"
          ></Progress>
          <Progress
            value={participant.damageTaken}
            percentage={participant.damageTakenPercentage}
            style="row__damage--duotrio"
            description="Damage taken"
          ></Progress>
          <Progress
            value={participant.totalHeal}
            percentage={participant.totalHealPercentage}
            style="row__damage--duotrio"
            description="Damage healed"
          ></Progress>
        </div>
      )}
    </div>
  );
};

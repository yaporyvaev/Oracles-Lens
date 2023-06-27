import { useLoaderData } from "react-router-dom";
import { IParticipant } from "../../types/GameStat";
import "./ParticipantPage.scss";
import { useEffect } from "react";
import { backButtonSetted } from "../../models/backButton";
import { $gameInfo } from "../../models/gameInfo";
import { useStore } from "effector-react";
import { Progress } from "../Progress/Progress";

interface ILoaderData {
  participant: IParticipant;
}

export const ParticipantPage = () => {
  const { participant } = useLoaderData() as ILoaderData;
  const isNotSoloGame = useStore($gameInfo).participants.length > 1;

  useEffect(() => {
    if (isNotSoloGame) {
      backButtonSetted(true);
    }
  }, []);

  return (
    <div className="participant">
      <section className="participant__main">
        <img src={participant.avatar} className="participant__image"></img>
        <div className="participant__main-stat">
          <div className="participant__username">{participant.username}</div>
          {(participant.fb || participant.penta) && (
            <section className="participant__achievement">
              {participant.fb && (
                <div className="participant__achievement-fb">FIRST BLOOD</div>
              )}
              {participant.penta && <div>PENTA KILL</div>}
            </section>
          )}
        </div>
      </section>

      <div className="participant__kda">
        <div className="participant__kda-stats">{participant.kda}</div>
        <div className="participant__kda-total">{participant.kdaIndex} KDA</div>
        <div className="participant__kp">{participant.kp}</div>
        <div>{participant.cs}</div>
        <div>{participant.vs}</div>
      </div>
      <section className="participant__items">
        {participant.items.map((item, index) => {
          return item !== 0 ? (
            <img
              className="participant__item"
              key={index}
              src={`https://blitz-cdn.blitz.gg/blitz/lol/item/${item}.webp`}
            />
          ) : (
            <div className="participant__item--empty" key={index}></div>
          );
        })}
      </section>
      <Progress
        value={participant.damage}
        percentage={participant.damagePercentage}
        style="participant__damage"
        description="Damage dealt"
      ></Progress>
      <Progress
        value={participant.damageTaken}
        percentage={participant.damageTakenPercentage}
        style="participant__damage"
        description="Damage taken"
      ></Progress>
      <Progress
        value={participant.totalHeal}
        percentage={participant.totalHealPercentage}
        style="participant__damage"
        description="Damage healed"
      ></Progress>
    </div>
  );
};

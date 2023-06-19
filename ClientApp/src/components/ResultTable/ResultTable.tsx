import { IParticipant } from "../../types/GameStat";
import { Row } from "../Row/Row";
import "./ResultTable.scss";

interface IProps {
  participants: Array<IParticipant>;
}

export const ResultTable = ({ participants }: IProps) => {
  return (
    <section className="result-table">
      {participants &&
        participants.length !== 0 &&
        participants.map((participant, index) => (
          <Row key={index} {...participant} />
        ))}
    </section>
  );
};

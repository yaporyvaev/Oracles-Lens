import { IParticipant } from "../../types/GameStat";
import { ResultTable } from "../ResultTable/ResultTable";

interface IProps {
  participants: Array<IParticipant>;
}

export const Main = ({ participants }: IProps) => {
  return (
    <main>
      <ResultTable participants={participants}></ResultTable>
    </main>
  );
};

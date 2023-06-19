import { Row } from "../Row/Row";
import "./ResultTable.scss";

export const ResultTable = () => {
  const mockResults = [
    {
      image: "https://blitz-cdn.blitz.gg/blitz/lol/champion/Seraphine.webp",
      username: "MOV BX 69",
      kdaStats: "10/0/20",
      kdaTotal: "KDA: 500",
      damage: "200000",
    },
    {
      image: "https://blitz-cdn.blitz.gg/blitz/lol/champion/MissFortune.webp",
      username: "KARTOSHKA",
      kdaStats: "10/0/20",
      kdaTotal: "KDA: 500",
      damage: "200000",
    },
    {
      image: "https://blitz-cdn.blitz.gg/blitz/lol/champion/MissFortune.webp",
      username: "KARTOSHKA",
      kdaStats: "10/0/20",
      kdaTotal: "KDA: 500",
      damage: "200000",
    },
    {
      image: "https://blitz-cdn.blitz.gg/blitz/lol/champion/MissFortune.webp",
      username: "KARTOSHKA",
      kdaStats: "10/0/20",
      kdaTotal: "KDA: 500",
      damage: "200000",
    },
    {
      image: "https://blitz-cdn.blitz.gg/blitz/lol/champion/MissFortune.webp",
      username: "KARTOSHKA",
      kdaStats: "10/0/20",
      kdaTotal: "KDA: 500",
      damage: "200000",
    },
  ];
  return (
    <section className="result-table">
      {mockResults.map((result, index) => (
        <Row key={index} {...result} />
      ))}
    </section>
  );
};

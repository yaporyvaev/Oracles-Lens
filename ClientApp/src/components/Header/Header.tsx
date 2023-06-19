import "./Header.scss";

interface IProps {
  title: string;
  duration: string;
}
export const Header = ({ title, duration }: IProps) => {
  return (
    <header className="header">
      <span>{title}</span>
      <span className="header__duration">{duration}</span>
    </header>
  );
};

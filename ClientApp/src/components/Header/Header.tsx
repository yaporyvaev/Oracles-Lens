import { useStore } from "effector-react";
import { $backButton } from "../../models/backButton";
import { useNavigate } from "react-router-dom";
import { ReactComponent as IconBack } from "../../assets/arrow-back.svg";
import "./Header.scss";

interface IProps {
  title: string;
  duration: string;
}

export const Header = ({ title, duration }: IProps) => {
  const isBackButtonVisible = useStore($backButton);
  const navigate = useNavigate();

  return (
    <header className="header">
      <div className="header__wrapper">
        {isBackButtonVisible && (
          <IconBack
            onClick={() => {
              navigate(-1);
            }}
            className="header__back"
          ></IconBack>
        )}
        <div>{title}</div>
      </div>
      <span className="header__duration">{duration}</span>
    </header>
  );
};

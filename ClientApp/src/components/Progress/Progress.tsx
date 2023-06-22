import "./Progress.scss";

interface IProps {
  value: string;
  percentage: number;
  style?: string;
  description?: string;
}

export const Progress = ({ value, percentage, description, style }: IProps) => {
  return (
    <div className={style}>
      <div className={`${style} stats`}>
        {description && <div>{description}</div>}
        <div className={`${style} value`}>{value}</div>
      </div>
      <div
        className={`${style} progress-container`}
        style={
          {
            "--value": `${percentage}%`,
          } as React.CSSProperties
        }
      >
        <progress value={percentage} max="100" className="progress">
          {percentage}
        </progress>
      </div>
    </div>
  );
};

import React from "react";
import "gantt-task-react/dist/index.css";
import { Button } from "antd";
import { ViewMode } from "gantt-task-react";

export const ViewSwitcher = ({
  onViewModeChange,
}) => {
  return (
    <div className="ViewContainer">
      <Button
        onClick={() => onViewModeChange(ViewMode.Hour)}
      >
        Hour
      </Button>
      <Button
        onClick={() => onViewModeChange(ViewMode.QuarterDay)}
      >
        Quarter of Day
      </Button>
      <Button
        onClick={() => onViewModeChange(ViewMode.HalfDay)}
      >
        Half of Day
      </Button>
      <Button onClick={() => onViewModeChange(ViewMode.Day)}>
        Day
      </Button>
      <Button
        onClick={() => onViewModeChange(ViewMode.Week)}
      >
        Week
      </Button>
      <Button
        onClick={() => onViewModeChange(ViewMode.Month)}
      >
        Month
      </Button>
      <Button
        onClick={() => onViewModeChange(ViewMode.Year)}
      >
        Year
      </Button>
      <Button
        onClick={() => onViewModeChange(ViewMode.QuarterYear)}
      >
        Year
      </Button>
    </div>
  );
};
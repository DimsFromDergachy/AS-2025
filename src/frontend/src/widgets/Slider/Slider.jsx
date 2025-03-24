import React, { useState } from 'react';
import { Col, Row, Slider as AntSlider, InputNumber } from 'antd';

const Slider = props => {
  const {
    min = 0,
    max = 100,
    step = 1,
    value: initialValue,
    onChange: formOnChange,
  } = props;
  const [inputValue, setInputValue] = useState(initialValue || min);

  const onChange = newValue => {
    setInputValue(newValue);
    formOnChange(newValue);
  };

  return (
    <Row align="middle">
      <Col span={12}>
        <AntSlider
          min={min}
          max={max}
          step={step}
          onChange={onChange}
          value={typeof inputValue === 'number' ? inputValue : min}
        />
      </Col>
      <Col span={4}>
        <InputNumber
          min={min}
          max={max}
          step={step}
          style={{ margin: '0 16px' }}
          value={inputValue}
          onChange={onChange}
        />
      </Col>
    </Row>
  );
};

export default Slider;

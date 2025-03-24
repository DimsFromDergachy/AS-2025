import React from 'react';
import { Form, Input, Select, DatePicker, InputNumber, Checkbox } from 'antd';
import Slider from '../Slider';

const getFormControl = field => {
  const commonProps = {
    allowClear: true,
    placeholder: field.placeholder,
    style: { width: '100%' },
  };

  switch (field.displayType) {
    case 'Reference':
    case 'References':
      return (
        <Select
          {...commonProps}
          options={field.options}
          mode={field.displayType === 'References' ? 'multiple' : undefined}
          fieldNames={{ label: 'value', value: 'key' }}
        />
      );
    case 'Text':
      return (
        <Input.TextArea
          {...commonProps}
          autoSize={{ minRows: 3, maxRows: 5 }}
        />
      );
    case 'Date':
      return <DatePicker {...commonProps} format="DD.MM.YYYY" />;
    case 'Decimal':
    case 'Integer':
    case 'Percent': {
      const percent = field.displayType === 'Percent';
      const precision = field.displayType === 'Integer' ? 0 : 2;
      const { min, max, step } = field.numberFormat || {};

      return (
        <InputNumber
          {...commonProps}
          precision={precision}
          addonAfter={percent && '%'}
          min={min}
          max={max}
          step={step}
        />
      );
    }
    case 'Checkbox':
      return <Checkbox />;
    case 'Slider': {
      const { min, max, step } = field.numberFormat || {};
      return <Slider min={min} max={max} step={step} />;
    }
      default:
      return <Input {...commonProps} />;
  }
};

export const FormFieldsGroup = ({ fields, fieldErrors = {} }) => {
  const renderFieldItem = field => (
    <Form.Item
      key={field.key}
      name={field.key}
      valuePropName={field.displayType === 'Checkbox' ? 'checked' : 'value'}
      label={field.label}
      required={field.required}
      rules={[
        { required: field.required },
        {
          validator: () =>
            fieldErrors[field.key]
              ? Promise.reject(fieldErrors[field.key][0])
              : Promise.resolve(),
        },
      ]}
    >
      {getFormControl(field)}
    </Form.Item>
  );

  if (!fields) return null;

  return fields.map(renderFieldItem);
};

import React, { useState, useEffect, useMemo } from 'react';
import {
  Button,
  Form,
  Modal,
  Input,
  InputNumber,
  Select,
  Space,
  Typography,
  Checkbox,
  DatePicker,
  Drawer,
} from 'antd';
import dayjs from 'dayjs';

const { Text } = Typography;

const Footer = ({ disabled, okButton, cancelButton }) => (
  <Space>
    <Button onClick={okButton.onClick} disabled={disabled} type="primary">
      <Text strong style={{ color: disabled ? '#BABABA' : 'white' }}>
        {okButton.text}
      </Text>
    </Button>
    <Button onClick={cancelButton.onClick} type="text">
      <Text strong>{cancelButton.text}</Text>
    </Button>
  </Space>
);

const getFormControl = field => {
  const commonProps = {
    key: field.key,
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
          mode={field.displayType === 'References' && 'multiple'}
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
      return (
        <InputNumber
          {...commonProps}
          addonAfter={percent && '%'}
          min={percent && 0}
          max={percent && 100}
        />
      );
    }
    case 'Checkbox':
      return <Checkbox />;
    default:
      return <Input {...commonProps} />;
  }
};

const getInitValues = (formFields, item) => {
  return formFields.reduce((acc, field) => {
    const { key, displayType, options } = field;
    let value;

    if (item) {
      if (options) {
        const refKey = key.replace('Id', '');
        value = options.find(opt => opt.value === item[refKey])?.key;
      } else {
        value = item[key];

        if (displayType === 'Date' && value) {
          value = dayjs(value);
        }
      }
    } else {
      value = displayType.includes('Reference') ? null : '';

      if (displayType === 'Checkbox') {
        value = false;
      }
      if (displayType === 'Date') {
        value = null;
      }
    }

    return { ...acc, [key]: value };
  }, {});
};

export default function AddEditDrawer(props) {
  const { open, item, schema = [], editMode, onSubmit, setVisible } = props;

  const { inputs: formFields } = schema;

  const initValues = useMemo(
    () => getInitValues(formFields, item),
    [formFields, item]
  );

  const [form] = Form.useForm();
  const values = Form.useWatch([], form);
  const [isValid, setIsValid] = useState(false);

  const title = editMode && item ? 'Редактировать элемент' : 'Добавить элемент';

  const handleOk = () => {
    onSubmit(values);
  };

  const handleCancel = () => {
    setVisible(false);
  };

  useEffect(() => {
    form
      .validateFields({ validateOnly: true })
      .then(() => setIsValid(true))
      .catch(({ outOfDate }) => !outOfDate && setIsValid(false));
  }, [form, values]);

  useEffect(() => {
    form.resetFields();
  }, [form, open]);

  // useEffect(() => {
  //   if (fieldErrors) {
  //     form.validateFields();
  //   }
  // }, [fieldErrors]);

  return (
    <Drawer
      forceRender
      maskClosable={false}
      title={title}
      width={650}
      open={open}
      onClose={handleCancel}
      footer={
        <Footer
          disabled={!isValid}
          okButton={{
            text: editMode ? 'Сохранить' : 'Добавить',
            onClick: handleOk,
          }}
          cancelButton={{
            text: 'Отменить',
            onClick: handleCancel,
          }}
        />
      }
    >
      <div className="h-full flex flex-col justify-between">
        <Form
          form={form}
          autoComplete="off"
          labelCol={{ flex: '1 1 140px' }}
          labelAlign="left"
          labelWrap
          wrapperCol={{ flex: '1 1 380px' }}
          validateMessages={{ required: 'Обязательное поле' }}
          initialValues={initValues}
          onChange={() => {
            // if (fieldErrors) store.merge({ fieldErrors: null })
          }}
        >
          <Form.Item
            colon={false}
            label={
              <Text strong type="secondary">
                Название
              </Text>
            }
          >
            <Text strong type="secondary">
              Значение
            </Text>
          </Form.Item>

          {formFields.map(field => (
            /* (!editMode || (editMode && field.editable)) &&  */ <Form.Item
              key={field.key}
              name={field.key}
              valuePropName={
                field.displayType === 'Checkbox' ? 'checked' : 'value'
              }
              label={field.label}
              required={field.required}
              rules={[
                {
                  required: field.required,
                },
                () => ({
                  validator() {
                    // if (fieldErrors && fieldErrors[field.key]) {
                    //   return Promise.reject(fieldErrors[field.key][0]);
                    // }
                    return Promise.resolve();
                  },
                }),
              ]}
            >
              {getFormControl(field)}
            </Form.Item>
          ))}
        </Form>
        {/* <Text style={{ color: 'red' }}>{getErrorMessage(serverError, serverErrorType)}</Text> */}
      </div>
    </Drawer>
  );
}

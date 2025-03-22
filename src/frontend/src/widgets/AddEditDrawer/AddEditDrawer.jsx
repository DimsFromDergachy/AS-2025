import React, { useState, useEffect, useMemo } from 'react';
import {
  Button,
  Form,
  Space,
  Typography,
  Drawer,
} from 'antd';
import dayjs from 'dayjs';
import FormFieldsGroup from 'src/widgets/FormFieldsGroup';

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
  const { open, item, schema = [], editMode, onSubmit, onClose } = props;

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
    onClose(false);
  };

  useEffect(() => {
    form
      .validateFields({ validateOnly: true })
      .then(() => setIsValid(true))
      .catch(({ outOfDate }) => !outOfDate && setIsValid(false));
  }, [form, values]);

  useEffect(() => {
    open && form.resetFields();
  }, [form, open]);

  // useEffect(() => {
  //   if (fieldErrors) {
  //     form.validateFields();
  //   }
  // }, [fieldErrors]);

  return (
    <Drawer
      forceRender
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
          <FormFieldsGroup fields={formFields} />
        </Form>
        {/* <Text style={{ color: 'red' }}>{getErrorMessage(serverError, serverErrorType)}</Text> */}
      </div>
    </Drawer>
  );
}

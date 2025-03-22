import { Button, Form, Typography, Descriptions } from 'antd';
import React, { useEffect, useState } from 'react';
import { useParams } from 'react-router';
import { apiClient } from 'src/api/client';
import FormFieldsGroup from 'src/widgets/FormFieldsGroup';

const { Title, Text } = Typography;

const formFields = {
  backpack: [
    {
      key: 'capacity',
      label: 'Вместимость',
      placeholder: 'Вместимость рюкзака',
      visibilityType: 'Visible',
      displayType: 'Integer',
      referenceType: 'Model',
      referenceName: null,
      referenceRequest: null,
      numberFormat: null,
    },
    {
      key: 'temperature',
      label: 'Температура',
      placeholder: 'Температура',
      visibilityType: 'Visible',
      displayType: 'Decimal',
      referenceType: 'Model',
      referenceName: null,
      referenceRequest: null,
      numberFormat: null,
    },
    {
      key: 'coolingRate',
      label: 'Коэффициент охлаждения',
      placeholder: 'Коэффициент охлаждения',
      visibilityType: 'Visible',
      displayType: 'Decimal',
      referenceType: 'Model',
      referenceName: null,
      referenceRequest: null,
      numberFormat: null,
      precision: 3,
      min: 0.01,
      max: 0.9999,
    },
    {
      key: 'iterationsPerTemp',
      label: 'Итераций',
      placeholder: 'Итераций на температуру',
      visibilityType: 'Visible',
      displayType: 'Integer',
      referenceType: 'Model',
      referenceName: null,
      referenceRequest: null,
      numberFormat: null,
    },
  ],
  scheduler: [
    {
      key: 'quarterDays',
      label: 'Дни',
      placeholder: 'Дни',
      visibilityType: 'Visible',
      displayType: 'Integer',
      referenceType: 'Model',
      referenceName: null,
      referenceRequest: null,
      numberFormat: null,
    },
    {
      key: 'temperature',
      label: 'Температура',
      placeholder: 'Температура',
      visibilityType: 'Visible',
      displayType: 'Decimal',
      referenceType: 'Model',
      referenceName: null,
      referenceRequest: null,
      numberFormat: null,
    },
    {
      key: 'coolingRate',
      label: 'Коэффициент охлаждения',
      placeholder: 'Коэффициент охлаждения',
      visibilityType: 'Visible',
      displayType: 'Decimal',
      referenceType: 'Model',
      referenceName: null,
      referenceRequest: null,
      numberFormat: null,
      precision: 3,
      min: 0.01,
      max: 0.9999,
    },
    {
      key: 'iterationsPerTemp',
      label: 'Итераций',
      placeholder: 'Итераций на температуру',
      visibilityType: 'Visible',
      displayType: 'Integer',
      referenceType: 'Model',
      referenceName: null,
      referenceRequest: null,
      numberFormat: null,
    },
  ],
};

export default function Algos() {
  const { algorithm } = useParams();
  const [form] = Form.useForm();

  const [data, setData] = useState(null);

  const onFinish = values => {
    apiClient.post(`/algos/${algorithm}`, values).then(res => {
      setData(res);
    });
  };

  useEffect(() => {
    setData(null);
    form.resetFields();
  }, [form, algorithm]);

  return (
    <>
      <Form
        form={form}
        autoComplete="off"
        labelCol={{ flex: '1 1 140px' }}
        labelAlign="left"
        labelWrap
        wrapperCol={{ flex: '1 1 380px' }}
        validateMessages={{ required: 'Обязательное поле' }}
        onFinish={onFinish}
      >
        <FormFieldsGroup fields={formFields[algorithm]} />
        <Form.Item>
          <Button type="primary" htmlType="submit">
            Сделать уличную магию
          </Button>
        </Form.Item>
      </Form>
      <div>
        {data && (
          <div>
            <Title level={3}>Результат:</Title>
            <Text strong style={{ display: 'block', marginBottom: 16 }}>
              {`Лучшее значение: ${data.bestValue || data.bestScore || 'Нет данных'}`}
            </Text>

            {Object.entries(data).map(([key, value]) => {
              if (key === 'bestValue' || key === 'bestScore') return null;

              const renderValue = val => {
                // Для массивов
                if (Array.isArray(val)) {
                  return (
                    <Descriptions
                      bordered
                      size="small"
                      column={1}
                      items={val.map((item, index) => ({
                        key: index,
                        label: `Элемент ${index + 1}`,
                        children: renderValue(item),
                      }))}
                    />
                  );
                }

                // Для объектов
                if (typeof val === 'object' && val !== null) {
                  return (
                    <Descriptions
                      bordered
                      size="small"
                      column={1}
                      items={Object.entries(val).map(([objKey, objValue]) => ({
                        key: objKey,
                        label: objKey,
                        children: renderValue(objValue),
                      }))}
                    />
                  );
                }

                // Для примитивов
                return val;
              };

              return (
                <div key={key} style={{ marginBottom: 12 }}>
                  <Text strong>{`${key}: `}</Text>
                  {renderValue(value)}
                </div>
              );
            })}
          </div>
        )}
      </div>
    </>
  );
}

import { Button, DatePicker, Input, Select, Space } from 'antd';
import React, { useState, forwardRef } from 'react';

const filterOptions = {
  string: [
    { value: 'contains', label: 'Содержит' },
    { value: 'doesNotContain', label: 'Не содержит' },
    { value: 'equalsString', label: 'Равно' },
    { value: 'notEqualsString', label: 'Не равно' },
    { value: 'startsWith', label: 'Начинается с' },
    { value: 'endsWith', label: 'Заканчивается на' },
  ],
  date: [
    { value: 'laterOrEquals', label: 'Больше или равно' },
    { value: 'earlierOrEquals', label: 'Меньше или равно' },
    { value: 'equalsDate', label: 'Равно (=)' },
    { value: 'notEqualsDate', label: 'Не равно' },
    { value: 'betweenDate', label: 'Между' },
  ],
  number: [
    { value: 'moreOrEquals', label: 'Больше или равно' },
    { value: 'lessOrEquals', label: 'Меньше или равно' },
    { value: 'equalsNumber', label: 'Равно (=)' },
    { value: 'notEqualsNumber', label: 'Не равно' },
    { value: 'betweenNumber', label: 'Между' },
  ],
};

const getFilterOptions = displayType => {
  switch (displayType) {
    case 'Date':
      return filterOptions.date;
    case 'Integer':
    case 'Double':
    case 'Percent':
      return filterOptions.number;
    default:
      return filterOptions.string;
  }
};

function FilterPopup(props, ref) {
  const { setSelectedKeys, selectedKeys, confirm, clearFilters, column } =
    props;
  const { title, displayType } = column;

  const options = getFilterOptions(displayType);
  const [defaultOption] = options;

  const isCheckbox = displayType === 'Checkbox';

  // Состояния для оператора и значений
  const initialOperator =
    !isCheckbox && selectedKeys[0] ? selectedKeys[0] : defaultOption?.value;
  const initialValues = !isCheckbox ? selectedKeys.slice(1) : [];
  const [selectedOperator, setSelectedOperator] = useState(initialOperator);
  const [values, setValues] = useState(initialValues);

  // Обработчик применения фильтра
  const handleConfirm = () => {
    if (isCheckbox) {
      confirm();
      return;
    }
    const newKeys = [
      [selectedOperator, ...values.filter(v => v !== undefined)],
    ];
    setSelectedKeys(newKeys);
    setTimeout(() => confirm());
  };

  // Обработчик сброса фильтра
  const handleReset = () => {
    if (isCheckbox) {
      clearFilters?.();
      confirm?.();
      return;
    }
    setSelectedOperator(defaultOption.value);
    setValues([]);
    setSelectedKeys([]);
    clearFilters?.();
    confirm?.();
  };

  // Генерация полей ввода в зависимости от оператора
  const renderInputs = () => {
    if (isCheckbox) {
      return (
        <Select
          placeholder="Выберите значение"
          className="block mb-2"
          value={selectedKeys[0]}
          onChange={val => setSelectedKeys(val ? [[val]] : [])}
          options={[
            { value: 'true', label: 'Да' },
            { value: 'false', label: 'Нет' },
          ]}
        />
      );
    }

    const isBetween = selectedOperator.includes('between');
    const isDate = displayType === 'Date';
    const isNumber = ['Integer', 'Double', 'Percent'].includes(displayType);

    if (isBetween) {
      return isDate ? (
        <>
          <DatePicker
            className="block mb-2"
            value={values[0]}
            onChange={date => setValues([date, values[1]])}
            placeholder="Начало"
          />
          <DatePicker
            className="block mb-2"
            value={values[1]}
            onChange={date => setValues([values[0], date])}
            placeholder="Конец"
          />
        </>
      ) : (
        <>
          <Input
            ref={ref}
            className="block mb-2"
            type={isNumber ? 'number' : 'text'}
            value={values[0]}
            onChange={e => setValues([e.target.value, values[1]])}
            placeholder="От"
          />
          <Input
            className="block mb-2"
            type={isNumber ? 'number' : 'text'}
            value={values[1]}
            onChange={e => setValues([values[0], e.target.value])}
            placeholder="До"
          />
        </>
      );
    }

    if (isDate) {
      return (
        <DatePicker
          className="block mb-2"
          value={values[0]}
          onChange={date => setValues(date ? [date] : [])}
          placeholder={`Фильтр по ${title}`}
        />
      );
    }

    return (
      <Input
        ref={ref}
        className="block"
        type={isNumber ? 'number' : 'text'}
        value={values[0] || ''}
        onChange={e => setValues(e.target.value ? [e.target.value] : [])}
        placeholder={`Фильтр по ${title}`}
      />
    );
  };

  return (
    <div className="p-2" onKeyDown={e => e.stopPropagation()}>
      {!isCheckbox && (
        <Select
          className="w-full mb-2"
          value={selectedOperator}
          onChange={setSelectedOperator}
          options={options}
        />
      )}
      {renderInputs()}
      <Space className="mt-4">
        <Button className="w-[90px]" type="primary" onClick={handleConfirm}>
          Применить
        </Button>
        <Button className="w-[90px]" onClick={handleReset}>
          Сбросить
        </Button>
      </Space>
    </div>
  );
}

export default forwardRef(FilterPopup);

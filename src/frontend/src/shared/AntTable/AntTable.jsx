import React, { useEffect, useRef, useState } from 'react';
import {
  Button,
  DatePicker,
  Input,
  Progress,
  Select,
  Space,
  Table,
  Tag,
} from 'antd';
import dayjs from 'dayjs';
import isSameOrAfter from 'dayjs/plugin/isSameOrAfter';
import isSameOrBefore from 'dayjs/plugin/isSameOrBefore';
import isBetween from 'dayjs/plugin/isBetween';
import { CheckCircleFilled } from '@ant-design/icons';
import { useGlobalStore } from 'src/stores/globalStore';
import FilterPopup from './FilterPopup';

dayjs.extend(isSameOrAfter);
dayjs.extend(isSameOrBefore);
dayjs.extend(isBetween);

const fullWidthColumns = ['Tags', 'Tag', 'Percent'];

function getValueFromPattern(urlPattern, item) {
  return urlPattern.replace(/\{([^}]+)\}/g, (match, key) => {
    return item[key] || match;
  });
}

export default function AntTable(props) {
  const { columns, dataSource } = props;

  const globalStore = useGlobalStore();
  const [currentColumns, setCurrentColumns] = useState([]);
  const [currentData, setCurrentData] = useState([]);

  const searchInput = useRef(null);

  const getColumnSearchProps = column => {
    const { key, displayType } = column;

    return {
      filterDropdown: props => <FilterPopup {...props} column={column} />,
      onFilter: (value, record) => {
        const recordValue = record[key];
        const [operator, filterValue, secondFilterValue] = value;

        // Для Checkbox
        if (displayType === 'Checkbox') {
          return recordValue === (value[0] === 'true');
        }

        // Обработка пустых значений
        if (recordValue === null || recordValue === undefined) return false;
        if (!filterValue) return true;

        // Отдельная обработка для дат
        if (displayType === 'Date') {
          const recordDate = dayjs(recordValue);
          const filterDate1 = dayjs(filterValue);
          const filterDate2 = dayjs(secondFilterValue);

          switch (operator) {
            case 'laterOrEquals':
              return recordDate.isSameOrAfter(filterDate1, 'day');

            case 'earlierOrEquals':
              return recordDate.isSameOrBefore(filterDate1, 'day');

            case 'equalsDate':
              return recordDate.isSame(filterDate1, 'day');

            case 'notEqualsDate':
              return !recordDate.isSame(filterDate1, 'day');

            case 'betweenDate':
              return recordDate.isBetween(
                filterDate1,
                filterDate2,
                'day',
                '[]'
              );

            default:
              return true;
          }
        }

        // Обработка для числовых типов
        if (['Integer', 'Double', 'Percent'].includes(displayType)) {
          const numValue = Number(recordValue);
          const filterNum1 = Number(filterValue);
          const filterNum2 = Number(secondFilterValue);

          switch (operator) {
            case 'moreOrEquals':
              return numValue >= filterNum1;

            case 'lessOrEquals':
              return numValue <= filterNum1;

            case 'equalsNumber':
              return numValue === filterNum1;

            case 'notEqualsNumber':
              return numValue !== filterNum1;

            case 'betweenNumber':
              return numValue >= filterNum1 && numValue <= filterNum2;

            default:
              return true;
          }
        }

        // Обработка для строковых типов
        const strValue = String(recordValue).toLowerCase();
        const filterStr = String(filterValue).toLowerCase();

        switch (operator) {
          case 'contains':
            return strValue.includes(filterStr);

          case 'doesNotContain':
            return !strValue.includes(filterStr);

          case 'equalsString':
            return strValue === filterStr;

          case 'notEqualsString':
            return strValue !== filterStr;

          case 'startsWith':
            return strValue.startsWith(filterStr);

          case 'endsWith':
            return strValue.endsWith(filterStr);

          default:
            return true;
        }
      },
      filterDropdownProps: {
        onOpenChange(open) {
          if (open) {
            setTimeout(() => searchInput.current?.select(), 100);
          }
        },
      },
    };
  };

  const getColumnSortProps = column => {
    const { key, displayType } = column;

    return {
      sorter: (a, b) => {
        const valueA = a[key];
        const valueB = b[key];

        if (displayType === 'Date') {
          return dayjs(valueA).unix() - dayjs(valueB).unix();
        } else if (['Percent', 'Integer', 'Double'].includes(displayType)) {
          return valueA - valueB;
        } else {
          return valueA.localeCompare(valueB);
        }
      },
    };
  };

  useEffect(() => {
    if (columns) {
      const taggedEnums = globalStore.taggedEnums.get();
      setCurrentColumns(
        columns
          .filter(col => col.visibilityType === 'Visible')
          .map(col => {
            const { urlPattern, displayPattern, tagReferenceEnum } = col;

            // убрать true
            if (col.filterable || true) {
              col = { ...col, ...getColumnSearchProps(col) };
            }

            // убрать true
            if (col.sortable || true) {
              col = { ...col, ...getColumnSortProps(col) };
            }

            return {
              ...col,
              dataIndex: col.key,
              align: col.displayType === 'Checkbox' ? 'center' : 'left',
              ellipsis: fullWidthColumns.includes(col.displayType)
                ? false
                : { showTitle: true },
              sortDirections: ['ascend', 'descend', 'ascend'],
              render: (value, record) => {
                switch (col.displayType) {
                  case 'Tags':
                    return (
                      <div className="flex flex-wrap gap-1">
                        {value.map((tag, i) => {
                          const { style } =
                            taggedEnums[tagReferenceEnum]?.find(
                              el => el.key === (tag.tag || tag)
                            ) || {};
                          return (
                            <Tag
                              key={(tag.tag || tag) + i}
                              color={style || 'geekblue'}
                            >
                              {tag.tag || tag}
                            </Tag>
                          );
                        })}
                      </div>
                    );
                  case 'Tag': {
                    const { style } =
                      taggedEnums[tagReferenceEnum]?.find(
                        el => el.key === value
                      ) || {};
                    return (
                      <Tag key={value} color={style || 'geekblue'}>
                        {value}
                      </Tag>
                    );
                  }
                  case 'Date':
                    return (
                      <span>
                        {value
                          ? new Date(value).toLocaleDateString()
                          : 'Нет данных'}
                      </span>
                    );
                  case 'Checkbox':
                    return value ? (
                      <CheckCircleFilled
                        style={{ color: 'green', fontSize: '18px' }}
                      />
                    ) : (
                      ''
                    );
                  case 'Percent':
                    return (
                      <Progress
                        percent={value}
                        status={value >= 100 ? 'success' : 'active'}
                        strokeColor={value >= 80 ? '#52c41a' : '#1890ff'}
                      />
                    );
                  case 'Link':
                    return (
                      <a
                        href={
                          urlPattern
                            ? getValueFromPattern(urlPattern, record)
                            : value
                        }
                        target={urlPattern ? '_self' : '_blank'}
                      >
                        {value}
                      </a>
                    );

                  default:
                    return displayPattern
                      ? getValueFromPattern(displayPattern, record)
                      : value;
                }
              },
              ...getColumnSearchProps(col),
            };
          })
      );
    }
  }, [columns]);

  useEffect(() => {
    setCurrentData(dataSource);
  }, [dataSource]);

  return (
    <Table
      size="small"
      {...props}
      columns={currentColumns}
      dataSource={currentData}
      scroll={{ x: 1800 }}
      rowKey={item => item.id}
    />
  );
}

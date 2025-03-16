import React, { useEffect, useState} from 'react'
import { Progress, Table, Tag } from 'antd';
import dayjs from 'dayjs';
import { CheckCircleFilled } from '@ant-design/icons';
import { useGlobalStore } from 'src/stores/globalStore';

const fullWidthColumns = ['Tags', 'Tag', 'Percent'];

function getValueFromPattern(urlPattern, item) {
  return urlPattern.replace(/\{([^}]+)\}/g, (match, key) => {
    return item[key] || match;
  });
};

export default function AntTable(props) {
  const { columns, dataSource } = props;

  const globalStore = useGlobalStore();
  const [currentColumns, setCurrentColumns] = useState([])
  const [currentData, setCurrentData] = useState([])

  useEffect(() => {
    if (columns) {
      const taggedEnums = globalStore.taggedEnums.get();
      setCurrentColumns(columns.filter(col => col.visibilityType === 'Visible').map((col) => {
        const { urlPattern, displayPattern, tagReferenceEnum } = col;

        return {
          ...col,
          dataIndex: col.key,
          filtered: col.filterable,
          align: col.displayType === 'Checkbox' ? 'center' : 'left',
          ellipsis: fullWidthColumns.includes(col.displayType) ? false : { showTitle: true },
          sorter: col.sortable ? (a, b) => {
            const valueA = a[col.key];
            const valueB = b[col.key];
      
            if (col.displayType === 'Date') {
              return dayjs(valueA, 'DD.MM.YYYY').isAfter(dayjs(valueB, 'DD.MM.YYYY')) ? 1 : -1;
            }
      
            return valueA > valueB ? 1 : -1;
          } : false,
          render: (value, record) => {
            switch (col.displayType) {
              case 'Tags':
                return <div className="flex flex-wrap gap-1">
                  {value.map(tag => {
                    const { style } = taggedEnums[tagReferenceEnum]?.find(el => el.key === (tag.tag || tag)) || {};
                    return <Tag key={tag.tag || tag} color={style || "geekblue"}>{tag.tag || tag}</Tag>
                  })}
                </div>
              case 'Tag': {
                const { style } = taggedEnums[tagReferenceEnum]?.find(el => el.key === value) || {};
                return <Tag key={value} color={style || "geekblue"}>{value}</Tag>
              }
              case 'Date':
                return <span>{value ? new Date(value).toLocaleDateString() : 'Нет данных'}</span>
              case 'Checkbox':
                return value ? <CheckCircleFilled style={{ color: 'green', fontSize: '18px' }} /> : ''
                case 'Percent':
                  return         <Progress 
                  percent={value} 
                  status={value >= 100 ? 'success' : 'active'}
                  strokeColor={value >= 80 ? '#52c41a' : '#1890ff'}
                />
              case 'Link':
                return <a href={urlPattern ? getValueFromPattern(urlPattern, record) : value} target={urlPattern ? '_self' : '_blank'}>{value}</a>
        
              default:
                return displayPattern ? getValueFromPattern(displayPattern, record) : value
            }
          }
        }
      }))
    }
  }, [columns])

  useEffect(() => {
    setCurrentData(dataSource)
  }, [dataSource])

  return (
    <Table size='small' {...props} columns={currentColumns} dataSource={currentData} scroll={{ x: 1800 }}/>
  )
}

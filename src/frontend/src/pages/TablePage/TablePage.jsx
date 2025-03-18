import React, { useEffect, useMemo, useState } from 'react';
import { Input, Button, App } from 'antd';
import AntTable from 'src/shared/AntTable';
import { SearchOutlined, PlusOutlined } from '@ant-design/icons';

import { apiClient } from 'src/api/client';
import { filterByFields } from 'src/helpers/functions';

const TablePage = ({ tableKey }) => {
  const { modal } = App.useApp();
  const [schema, setSchema] = useState({});
  const [data, setData] = useState([]);
  const [filteredData, setFilteredData] = useState([]);
  const [searchText, setSearchText] = useState('');

  const {
    title = '',
    commonActions = [],
    columnActions = [],
    columns = [],
    fieldsToSearch = [],
  } = schema;

  const actionsColumn = useMemo(
    () => ({
      key: 'actions',
      displayType: 'Actions',
      title: 'Действия',
      visibilityType: 'Visible',
      availableActions: columnActions,
      actions: {
        View: {
          iconName: 'EyeOutlined',
          onClick: id => {
            console.log(id);
          },
        },
        Edit: {
          iconName: 'EditOutlined',
          onClick: id => {
            console.log(id);
          },
        },
        Delete: {
          className: 'text-red-500 hover:text-red-700',
          iconName: 'DeleteOutlined',
          onClick: id => {
            modal
              .confirm({
                title: 'Удаление',
                content: 'Вы действительно хотите удалить элемент?',
                okText: 'Да',
                cancelText: 'Нет',
                footer: (_, { OkBtn, CancelBtn }) => (
                  <>
                    <OkBtn />
                    <CancelBtn />
                  </>
                ),
              })
              .then(confirmed => {
                confirmed &&
                  apiClient.delete(`/${tableKey}/${id}`).then(() => {
                    setData(prev => prev.filter(item => item.id !== id));
                  });
              });
          },
        },
      },
    }),
    [tableKey, columnActions]
  );

  useEffect(() => {
    if (tableKey) {
      apiClient.get(`/${tableKey}/list`).then(({ items }) => {
        setData(items);
      });
      apiClient.get(`/${tableKey}/list/schema`).then(res => {
        if (res) {
          const [action] = res.columnActions || [];
          if (action && action !== 'None') {
            res.columns.push(actionsColumn);
          }
          const fieldsToSearch = res.columns.reduce(
            (acc, col) => (col.searchable ? [...acc, col.key] : acc),
            []
          );
          setSchema({ ...res, fieldsToSearch });
        }
      });
    }
  }, [tableKey]);

  useEffect(() => {
    setFilteredData(filterByFields(data, searchText, fieldsToSearch));
  }, [searchText, data, fieldsToSearch]);

  return (
    <div>
      <div className="mb-6 flex flex-col sm:flex-row justify-between items-start sm:items-center gap-4">
        <h1 className="text-2xl font-bold">{title}</h1>
        <div className="flex gap-2 w-full sm:w-auto">
          {commonActions.includes('Search') && !!fieldsToSearch.length && (
            <Input.Search
              placeholder="Поиск"
              enterButton={<SearchOutlined />}
              onSearch={setSearchText}
              className="w-full sm:w-64"
            />
          )}
          {commonActions.includes('Create') && (
            <Button
              type="primary"
              icon={<PlusOutlined />}
              className="bg-blue-500 hover:bg-blue-600"
            >
              Добавить
            </Button>
          )}
        </div>
      </div>

      <AntTable
        availableActions={columnActions}
        columns={columns}
        dataSource={filteredData}
        pagination={{
          pageSize: 10,
          showTotal: total => `Всего записей: ${total}`,
          hideOnSinglePage: true,
        }}
        scroll={{ x: true }}
        bordered
      />
    </div>
  );
};

export default TablePage;

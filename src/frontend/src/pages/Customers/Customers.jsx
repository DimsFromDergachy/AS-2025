import React, { useEffect, useState } from 'react';
import { Tag, Input, Space, Button } from 'antd';
import AntTable from 'src/shared/AntTable';
import { SearchOutlined, PlusOutlined } from '@ant-design/icons';

import { apiClient } from 'src/api/client';
import { filterByFields } from 'src/helpers/functions';

const Customers = () => {
  const [data, setData] = useState([]);
  const [filteredData, setFilteredData] = useState([]);
  const [columns, setColumns] = useState([]);
  const [searchText, setSearchText] = useState('');
  const [expandedRowKeys, setExpandedRowKeys] = useState([]);

  useEffect(() => {
    apiClient.get('/client/list').then(({ items }) => {
      setData(items);
    });
    apiClient.get('/client/list/schema').then(({ columns }) => {
      setColumns(columns);
    });
  }, []);

  useEffect(() => {
    setFilteredData(filterByFields(data, searchText, ['fullName', 'type']));
  }, [searchText, data]);

  return (
    <div>
      <div className="mb-6 flex flex-col sm:flex-row justify-between items-start sm:items-center gap-4">
        <h1 className="text-2xl font-bold">Управление клиентами</h1>
        <div className="flex gap-2 w-full sm:w-auto">
          <Input.Search
            placeholder="Поиск по имени"
            enterButton={<SearchOutlined />}
            onSearch={setSearchText}
            className="w-full sm:w-64"
          />
          <Button
            type="primary"
            icon={<PlusOutlined />}
            className="bg-blue-500 hover:bg-blue-600"
          >
            Добавить клиента
          </Button>
        </div>
      </div>

      <AntTable
        columns={columns}
        dataSource={filteredData}
        rowClassName={record => (record.children ? 'bg-gray-50' : '')}
        expandable={{
          expandedRowKeys,
          onExpand: (expanded, record) => {
            if (expanded) {
              setExpandedRowKeys([...expandedRowKeys, record.key]);
            } else {
              setExpandedRowKeys(
                expandedRowKeys.filter(key => key !== record.key)
              );
            }
          },
          rowExpandable: record => !!record.children,
        }}
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

export default Customers;

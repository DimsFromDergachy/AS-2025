import React, { useEffect, useState } from 'react';
import { Tag, Input, Space, Button } from 'antd';
import AntTable from 'src/shared/AntTable';
import { 
  SearchOutlined,
  PlusOutlined
} from '@ant-design/icons';

import { apiClient } from 'src/api/client';

const Tasks = () => {
  const [data, setData] = useState(null);
  const [columns, setColumns] = useState(null);
  const [searchText, setSearchText] = useState('');
  const [expandedRowKeys, setExpandedRowKeys] = useState([]);

  useEffect(() => {
    apiClient.get('/task/list').then(({ items }) => {
      setData(items);
    });
    apiClient.get('/task/list/schema').then(({ columns }) => {
      setColumns(columns);
    })
  }, []);

  return (
    <div>
      <div className="mb-6 flex flex-col sm:flex-row justify-between items-start sm:items-center gap-4">
        <h1 className="text-2xl font-bold">Управление задачами</h1>
        <div className="flex gap-2 w-full sm:w-auto">
          <Input.Search
            placeholder="Поиск по названию"
            enterButton={<SearchOutlined />}
            onSearch={setSearchText}
            className="w-full sm:w-64"
          />
          <Button 
            type="primary" 
            icon={<PlusOutlined />}
            className="bg-blue-500 hover:bg-blue-600"
          >
            Добавить задачу
          </Button>
        </div>
      </div>

      <AntTable
        columns={columns}
        dataSource={data}
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

export default Tasks;
import React, { useEffect, useState } from 'react';
import { Tag, Input, Space, Button } from 'antd';
import AntTable from 'src/shared/AntTable';
import { 
  SearchOutlined,
  UserOutlined,
  TeamOutlined,
  EditOutlined,
  DeleteOutlined,
  PlusOutlined
} from '@ant-design/icons';

import { apiClient } from 'src/api/client';

const SuperTable = () => {
  const [data, setData] = useState(null);
  const [columns, setColumns] = useState(null);

  useEffect(() => {
    apiClient.get('/tableControlsPresentation/list').then(({ items }) => {
      setData(items);
    });
    apiClient.get('/tableControlsPresentation/list/schema').then(({ columns }) => {
      setColumns(columns);
    })
  }, []);

  return (
    <div>
      <div className="mb-6 flex flex-col sm:flex-row justify-between items-start sm:items-center gap-4">
        <h1 className="text-2xl font-bold">Просто таблица</h1>
      </div>

      <AntTable
        columns={columns}
        dataSource={data}
        rowClassName={record => record.children ? 'bg-gray-50' : ''}
        pagination={{
          pageSize: 10,
          showTotal: total => `Всего записей: ${total}`,
          hideOnSinglePage: true,
        }}
        scroll={{ x: '100%' }}
        bordered
      />
    </div>
  );
};

export default SuperTable;
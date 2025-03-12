import React, { useState } from 'react';
import { Table, Input, Space, Tag, Spin } from 'antd';
import { SearchOutlined, EditOutlined, DeleteOutlined } from '@ant-design/icons';

const Customers = () => {
  const [searchText, setSearchText] = useState('');
  const [loading, setLoading] = useState(false);

  // Пример данных
  const dataSource = [
    {
      key: '1',
      name: 'Иван Петров',
      email: 'ivan@example.com',
      phone: '+7 (900) 123-45-67',
      address: 'Москва, ул. Пушкина 10',
      registrationDate: '2023-01-15',
      status: 'active',
    },
    {
      key: '2',
      name: 'Анна Сидорова',
      email: 'anna@example.com',
      phone: '+7 (900) 765-43-21',
      address: 'Санкт-Петербург, Невский пр. 25',
      registrationDate: '2023-03-22',
      status: 'inactive',
    },
    // ... больше данных
  ];

  const columns = [
    {
      title: 'Имя',
      dataIndex: 'name',
      key: 'name',
      sorter: (a, b) => a.name.localeCompare(b.name),
      filterDropdown: ({ setSelectedKeys, selectedKeys, confirm }) => (
        <div style={{ padding: 8 }}>
          <Input
            placeholder="Поиск по имени"
            value={selectedKeys[0]}
            onChange={e => setSelectedKeys(e.target.value ? [e.target.value] : [])}
            onPressEnter={confirm}
            style={{ width: 188, marginBottom: 8, display: 'block' }}
          />
        </div>
      ),
      onFilter: (value, record) => record.name.toLowerCase().includes(value.toLowerCase()),
    },
    {
      title: 'Email',
      dataIndex: 'email',
      key: 'email',
      responsive: ['md'],
    },
    {
      title: 'Телефон',
      dataIndex: 'phone',
      key: 'phone',
      responsive: ['lg'],
    },
    {
      title: 'Адрес',
      dataIndex: 'address',
      key: 'address',
      responsive: ['xl'],
    },
    {
      title: 'Дата регистрации',
      dataIndex: 'registrationDate',
      key: 'registrationDate',
      sorter: (a, b) => new Date(a.registrationDate) - new Date(b.registrationDate),
      responsive: ['md'],
    },
    {
      title: 'Статус',
      dataIndex: 'status',
      key: 'status',
      render: status => (
        <Tag color={status === 'active' ? 'green' : 'volcano'}>
          {status === 'active' ? 'Активен' : 'Неактивен'}
        </Tag>
      ),
      filters: [
        { text: 'Активен', value: 'active' },
        { text: 'Неактивен', value: 'inactive' },
      ],
      onFilter: (value, record) => record.status === value,
    },
    {
      title: 'Действия',
      key: 'actions',
      render: () => (
        <Space size="middle">
          <EditOutlined className="text-blue-500 hover:text-blue-700 cursor-pointer" />
          <DeleteOutlined className="text-red-500 hover:text-red-700 cursor-pointer" />
        </Space>
      ),
    },
  ];

  return (
    <div className="p-4 bg-white rounded-lg shadow">
      <div className="mb-4 flex justify-between items-center">
        <h1 className="text-2xl font-semibold">Управление заказчиками</h1>
        <Input.Search
          placeholder="Поиск по имени или email"
          enterButton
          onSearch={value => setSearchText(value)}
          className="w-64"
        />
      </div>

      <Spin spinning={loading}>
        <Table
          columns={columns}
          dataSource={dataSource.filter(item =>
            item.name.toLowerCase().includes(searchText.toLowerCase()) ||
            item.email.toLowerCase().includes(searchText.toLowerCase())
          )}
          pagination={{
            pageSize: 10,
            showSizeChanger: false,
            showTotal: total => `Всего ${total} записей`,
          }}
          scroll={{ x: true }}
          className="custom-table"
        />
      </Spin>
    </div>
  );
};

export default Customers;
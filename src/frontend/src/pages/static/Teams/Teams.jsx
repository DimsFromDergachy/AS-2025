import React, { useState } from 'react';
import { Table, Tag, Input, Space, Button } from 'antd';
import { 
  SearchOutlined,
  UserOutlined,
  TeamOutlined,
  EditOutlined,
  DeleteOutlined,
  PlusOutlined
} from '@ant-design/icons';

const Teams = () => {
  const [searchText, setSearchText] = useState('');
  const [expandedRowKeys, setExpandedRowKeys] = useState([]);

  const teamsData = [
    {
      key: 'team-1',
      name: 'Frontend Team',
      role: 'Команда',
      accessLevel: 'high',
      status: 'active',
      children: [
        {
          key: 'dev-1',
          name: 'Иван Петров',
          role: 'Senior Frontend',
          accessLevel: 'medium',
          status: 'active',
        },
        {
          key: 'dev-2',
          name: 'Анна Сидорова',
          role: 'Middle Frontend',
          accessLevel: 'medium',
          status: 'vacation',
        },
      ],
    },
    {
      key: 'team-2',
      name: 'Backend Team',
      role: 'Команда',
      accessLevel: 'high',
      status: 'active',
      children: [
        {
          key: 'dev-3',
          name: 'Сергей Иванов',
          role: 'Tech Lead',
          accessLevel: 'high',
          status: 'active',
        },
        {
          key: 'dev-4',
          name: 'Ольга Кузнецова',
          role: 'DevOps Engineer',
          accessLevel: 'medium',
          status: 'active',
        },
      ],
    },
  ];

  const columns = [
    {
      title: 'Название / Имя',
      dataIndex: 'name',
      key: 'name',
      sorter: (a, b) => a.name.localeCompare(b.name),
      render: (text, record) => (
        <div className="flex items-center gap-2">
          {record.children ? (
            <TeamOutlined className="text-blue-500" />
          ) : (
            <UserOutlined className="text-gray-500" />
          )}
          <span className={record.children ? 'font-medium' : 'ml-4'}>{text}</span>
        </div>
      ),
    },
    {
      title: 'Роль',
      dataIndex: 'role',
      key: 'role',
      responsive: ['md'],
    },
    {
      title: 'Уровень доступа',
      dataIndex: 'accessLevel',
      key: 'accessLevel',
      render: level => {
        const colorMap = {
          high: 'red',
          medium: 'orange',
          low: 'green'
        };
        return (
          <Tag color={colorMap[level]} className="capitalize">
            {level}
          </Tag>
        );
      },
      filters: [
        { text: 'Высокий', value: 'high' },
        { text: 'Средний', value: 'medium' },
        { text: 'Низкий', value: 'low' },
      ],
      onFilter: (value, record) => record.accessLevel === value,
      responsive: ['lg'],
    },
    {
      title: 'Статус',
      dataIndex: 'status',
      key: 'status',
      render: status => {
        const statusMap = {
          active: { color: 'green', text: 'Активен' },
          vacation: { color: 'gold', text: 'Отпуск' },
          inactive: { color: 'volcano', text: 'Неактивен' }
        };
        return (
          <Tag color={statusMap[status].color}>
            {statusMap[status].text}
          </Tag>
        );
      },
      filters: [
        { text: 'Активен', value: 'active' },
        { text: 'Отпуск', value: 'vacation' },
        { text: 'Неактивен', value: 'inactive' },
      ],
      onFilter: (value, record) => record.status === value,
    },
    {
      title: 'Действия',
      key: 'actions',
      render: (_, record) => (
        <Space size="middle">
          {!record.children && (
            <>
              <EditOutlined className="text-green-500 hover:text-green-700 cursor-pointer" />
              <DeleteOutlined className="text-red-500 hover:text-red-700 cursor-pointer" />
            </>
          )}
          {record.children && (
            <Button 
              type="link"
              className="text-blue-500"
              onClick={() => {
                expandedRowKeys.includes(record.key)
                  ? setExpandedRowKeys(expandedRowKeys.filter(key => key !== record.key))
                  : setExpandedRowKeys([...expandedRowKeys, record.key])
              }}
            >
              {expandedRowKeys.includes(record.key) ? 'Свернуть' : 'Развернуть'}
            </Button>
          )}
        </Space>
      ),
    },
  ];

  return (
    <div>
      <div className="mb-6 flex flex-col sm:flex-row justify-between items-start sm:items-center gap-4">
        <h1 className="text-2xl font-bold">Управление командами</h1>
        <div className="flex gap-2 w-full sm:w-auto">
          <Input.Search
            placeholder="Поиск по имени или роли"
            enterButton={<SearchOutlined />}
            onSearch={setSearchText}
            className="w-full sm:w-64"
          />
          <Button 
            type="primary" 
            icon={<PlusOutlined />}
            className="bg-blue-500 hover:bg-blue-600"
          >
            Добавить команду
          </Button>
        </div>
      </div>

      <Table
        columns={columns}
        dataSource={teamsData}
        rowClassName={record => record.children ? 'bg-gray-50' : ''}
        expandable={{
          expandedRowKeys,
          onExpand: (expanded, record) => {
            if (expanded) {
              setExpandedRowKeys([...expandedRowKeys, record.key]);
            } else {
              setExpandedRowKeys(expandedRowKeys.filter(key => key !== record.key));
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

export default Teams;
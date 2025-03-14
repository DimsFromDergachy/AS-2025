import React, { useState } from 'react';
import { Table, Tag, Progress, Input, Space, Button } from 'antd';
import { 
  SearchOutlined, 
  EditOutlined, 
  DeleteOutlined, 
  EyeOutlined,
  PlusOutlined 
} from '@ant-design/icons';

const Projects = () => {
  const [searchText, setSearchText] = useState('');
  const [selectedRowKeys, setSelectedRowKeys] = useState([]);

  const projectsData = [
    {
      key: '1',
      projectName: 'Мобильное приложение для банка',
      status: 'active',
      startDate: '2024-01-15',
      deadline: '2024-06-30',
      manager: 'Иван Петров',
      budget: 2500000,
      progress: 65,
      stack: ['React Native', 'Node.js', 'MongoDB'],
    },
    {
      key: '2',
      projectName: 'Облачная CRM система',
      status: 'on-hold',
      startDate: '2023-11-01',
      deadline: '2024-09-01',
      manager: 'Анна Сидорова',
      budget: 4500000,
      progress: 30,
      stack: ['Angular', '.NET', 'PostgreSQL'],
    },
    // ... больше данных
  ];

  const columns = [
    {
      title: 'Название проекта',
      dataIndex: 'projectName',
      key: 'projectName',
      sorter: (a, b) => a.projectName.localeCompare(b.projectName),
      filterDropdown: ({ setSelectedKeys, selectedKeys, confirm }) => (
        <div className="p-2">
          <Input
            placeholder="Поиск по названию"
            value={selectedKeys[0]}
            onChange={e => setSelectedKeys(e.target.value ? [e.target.value] : [])}
            onPressEnter={confirm}
            className="w-48 mb-2"
          />
          <Button type="primary" onClick={confirm} size="small">
            Поиск
          </Button>
        </div>
      ),
      onFilter: (value, record) => 
        record.projectName.toLowerCase().includes(value.toLowerCase()),
    },
    {
      title: 'Статус',
      dataIndex: 'status',
      key: 'status',
      render: status => {
        let color = status === 'active' ? 'green' : 
                   status === 'completed' ? 'blue' : 'orange';
        return (
          <Tag color={color} className="capitalize">
            {status}
          </Tag>
        );
      },
      filters: [
        { text: 'Активный', value: 'active' },
        { text: 'Завершен', value: 'completed' },
        { text: 'На паузе', value: 'on-hold' },
      ],
      onFilter: (value, record) => record.status === value,
    },
    {
      title: 'Прогресс',
      dataIndex: 'progress',
      key: 'progress',
      render: progress => (
        <Progress 
          percent={progress} 
          status={progress >= 100 ? 'success' : 'active'}
          strokeColor={progress >= 80 ? '#52c41a' : '#1890ff'}
        />
      ),
      sorter: (a, b) => a.progress - b.progress,
      responsive: ['md'],
    },
    {
      title: 'Стек технологий',
      dataIndex: 'stack',
      key: 'stack',
      render: stack => (
        <div className="flex flex-wrap gap-1">
          {stack.map(tech => (
            <Tag key={tech} color="geekblue">{tech}</Tag>
          ))}
        </div>
      ),
      responsive: ['lg'],
    },
    {
      title: 'Бюджет',
      dataIndex: 'budget',
      key: 'budget',
      render: budget => `${(budget / 1000000).toFixed(1)} млн руб.`,
      sorter: (a, b) => a.budget - b.budget,
      responsive: ['xl'],
    },
    {
      title: 'Действия',
      key: 'actions',
      render: () => (
        <Space size="middle">
          <EyeOutlined className="text-blue-500 hover:text-blue-700 cursor-pointer" />
          <EditOutlined className="text-green-500 hover:text-green-700 cursor-pointer" />
          <DeleteOutlined className="text-red-500 hover:text-red-700 cursor-pointer" />
        </Space>
      ),
    },
  ];

  return (
    <div >
      <div className="mb-6 flex flex-col sm:flex-row justify-between items-start sm:items-center gap-4">
        <h1 className="text-2xl font-bold">Управление IT-проектами</h1>
        <div className="flex gap-2 w-full sm:w-auto">
          <Input.Search
            placeholder="Поиск проектов..."
            enterButton={<SearchOutlined />}
            onSearch={setSearchText}
            className="w-full sm:w-64"
          />
          <Button 
            type="primary" 
            icon={<PlusOutlined />}
            className="bg-blue-500 hover:bg-blue-600"
          >
            Новый проект
          </Button>
        </div>
      </div>

      <Table
        columns={columns}
        dataSource={projectsData.filter(project =>
          project.projectName.toLowerCase().includes(searchText.toLowerCase())
        )}
        rowSelection={{
          selectedRowKeys,
          onChange: setSelectedRowKeys,
        }}
        pagination={{
          pageSize: 10,
          showTotal: total => `Найдено ${total} проектов`,
          showSizeChanger: true,
          pageSizeOptions: ['10', '20', '50'],
        }}
        scroll={{ x: 1000 }}
        bordered
        rowClassName="hover:bg-gray-50 cursor-pointer"
      />
    </div>
  );
};

export default Projects;
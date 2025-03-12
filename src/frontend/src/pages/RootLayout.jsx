import React, { useState, useMemo } from 'react';
import { Button, Spin } from 'antd';
import { Suspense } from 'react';
import { Outlet, useNavigate } from 'react-router';
import {
  AppstoreOutlined,
  ContactsOutlined,
  PartitionOutlined,
  TeamOutlined,
  MenuFoldOutlined,
  MenuUnfoldOutlined,
} from '@ant-design/icons';
import { Layout, Menu, theme } from 'antd';

const { Header, Content, Footer, Sider } = Layout;

// const items = [
//   AppstoreOutlined,
//   PartitionOutlined,
//   ContactsOutlined,
//   TeamOutlined,
// ].map((icon, index) => ({
//   key: String(index + 1),
//   icon: React.createElement(icon),
//   label: `nav ${index + 1}`,
// }));

export default function RootLayout() {
  const navigate = useNavigate();
  const [collapsed, setCollapsed] = useState(true);
  const [breakpointBroken, setBreakpointBroken] = useState(false);
  
  const {
    token: { colorBgContainer, borderRadiusLG },
  } = theme.useToken();

  const items = useMemo(() => ([
    {
      key: 'dashboard',
      icon: <AppstoreOutlined />,
      label: 'Стартовая панель',
      onClick: () => {
        navigate('/dashboard');
      }
    },
    {
      key: 'projects',
      icon: <PartitionOutlined />,
      label: 'Проекты',
      onClick: () => {
        navigate('/projects');
      }
    },
    {
      key: 'customers',
      icon: <ContactsOutlined />,
      label: 'Заказчики',
      onClick: () => {
        navigate('/customers');
      }
    },
    {
      key: 'teams',
      icon: <TeamOutlined />,
      label: 'Команды',
      onClick: () => {
        navigate('/teams');
      }
    },
  ]), [navigate]);
    

  // Обработчик брейкпоинтов для адаптивности
  const handleBreakpoint = broken => {
    setBreakpointBroken(broken);
    if (broken) setCollapsed(true);
  };

  return (
    <Suspense fallback={<Spin fullscreen />}>
      <Layout hasSider className="min-h-screen">
        {/* Адаптивный сайдбар */}
        <Sider
          collapsible
          collapsed={collapsed}
          collapsedWidth={breakpointBroken ? 0 : 60}
          breakpoint="md"
          onBreakpoint={handleBreakpoint}
          // onCollapse={setCollapsed}
          className="h-screen sticky top-0 left-0 overflow-y-auto shadow-lg transition-all"
          trigger={null}
        >
          <div className="h-8 bg-gray-800 m-4 rounded" />
          <Menu
            theme="dark"
            mode="inline"
            defaultSelectedKeys={['1']}
            items={items}
            className="bg-gray-800 [&_.ant-menu-item]:!mt-0"
          />
        </Sider>

        <Layout className="bg-gray-50">
          {/* Хедер с кнопкой меню */}
          <Header 
            className="sticky top-0 z-10 p-0 shadow-sm flex items-center"
            style={{ background: colorBgContainer }}
          >
            <Button
              type="text"
              icon={collapsed ? <MenuUnfoldOutlined /> : <MenuFoldOutlined />}
              onClick={() => setCollapsed(!collapsed)}
            style={{
              fontSize: '24px',
              width: 48,
              height: 48,
            }}
          />
            <div className="flex-1 px-4">Header Content</div>
          </Header>

          {/* Основной контент */}
          <Content className="mx-4 mt-4 mb-0 overflow-hidden">
            <div 
              className="min-h-[calc(100vh-180px)] max-h-[calc(100vh-180px)] overflow-auto p-6 bg-white rounded-lg shadow-sm"
              style={{ 
                background: colorBgContainer,
                borderRadius: borderRadiusLG,
              }}
            >
              <Outlet />
            </div>
          </Content>

          {/* Футер */}
          <Footer className="text-center py-4 text-gray-500 bg-white mt-4 shadow-[0_-8px_16px_-6px_rgba(0,0,0,0.02)]">
            Здесь может быть ваша реклама ©{new Date().getFullYear()}
          </Footer>
        </Layout>
      </Layout>
    </Suspense>
  );
}
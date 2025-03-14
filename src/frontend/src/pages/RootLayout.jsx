import React, { useState, useMemo, useContext, useEffect } from 'react';
import { AuthContext } from 'src/shared/Auth/AuthContext';
import { Button, Spin, Radio } from 'antd';
import { Suspense } from 'react';
import { Outlet, useLocation, useNavigate } from 'react-router';
import {
  AppstoreOutlined,
  ContactsOutlined,
  PartitionOutlined,
  TeamOutlined,
  LogoutOutlined,
  UserOutlined,
  TableOutlined,
  DatabaseOutlined,
} from '@ant-design/icons';
import { Layout, Menu, theme } from 'antd';
import { apiClient } from 'src/api/client';
import { useGlobalStore } from 'src/stores/globalStore';
import Loader from 'src/widgets/Loader';

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
  const { pathname } = useLocation();
  const { token } = theme.useToken();
  const { authorized, logout } = useContext(AuthContext);
  const [collapsed, setCollapsed] = useState(true);
  const [breakpointBroken, setBreakpointBroken] = useState(false);

  const globalStore = useGlobalStore();
  const { loading } = globalStore;

  const [_, pageKey] = pathname.split('/');

  const { colorBgContainer, borderRadiusLG } = token;

  const items = useMemo(
    () => [
      {
        key: 'dashboard',
        icon: <AppstoreOutlined />,
        label: 'Стартовая панель',
        onClick: () => {
          navigate('/dashboard');
        },
      },
      {
        key: 'projects-static',
        icon: <PartitionOutlined />,
        label: 'Проекты(static)',
        onClick: () => {
          navigate('/projects-static');
        },
      },
      {
        key: 'customers-static',
        icon: <ContactsOutlined />,
        label: 'Заказчики(static)',
        onClick: () => {
          navigate('/customers-static');
        },
      },
      {
        key: 'teams-static',
        icon: <TeamOutlined />,
        label: 'Команды(static)',
        onClick: () => {
          navigate('/teams-static');
        },
      },
      // {
      //   key: 'projects',
      //   icon: <PartitionOutlined />,
      //   label: 'Проекты',
      //   onClick: () => {
      //     navigate('/projects');
      //   },
      // },
      // {
      //   key: 'customers',
      //   icon: <ContactsOutlined />,
      //   label: 'Заказчики',
      //   onClick: () => {
      //     navigate('/customers');
      //   },
      // },
      {
        key: 'superTable',
        icon: <TableOutlined />,
        label: 'Просто таблица',
        onClick: () => {
          navigate('/superTable');
        },
      },

      {
        key: 'departments',
        icon: <DatabaseOutlined />,
        label: 'Отделы',
        onClick: () => {
          navigate('/departments');
        },
      },


      {
        key: 'teams',
        icon: <TeamOutlined />,
        label: 'Команды',
        onClick: () => {
          navigate('/teams');
        },
      },
      {
        key: 'employees',
        icon: <UserOutlined />,
        label: 'Сотрудники',
        onClick: () => {
          navigate('/employees');
        },
      },
    ],
    [navigate]
  );

  const options = useMemo(
    () =>
      items.map(item => ({
        label: item.icon,
        value: item.key,
      })),
    [items]
  );

  // Обработчик брейкпоинтов для адаптивности
  const handleBreakpoint = broken => {
    setBreakpointBroken(broken);
    if (broken) setCollapsed(true);
  };

  const handleCollapse = (collapsed, type) => {
    if (type === 'clickTrigger') {
      setCollapsed(collapsed);
    }
  };

  const handleChange = e => {
    navigate(`/${e.target.value}`);
  };

  useEffect(() => {
    if (!authorized) navigate('/login');
    else {
      apiClient('/utils/tagged-enums').then(res => {
        globalStore.taggedEnums.set(res);
      });
    }
  }, [navigate, authorized]);

  return (
    authorized && (
      <Suspense fallback={<Loader />}>
        <Layout hasSider className="min-h-screen max-h-screen">
          {/* Адаптивный сайдбар */}
          <Sider
            collapsible
            collapsed={collapsed}
            collapsedWidth={breakpointBroken ? 0 : 60}
            breakpoint="md"
            onBreakpoint={handleBreakpoint}
            onCollapse={handleCollapse}
            className="h-screen sticky top-0 left-0 overflow-y-auto shadow-lg transition-all"
            // trigger={null}
          >
            <div className="h-8 bg-gray-800 m-4 rounded" />
            <div className="h-[calc(100%-5rem)] flex flex-col justify-between">
              <Menu
                theme="dark"
                mode="inline"
                defaultSelectedKeys={[pageKey]}
                items={items}
                className="[&_.ant-menu-item]:!mt-0"
              />

              <Menu
                theme="dark"
                mode="inline"
                selectable={false}
                items={[
                  {
                    key: 'logout',
                    icon: <LogoutOutlined />,
                    label: 'Выход',
                    onClick: () => {
                      logout();
                    },
                  },
                ]}
                className="[&_.ant-menu-item]:!mt-0"
              />
            </div>
          </Sider>

          <Layout className="bg-gray-50">
            {/* Хедер с кнопкой меню */}
            <Header
              className="sticky top-0 z-10 p-0 shadow-sm flex items-center"
              style={{
                background: colorBgContainer,
              }}
            >
              <div className="flex-1 px-4">Header Content</div>
            </Header>

            {/* Основной контент */}
            <Content className="m-2 overflow-hidden">
              <div
                className="h-full overflow-auto py-6 px-2"
                style={{
                  background: colorBgContainer,
                  borderRadius: borderRadiusLG,
                }}
              >
                <Outlet />
              </div>
            </Content>

            {/* Футер */}
            {breakpointBroken && (
              <Footer className="p-2 m-0">
                <Radio.Group
                  block
                  size="large"
                  options={options}
                  defaultValue={pageKey}
                  optionType="button"
                  buttonStyle="solid"
                  onChange={handleChange}
                />
              </Footer>
            )}
            {loading.get() && <Loader />}
          </Layout>
        </Layout>
      </Suspense>
    )
  );
}

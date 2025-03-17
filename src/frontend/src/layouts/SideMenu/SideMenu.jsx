import React, { useMemo, useContext } from 'react';
import { AuthContext } from 'src/shared/Auth/AuthContext';
import { Menu } from 'antd';
import { useLocation, useNavigate } from 'react-router';
import { useGlobalStore } from 'src/stores/globalStore';

import {
  AppstoreOutlined,
  ContactsOutlined,
  PartitionOutlined,
  TeamOutlined,
  LogoutOutlined,
  UserOutlined,
  TableOutlined,
  DatabaseOutlined,
  SmileOutlined,
  CarryOutOutlined,
  ProjectOutlined,
} from '@ant-design/icons';

export default function SideMenu({ setCollapsed }) {
  const navigate = useNavigate();
  const { pathname } = useLocation();
  const { logout } = useContext(AuthContext);
  const globalStore = useGlobalStore();

  const mobile = globalStore.mobile.get();

  const [_, pageKey] = pathname.split('/');

  const items = useMemo(
    () => [
      {
        key: 'dashboard',
        icon: <AppstoreOutlined />,
        label: 'Стартовая панель',
      },
      {
        key: 'projects-static',
        icon: <PartitionOutlined />,
        label: 'Проекты(static)',
      },
      {
        key: 'customers-static',
        icon: <ContactsOutlined />,
        label: 'Заказчики(static)',
      },
      {
        key: 'teams-static',
        icon: <TeamOutlined />,
        label: 'Команды(static)',
      },
      // {
      //   key: 'projects',
      //   icon: <PartitionOutlined />,
      //   label: 'Проекты',
      // },
      // {
      //   key: 'customers',
      //   icon: <ContactsOutlined />,
      //   label: 'Заказчики',
      // },
      {
        key: 'superTable',
        icon: <TableOutlined />,
        label: 'Просто таблица',
      },

      {
        key: 'departments',
        icon: <DatabaseOutlined />,
        label: 'Отделы',
      },

      {
        key: 'teams',
        icon: <TeamOutlined />,
        label: 'Команды',
      },
      {
        key: 'employees',
        icon: <UserOutlined />,
        label: 'Сотрудники',
      },
      {
        key: 'projects',
        icon: <ProjectOutlined />,
        label: 'Проекты',
      },

      {
        key: 'tasks',
        icon: <CarryOutOutlined />,
        label: 'Задачи',
      },

      {
        key: 'customers',
        icon: <SmileOutlined />,
        label: 'Клиенты',
      },
    ],
    []
  );

  return (
    <div className="SideMenu h-full flex flex-col">
      <div className="h-8 bg-gray-800 mb-4 mx-4 mt-4 rounded" />
      <div className="flex-1 flex flex-col overflow-hidden">
        <div className="flex-1 overflow-y-auto">
          <Menu
            theme="dark"
            mode="inline"
            defaultSelectedKeys={[pageKey]}
            items={items}
            onClick={({ key }) => {
              if (mobile) setCollapsed(true);
              navigate(`/${key}`);
            }}
          />
        </div>

        <div className="shrink-0">
          <Menu
            theme="dark"
            mode="inline"
            selectable={false}
            items={[
              {
                key: 'logout',
                icon: <LogoutOutlined />,
                label: 'Выход',
                onClick: () => logout(),
              },
            ]}
          />
        </div>
      </div>
    </div>
  );
}

import React, { useMemo, useContext } from 'react';
import { AuthContext } from 'src/shared/Auth/AuthContext';
import { Menu } from 'antd';
import { useLocation, useNavigate } from 'react-router';
import { useGlobalStore } from 'src/stores/globalStore';

import AntIcon from 'src/shared/AntIcon';

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
        icon: <AntIcon name="AppstoreOutlined" />,
        label: 'Стартовая панель',
      },
      {
        key: 'projects-static',
        icon: <AntIcon name="ProjectOutlined" />,
        label: 'Проекты(static)',
      },
      {
        key: 'customers-static',
        icon: <AntIcon name="ContactsOutlined" />,
        label: 'Заказчики(static)',
      },
      {
        key: 'teams-static',
        icon: <AntIcon name="TeamOutlined" />,
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
        icon: <AntIcon name="TableOutlined" />,
        label: 'Просто таблица',
      },

      {
        key: 'department',
        icon: <AntIcon name="DatabaseOutlined" />,
        label: 'Отделы',
      },

      {
        key: 'team',
        icon: <AntIcon name="TeamOutlined" />,
        label: 'Команды',
      },
      {
        key: 'employee',
        icon: <AntIcon name="UserOutlined" />,
        label: 'Сотрудники',
      },
      {
        key: 'project',
        icon: <AntIcon name="ProjectOutlined" />,
        label: 'Проекты',
      },

      {
        key: 'task',
        icon: <AntIcon name="CarryOutOutlined" />,
        label: 'Задачи',
      },

      {
        key: 'customer',
        icon: <AntIcon name="SmileOutlined" />,
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
                icon: <AntIcon name="LogoutOutlined" />,
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

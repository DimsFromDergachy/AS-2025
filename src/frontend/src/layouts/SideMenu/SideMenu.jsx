import React, { useMemo, useContext } from 'react';
import { AuthContext } from 'src/shared/Auth/AuthContext';
import { Menu, Switch } from 'antd';
import { useLocation, useNavigate } from 'react-router';
import { useGlobalStore } from 'src/stores/globalStore';

import AntIcon from 'src/shared/AntIcon';
import {
  MoonFilled,
  SunFilled,
} from '@ant-design/icons';

export default function SideMenu({ setCollapsed }) {
  const navigate = useNavigate();
  const globalStore = useGlobalStore();
  const { pathname } = useLocation();
  const { logout } = useContext(AuthContext);

  const mobile = globalStore.mobile.get();
  const darkMode = globalStore.darkMode.get();
  const menuItems = globalStore.menuItems.get({ noproxy: true });

  const [_, pageKey, subPageKey] = pathname.split('/');

  const items = useMemo(() => {
    const res = menuItems.map(item => ({
      label: item.label,
      key: item.pageKey || item.modelKey,
      icon: <AntIcon name={item.icon} />,
    }));
    res.push({
      label: 'Алгоритмы',
      key: 'algos',
      icon: <AntIcon name="ExperimentOutlined" />,
      children: [
        {
          label: 'Рюкзак',
          key: 'backpack',
          icon: <AntIcon name="ShoppingOutlined" />,
        },
        {
          label: 'Планировщик',
          key: 'scheduler',
          icon: <AntIcon name="ScheduleOutlined" />,
        },
      ],
    });
    return res;
  }, [menuItems]);

  const toggleTheme = () => {
    globalStore.darkMode.set(prev => !prev);
  };

  return (
    <div className="SideMenu h-full flex flex-col">
      <div className="h-8 bg-stone-900 mb-4 mx-4 mt-4 rounded" />
      <div className="flex-1 flex flex-col overflow-hidden">
        <div className="flex-1 overflow-y-auto">
          <Menu
            theme={darkMode ? 'light' : 'dark'}
            mode="inline"
            defaultSelectedKeys={[subPageKey, pageKey]}
            items={items}
            onClick={({ keyPath }) => {
              const path = keyPath.reverse().join('/');
              if (mobile) setCollapsed(true);
              navigate(`/${path}`);
            }}
          />
        </div>

        <div className="shrink-0">
          <Switch
            className="ml-1 w-[54px]"
            checked={darkMode}
            onChange={toggleTheme}
            checkedChildren={<SunFilled />}
            unCheckedChildren={<MoonFilled />}
          />
          <Menu
            theme={darkMode ? 'light' : 'dark'}
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

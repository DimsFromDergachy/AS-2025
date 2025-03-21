import React, { useState, useContext, useEffect } from 'react';
import { HubConnectionBuilder } from '@microsoft/signalr';
import { AuthContext } from 'src/shared/Auth/AuthContext';
import { App } from 'antd';
import { Suspense } from 'react';
import { Outlet, useLocation, useNavigate } from 'react-router';
import { Layout, theme } from 'antd';
import { apiClient } from 'src/api/client';
import { useGlobalStore } from 'src/stores/globalStore';
import SideMenu from 'src/layouts/SideMenu/SideMenu';
import Loader from 'src/widgets/Loader';

const { Header, Content, Sider } = Layout;

const connectToHub = () => {
  const connection = new HubConnectionBuilder()
    .withUrl('/api-events', {
      skipNegotiation: true
    })
    .withAutomaticReconnect()
    .build();

  connection.on('ApiCallEvent', apiEvent => {
    console.log(
      `API Event: ${apiEvent.method} ${apiEvent.path} at ${apiEvent.timestamp}`
    );
  });

  connection.start().catch(err => console.error(err));
};

export default function RootLayout() {
  const navigate = useNavigate();
  const { pathname } = useLocation();
  const { token } = theme.useToken();
  const { authorized } = useContext(AuthContext);
  const { message } = App.useApp();
  const [collapsed, setCollapsed] = useState(true);

  const globalStore = useGlobalStore();
  const loading = globalStore.loading.get();
  const error = globalStore.serverError.get({ noproxy: true });
  const mobile = globalStore.mobile.get();
  const enums = globalStore.enums.get();

  const [_, pageKey] = pathname.split('/');

  const { colorBgContainer, borderRadiusLG } = token;

  const handleBreakpoint = broken => {
    globalStore.mobile.set(broken);
    if (broken) setCollapsed(true);
  };

  const handleCollapse = (collapsed, type) => {
    if (type === 'clickTrigger') {
      setCollapsed(collapsed);
    }
  };

  useEffect(() => {
    connectToHub();
  }, []);

  useEffect(() => {
    if (!authorized) navigate('/login');
    else {
      if (!pageKey) navigate('/dashboard');
      const taggedEnumsReq = apiClient('/utils/tagged-enums');
      const referenceEnumsReq = apiClient('/utils/reference-enums');
      Promise.all([taggedEnumsReq, referenceEnumsReq]).then(
        ([taggedEnums, referenceEnums]) => {
          globalStore.enums.set({ taggedEnums, referenceEnums });
        }
      );
    }
  }, [navigate, authorized]);

  useEffect(() => {
    if (error.message) {
      message.error(error.message);
      globalStore.serverError.set({ message: '' });
    }
  }, [message, error]);

  return (
    authorized && (
      <Suspense fallback={<Loader />}>
        <Layout hasSider className="min-h-screen max-h-dvh">
          <Sider
            collapsible
            collapsed={collapsed}
            collapsedWidth={mobile ? 0 : 60}
            breakpoint="md"
            onBreakpoint={handleBreakpoint}
            onCollapse={handleCollapse}
            className={`${mobile ? 'absolute z-100' : 'sticky'} h-screen top-0 left-0 shadow-lg transition-all`}
            zeroWidthTriggerStyle={{ top: '12px' }}
          >
            <SideMenu setCollapsed={setCollapsed} />
          </Sider>

          <Layout className="bg-gray-50">
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
                className="h-full overflow-auto py-4 px-2"
                style={{
                  background: colorBgContainer,
                  borderRadius: borderRadiusLG,
                }}
              >
                {enums && <Outlet />}
              </div>
            </Content>

            {loading && <Loader />}
          </Layout>
        </Layout>
      </Suspense>
    )
  );
}

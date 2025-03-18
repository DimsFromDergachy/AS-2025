import React, { lazy } from 'react';
import { Spin } from 'antd';

const iconComponents = {
  AppstoreOutlined: lazy(() => import('@ant-design/icons/AppstoreOutlined')),
  ContactsOutlined: lazy(() => import('@ant-design/icons/ContactsOutlined')),
  PartitionOutlined: lazy(() => import('@ant-design/icons/PartitionOutlined')),
  TeamOutlined: lazy(() => import('@ant-design/icons/TeamOutlined')),
  LogoutOutlined: lazy(() => import('@ant-design/icons/LogoutOutlined')),
  UserOutlined: lazy(() => import('@ant-design/icons/UserOutlined')),
  TableOutlined: lazy(() => import('@ant-design/icons/TableOutlined')),
  DatabaseOutlined: lazy(() => import('@ant-design/icons/DatabaseOutlined')),
  SmileOutlined: lazy(() => import('@ant-design/icons/SmileOutlined')),
  CarryOutOutlined: lazy(() => import('@ant-design/icons/CarryOutOutlined')),
  ProjectOutlined: lazy(() => import('@ant-design/icons/ProjectOutlined')),

  EyeOutlined: lazy(() => import('@ant-design/icons/EyeOutlined')),
  EditOutlined: lazy(() => import('@ant-design/icons/EditOutlined')),
  DeleteOutlined: lazy(() => import('@ant-design/icons/DeleteOutlined')),
};

const AntIcon = ({ name, ...props }) => {
  const IconComponent = iconComponents[name];
  
  return IconComponent ? (
    <React.Suspense fallback={<Spin size="small" />}>
      <IconComponent {...props} />
    </React.Suspense>
  ) : null;
};

export default AntIcon;
import { theme } from 'antd';
import { colors } from ".."

const { dark } = colors;

export const darkTheme = {
  algorithm: theme.darkAlgorithm,
  borderRadius: 1,
  colorBgBase: '#222629',
  colorPrimary: '#26547c',
  colorInfo: '#26547c',
  colorSuccess: '#2bc199',
  colorError: '#de2629',
  colorWarning: '#ec8063',
  wireframe: false,
  colorPrimaryBg: '#26547c',
  colorBgContainer: '#222629',
  components: {
    Layout: {
      // headerBg: dark['secondary-1'],
      siderBg: '#0d0d0d',
      triggerBg: "#26547c",
    },
    Menu: {
      iconSize: 18,
      collapsedIconSize: 24,

      itemActiveBg: "#fff",
      // itemColor: dark['secondary-2'],
      itemBg: "#0d0d0d",
      // itemHoverBg: dark['secondary-2'],
      itemHoverColor: "#fff",
      itemSelectedBg: "#26547c",
      itemSelectedColor: "#fff",
      colorText: "#918f8f",
      subMenuItemSelectedColor: "#26547c"
    },
    Input: {
      controlHeight: 38,
    },
    Select: {
      controlHeight: 38,
    },
    Button: {
      controlHeight: 38,
      fontWeight: 500,
      colorPrimary: "#26547c",
      algorithm: false
    },
    Typography: {
      // colorLink: dark['link-1'],
    },
    Form: {
      // labelRequiredMarkColor: dark['primary-2'],
    },
    Alert: {
      // colorInfoBg: '#e5f0ff',
    },
  },
};

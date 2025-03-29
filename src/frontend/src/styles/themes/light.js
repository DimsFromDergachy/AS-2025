import { colors } from ".."

const { light } = colors;

export const lightTheme = {
  components: {
    Layout: {
      // headerBg: light['secondary-1'],
      // siderBg: '#fff',
      // triggerBg: light['primary-2'],
    },
    Menu: {
      iconSize: 18,
      collapsedIconSize: 24,
      itemActiveBg: light['secondary-2'],
      itemColor: light['link-1'],
      itemHoverBg: light['secondary-2'],
      itemHoverColor: light['link-1'],
      itemSelectedBg: light['secondary-2'],
      itemSelectedColor: light['primary-2'],
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
    },
    Typography: {
      colorLink: light['link-1'],
    },
    Form: {
      labelRequiredMarkColor: light['primary-2'],
    },
    Alert: {
      colorInfoBg: '#e5f0ff',
    },
  },
}

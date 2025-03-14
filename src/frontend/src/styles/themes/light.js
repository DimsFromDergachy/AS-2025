import { colors } from ".."

export const lightTheme = {
  components: {
    Layout: {
      // headerBg: colors['secondary-1'],
      // siderBg: '#fff',
      // triggerBg: colors['primary-2'],
    },
    Menu: {
      iconSize: 18,
      collapsedIconSize: 24,
      itemActiveBg: colors['secondary-2'],
      itemColor: colors['link-1'],
      itemHoverBg: colors['secondary-2'],
      itemHoverColor: colors['link-1'],
      itemSelectedBg: colors['secondary-2'],
      itemSelectedColor: colors['primary-2'],
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
      colorLink: colors['link-1'],
    },
    Form: {
      labelRequiredMarkColor: colors['primary-2'],
    },
    Alert: {
      colorInfoBg: '#e5f0ff',
    },
  },
}

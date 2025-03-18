import { hookstate, useHookstate } from '@hookstate/core';
import { devtools } from '@hookstate/devtools';

export const globalStore = hookstate({
  loading: false,
  serverError: {
    message: '',
  },
  enums: null,
  mobile: false,
  menuItems: [],
  menuPermissions: {},
  rolePermissions: {},
  modal: {
    open: false,
  }
}, devtools({ key: 'globalStore' }));

export function useGlobalStore() {
  return useHookstate(globalStore)
};

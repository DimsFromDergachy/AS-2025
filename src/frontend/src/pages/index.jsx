import { lazy } from 'react';
import RootLayout from './RootLayout';

export const routes = [
  {
    path: 'login',
    Component: lazy(() => import('./Login')),
  },
  {
    path: '/',
    // ErrorElement: ErrorPage,
    Component: RootLayout,
    children: [
      {
        path: 'dashboard',
        Component: lazy(() => import('./Dashboard')),
        handle: {
          crumb: () => ({
            page: 'dashboard',
            title: 'Dashboard',
          }),
        },
      },
      {
        path: 'projects-static',
        Component: lazy(() => import('./static/Projects')),
        handle: {
          crumb: () => ({
            page: 'projects-static',
            title: 'Projects',
          }),
        },
      },
      {
        path: 'customers-static',
        Component: lazy(() => import('./static/Customers')),
        handle: {
          crumb: () => ({
            page: 'customers-static',
            title: 'Customers',
          }),
        },
      },
      {
        path: 'teams-static',
        Component: lazy(() => import('./static/Teams')),
        handle: {
          crumb: () => ({
            page: 'teams-static',
            title: 'Teams',
          }),
        },
      },



      // {
      //   path: 'projects',
      //   Component: lazy(() => import('./Projects')),
      //   handle: {
      //     crumb: () => ({
      //       page: 'projects',
      //       title: 'Projects',
      //     }),
      //   },
      //   //   children: [
      //   //     {
      //   //       path: ':projectId',
      //   //       Component: lazy(() => import('./Project')),
      //   //       handle: {
      //   //         crumb: () => ({
      //   //           title: 'projectId',
      //   //         }),
      //   //       },
      //   //     },
      //   //   ],
      // },
      // {
      //   path: 'customers',
      //   Component: lazy(() => import('./Customers')),
      //   handle: {
      //     crumb: () => ({
      //       page: 'customers',
      //       title: 'Customers',
      //     }),
      //   },
      //   // children: [
      //   //   {
      //   //     path: ':customerId',
      //   //     Component: lazy(() => import('./Customer')),
      //   //     handle: {
      //   //       crumb: () => ({
      //   //         title: 'customerId',
      //   //       }),
      //   //     },
      //   //   },
      //   // ],
      // },
      {
        path: 'superTable',
        Component: lazy(() => import('./SuperTable')),
      },    
      {
        path: 'departments',
        Component: lazy(() => import('./Departments')),
        handle: {
          crumb: () => ({
            page: 'departments',
            title: 'Departments',
          }),
        },
        // children: [
        //   {
        //     path: ':developerId',
        //     Component: lazy(() => import('./Developer')),
        //     handle: {
        //       crumb: () => ({
        //         title: 'developerId',
        //       }),
        //     },
        //   },
        // ],
      },

      {
        path: 'teams',
        Component: lazy(() => import('./Teams')),
        handle: {
          crumb: () => ({
            page: 'teams',
            title: 'Teams',
          }),
        },
        // children: [
        //   {
        //     path: ':developerId',
        //     Component: lazy(() => import('./Developer')),
        //     handle: {
        //       crumb: () => ({
        //         title: 'developerId',
        //       }),
        //     },
        //   },
        // ],
      },
      {
        path: 'employees',
        Component: lazy(() => import('./Employees')),
        handle: {
          crumb: () => ({
            page: 'employees',
            title: 'Employees',
          }),
        },
        // children: [
        //   {
        //     path: ':developerId',
        //     Component: lazy(() => import('./Developer')),
        //     handle: {
        //       crumb: () => ({
        //         title: 'developerId',
        //       }),
        //     },
        //   },
        // ],
      },



    ],
  },
];

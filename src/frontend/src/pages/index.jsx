import { lazy } from 'react';
import RootLayout from './RootLayout';

const tabularPages = [
  'department',
  'team',
  'employee',
  'project',
  'task',
  'customer',
];

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
      {
        path: 'superTable',
        Component: lazy(() => import('./SuperTable')),
      },    

      ...tabularPages.map((page) => ({
        path: page,
        Component: lazy(() => 
          import('./TablePage').then(
            // eslint-disable-next-line no-unused-vars
            ({ default: TablePageComponent }) => ({
              default: (props) => (
                <TablePageComponent {...props} tableKey={page} />
              )
            })
          )
        ),
        handle: {
          crumb: () => ({
            page,
            title: `${page.charAt(0).toUpperCase() + page.slice(1)}`,
          }),
        },
      })),
    ],
  },
];

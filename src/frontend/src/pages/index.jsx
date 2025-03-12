import { lazy } from 'react'
import RootLayout from './RootLayout'

export const routes = [
  {
    path: 'login',
    Component: lazy(() => import('./Login')),
  },
  {
    path: 'register',
    Component: lazy(() => import('./Register')),
  },
  {
    path: "/",
    // ErrorElement: ErrorPage,
    Component: RootLayout,
    children: [
      {
        path: 'dashboard',
        Component: lazy(() => import('./Dashboard')),
      },
      {
        path: 'projects',
        Component: lazy(() => import('./Projects')),
        handle: {
          crumb: () => ({
            title: 'Projects',
          }),
        },
      //   children: [
      //     {
      //       path: ':projectId',
      //       Component: lazy(() => import('./Project')),
      //       handle: {
      //         crumb: () => ({
      //           title: 'projectId',
      //         }),
      //       },
      //     },
      //   ],
      },
      {
        path: 'customers',
        Component: lazy(() => import('./Customers')),
        handle: {
          crumb: () => ({
            title: 'Customers',
          }),
        },
        // children: [
        //   {
        //     path: ':customerId',
        //     Component: lazy(() => import('./Customer')),
        //     handle: {
        //       crumb: () => ({
        //         title: 'customerId',
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
      }
    ],
  },
]

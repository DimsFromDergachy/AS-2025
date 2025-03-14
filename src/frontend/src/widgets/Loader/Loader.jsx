import React from 'react'
import { Spin } from 'antd'
import { LoadingOutlined } from '@ant-design/icons'

export default function Loader() {
  return (
    <div className='fixed inset-0 z-[1000] flex justify-center items-center'>
      <Spin indicator={<LoadingOutlined style={{ fontSize: 48 }} spin />} />
    </div>  )
}

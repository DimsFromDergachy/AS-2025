import React, { useState } from 'react';
import { Form, InputNumber, Upload, Button, message as antMessage, Input, Image } from 'antd';
import { UploadOutlined } from '@ant-design/icons';
import { apiClient } from 'src/api/client';
import { downloadFile } from 'src/helpers/functions';

const FileUploader = () => {
  const [fileList, setFileList] = useState([]);
  const [uploading, setUploading] = useState(false);
  const [imageUrl, setImageUrl] = useState(null);

  const uploadProps = {
    beforeUpload: file => {
      setFileList([file]);
      return false;
    },
    onRemove: () => {
      setFileList([]);
    },
    fileList,
  };

  const handleUpload = async (values) => {
    if (fileList.length === 0) {
      antMessage.error('Выберите файл для загрузки');
      return;
    }

    const { x, y, text } = values;
    const formData = new FormData();
    formData.append('x', x);
    formData.append('y', y);
    formData.append('text', text);
    formData.append('file', fileList[0]);

    try {
      setUploading(true);
      const response = await apiClient.post('/image/edit', formData, {
        headers: { 'Content-Type': 'multipart/form-data' },
      });
      antMessage.success('Файл успешно загружен');
      console.log('Response:', response);
      setFileList([]);
      const modifiedFile = await apiClient.get(`/minio/get/${response.modifiedFileId}`, {responseType: 'arraybuffer', withHeaders: true});

      const modifiedFileRes = await apiClient.get(`/minio/get/${response.modifiedFileId}`, { responseType: 'blob' });
      const blob = new Blob([modifiedFileRes], { type: 'image/png' });
      const url = URL.createObjectURL(blob);
      setImageUrl(url);
      downloadFile(modifiedFile, 'result');
    } catch (error) {
      antMessage.error(`Ошибка загрузки: ${error.message}`);
      console.error('Upload error:', error);
    } finally {
      setUploading(false);
    }
  };


  return (
    <>
      <Form onFinish={handleUpload} layout="vertical">
        <Form.Item
          label="x"
          name="x"
          rules={[{ required: true, message: 'Введите значение x' }]}
        >
          <InputNumber style={{ width: '100%' }} />
        </Form.Item>
        <Form.Item
          label="y"
          name="y"
          rules={[{ required: true, message: 'Введите значение y' }]}
        >
          <InputNumber style={{ width: '100%' }} />
        </Form.Item>
        <Form.Item
          label="Текст"
          name="text"
          rules={[{ required: true, message: 'Введите текст' }]}
        >
          <Input style={{ width: '100%' }} />
        </Form.Item>
  
        <Form.Item label="Выберите файл">
          <Upload {...uploadProps}>
            <Button icon={<UploadOutlined />}>Выбрать файл</Button>
          </Upload>
        </Form.Item>
        <Form.Item>
          <Button type="primary" htmlType="submit" loading={uploading}>
            {uploading ? 'Загрузка...' : 'Загрузить'}
          </Button>
        </Form.Item>
      </Form>
      {imageUrl && (
        <div style={{ marginTop: 24 }}>
          <h3>Результат:</h3>
          <Image
            src={imageUrl}
            width={200}
            alt="Результат обработки"
          />
        </div>
      )}
    </>
  );
};

export default FileUploader;

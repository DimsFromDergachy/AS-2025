import React, { useState, useEffect } from 'react';
import { useNavigate } from "react-router";
import { Form, Input, Button, Checkbox, Layout, Row, Col, App } from 'antd';
import { UserOutlined, LockOutlined } from '@ant-design/icons';

const { Content } = Layout;

const LoginPage = () => {
  const navigate = useNavigate();
  const { message, notification } = App.useApp();
  const [loading, setLoading] = useState(false);
  const [disableSubmit, setDisableSubmit] = useState(true);
  const [form] = Form.useForm();
  const values = Form.useWatch([], form);
  
  const onFinish = async (values) => {
    setLoading(true);
    try {
      fetch('/api/utils/app-state');
      await new Promise(resolve => setTimeout(resolve, 1000));
      // message.success('Вход выполнен успешно!');
      // notification.success({ message: 'Вход выполнен успешно!' });
      navigate('/dashboard');
    } catch (error) {
      message.error('Ошибка авторизации');
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    form
      .validateFields({ validateOnly: true })
      .then(() => setDisableSubmit(false))
      .catch(() => setDisableSubmit(true));
  }, [form, values]);

  return (
    <Layout className="min-h-screen">
      <Content>
        <Row 
          justify="center" 
          align="middle" 
          className="min-h-screen"
        >
          <Col 
            xs={22} 
            sm={16} 
            md={12} 
            lg={8} 
            xl={6}
            className="overflow-hidden"  // Обеспечивает предотвращение переполнения
          >
            <div className="bg-white rounded-lg shadow-lg p-6">
              <h2 className="text-center text-2xl mb-6 font-semibold">
                Вход
              </h2>
              <Form
                form={form}
                name="login_form"
                initialValues={{ remember: true }}
                onFinish={onFinish}
                layout="vertical"
              >
                <Form.Item
                  name="username"
                  rules={[{ required: true, message: 'Пожалуйста, введите логин!' }]}
                >
                  <Input 
                    allowClear
                    prefix={<UserOutlined />} 
                    placeholder="Логин" 
                    size="large"
                    // className="mb-4"
                  />
                </Form.Item>

                <Form.Item
                  name="password"
                  rules={[{ 
                    required: true, 
                    message: 'Пожалуйста, введите пароль!' 
                  }]}
                >
                  <Input.Password
                    allowClear
                    prefix={<LockOutlined />}
                    placeholder="Пароль"
                    size="large"
                    // className="mb-6"
                  />
                </Form.Item>

                <Form.Item 
                  name="remember" 
                  valuePropName="checked"
                  // className="mb-6"
                >
                  <Checkbox>Запомнить меня</Checkbox>
                </Form.Item>

                {/* <Form.Item > */}
                  <Button 
                    type="primary" 
                    disabled={disableSubmit}
                    htmlType="submit" 
                    loading={loading}
                    block
                    size="large"
                    // className='mb-4'
                  >
                    Войти
                  </Button>
                {/* </Form.Item> */}

                <div className="text-center space-x-2">
                  {/* <Button type="link" className="p-0">
                    Забыли пароль?
                  </Button>
                  <span className="text-gray-400">|</span> */}
                  <Button type="link" onClick={() => {navigate('/register')}}>
                    Регистрация
                  </Button>
                </div>
              </Form>
            </div>
          </Col>
        </Row>
      </Content>
    </Layout>
  );};

export default LoginPage;
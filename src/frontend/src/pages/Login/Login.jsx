import React, { useState, useEffect } from 'react';
import { apiClient } from 'src/api/client';
import { useNavigate } from 'react-router';
import { Form, Input, Button, Layout, Row, Col, App } from 'antd';
import { MailOutlined, LockOutlined } from '@ant-design/icons';
import { AuthContext } from 'src/shared/Auth/AuthContext';

const { Content } = Layout;

const LoginPage = () => {
  const navigate = useNavigate();
  const { message } = App.useApp();
  const { login, authorized } = React.useContext(AuthContext);
  const [loading, setLoading] = useState(false);
  const [registerMode, setRegisterMode] = useState(false);
  const [disableSubmit, setDisableSubmit] = useState(true);
  const [serverErrors, setServerErrors] = useState({});
  const [form] = Form.useForm();
  const values = Form.useWatch([], form);

  const passwordRules = [
    { required: true, message: 'Введите пароль' },
    { min: 8, message: 'Минимум 8 символов' },
    { pattern: /^(?=.*[A-Z])(?=.*\d).+$/, message: 'Заглавная буква и цифра' },
  ];

  const handleSubmit = async values => {
    setLoading(true);
    try {
      const endpoint = registerMode ? 'identity/register' : 'identity/login';
      const data = await apiClient.post(endpoint, values);
  
      if (registerMode) {
        message.success('Регистрация успешна!');
        setRegisterMode(false);
        // form.resetFields();
      } else {
        login(data.accessToken);
        navigate('/dashboard');
      }
    } catch (error) {
      message.error(error.message);
    } finally {
      setLoading(false);
    }
  };
  
  useEffect(() => {
    if (authorized) navigate('/dashboard');
  }, [authorized, navigate]);

  useEffect(() => {
    form
      .validateFields({ validateOnly: true })
      .then(() => setDisableSubmit(false))
      .catch(() => setDisableSubmit(true));
  }, [form, values]);

  return (
    !authorized && (
      <Layout className="min-h-screen bg-gray-100">
        <Content>
          <Row justify="center" align="middle" className="min-h-screen">
            <Col xs={24} sm={20} md={16} lg={12} xl={8}>
              <div className="bg-white rounded-xl shadow-lg p-8 mx-4">
                <h1 className="text-3xl font-bold text-center mb-8">
                  {registerMode ? 'Создать аккаунт' : 'Вход'}
                </h1>

                <Form
                  form={form}
                  onFinish={handleSubmit}
                  layout="vertical"
                  autoComplete="off"
                >
                  <Form.Item
                    name="email"
                    rules={[
                      { required: true, message: 'Введите email' },
                      { type: 'email', message: 'Неверный формат email' },
                    ]}
                  >
                    <Input
                      prefix={<MailOutlined />}
                      placeholder="Email"
                      size="large"
                      status={serverErrors.InvalidEmail ? 'error' : ''}
                      onChange={() => setServerErrors({})}
                      autoFocus
                    />
                  </Form.Item>

                  <Form.Item name="password" rules={passwordRules}>
                    <Input.Password
                      prefix={<LockOutlined />}
                      placeholder="Пароль"
                      size="large"
                      onChange={() => setServerErrors({})}
                    />
                  </Form.Item>

                  {registerMode && (
                    <Form.Item
                      name="confirmPassword"
                      dependencies={['password']}
                      rules={[
                        { required: true, message: 'Подтвердите пароль' },
                        ({ getFieldValue }) => ({
                          validator(_, value) {
                            if (!value || getFieldValue('password') === value) {
                              return Promise.resolve();
                            }
                            return Promise.reject(
                              new Error('Пароли не совпадают')
                            );
                          },
                        }),
                      ]}
                    >
                      <Input.Password
                        prefix={<LockOutlined />}
                        placeholder="Подтвердите пароль"
                        size="large"
                        onChange={() => setServerErrors({})}
                      />
                    </Form.Item>
                  )}

                  <Button
                    type="primary"
                    htmlType="submit"
                    block
                    size="large"
                    loading={loading}
                    disabled={disableSubmit}
                    className="mt-6 h-12 text-lg"
                  >
                    {registerMode ? 'Зарегистрироваться' : 'Войти'}
                  </Button>

                  <div className="text-center mt-2">
                    <Button
                      type="link"
                      onClick={() => setRegisterMode(!registerMode)}
                      className="text-gray-600"
                    >
                      {registerMode
                        ? 'Уже есть аккаунт? Войти'
                        : 'Создать новый аккаунт'}
                    </Button>
                  </div>
                </Form>
              </div>
            </Col>
          </Row>
        </Content>
      </Layout>
    )
  );
};

export default LoginPage;
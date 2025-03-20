import React, { useEffect, useMemo, useState } from 'react';
import { Input, Button, App, Flex, Descriptions, Drawer } from 'antd';
import AntTable from 'src/shared/AntTable';
import AddEditDrawer from 'src/shared/AddEditDrawer/AddEditDrawer';
import { SearchOutlined, PlusOutlined } from '@ant-design/icons';

import { useGlobalStore } from 'src/stores/globalStore';

import { apiClient } from 'src/api/client';
import { filterByFields } from 'src/helpers/functions';

const TablePage = ({ tableKey }) => {
  const { modal } = App.useApp();
  const [schema, setSchema] = useState({});
  const [formSchema, setFormSchema] = useState(null);
  const [showForm, setShowForm] = useState(false);
  const [editMode, setEditMode] = useState(false);
  const [editItem, setEditItem] = useState(null);
  const [data, setData] = useState([]);
  const [filteredData, setFilteredData] = useState([]);
  const [searchText, setSearchText] = useState('');
  const [detailedView, setDetailedView] = useState({});

  const store = useGlobalStore();
  const { referenceEnums = {} } = store.enums.get({ noproxy: true }) || {};

  const {
    title = '',
    commonActions = [],
    columnActions = [],
    columns = [],
    fieldsToSearch = [],
  } = schema;

  const actionsColumn = useMemo(
    () => ({
      key: 'actions',
      displayType: 'Actions',
      title: 'Действия',
      visibilityType: 'Visible',
      availableActions: columnActions,
      actions: {
        View: {
          iconName: 'EyeOutlined',
          onClick: item => {
            setDetailedView(prev => ({
              visible: item.id === prev.data?.id ? !prev.visible : true,
              data: item,
            }));
          },
        },
        Edit: {
          iconName: 'EditOutlined',
          onClick: item => {
            setShowForm(true);
            setEditMode(true);
            setEditItem(item);
          },
        },
        Delete: {
          className: 'text-red-500 hover:text-red-700',
          iconName: 'DeleteOutlined',
          onClick: ({ id }) => {
            modal
              .confirm({
                title: 'Удаление',
                content: 'Вы действительно хотите удалить элемент?',
                okText: 'Да',
                cancelText: 'Нет',
                footer: (_, { OkBtn, CancelBtn }) => (
                  <>
                    <OkBtn />
                    <CancelBtn />
                  </>
                ),
              })
              .then(confirmed => {
                confirmed &&
                  apiClient.delete(`/${tableKey}/${id}`).then(() => {
                    setData(prev => prev.filter(item => item.id !== id));
                  });
              });
          },
        },
      },
    }),
    [tableKey, columnActions]
  );

  useEffect(() => {
    if (tableKey) {
      apiClient.get(`/${tableKey}/list`).then(({ items }) => {
        setData(items);
      });
      apiClient.get(`/${tableKey}/list/schema`).then(res => {
        if (res) {
          const [action] = res.columnActions || [];
          if (action && action !== 'None') {
            res.columns.push(actionsColumn);
          }
          const fieldsToSearch = res.columns.reduce(
            (acc, col) => (col.searchable ? [...acc, col.key] : acc),
            []
          );
          setSchema({ ...res, fieldsToSearch });
        }
      });
      apiClient.get(`/${tableKey}/create-schema`).then(formData => {
        const referenceFields = formData.inputs.filter(
          input => input.referenceName
        );
        if (referenceFields.length) {
          referenceFields.forEach(field => {
            if (referenceEnums[field.referenceName]) {
              field.options = referenceEnums[field.referenceName];
            } else {
              const referenceRequest = field.referenceRequest || '';
              const query = referenceRequest ? `?query=${referenceRequest}` : '';
              apiClient
                .get(`/${field.referenceName}/reference-list${query}`)
                .then(({ items }) => {
                  field.options = items;
                });
            }
          });
        }
        formData.inputs = formData.inputs.filter(
          input => input.visibilityType === 'Visible'
        );
        setFormSchema(formData);
      });
    }
  }, [tableKey]);

  useEffect(() => {
    setFilteredData(filterByFields(data, searchText, fieldsToSearch));
  }, [searchText, data, fieldsToSearch]);

  return (
    <div className="h-full flex flex-col">
      <div className="mb-6 flex flex-col sm:flex-row justify-between items-start sm:items-center gap-4">
        <h1 className="text-2xl font-bold">{title}</h1>
        <div className="flex gap-2 w-full sm:w-auto">
          {commonActions.includes('Search') && !!fieldsToSearch.length && (
            <Input.Search
              placeholder="Поиск"
              enterButton={<SearchOutlined />}
              onSearch={setSearchText}
              className="w-full sm:w-64"
            />
          )}
          {commonActions.includes('Create') && (
            <Button
              type="primary"
              icon={<PlusOutlined />}
              disabled={!formSchema}
              onClick={() => {
                setEditMode(false);
                setShowForm(true);
                setEditItem(null);
              }}
            >
              Добавить
            </Button>
          )}
        </div>
      </div>

      <Flex className="flex-1" vertical justify="space-between">
        <AntTable
          availableActions={columnActions}
          columns={columns}
          dataSource={filteredData}
          pagination={{
            pageSize: 20,
            showTotal: total => `Всего записей: ${total}`,
            hideOnSinglePage: true,
          }}
          scroll={{ x: true }}
          bordered
        />
        <Drawer
          width={600}
          open={detailedView.visible}
          onClose={() => setDetailedView(prev => ({ ...prev, visible: false }))}
          title="Подробная информация"
        >
          <Descriptions column={1} bordered>
            {Object.entries(detailedView.data || {}).map(([key, value]) => (
              <Descriptions.Item key={key} label={key}>
                {Array.isArray(value)
                  ? typeof value[0] === 'object'
                    ? JSON.stringify(value)
                    : value.join(', ')
                  : value}
              </Descriptions.Item>
            ))}
          </Descriptions>
        </Drawer>
      </Flex>
      {formSchema && (
        <AddEditDrawer
          open={showForm}
          schema={formSchema}
          setVisible={setShowForm}
          editMode={editMode}
          item={editItem}
          onSubmit={item => {
            apiClient.post(`/${tableKey}`, item).then(resItem => {
              setData(prev => [...prev, resItem]);
              setShowForm(false);
            });
          }}
        />
      )}
    </div>
  );
};

export default TablePage;

import React, { useEffect, useMemo, useRef, useState } from 'react';
import { Input, Button, Flex, Descriptions, Drawer } from 'antd';
import AntTable from 'src/widgets/AntTable';
import AddEditDrawer from 'src/widgets/AddEditDrawer/AddEditDrawer';
import {
  SearchOutlined,
  PlusOutlined,
  DownloadOutlined,
} from '@ant-design/icons';

import { useGlobalStore } from 'src/stores/globalStore';

import { apiClient } from 'src/api/client';
import { downloadFile, filterByFields } from 'src/helpers/functions';
import { useTableHeight } from 'src/hooks/useTableHeight';

const TablePage = ({ tableKey }) => {
  const [schema, setSchema] = useState({});
  const [formSchema, setFormSchema] = useState(null);
  const [formParams, setFormParams] = useState({
    visible: false,
    editMode: false,
    item: null,
  });
  const [data, setData] = useState(null);
  const [filteredData, setFilteredData] = useState(null);
  const [searchText, setSearchText] = useState('');
  const [detailedView, setDetailedView] = useState({});

  const [headerContainerRef, setHeaderContainerRef] = useState(null);
  const [tableContainerRef, setTableContainerRef] = useState(null);
  const tableHeight = useTableHeight(tableContainerRef);
  
  const headerHeight =
    headerContainerRef?.getBoundingClientRect().height || 0;

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
            setFormParams({ visible: true, editMode: true, item });
          },
        },
        Delete: {
          className: 'text-red-500 hover:text-red-700',
          iconName: 'DeleteOutlined',
          confirmable: true,
          confirmTitle: 'Удалить элемент?',
          onClick: ({ id }) => {
            apiClient.delete(`/${tableKey}/${id}`).then(() => {
              setData(prev => prev.filter(item => item.id !== id));
            });
          },
        },
      },
    }),
    [tableKey, columnActions]
  );

  const onSubmit = (item, isEdit) => {
    const request = isEdit ? apiClient.put : apiClient.post;
    const url = isEdit ? `/${tableKey}/${item.id}` : `/${tableKey}`;
    request(url, item).then(resItem => {
      if (isEdit) {
        setData(prev =>
          prev.map(el => (el.id === resItem.id ? resItem : el))
        );
      } else {
        setData(prev => [...prev, resItem]);
      }
      setFormParams(prev => ({ ...prev, visible: false }));
    });
  };

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
              const query = referenceRequest
                ? `?query=${referenceRequest}`
                : '';
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
    <div className="h-full overflow-hidden">
      <div
        ref={setHeaderContainerRef}
        className="mb-6 flex flex-col sm:flex-row justify-between items-start sm:items-center gap-4"
      >
        <Flex>
          <h1 className="text-2xl font-bold">{title}</h1>
          {commonActions.includes('ExportXlsx') && (
            <Button
              className="ml-6"
              icon={<DownloadOutlined className="text-lg" />}
              onClick={async () => {
                apiClient
                  .get(`/${tableKey}/list/export/Excel`, {
                    responseType: 'arraybuffer',
                    withHeaders: true,
                  })
                  .then(response => {
                    downloadFile(response, `${tableKey}.xlsx`);
                  });
              }}
            >
              Экспорт в Excel
            </Button>
          )}
        </Flex>
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
                setFormParams({ visible: true, editMode: false });
              }}
            >
              Добавить
            </Button>
          )}
        </div>
      </div>

      <div
        ref={setTableContainerRef}
        style={{ height: `calc(100% - ${headerHeight + 10}px)` }}
      >
        <AntTable
          availableActions={columnActions}
          columns={columns}
          dataSource={filteredData}
          pagination={{
            pageSize: 20,
            showTotal: total => `Всего записей: ${total}`,
            // hideOnSinglePage: true,
          }}
          scroll={{ x: 1800, y: tableHeight }}
          bordered
        />
      </div>
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
      {formSchema && (
        <AddEditDrawer
          open={formParams.visible}
          schema={formSchema}
          editMode={formParams.editMode}
          item={formParams.item}
          onClose={() => setFormParams(prev => ({ ...prev, visible: false }))}
          onSubmit={onSubmit}
        />
      )}
    </div>
  );
};

export default TablePage;

import { useState, useLayoutEffect } from 'react';
import { useDebouncedCallback } from 'use-debounce';

export const useTableHeight = (tableContainerRef, schema) => {
  const [tableHeight, setTableHeight] = useState();
  const resizeTable = useDebouncedCallback(
    () => {
      const node = tableContainerRef.current;
      if (!node) return;
      const { height } = node.getBoundingClientRect();
      setTableHeight(height - 98);
    },
    100,
    { trailing: true, maxWait: 100 }
  );

  useLayoutEffect(() => {
    resizeTable();
    window.addEventListener('resize', resizeTable);

    return () => {
      window.removeEventListener('resize', resizeTable);
    };
  }, [resizeTable, schema]);

  return tableHeight;
};

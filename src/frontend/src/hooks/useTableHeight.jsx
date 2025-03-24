import { useState, useEffect } from 'react';

export const useTableHeight = tableContainerRef => {
  const [tableHeight, setTableHeight] = useState(0);

  useEffect(() => {
    if (!tableContainerRef) return;

    const updateHeight = () => {
      const { height } = tableContainerRef.getBoundingClientRect();
      setTableHeight(height - 98);
    };

    updateHeight();

    const resizeObserver = new ResizeObserver(() => {
      updateHeight();
    });
    resizeObserver.observe(tableContainerRef);

    window.addEventListener('resize', updateHeight);

    return () => {
      resizeObserver.disconnect();
      window.removeEventListener('resize', updateHeight);
    };
  }, [tableContainerRef]);

  return tableHeight;
};

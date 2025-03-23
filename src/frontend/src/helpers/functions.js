export const filterByFields = (data, searchText, fields, options = {}) => {
  const {
    caseSensitive = false,    // Учитывать регистр
    exact = false,            // Точное совпадение
    trim = true,             // Удалять пробелы
    absolute = false         // Все поля должны содержать searchText
  } = options;

  if (!searchText || !Array.isArray(data) || !fields.length) return data;

  const searchValue = trim ? searchText.trim() : searchText;
  if (!searchValue) return data;

  const searchLower = caseSensitive ? searchValue : searchValue.toLowerCase();

  return data.filter(item => {

    const fieldMatches = fields.map(field => {
      const value = item[field];
      if (value == null) return false;

      const stringValue = String(value);
      const compareValue = caseSensitive ? stringValue : stringValue.toLowerCase();

      if (exact) {
        return compareValue === searchLower;
      }
      return compareValue.includes(searchLower);
    });

    return absolute ? fieldMatches.every(match => match) : fieldMatches.some(match => match);
  });
};

export const downloadFile = (response, defaultFileName) => {
  const contentType = response.headers['content-type'] || 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet';              
  const contentDisposition = response.headers['content-disposition'];
  let fileName = `${defaultFileName}.xlsx`;
  if (contentDisposition) {
    const fileNameMatch = contentDisposition.match(/filename\*?=(?:UTF-8'')?([^;\n]+)/);
    if (fileNameMatch && fileNameMatch.length > 1) {
      fileName = decodeURIComponent(fileNameMatch[1].trim());
    }
  }
  const blob = new Blob([response.data], { type: contentType });
  const url = window.URL.createObjectURL(blob);
  const a = document.createElement('a');
  a.href = url;
  a.download = fileName;
  document.body.appendChild(a);
  a.click();
  window.URL.revokeObjectURL(url);
  document.body.removeChild(a);
}
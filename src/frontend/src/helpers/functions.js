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
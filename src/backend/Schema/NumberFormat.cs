using AS_2025.Schema.Form;
using AS_2025.Schema.List;

namespace AS_2025.Schema;

// https://learn.javascript.ru/intl#chisla-intl-numberformat
public record NumberFormat
{
    /*
     * localeMatcher
     * Алгоритм подбора локали
     * values: lookup, best fit
     * default: best fit
     */
    /*
     * style
     * Стиль форматирования
     * values: decimal, percent, currency
     * default: decimal
     */
    /*
     * currency
     * Алфавитный код валюты
     * values: См.Список кодов валюты, например USD
     * default: 
     */
    /*
     * currencyDisplay
     * Показывать валюту в виде кода, локализованного символа или локализованного названия
     * values: code, symbol, name
     * default: symbol
     */
    /*
     * useGrouping
     * Разделять ли цифры на группы
     * values: true, false
     * default: true
     */
    public bool UseGrouping { get; init; } = true;
    /*
     * minimumIntegerDigits
     * Минимальное количество цифр целой части
     * values: от 1 до 21
     * default: 21
     */
    /*
     * minimumFractionDigits
     * Минимальное количество десятичных цифр
     * values: от 0 до 20
     * default: для чисел и процентов 0, для валюты зависит от кода.
     */
    /*
     * maximumFractionDigits
     * Максимальное количество десятичных цифр
     * values: от minimumFractionDigits до 20
     * default: для чисел max(minimumFractionDigits, 3), для процентов 0, для валюты зависит от кода.
     */
    public int MaximumFractionDigits { get; init; } = 20;
    /*
     * minimumSignificantDigits
     * Минимальное количество значимых цифр
     * values: от 1 до 21
     * default: 1
     */
    /*
     * maximumSignificantDigits
     * Максимальное количество значимых цифр
     * values: от minimumSignificantDigits до 21
     * default: 21
     */

    public double? Min { get; init; }

    public double? Max { get; init; }

    public double? Step { get; init; }

    public static NumberFormat? From(ListColumnSchemaAttribute attribute)
    {
        if (attribute.DisplayType is not (ListColumnDisplayType.Integer or ListColumnDisplayType.Decimal))
        {
            return null;
        }

        return new NumberFormat
        {
            UseGrouping = attribute.NumberFormatUseGrouping,
            MaximumFractionDigits = attribute.NumberFormatMaximumFractionDigits
        };
    }

    public static NumberFormat? From(FormInputSchemaAttribute attribute)
    {
        if (attribute.DisplayType is not (FormInputDisplayType.Integer or FormInputDisplayType.Decimal or FormInputDisplayType.Slider))
        {
            return null;
        }

        return new NumberFormat
        {
            UseGrouping = attribute.NumberFormatUseGrouping,
            MaximumFractionDigits = attribute.NumberFormatMaximumFractionDigits,
            Min = attribute.NumberMin,
            Max = attribute.NumberMax,
            Step = attribute.NumberStep
        };
    }
}
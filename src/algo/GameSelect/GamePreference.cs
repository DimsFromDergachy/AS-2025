namespace Algo.GameSelect;

public enum GamePreference
{
    Hated = -2,         // Категорически не готов, обидится и уйдет
    Undesirable = -1,   // Не очень хочет, но готов
    Neutral = 0,        // Неизвестная игра
    Pleasant = 1,       // Готов сыграть и получит немного удовольствия
    Favorite = 2        // Всегда готов и получит много удовольствия
}
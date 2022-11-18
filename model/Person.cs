namespace HelloApp.model;

/// <summary>
/// Человек.
/// </summary>
public class Person
{
    /// <summary>
    /// Возвращает или задает идентификационный номер.
    /// </summary>
    public string Id { get; set; } = String.Empty;

    /// <summary>
    /// Возвращает или задает имя.
    /// </summary>
    public string Name { get; set; } = String.Empty;

    /// <summary>
    /// Возвращает или задает возраст.
    /// </summary>
    public int Age { get; set; }
}
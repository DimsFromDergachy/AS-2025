namespace AS_2025.Menu;

public record MenuItem
{
    public string Label { get; }

    public string Icon { get; }

    public MenuItemType Type { get; }

    public string? ModelKey { get; }

    public string? RequestUrl { get; }

    public string? PageKey { get; }

    private MenuItem(string label, string icon, MenuItemType type, string? modelKey = null, string? requestUrl = null, string? pageKey = null)
    {
        Label = label;
        Icon = icon;
        Type = MenuItemType.ModelPage;
        ModelKey = modelKey;
        RequestUrl = requestUrl;
        PageKey = pageKey;
    }

    public static MenuItem ModelPage(string label, string icon, string modelKey)
    {
        return new MenuItem(label: label, icon: icon, type: MenuItemType.ModelPage, modelKey: modelKey);
    }

    public static MenuItem StaticServerPage(string label, string icon, string requestUrl)
    {
        return new MenuItem(label: label, icon: icon, type: MenuItemType.StaticServerPage, requestUrl: requestUrl);
    }

    public static MenuItem ClientPage(string label, string icon, string pageKey)
    {
        return new MenuItem(label, icon, type: MenuItemType.ClientPage, pageKey: pageKey);
    }
}
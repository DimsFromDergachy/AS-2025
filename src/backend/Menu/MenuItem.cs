namespace AS_2025.Menu;

public record MenuItem
{
    public string Label { get; }

    public string Icon { get; }

    public MenuItemType Type { get; }

    public string? ModelKey { get; }

    public string? RequestUrl { get; }

    public string? PageKey { get; }

    public string? Location { get; }

    private MenuItem(string label, string icon, MenuItemType type, string? modelKey = null, string? requestUrl = null, string? pageKey = null, string? location = null)
    {
        Label = label;
        Icon = icon;
        Type = MenuItemType.ModelPage;
        ModelKey = modelKey;
        RequestUrl = requestUrl;
        PageKey = pageKey;
        Location = location;
    }

    public static MenuItem ModelPage(string label, string icon, string modelKey, string location)
    {
        return new MenuItem(label: label, icon: icon, type: MenuItemType.ModelPage, modelKey: modelKey, location: location);
    }

    public static MenuItem StaticServerPage(string label, string icon, string requestUrl, string location)
    {
        return new MenuItem(label: label, icon: icon, type: MenuItemType.StaticServerPage, requestUrl: requestUrl, location: location);
    }

    public static MenuItem ClientPage(string label, string icon, string pageKey, string location)
    {
        return new MenuItem(label, icon, type: MenuItemType.ClientPage, pageKey: pageKey, location: location);
    }
}
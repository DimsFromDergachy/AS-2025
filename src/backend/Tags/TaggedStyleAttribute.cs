namespace AS_2025.Tags;

public class TagStyleAttribute : Attribute
{
    public TagStyle Style { get; }

    public TagStyleAttribute(TagStyle style)
    {
        Style = style;
    }
}
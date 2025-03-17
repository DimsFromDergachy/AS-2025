internal enum Feature
{
    Domain,
    EFConfiguration,
    ApplicationService,
    DeleteRepr,
    ListRepr,
    ReferenceListRepr,
    Endpoints,
    ReprMapper
}

static class Features
{
    public static Feature[] All = new[]
    {
        Feature.Domain,
        Feature.EFConfiguration,
        Feature.ApplicationService,
        Feature.DeleteRepr,
        Feature.ListRepr,
        Feature.ReferenceListRepr,
        Feature.Endpoints,
        Feature.ReprMapper
    };
}
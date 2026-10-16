//#if (mapster)
using Mapster;
//#endif

namespace TemplateProject.API.Mapper
{
    public static class MappingConfig
    {
        public static void Configure()
        {
//#if (mapster)
            TypeAdapterConfig.GlobalSettings.Scan(typeof(MappingConfig).Assembly);
//#endif
        }
    }
}

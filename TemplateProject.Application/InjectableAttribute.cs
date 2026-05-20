using Microsoft.Extensions.DependencyInjection;

namespace TemplateProject.Application
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public sealed class InjectableAttribute(ServiceLifetime lifetime = ServiceLifetime.Scoped) : Attribute
    {
        public ServiceLifetime Lifetime { get; } = lifetime;
    }
}

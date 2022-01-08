using System.Collections;
using Microsoft.Extensions.DependencyInjection;

namespace AppSettingsDocGenerator.Scrapper.Injection;

public class ServiceCollectionInjection : IServiceCollection
{
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public IEnumerator<ServiceDescriptor> GetEnumerator()
    {
        throw new NotImplementedException();
    }

    public void Add(ServiceDescriptor item)
    {
        throw new NotImplementedException();
    }

    public void Clear()
    {
        throw new NotImplementedException();
    }

    public bool Contains(ServiceDescriptor item)
    {
        throw new NotImplementedException();
    }

    public void CopyTo(ServiceDescriptor[] array, int arrayIndex)
    {
        throw new NotImplementedException();
    }

    public bool Remove(ServiceDescriptor item)
    {
        throw new NotImplementedException();
    }

    public int Count { get; }
    public bool IsReadOnly { get; }
    public int IndexOf(ServiceDescriptor item)
    {
        throw new NotImplementedException();
    }

    public void Insert(int index, ServiceDescriptor item)
    {
        throw new NotImplementedException();
    }

    public void RemoveAt(int index)
    {
        throw new NotImplementedException();
    }

    public ServiceDescriptor this[int index]
    {
        get => throw new NotImplementedException();
        set => throw new NotImplementedException();
    }
}
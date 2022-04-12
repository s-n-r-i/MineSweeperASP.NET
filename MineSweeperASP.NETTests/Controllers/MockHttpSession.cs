using Microsoft.AspNetCore.Http;

#nullable disable
namespace MineSweeperASP.NET.Controllers.Tests;

/// <summary>
/// MockHttpSession
/// </summary>
/// <remarks>
/// 参考
/// https://stackoverflow.com/questions/42269770/how-to-mock-session-variables-in-asp-net-core-unit-testing-project
/// </remarks>
public class MockHttpSession : ISession
{
    private readonly Dictionary<string, object> sessionStorage = new();

    public object this[string name]
    {
        get { return sessionStorage[name]; }
        set { sessionStorage[name] = value; }
    }

    public string Id => throw new NotImplementedException();

    public bool IsAvailable => throw new NotImplementedException();

    public IEnumerable<string> Keys => sessionStorage.Keys;

    public Task CommitAsync(CancellationToken cancellationToken = default) => throw new NotImplementedException();

    public Task LoadAsync(CancellationToken cancellationToken = default) => throw new NotImplementedException();

    public void Clear() => sessionStorage.Clear();

    public void Remove(string key) => sessionStorage.Remove(key);

    public void Set(string key, byte[] value) => sessionStorage[key] = value;

    public bool TryGetValue(string key, out byte[] value)
    {
        if (!sessionStorage.ContainsKey(key))
        {
            value = null;
            return false;
        }

        if (sessionStorage[key] != null)
        {
            //value = Encoding.ASCII.GetBytes(sessionStorage[key].ToString());
            value = (byte[])sessionStorage[key]; //保存値のまま(byte[]にキャストして)返す
            return true;
        }
        else
        {
            value = null;
            return false;
        }
    }
}

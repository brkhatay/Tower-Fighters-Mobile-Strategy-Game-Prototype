using System.Threading.Tasks;

public interface ISaveClient
{
    Task Save(string key, object value);
    Task<T> Load<T>(string key);
    Task Delete(string key);
}

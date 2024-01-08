namespace CelebraTix.Promotions.Data;

public interface INotifier<T>
{
    Task Notify(T entityAdded);
}
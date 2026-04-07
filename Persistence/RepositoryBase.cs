namespace Persistence;

public abstract class RepositoryBase<T>
{
    protected readonly List<T> Items = [];

    public virtual IEnumerable<T> GetAll() => Items;

    public virtual T? Get(Func<T, bool> predicate) => Items.FirstOrDefault(predicate);

    public virtual IEnumerable<T> Find(Func<T, bool> predicate) => Items.Where(predicate);

    public virtual void Add(T entity) => Items.Add(entity);

    public virtual void Remove(T entity) => Items.Remove(entity);
}
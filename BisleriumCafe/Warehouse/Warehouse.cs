namespace BisleriumCafe.Repositories;
using BisleriumCafe.Model;
using BisleriumCafe.Providers;

internal class Warehouse<TSource> : WarehouseIO<TSource>, IWarehouse<TSource> where TSource : IModel
{
    public Warehouse(FileProvider<TSource> fileProvider) : base(fileProvider)
    {
    }

    public virtual int Count => _sourceData.Count;

    public virtual void Add(TSource item)
    {
        _sourceData.Add(item);
    }

    public virtual bool Remove(TSource item)
    {
        return _sourceData.Remove(item);
    }

    public virtual void Clear()
    {
        _sourceData.Clear();
    }

    public virtual bool Contains(TSource item)
    {
        return _sourceData.Contains(item);
    }

    public virtual bool Contains<TKey>(Func<TSource, TKey> keySelector, TKey byValue)
    {
        return Get(keySelector, byValue) is not null;
    }

    public virtual TSource? Get<TKey>(Func<TSource, TKey> keySelector, TKey byValue)
    {
        return _sourceData.FirstOrDefault(a => keySelector.Invoke(a).Equals(byValue));
    }

    public virtual ICollection<TSource> GetAll()
    {
        return _sourceData;
    }

    public virtual ICollection<TSource> GetAllSorted<TKey>(Func<TSource, TKey> keySelector, Enums.SortDirectionEnum direction)
    {
        return direction switch
        {
            Enums.SortDirectionEnum.Ascending => _sourceData.OrderBy(keySelector).ToList(),
            Enums.SortDirectionEnum.Descending => _sourceData.OrderByDescending(keySelector).ToList(),
            _ => throw new Exception("Invalid sort direction!"),
        };
    }

    public virtual bool Update(TSource item)
    {
        TSource? oldItem = Get(x => x.Id, item.Id);
        if (oldItem is null)
        {
            return false;
        }
        Remove(oldItem);
        Add(item);
        return true;
    }
}
using System.Collections.Generic;

namespace GestionBanque.Models.DataService
{
    public interface IDataService<T>
    {
        IEnumerable<T> GetAll();
        T? Get(long id);
        bool Insert(T record);
        bool Update(T record);
        bool Delete(T record);
    }
}

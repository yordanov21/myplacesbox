namespace MyPlacesBox.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;

    public interface IAllPlacesService
    {
        IEnumerable<T> GetAll<T>(int page, int itemsPage = 10);

        IEnumerable<T> GetRandom<T>(int count);

        int GetCount();

        T GetById<T>(int id);

        Task DeleteAsync(int id);
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MyPlacesBox.Services
{
    public interface IHikeScraperService
    {
        Task PopulateDbWithHikesAsync();
    }
}

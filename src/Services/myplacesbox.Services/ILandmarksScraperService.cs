namespace MyPlacesBox.Services
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;

    public interface ILandmarksScraperService
    {
         Task PopulateDbWithLandmarksAsync();
    }
}

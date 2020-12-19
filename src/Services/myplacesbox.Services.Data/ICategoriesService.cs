namespace MyPlacesBox.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public interface ICategoriesService
    {
        IEnumerable<KeyValuePair<string, string>> GetAllHikeCategotiesAsKeyValuePairs();

        IEnumerable<KeyValuePair<string, string>> GetAllLandmarkCategotiesAsKeyValuePairs();
    }
}

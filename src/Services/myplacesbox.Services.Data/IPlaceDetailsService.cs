namespace MyPlacesBox.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public interface IPlaceDetailsService
    {
        IEnumerable<KeyValuePair<string, string>> GetAllAsKeyValuePairs();
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace MyPlacesBox.Services.Data
{
    public interface IPlaceDetailsService
    {
        IEnumerable<KeyValuePair<string, string>> GetAllAsKeyValuePairs();
    }
}

﻿using System;
using System.Collections.Generic;
using System.Text;

namespace MyPlacesBox.Services.Data
{
    public interface ICategoriesService 
    {
        IEnumerable<KeyValuePair<string, string>> GetAllHikeCategotiesAsKeyValuePairs();

        IEnumerable<KeyValuePair<string, string>> GetAllLandmarkCategotiesAsKeyValuePairs();
    }
}

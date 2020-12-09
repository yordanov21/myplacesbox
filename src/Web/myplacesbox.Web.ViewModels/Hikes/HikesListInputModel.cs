namespace MyPlacesBox.Web.ViewModels.Hikes
{
    using System.Collections.Generic;

    public class HikesListInputModel : PagingViewModel
    {
        public IEnumerable<HikeInListInputModel> Hikes { get; set; }

    }
}

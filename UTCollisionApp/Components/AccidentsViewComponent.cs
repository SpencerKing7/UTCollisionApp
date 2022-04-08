using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using UTCollisionApp.Models;

namespace UTCollisionApp.Components
{
    public class AccidentsViewComponent : ViewComponent
    {
        private ICollisionRepository _repo { get; set; }

        public AccidentsViewComponent(ICollisionRepository temp)
        {
            _repo = temp;
        }

        public IViewComponentResult Invoke()
        {
            var x = _repo.Crashes
                .Select(x => x.Location.COUNTY_NAME)
                .Distinct()
                .OrderBy(x => x);

            return View(x);
        }
    }
}

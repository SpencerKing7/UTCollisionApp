using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UTCollisionApp.Models;
using UTCollisionApp.Models.ViewModels;

namespace UTCollisionApp.Components
{
    public class CountiesViewComponent : ViewComponent
    {
        private ICollisionRepository _repo { get; set; }

        public CountiesViewComponent (ICollisionRepository temp)
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

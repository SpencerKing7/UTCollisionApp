using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UTCollisionApp.Models.ViewModels
{
    public class CrashViewModel
    {
        public IQueryable<Crash> Crashes { get; set; }
        public IQueryable<Location> Locations { get; set; }
        public PageInfo PageInfo { get; set; }
    }
}

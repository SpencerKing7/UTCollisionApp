using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UTCollisionApp.Models
{
    public interface ICollisionRepository
    {
        IQueryable<Crash> Crashes { get; }
        IQueryable<Factor> Factors { get; }
        IQueryable<Location> Locations { get; }

        //Edit
        public void SaveCrash(Crash c);
        //Create
        public void CreateCrash(Crash c);
        //Delete
        public void DeleteCrash(Crash c);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UTCollisionApp.Models
{
    public class EFCollisionRepository : ICollisionRepository
    {
        private CollisionDbContext _context { get; set; }
        public EFCollisionRepository(CollisionDbContext temp)
        {
            _context = temp;
        }


        public IQueryable<Crash> Crashes => _context.Crashes;

        public IQueryable<Factor> Factors => _context.Factors;

        public IQueryable<Location> Locations => _context.Locations;

        //Editing
        public void CreateCrash(Crash c)
        {
            _context.Update(c);
            _context.SaveChanges();
        }

        //Adding
        public void DeleteCrash(Crash c)
        {
            _context.Add(c);
            _context.SaveChanges();
        }

        //Deleting
        public void SaveCrash(Crash c)
        {
            _context.Remove(c);
            _context.SaveChanges();
        }
    }
}

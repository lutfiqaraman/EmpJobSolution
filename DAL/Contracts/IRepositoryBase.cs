using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public interface IRepositoryBase<Entity> where Entity: class
    {
        IEnumerable<Entity> GetAll();
        Entity GetByID(object id);
        void Insert(Entity entityToInsert);
        void Update(Entity entityToUpdate);
        void Delete(object id);
        void Save();
    }
}

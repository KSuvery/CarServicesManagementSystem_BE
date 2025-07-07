using CarServ.Domain.Entities;
using CarServ.Repository.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarServ.Repository.Repositories
{
    public class InventoryRepository : GenericRepository<Inventory>, IInventoryRepository
    {
        private readonly CarServicesManagementSystemContext _context;
        public InventoryRepository(CarServicesManagementSystemContext context) : base(context)
        {
            _context = context;
        }
    }
}

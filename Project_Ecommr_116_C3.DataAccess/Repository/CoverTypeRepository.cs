using Project_Ecommr_116_C3.Data;
using Project_Ecommr_116_C3.DataAccess.Repository.IRepository;
using Project_Ecommr_116_C3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_Ecommr_116_C3.DataAccess.Repository
{
    public class CoverTypeRepository:Repository<CoverType>,ICoverTypeRepository
    {
        private readonly ApplicationDbContext _context;
        public CoverTypeRepository(ApplicationDbContext context):base(context)
        {
            _context = context;
        }
    }
}

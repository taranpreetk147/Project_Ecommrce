using Project_Ecommr_116_C3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_Ecommr_116_C3.DataAccess.Repository.IRepository
{
    public interface IUnitOfWork
    {
        public ICategoryRepository Category { get; }
        public ICoverTypeRepository CoverType { get; }
        public IProductRepository Product { get; }
        public ICompanyRepository Company { get; }
        public  IApplicationUserRepository ApplicationUser { get; }
        public IOrderHeaderRepository OrderHeader { get; }
        public IOrderDetailRepository OrderDetail { get; }
        public IShoppingCartRepository ShoppingCart { get; }

        void Save();
        
    }
}

using Contributors.Interfaces.Repository;
using InternetMarket.DataAccess;
using InternetMarket.Models.DbModels;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Contributors.DataAccess
{
    class ProductRepository : BaseRepository<Product>, IProductRepository
    {
        public ProductRepository(AppDbContext context)
            : base(context)
        {

        }

        public List<Product> Find(string stringFind)
        {
            var productions = _dbSet.AsNoTracking().Where(p => EF.Functions.Like(p.Name, string.Format("%{0}%", stringFind))).ToList();
            if (productions is null || productions.Count == 0)
            {
                //productions = _dbSet.AsNoTracking().Where(p => p.ArticleNumber.Contains(stringFind)).ToList();
                productions = _dbSet.AsNoTracking().Where(p => EF.Functions.Like(p.ArticleNumber, string.Format("%{0}%", stringFind))).ToList();
            }    
            return productions;
        }
    }
}

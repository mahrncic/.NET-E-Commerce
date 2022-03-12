using Dapper;
using Discount.API.Entities;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System.Threading.Tasks;

namespace Discount.API.Repositories
{
    public class DiscountRepository : IDiscountRepository
    {
        private readonly IConfiguration _configuration;

        public DiscountRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<bool> CreateDiscount(Coupon coupon)
        {
            throw new System.NotImplementedException();
        }

        public async Task<bool> DeleteDiscount(string productName)
        {
            throw new System.NotImplementedException();
        }

        public async Task<Coupon> GetDiscount(string productName)
        {
            using var connection = CreateAndReturnConnection();

            var coupon = await connection
                .QueryFirstOrDefaultAsync<Coupon>
                ("SELECT * FROM Coupon WHERE ProductName = @ProductName", 
                new { ProductName = productName });

            if (coupon == null)
                return CreateAndReturnEmptyCoupon();

            return coupon;
        }

        public async Task<bool> UpdateDiscount(Coupon coupon)
        {
            throw new System.NotImplementedException();
        }

        private NpgsqlConnection CreateAndReturnConnection()
        {
            return new NpgsqlConnection(_configuration["DatabaseSettings:ConnectionString"]);
        }

        private static Coupon CreateAndReturnEmptyCoupon()
        {
            return new Coupon
            {
                Amount = 0,
                ProductName = "No Discount",
                Description = "No Discount for this product"
            };
        }
    }
}

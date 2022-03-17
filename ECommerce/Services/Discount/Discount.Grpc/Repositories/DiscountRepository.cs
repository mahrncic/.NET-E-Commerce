using Dapper;
using Discount.Grpc.Entities;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System.Threading.Tasks;

namespace Discount.Grpc.Repositories
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
            using var connection = CreateAndReturnConnection();

            var affected = await connection
                                    .ExecuteAsync("INSERT INTO Coupon (ProductName, Description, Amount) VALUES (@ProductName, @Description, @Amount)",
                                    new { ProductName = coupon.ProductName, Description = coupon.Description, Amount = coupon.Amount });
            
            return IsOperationExecutedSuccessfully(affected);
        }

        public async Task<bool> DeleteDiscount(string productName)
        {
            using var connection = CreateAndReturnConnection();

            var affected = await connection
                                    .ExecuteAsync("Delete FROM Coupon WHERE ProductName = @ProductName",
                                    new { ProductName = productName });

            return IsOperationExecutedSuccessfully(affected);
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
            using var connection = CreateAndReturnConnection();

            var affected = await connection
                                    .ExecuteAsync("UPDATE Coupon SET ProductName = @ProductName, Description = @Description, Amount = @Amount WHERE Id = @Id",
                                    new { ProductName = coupon.ProductName, Description = coupon.Description, Amount = coupon.Amount, Id = coupon.Id });

            return IsOperationExecutedSuccessfully(affected);
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

        private static bool IsOperationExecutedSuccessfully(int affected)
        {
            if (affected == 0)
                return false;

            return true;
        }
    }
}

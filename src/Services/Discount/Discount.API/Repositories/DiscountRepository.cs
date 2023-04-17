using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Discount.API.Entities;
using Npgsql;

namespace Discount.API.Repositories
{
    public class DiscountRepository : IDiscountRepository
    {
        private readonly IConfiguration _config;

        public DiscountRepository(IConfiguration config)
        {
            _config = config;
        }

        public async Task<bool> CreateDiscount(Coupon coupon)
        {
            using var connection=new NpgsqlConnection
            (_config.GetValue<string>("DatabaseSettings:ConnectionString"));

            var affected =await connection.ExecuteAsync
                ("INSERT INTO Coupon (ProductName,Description,Amount) VALUES (@ProductName,@Description,@Amount)"
                ,new {ProductName=coupon.ProductName,Amount=coupon.Amount,Description=coupon.Description});

            return affected!=0;
        }


        public async Task<Coupon> GetDiscount(string productName)
        {
            using var connection=new NpgsqlConnection
            (_config.GetValue<string>("DatabaseSettings:ConnectionString"));

            var coupon = await connection.QueryFirstOrDefaultAsync<Coupon>
                ("SELECT * FROM Coupon WHERE ProductName = @ProductName",new {ProductName=productName});

            if(coupon==null)                    
                return new Coupon{ProductName="No Discount",Amount=0,Description="No Discount Disc"};
            return coupon;                
        }

        public async Task<bool> UpdateDiscount(Coupon coupon)
        {
            using var connection=new NpgsqlConnection
            (_config.GetValue<string>("DatabaseSettings:ConnectionString"));

            var affected =await connection.ExecuteAsync
                ("UPDATE Coupon SET ProductName=@ProductName, Description=@Description, Amount=@Amount WHERE Id=@Id"
                ,new {ProductName=coupon.ProductName,Amount=coupon.Amount,Description=coupon.Description,Id=coupon.Id});
            return affected!=0;
        }

        public async Task<bool> DeleteDiscount(string productName)
        {
            using var connection=new NpgsqlConnection
            (_config.GetValue<string>("DatabaseSettings:ConnectionString"));

            var affected =await connection.ExecuteAsync
                ("DELETE FROM Coupon WHERE ProductName=@ProductName"
                ,new {ProductName=productName});
            return affected!=0;
        }
    }
}
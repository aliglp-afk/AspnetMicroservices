using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Npgsql;

namespace Discount.Grpc.Extensions
{
    public static class HostExtensions
    {
        public static IHost MigrateDatabase<TContext>(this IHost host, int? retry=0){
            int retryForAvailablity = retry.Value;
            using( var scope=host.Services.CreateScope()){
                
                
                var services = scope.ServiceProvider;
                var configuration = services.GetRequiredService<IConfiguration>();
                var logger=  services.GetRequiredService<ILogger<TContext>>();

                try
                {
                    logger.LogInformation("Migrating postgresql database");

                    using var connection=new NpgsqlConnection
                    (configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
                    connection.Open();

                    using var command=new NpgsqlCommand{
                        Connection=connection
                    };

                    command.CommandText="DROP TABLE IF EXISTS Coupon";
                    command.ExecuteNonQuery();
                    
                    command.CommandText=@"CREATE TABLE Coupon(Id SERIAL PRIMARY KEY,
                                                            ProductName VARCHAR(24),
                                                            Description TEXT,
                                                            Amount INT)";
                    command.ExecuteNonQuery();

                    command.CommandText=@"INSERT INTO Coupon (ProductName,Description,Amount) VALUES('IPhone X','IPhone Discounts',150)";
                    command.ExecuteNonQuery();

                    command.CommandText=@"INSERT INTO Coupon (ProductName,Description,Amount) VALUES('Samsung 10','Samsung Discount',100)";
                    command.ExecuteNonQuery();

                    logger.LogInformation("Migrated postgresql database.");}

                catch(NpgsqlException e){

                    logger.LogError(e,"An error occurred while migrating the postgresql database");
                    if(retryForAvailablity<5){
                        retryForAvailablity++;
                        System.Threading.Thread.Sleep(2000);
                        MigrateDatabase<TContext>(host,retryForAvailablity);
                    }

                }
                return host;

            }
        }
    }
}
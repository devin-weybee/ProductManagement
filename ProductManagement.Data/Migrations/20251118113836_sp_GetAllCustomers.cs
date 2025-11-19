using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProductManagement.Data.Migrations
{
    /// <inheritdoc />
    public partial class sp_GetAllCustomers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            string sp_GetAllCustomer = @"
                create procedure [dbo].[GetAllCustomers]
                as begin
                select CustomerID, CustomerName, CustomerEmail, ProductIDs from [dbo].[Customers]
                end
            ";
            migrationBuilder.Sql(sp_GetAllCustomer);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            string sp_GetAllCustomer = @"
                drop procedure [dbo].[GetAllCustomers]
            ";
            migrationBuilder.Sql(sp_GetAllCustomer);
        }

    }
}

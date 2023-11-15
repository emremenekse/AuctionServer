using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuctionAPI.Migrations
{
    /// <inheritdoc />
    public partial class mig_7 : Migration
    {
        /// <inheritdoc />
        
            /// <inheritdoc />
            protected override void Up(MigrationBuilder migrationBuilder)
            {
                migrationBuilder.Sql(@"
        CREATE PROCEDURE AddProduct
    @Name NVARCHAR(100),
    @ProviderId INT
AS
BEGIN
    INSERT INTO Product (Name, ProviderId)
    VALUES (@Name, @ProviderId)
END
    ");
            }


            /// <inheritdoc />
            protected override void Down(MigrationBuilder migrationBuilder)
            {
                migrationBuilder.Sql("DROP PROCEDURE UpdateOrganizationStatus");
            }

        }
    
}

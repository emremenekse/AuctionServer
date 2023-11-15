using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuctionAPI.Migrations
{
    /// <inheritdoc />
    public partial class CreateStoredProcedure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
        CREATE PROCEDURE UpdateOrganizationStatus
        AS
        BEGIN
            -- Tüm kayıtları IsActive = false olarak ayarla
            UPDATE Organizations
            SET IsActive = 0;
            
            -- Sonra, en büyük OrganizationId'ye sahip kaydı IsActive = true olarak güncelle
            UPDATE Organizations
            SET IsActive = 1
            WHERE OrganizationId = (SELECT TOP 1 OrganizationId FROM Organizations ORDER BY OrganizationId DESC);
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

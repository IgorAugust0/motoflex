using Microsoft.EntityFrameworkCore.Migrations;

namespace Motoflex.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder) =>
            migrationBuilder
                .CreateAdminTable()
                .CreateRenterTable()
                .CreateMotorcycleTable()
                .CreateOrderTable()
                .CreateRentalTable()
                .CreateNotificationTable()
                .CreateIndexes();

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder) =>
            migrationBuilder.DropAllTables();
    }

    public static class MigrationBuilderExtensions
    {
        public static MigrationBuilder CreateAdminTable(this MigrationBuilder builder)
        {
            builder.CreateTable(
                name: "Admin",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table => table.PrimaryKey("PK_Admins", x => x.Id));
            return builder;
        }

        public static MigrationBuilder CreateRenterTable(this MigrationBuilder builder)
        {
            builder.CreateTable(
                name: "Renter",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "varchar(100)", nullable: false),
                    cnpj = table.Column<string>(type: "char(14)", nullable: false),
                    birthdate = table.Column<DateTime>(type: "date", nullable: false), // type: "timestamp with time zone"
                    cnh = table.Column<string>(type: "char(12)", nullable: false),
                    cnh_type = table.Column<string>(type: "varchar(5)", nullable: false),
                    cnh_image = table.Column<string>(type: "varchar(255)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Renter", x => x.Id);
                    // table.UniqueConstraint("UQ_Renters_cnpj", x => x.cnpj);
                    // table.UniqueConstraint("UQ_Renters_cnh", x => x.cnh);
                });
            return builder;
        }

        public static MigrationBuilder CreateMotorcycleTable(this MigrationBuilder builder)
        {
            builder.CreateTable(
                name: "Motorcycle",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    year = table.Column<int>(type: "integer", nullable: false),
                    model = table.Column<string>(type: "varchar(50)", nullable: false),
                    license_plate = table.Column<string>(type: "char(7)", nullable: false),
                    available = table.Column<bool>(type: "boolean", defaultValue: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Motorcycle", x => x.Id);
                    // table.UniqueConstraint("UQ_Motorcycles_plate", x => x.plate);
                });
            return builder;
        }

        public static MigrationBuilder CreateOrderTable(this MigrationBuilder builder)
        {
            builder.CreateTable(
                name: "Order",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    delivery_fee = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: false),
                    status = table.Column<string>(type: "varchar(20)", nullable: false),
                    renter_id = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Order", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Order_Renter_renter_id",
                        column: x => x.renter_id,
                        principalTable: "Renters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });
            return builder;
        }

        public static MigrationBuilder CreateRentalTable(this MigrationBuilder builder)
        {
            builder.CreateTable(
                name: "Rental",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    plan = table.Column<string>(type: "varchar(20)", nullable: false),
                    renter_id = table.Column<Guid>(type: "uuid", nullable: false),
                    motorcycle_id = table.Column<Guid>(type: "uuid", nullable: false),
                    begin_at = table.Column<DateTime>(type: "date", nullable: false),
                    finish_at = table.Column<DateTime>(type: "date", nullable: false),
                    return_at = table.Column<DateTime>(type: "date", nullable: true),
                    active = table.Column<bool>(type: "boolean", defaultValue: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rental", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Rentals_Renter_renter_id",
                        column: x => x.renter_id,
                        principalTable: "Renter",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Rental_Motorcycle_motorcycle_id",
                        column: x => x.motorcycle_id,
                        principalTable: "Motorcycle",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });
            return builder;
        }

        public static MigrationBuilder CreateNotificationTable(this MigrationBuilder builder)
        {
            builder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    order_id = table.Column<Guid>(type: "uuid", nullable: false),
                    renter_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => new { x.order_id, x.renter_id });
                    table.ForeignKey(
                        name: "FK_Notifications_Order_order_id",
                        column: x => x.order_id,
                        principalTable: "Order",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Notifications_Renter_renter_id",
                        column: x => x.renter_id,
                        principalTable: "Renter",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
            return builder;
        }

        public static MigrationBuilder CreateIndexes(this MigrationBuilder builder)
        {
            builder.CreateIndex(
                name: "IX_Renter_cnpj",
                table: "Renter",
                column: "cnpj",
                unique: true);

            builder.CreateIndex(
                name: "IX_Renter_cnh",
                table: "Renter",
                column: "cnh",
                unique: true);

            builder.CreateIndex(
                name: "IX_Motorcycle_license_plate",
                table: "Motorcycle",
                column: "license_plate",
                unique: true);

            builder.CreateIndex(
                name: "IX_Order_renter_id",
                table: "Order",
                column: "renter_id");

            builder.CreateIndex(
                name: "IX_Rental_renter_id",
                table: "Rental",
                column: "renter_id");

            builder.CreateIndex(
                name: "IX_Rental_motorcycle_id",
                table: "Rental",
                column: "motorcycle_id");

            builder.CreateIndex(
                name: "IX_Notifications_renter_id",
                table: "Notifications",
                column: "renter_id");

            return builder;
        }

        public static MigrationBuilder DropAllTables(this MigrationBuilder builder)
        {
            var tables = new[]
            {
            "Notifications", "Rental", "Order",
            "Motorcycle", "Renter", "Admin"
            };

            foreach (var table in tables)
                builder.DropTable(table);

            return builder;
        }
    }
}

// uncomment this code below to use the old migration

/* 

using Microsoft.EntityFrameworkCore.Migrations;

namespace Motoflex.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Admin",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Admin", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Renter",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "varchar", nullable: false),
                    Cnpj = table.Column<string>(type: "char(14)", nullable: false),
                    Birthdate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Cnh = table.Column<string>(type: "char(12)", nullable: false),
                    CnhType = table.Column<string>(type: "varchar", nullable: false),
                    CnhImage = table.Column<string>(type: "varchar", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Renter", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Motorcycle",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Year = table.Column<int>(type: "integer", nullable: false),
                    Model = table.Column<string>(type: "varchar", maxLength: 50, nullable: false),
                    LicensePlate = table.Column<string>(type: "char(7)", nullable: false),
                    Available = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Motorcycle", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Order",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    DeliveryFee = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: false),
                    Status = table.Column<string>(type: "varchar", nullable: false),
                    RenterId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Order", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Order_Renter_RenterId",
                        column: x => x.RenterId,
                        principalTable: "Renter",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Rental",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Plan = table.Column<string>(type: "varchar", nullable: false),
                    RenterId = table.Column<Guid>(type: "uuid", nullable: false),
                    MotorcycleId = table.Column<Guid>(type: "uuid", nullable: false),
                    BeginAt = table.Column<DateTime>(type: "Date", nullable: false),
                    FinishAt = table.Column<DateTime>(type: "Date", nullable: false),
                    ReturnAt = table.Column<DateTime>(type: "Date", nullable: false),
                    Active = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rental", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Rental_Renter_RenterId",
                        column: x => x.RenterId,
                        principalTable: "Renter",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Rental_Motorcycle_MotorcycleId",
                        column: x => x.MotorcycleId,
                        principalTable: "Motorcycle",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    OrderId = table.Column<Guid>(type: "uuid", nullable: false),
                    RenterId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => new { x.OrderId, x.RenterId });
                    table.ForeignKey(
                        name: "FK_Notifications_Renter_RenterId",
                        column: x => x.RenterId,
                        principalTable: "Renter",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Notifications_Order_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Order",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Renter_Cnh",
                table: "Renter",
                column: "Cnh",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Renter_Cnpj",
                table: "Renter",
                column: "Cnpj",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Motorcycle_LicensePlate",
                table: "Motorcycle",
                column: "LicensePlate",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_RenterId",
                table: "Notifications",
                column: "RenterId");

            migrationBuilder.CreateIndex(
                name: "IX_Order_RenterId",
                table: "Order",
                column: "RenterId");

            migrationBuilder.CreateIndex(
                name: "IX_Rental_RenterId",
                table: "Rental",
                column: "RenterId");

            migrationBuilder.CreateIndex(
                name: "IX_Rental_MotorcycleId",
                table: "Rental",
                column: "MotorcycleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Admin");

            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.DropTable(
                name: "Rental");

            migrationBuilder.DropTable(
                name: "Order");

            migrationBuilder.DropTable(
                name: "Motorcycle");

            migrationBuilder.DropTable(
                name: "Renter");
        }
    }
}

*/
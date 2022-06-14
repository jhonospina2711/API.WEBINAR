using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Dependencies",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(maxLength: 50, nullable: false),
                    Enabled = table.Column<bool>(nullable: false, defaultValue: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dependencies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Profiles",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(maxLength: 50, nullable: false),
                    Enabled = table.Column<bool>(nullable: false, defaultValue: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Profiles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Profitabilities",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProfitabilityPercentage = table.Column<decimal>(nullable: false),
                    StartTime = table.Column<string>(nullable: false),
                    FinalTIme = table.Column<string>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Profitabilities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProjectTypes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(maxLength: 50, nullable: false),
                    Enabled = table.Column<bool>(nullable: false, defaultValue: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TransactionTypes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(maxLength: 50, nullable: false),
                    Enabled = table.Column<bool>(nullable: false, defaultValue: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 50, nullable: true),
                    LastName = table.Column<string>(maxLength: 50, nullable: true),
                    Email = table.Column<string>(maxLength: 50, nullable: false),
                    Password = table.Column<string>(maxLength: 150, nullable: true),
                    Activated = table.Column<bool>(nullable: false, defaultValue: false),
                    Enabled = table.Column<bool>(nullable: false, defaultValue: true),
                    IsFirstLogin = table.Column<bool>(nullable: false, defaultValue: true),
                    DependencyId = table.Column<int>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Dependencies_DependencyId",
                        column: x => x.DependencyId,
                        principalTable: "Dependencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Projects",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 500, nullable: true),
                    Description = table.Column<string>(type: "varchar(MAX)", nullable: true),
                    ImageName = table.Column<string>(type: "varchar(MAX)", nullable: false),
                    VideoName = table.Column<string>(type: "varchar(MAX)", nullable: true),
                    ImplementDate = table.Column<string>(maxLength: 200, nullable: false),
                    ImpactDescription = table.Column<string>(type: "varchar(MAX)", nullable: false),
                    Enabled = table.Column<bool>(nullable: false, defaultValue: true),
                    DependencyId = table.Column<int>(nullable: false),
                    ProjectTypeId = table.Column<int>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Projects_Dependencies_DependencyId",
                        column: x => x.DependencyId,
                        principalTable: "Dependencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Projects_ProjectTypes_ProjectTypeId",
                        column: x => x.ProjectTypeId,
                        principalTable: "ProjectTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserProfiles",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(nullable: false),
                    ProfileId = table.Column<int>(nullable: false),
                    Enabled = table.Column<bool>(nullable: false, defaultValue: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserProfiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserProfiles_Profiles_ProfileId",
                        column: x => x.ProfileId,
                        principalTable: "Profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserProfiles_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UsersOneTimePass",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OTPCode = table.Column<int>(maxLength: 4, nullable: false),
                    Used = table.Column<bool>(nullable: false, defaultValue: false),
                    ReceivedByUser = table.Column<bool>(nullable: false, defaultValue: false),
                    UserId = table.Column<int>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsersOneTimePass", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UsersOneTimePass_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserWalletAccounts",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Coins = table.Column<decimal>(nullable: false, defaultValue: 0m),
                    Enabled = table.Column<bool>(nullable: false, defaultValue: false),
                    UserId = table.Column<int>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserWalletAccounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserWalletAccounts_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserWalletAccountTransactions",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Coins = table.Column<decimal>(nullable: false),
                    CoinsEarned = table.Column<decimal>(nullable: false, defaultValue: 0m),
                    ProfitabilityPercentaje = table.Column<decimal>(nullable: false, defaultValue: 0m),
                    UserWalletAccountId = table.Column<string>(nullable: false),
                    TransactionTypeId = table.Column<int>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserWalletAccountTransactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserWalletAccountTransactions_TransactionTypes_TransactionTypeId",
                        column: x => x.TransactionTypeId,
                        principalTable: "TransactionTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserWalletAccountTransactions_UserWalletAccounts_UserWalletAccountId",
                        column: x => x.UserWalletAccountId,
                        principalTable: "UserWalletAccounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "WalletAccountWords",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Words = table.Column<string>(maxLength: 500, nullable: false),
                    UserWalletAccountId = table.Column<string>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WalletAccountWords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WalletAccountWords_UserWalletAccounts_UserWalletAccountId",
                        column: x => x.UserWalletAccountId,
                        principalTable: "UserWalletAccounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserInvestmentProjects",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserWalletAccountTransactionId = table.Column<string>(nullable: false),
                    ProjectId = table.Column<int>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserInvestmentProjects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserInvestmentProjects_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserInvestmentProjects_UserWalletAccountTransactions_UserWalletAccountTransactionId",
                        column: x => x.UserWalletAccountTransactionId,
                        principalTable: "UserWalletAccountTransactions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Dependencies",
                columns: new[] { "Id", "CreatedDate", "Description", "UpdatedDate" },
                values: new object[] { 1, new DateTime(2021, 9, 27, 19, 46, 0, 140, DateTimeKind.Utc).AddTicks(2884), "Sin dependencia", new DateTime(2021, 9, 27, 19, 46, 0, 140, DateTimeKind.Utc).AddTicks(2884) });

            migrationBuilder.InsertData(
                table: "Profiles",
                columns: new[] { "Id", "CreatedDate", "Description", "UpdatedDate" },
                values: new object[,]
                {
                    { 1, new DateTime(2021, 9, 27, 19, 46, 0, 140, DateTimeKind.Utc).AddTicks(2884), "Administrator", new DateTime(2021, 9, 27, 19, 46, 0, 140, DateTimeKind.Utc).AddTicks(2884) },
                    { 2, new DateTime(2021, 9, 27, 19, 46, 0, 140, DateTimeKind.Utc).AddTicks(2884), "User", new DateTime(2021, 9, 27, 19, 46, 0, 140, DateTimeKind.Utc).AddTicks(2884) },
                    { 3, new DateTime(2021, 9, 27, 19, 46, 0, 140, DateTimeKind.Utc).AddTicks(2884), "PlatformAdministrator", new DateTime(2021, 9, 27, 19, 46, 0, 140, DateTimeKind.Utc).AddTicks(2884) }
                });

            migrationBuilder.InsertData(
                table: "ProjectTypes",
                columns: new[] { "Id", "CreatedDate", "Description", "UpdatedDate" },
                values: new object[] { 1, new DateTime(2021, 9, 27, 19, 46, 0, 140, DateTimeKind.Utc).AddTicks(2884), "Davivienda", new DateTime(2021, 9, 27, 19, 46, 0, 140, DateTimeKind.Utc).AddTicks(2884) });

            migrationBuilder.InsertData(
                table: "TransactionTypes",
                columns: new[] { "Id", "CreatedDate", "Description", "UpdatedDate" },
                values: new object[,]
                {
                    { 1, new DateTime(2021, 9, 27, 19, 46, 0, 140, DateTimeKind.Utc).AddTicks(2884), "Consignación Inicial", new DateTime(2021, 9, 27, 19, 46, 0, 140, DateTimeKind.Utc).AddTicks(2884) },
                    { 2, new DateTime(2021, 9, 27, 19, 46, 0, 140, DateTimeKind.Utc).AddTicks(2884), "Inversión", new DateTime(2021, 9, 27, 19, 46, 0, 140, DateTimeKind.Utc).AddTicks(2884) },
                    { 3, new DateTime(2021, 9, 27, 19, 46, 0, 140, DateTimeKind.Utc).AddTicks(2884), "Consignación ", new DateTime(2021, 9, 27, 19, 46, 0, 140, DateTimeKind.Utc).AddTicks(2884) }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Dependencies_Description",
                table: "Dependencies",
                column: "Description",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Profiles_Description",
                table: "Profiles",
                column: "Description",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Projects_DependencyId",
                table: "Projects",
                column: "DependencyId");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_Name",
                table: "Projects",
                column: "Name",
                unique: true,
                filter: "[Name] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_ProjectTypeId",
                table: "Projects",
                column: "ProjectTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectTypes_Description",
                table: "ProjectTypes",
                column: "Description",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TransactionTypes_Description",
                table: "TransactionTypes",
                column: "Description",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserInvestmentProjects_ProjectId",
                table: "UserInvestmentProjects",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_UserInvestmentProjects_UserWalletAccountTransactionId",
                table: "UserInvestmentProjects",
                column: "UserWalletAccountTransactionId");

            migrationBuilder.CreateIndex(
                name: "IX_UserProfiles_ProfileId",
                table: "UserProfiles",
                column: "ProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_UserProfiles_UserId",
                table: "UserProfiles",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_DependencyId",
                table: "Users",
                column: "DependencyId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UsersOneTimePass_UserId",
                table: "UsersOneTimePass",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserWalletAccounts_UserId",
                table: "UserWalletAccounts",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserWalletAccountTransactions_TransactionTypeId",
                table: "UserWalletAccountTransactions",
                column: "TransactionTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_UserWalletAccountTransactions_UserWalletAccountId",
                table: "UserWalletAccountTransactions",
                column: "UserWalletAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_WalletAccountWords_UserWalletAccountId",
                table: "WalletAccountWords",
                column: "UserWalletAccountId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Profitabilities");

            migrationBuilder.DropTable(
                name: "UserInvestmentProjects");

            migrationBuilder.DropTable(
                name: "UserProfiles");

            migrationBuilder.DropTable(
                name: "UsersOneTimePass");

            migrationBuilder.DropTable(
                name: "WalletAccountWords");

            migrationBuilder.DropTable(
                name: "Projects");

            migrationBuilder.DropTable(
                name: "UserWalletAccountTransactions");

            migrationBuilder.DropTable(
                name: "Profiles");

            migrationBuilder.DropTable(
                name: "ProjectTypes");

            migrationBuilder.DropTable(
                name: "TransactionTypes");

            migrationBuilder.DropTable(
                name: "UserWalletAccounts");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Dependencies");
        }
    }
}

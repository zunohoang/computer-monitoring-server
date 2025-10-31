using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ComputerMonitoringServerAPI.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "alerts",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    code = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    description = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    severity = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_alerts", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "images",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    url = table.Column<string>(type: "text", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    meta = table.Column<string>(type: "text", nullable: true),
                    status = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_images", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "process_blacklists",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_process_blacklists", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    username = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    email = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    password_hash = table.Column<string>(type: "text", nullable: false),
                    role = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    full_name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    new_column = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "contests",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    start_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    end_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    created_by = table.Column<long>(type: "bigint", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_contests", x => x.id);
                    table.ForeignKey(
                        name: "FK_contests_users_created_by",
                        column: x => x.created_by,
                        principalTable: "users",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "contest_process_blacklist",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    process_id = table.Column<long>(type: "bigint", nullable: false),
                    contest_id = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_contest_process_blacklist", x => x.id);
                    table.ForeignKey(
                        name: "FK_contest_process_blacklist_contests_contest_id",
                        column: x => x.contest_id,
                        principalTable: "contests",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_contest_process_blacklist_process_blacklists_process_id",
                        column: x => x.process_id,
                        principalTable: "process_blacklists",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "contest_sbd",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    sbd = table.Column<long>(type: "bigint", nullable: false),
                    full_name = table.Column<string>(type: "text", nullable: false),
                    contest_id = table.Column<long>(type: "bigint", nullable: false),
                    user_id = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_contest_sbd", x => x.id);
                    table.ForeignKey(
                        name: "FK_contest_sbd_contests_contest_id",
                        column: x => x.contest_id,
                        principalTable: "contests",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_contest_sbd_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "rooms",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    contest_id = table.Column<long>(type: "bigint", nullable: false),
                    access_code = table.Column<string>(type: "text", nullable: false),
                    rg_start_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    rg_end_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    capacity = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rooms", x => x.id);
                    table.ForeignKey(
                        name: "FK_rooms_contests_contest_id",
                        column: x => x.contest_id,
                        principalTable: "contests",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "attempt",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    contest_id = table.Column<long>(type: "bigint", nullable: false),
                    sbd = table.Column<long>(type: "bigint", nullable: false),
                    ip_ad = table.Column<string>(type: "text", nullable: false),
                    location = table.Column<string>(type: "text", nullable: true),
                    room_id = table.Column<long>(type: "bigint", nullable: true),
                    status = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    started_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ended_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_attempt", x => x.id);
                    table.ForeignKey(
                        name: "FK_attempt_contest_sbd_sbd",
                        column: x => x.sbd,
                        principalTable: "contest_sbd",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_attempt_contests_contest_id",
                        column: x => x.contest_id,
                        principalTable: "contests",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_attempt_rooms_room_id",
                        column: x => x.room_id,
                        principalTable: "rooms",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "messages",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    type = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    content = table.Column<string>(type: "text", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    created_by = table.Column<long>(type: "bigint", nullable: false),
                    room_id = table.Column<long>(type: "bigint", nullable: false),
                    attempt_id = table.Column<long>(type: "bigint", nullable: false),
                    contest_id = table.Column<long>(type: "bigint", nullable: false),
                    title = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_messages", x => x.id);
                    table.ForeignKey(
                        name: "FK_messages_attempt_attempt_id",
                        column: x => x.attempt_id,
                        principalTable: "attempt",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_messages_contests_contest_id",
                        column: x => x.contest_id,
                        principalTable: "contests",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_messages_rooms_room_id",
                        column: x => x.room_id,
                        principalTable: "rooms",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_messages_users_created_by",
                        column: x => x.created_by,
                        principalTable: "users",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "process",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    pid = table.Column<int>(type: "integer", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    parent_id = table.Column<long>(type: "bigint", nullable: true),
                    start_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    end_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    data = table.Column<string>(type: "text", nullable: true),
                    attempt_id = table.Column<long>(type: "bigint", nullable: true),
                    status = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_process", x => x.id);
                    table.ForeignKey(
                        name: "FK_process_attempt_attempt_id",
                        column: x => x.attempt_id,
                        principalTable: "attempt",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "violations",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    severity = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    note = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    handled = table.Column<bool>(type: "boolean", nullable: false),
                    handled_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    handled_by = table.Column<long>(type: "bigint", nullable: false),
                    attempt_id = table.Column<long>(type: "bigint", nullable: false),
                    alert_id = table.Column<long>(type: "bigint", nullable: false),
                    created_by = table.Column<long>(type: "bigint", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    log_start_time = table.Column<long>(type: "bigint", nullable: false),
                    log_end_time = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_violations", x => x.id);
                    table.ForeignKey(
                        name: "FK_violations_alerts_alert_id",
                        column: x => x.alert_id,
                        principalTable: "alerts",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_violations_attempt_attempt_id",
                        column: x => x.attempt_id,
                        principalTable: "attempt",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_violations_users_created_by",
                        column: x => x.created_by,
                        principalTable: "users",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_violations_users_handled_by",
                        column: x => x.handled_by,
                        principalTable: "users",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "audit_logs",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    type = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    attempt_id = table.Column<long>(type: "bigint", nullable: true),
                    process_id = table.Column<long>(type: "bigint", nullable: true),
                    image_id = table.Column<long>(type: "bigint", nullable: true),
                    alert_id = table.Column<long>(type: "bigint", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    details = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_audit_logs", x => x.id);
                    table.ForeignKey(
                        name: "FK_audit_logs_alerts_alert_id",
                        column: x => x.alert_id,
                        principalTable: "alerts",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_audit_logs_attempt_attempt_id",
                        column: x => x.attempt_id,
                        principalTable: "attempt",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_audit_logs_images_image_id",
                        column: x => x.image_id,
                        principalTable: "images",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_audit_logs_process_process_id",
                        column: x => x.process_id,
                        principalTable: "process",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_attempt_contest_id",
                table: "attempt",
                column: "contest_id");

            migrationBuilder.CreateIndex(
                name: "IX_attempt_room_id",
                table: "attempt",
                column: "room_id");

            migrationBuilder.CreateIndex(
                name: "IX_attempt_sbd",
                table: "attempt",
                column: "sbd");

            migrationBuilder.CreateIndex(
                name: "IX_audit_logs_alert_id",
                table: "audit_logs",
                column: "alert_id");

            migrationBuilder.CreateIndex(
                name: "IX_audit_logs_attempt_id",
                table: "audit_logs",
                column: "attempt_id");

            migrationBuilder.CreateIndex(
                name: "IX_audit_logs_image_id",
                table: "audit_logs",
                column: "image_id");

            migrationBuilder.CreateIndex(
                name: "IX_audit_logs_process_id",
                table: "audit_logs",
                column: "process_id");

            migrationBuilder.CreateIndex(
                name: "IX_audit_logs_type",
                table: "audit_logs",
                column: "type");

            migrationBuilder.CreateIndex(
                name: "IX_contest_process_blacklist_contest_id",
                table: "contest_process_blacklist",
                column: "contest_id");

            migrationBuilder.CreateIndex(
                name: "IX_contest_process_blacklist_process_id",
                table: "contest_process_blacklist",
                column: "process_id");

            migrationBuilder.CreateIndex(
                name: "IX_contest_sbd_contest_id",
                table: "contest_sbd",
                column: "contest_id");

            migrationBuilder.CreateIndex(
                name: "IX_contest_sbd_sbd",
                table: "contest_sbd",
                column: "sbd",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_contest_sbd_user_id",
                table: "contest_sbd",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_contests_created_by",
                table: "contests",
                column: "created_by");

            migrationBuilder.CreateIndex(
                name: "IX_messages_attempt_id",
                table: "messages",
                column: "attempt_id");

            migrationBuilder.CreateIndex(
                name: "IX_messages_contest_id",
                table: "messages",
                column: "contest_id");

            migrationBuilder.CreateIndex(
                name: "IX_messages_created_by",
                table: "messages",
                column: "created_by");

            migrationBuilder.CreateIndex(
                name: "IX_messages_room_id",
                table: "messages",
                column: "room_id");

            migrationBuilder.CreateIndex(
                name: "IX_process_attempt_id",
                table: "process",
                column: "attempt_id");

            migrationBuilder.CreateIndex(
                name: "IX_process_name",
                table: "process",
                column: "name");

            migrationBuilder.CreateIndex(
                name: "IX_process_blacklists_name",
                table: "process_blacklists",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_rooms_access_code",
                table: "rooms",
                column: "access_code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_rooms_contest_id",
                table: "rooms",
                column: "contest_id");

            migrationBuilder.CreateIndex(
                name: "IX_users_email",
                table: "users",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_users_username",
                table: "users",
                column: "username",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_violations_alert_id",
                table: "violations",
                column: "alert_id");

            migrationBuilder.CreateIndex(
                name: "IX_violations_attempt_id",
                table: "violations",
                column: "attempt_id");

            migrationBuilder.CreateIndex(
                name: "IX_violations_created_by",
                table: "violations",
                column: "created_by");

            migrationBuilder.CreateIndex(
                name: "IX_violations_handled_by",
                table: "violations",
                column: "handled_by");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "audit_logs");

            migrationBuilder.DropTable(
                name: "contest_process_blacklist");

            migrationBuilder.DropTable(
                name: "messages");

            migrationBuilder.DropTable(
                name: "violations");

            migrationBuilder.DropTable(
                name: "images");

            migrationBuilder.DropTable(
                name: "process");

            migrationBuilder.DropTable(
                name: "process_blacklists");

            migrationBuilder.DropTable(
                name: "alerts");

            migrationBuilder.DropTable(
                name: "attempt");

            migrationBuilder.DropTable(
                name: "contest_sbd");

            migrationBuilder.DropTable(
                name: "rooms");

            migrationBuilder.DropTable(
                name: "contests");

            migrationBuilder.DropTable(
                name: "users");
        }
    }
}

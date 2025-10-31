#!/bin/bash

# ====================================
# Script chạy migration tự động
# Được thực thi bởi Docker container khi khởi động
# ====================================

set -e  # Exit ngay khi có lỗi

echo "========================================="
echo "🚀 Starting Database Migration Script"
echo "========================================="

# Kiểm tra biến môi trường Connection String
if [ -z "$ConnectionStrings__DefaultConnection" ]; then
    echo "❌ Error: ConnectionStrings__DefaultConnection is not set!"
    exit 1
fi

echo "✅ Connection string is configured"
echo "📦 Database: $ConnectionStrings__DefaultConnection"

# Chờ PostgreSQL sẵn sàng (tối đa 60 giây)
echo ""
echo "⏳ Waiting for PostgreSQL to be ready..."
counter=0
max_attempts=60

while [ $counter -lt $max_attempts ]; do
    # Parse connection string để lấy host
    DB_HOST=$(echo $ConnectionStrings__DefaultConnection | sed -n 's/.*Host=\([^;]*\).*/\1/p')
    DB_PORT=${DB_PORT:-5432}
    
    if pg_isready -h "$DB_HOST" -p "$DB_PORT" > /dev/null 2>&1; then
        echo "✅ PostgreSQL is ready!"
        break
    fi
    
    counter=$((counter + 1))
    echo "Waiting... ($counter/$max_attempts)"
    sleep 1
done

if [ $counter -eq $max_attempts ]; then
    echo "❌ Error: PostgreSQL is not ready after $max_attempts seconds"
    exit 1
fi

# Chạy migration
echo ""
echo "🔧 Running Entity Framework migrations..."
echo "========================================="

if dotnet ef database update --no-build --verbose; then
    echo ""
    echo "========================================="
    echo "✅ Migration completed successfully!"
    echo "========================================="
else
    echo ""
    echo "========================================="
    echo "❌ Migration failed!"
    echo "========================================="
    exit 1
fi

echo ""
echo "🎉 Database is ready for use!"
echo ""

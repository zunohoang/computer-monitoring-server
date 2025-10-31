@echo off
REM ====================================
REM Script chạy migration trên Windows
REM ====================================

echo =========================================
echo Starting Database Migration Script
echo =========================================

REM Kiểm tra biến môi trường
if "%ConnectionStrings__DefaultConnection%"=="" (
    echo Error: ConnectionStrings__DefaultConnection is not set!
    exit /b 1
)

echo Connection string is configured
echo.

REM Chạy migration
echo Running Entity Framework migrations...
echo =========================================
dotnet ef database update --no-build --verbose

if %ERRORLEVEL% EQU 0 (
    echo.
    echo =========================================
    echo Migration completed successfully!
    echo =========================================
) else (
    echo.
    echo =========================================
    echo Migration failed!
    echo =========================================
    exit /b 1
)

echo.
echo Database is ready for use!
echo.

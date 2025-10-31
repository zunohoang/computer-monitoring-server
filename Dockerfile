# ====================================
# Stage 1: Build
# ====================================
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj và restore dependencies
COPY ["ComputerMonitoringServerAPI/ComputerMonitoringServerAPI.csproj", "ComputerMonitoringServerAPI/"]
RUN dotnet restore "ComputerMonitoringServerAPI/ComputerMonitoringServerAPI.csproj"

# Copy toàn bộ source code
COPY . .
WORKDIR "/src/ComputerMonitoringServerAPI"

# Build project
RUN dotnet build "ComputerMonitoringServerAPI.csproj" -c Release -o /app/build

# Cài đặt EF Core tools trong build stage
RUN dotnet tool install --global dotnet-ef --version 8.0.0
ENV PATH="${PATH}:/root/.dotnet/tools"

# ====================================
# Stage 2: Publish
# ====================================
FROM build AS publish
RUN dotnet publish "ComputerMonitoringServerAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

# ====================================
# Stage 3: Runtime (Final)
# ====================================
FROM mcr.microsoft.com/dotnet/sdk:8.0-jammy AS final
WORKDIR /app

# Cài đặt các dependencies cần thiết
RUN apt-get update && apt-get install -y \
    curl \
    postgresql-client \
    && rm -rf /var/lib/apt/lists/*

# Cài đặt dotnet-ef tools
RUN dotnet tool install --global dotnet-ef --version 8.0.0
ENV PATH="${PATH}:/root/.dotnet/tools"

# Expose ports
EXPOSE 8080
EXPOSE 8081

# Copy published files
COPY --from=publish /app/publish .

# Copy source code cần thiết cho migration
COPY --from=build /src/ComputerMonitoringServerAPI/ComputerMonitoringServerAPI.csproj ./
COPY --from=build /src/ComputerMonitoringServerAPI/Migrations ./Migrations
COPY --from=build /src/ComputerMonitoringServerAPI/Data ./Data
COPY --from=build /src/ComputerMonitoringServerAPI/Models ./Models

# Thiết lập entrypoint
ENTRYPOINT ["dotnet", "ComputerMonitoringServerAPI.dll"]

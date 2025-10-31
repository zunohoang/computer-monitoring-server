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

# ====================================
# Stage 2: Publish
# ====================================
FROM build AS publish
RUN dotnet publish "ComputerMonitoringServerAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

# ====================================
# Stage 3: Install EF Core Tools (cho migration)
# ====================================
FROM build AS ef-tools
RUN dotnet tool install --global dotnet-ef --version 8.0.0
ENV PATH="${PATH}:/root/.dotnet/tools"

# ====================================
# Stage 4: Runtime (Final)
# ====================================
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Cài đặt dotnet-ef để chạy migration trong production
RUN apt-get update && apt-get install -y curl \
    && curl -sSL https://dot.net/v1/dotnet-install.sh | bash /dev/stdin --channel 8.0 --install-dir /usr/share/dotnet \
    && ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet \
    && dotnet tool install --global dotnet-ef --version 8.0.0
ENV PATH="${PATH}:/root/.dotnet/tools"

# Expose ports
EXPOSE 8080
EXPOSE 8081

# Copy published files
COPY --from=publish /app/publish .

# Copy migration files (quan trọng cho việc chạy migration)
COPY --from=build /src/ComputerMonitoringServerAPI/Migrations ./Migrations

# Thiết lập entrypoint
ENTRYPOINT ["dotnet", "ComputerMonitoringServerAPI.dll"]

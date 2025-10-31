# 🚀 Computer Monitoring Server API - CI/CD Pipeline

Hệ thống CI/CD tự động cho dự án ASP.NET Core Web API với PostgreSQL, Docker, và GitHub Actions.

## 📋 Mục lục

- [Tổng quan](#tổng-quan)
- [Kiến trúc hệ thống](#kiến-trúc-hệ-thống)
- [Yêu cầu](#yêu-cầu)
- [Cấu hình GitHub Secrets](#cấu-hình-github-secrets)
- [Cấu hình VPS](#cấu-hình-vps)
- [Triển khai](#triển-khai)
- [Cấu trúc file](#cấu-trúc-file)
- [Xử lý sự cố](#xử-lý-sự-cố)

---

## 🎯 Tổng quan

Pipeline CI/CD này tự động hóa quá trình:

- ✅ Build project ASP.NET Core
- ✅ Tạo Docker image
- ✅ Push image lên Docker Hub
- ✅ Deploy lên VPS Ubuntu
- ✅ Chạy migration tự động
- ✅ Khởi động lại container

**Khi push code lên `main` branch** → Tự động deploy lên production!

---

## 🏗️ Kiến trúc hệ thống

```
┌─────────────────┐
│  GitHub Repo    │
│   (main branch) │
└────────┬────────┘
         │ push
         ▼
┌─────────────────┐
│ GitHub Actions  │
│  - Build        │
│  - Test         │
│  - Docker Build │
└────────┬────────┘
         │
         ▼
┌─────────────────┐
│   Docker Hub    │
│  (Image Store)  │
└────────┬────────┘
         │
         ▼
┌─────────────────┐         ┌──────────────┐
│   VPS Ubuntu    │────────▶│  PostgreSQL  │
│  - API Server   │         │   Database   │
│  - Docker       │         └──────────────┘
└─────────────────┘
```

---

## ✅ Yêu cầu

### VPS Ubuntu

- Ubuntu 22.04 hoặc mới hơn
- Docker 20.10+ đã cài đặt
- Docker Compose V2 đã cài đặt
- SSH access với key authentication

### GitHub Repository

- Repository với quyền Settings để thêm Secrets
- Branch `main` là production branch

### Docker Hub

- Tài khoản Docker Hub (miễn phí)
- Access token hoặc password

---

## 🔐 Cấu hình GitHub Secrets

Vào repository của bạn trên GitHub:

**Settings** → **Secrets and variables** → **Actions** → **New repository secret**

Thêm các secrets sau:

| Secret Name         | Mô tả                            | Ví dụ                                         |
| ------------------- | -------------------------------- | --------------------------------------------- |
| `DOCKER_USERNAME`   | Username Docker Hub              | `your-dockerhub-username`                     |
| `DOCKER_PASSWORD`   | Password hoặc Access Token       | `dckr_pat_xxxxxxxxxxxxx`                      |
| `VPS_IP`            | Địa chỉ IP của VPS               | `192.168.1.100`                               |
| `VPS_USER`          | Username SSH trên VPS            | `root` hoặc `ubuntu`                          |
| `VPS_PASSWORD`      | Password SSH trên VPS            | `your-vps-password`                           |
| `VPS_PORT`          | SSH port (optional, default: 22) | `22`                                          |
| `POSTGRES_USER`     | PostgreSQL username              | `postgres`                                    |
| `POSTGRES_PASSWORD` | PostgreSQL password              | `your-secure-password`                        |
| `JWT_SECRET_KEY`    | JWT secret key (min 32 chars)    | `your-super-secret-jwt-key-here-32-chars-min` |

### 📝 Lưu ý về SSH Password Authentication

**⚠️ Security Warning:** Sử dụng password authentication kém bảo mật hơn SSH key. Nên:

- Sử dụng password mạnh (min 16 ký tự, bao gồm chữ hoa, chữ thường, số, ký tự đặc biệt)
- Cân nhắc sử dụng SSH key thay vì password cho bảo mật tốt hơn
- Giới hạn số lần thử login thất bại trên VPS
- Cân nhắc đổi SSH port mặc định (22) sang port khác

### 📝 Cách lấy SSH Key

Trên máy local của bạn:

```bash
# Tạo SSH key mới (nếu chưa có)
ssh-keygen -t rsa -b 4096 -C "your-email@example.com"

# Copy public key lên VPS
ssh-copy-id -i ~/.ssh/id_rsa.pub user@vps-ip

# Copy TOÀN BỘ nội dung private key để paste vào GitHub Secret
cat ~/.ssh/id_rsa
```

**⚠️ Lưu ý:** Copy toàn bộ nội dung file, bao gồm:

```
-----BEGIN OPENSSH PRIVATE KEY-----
[nội dung key]
-----END OPENSSH PRIVATE KEY-----
```

---

## 🖥️ Cấu hình VPS

### 1. Enable SSH Password Authentication

```bash
# SSH vào VPS
ssh user@your-vps-ip

# Backup SSH config
sudo cp /etc/ssh/sshd_config /etc/ssh/sshd_config.backup

# Edit SSH config
sudo nano /etc/ssh/sshd_config

# Tìm và sửa dòng sau:
# PasswordAuthentication yes

# Lưu file (Ctrl+X, Y, Enter)

# Restart SSH service
sudo systemctl restart sshd
```

**⚠️ Lưu ý:** Đọc thêm tại [docs/SSH_PASSWORD_SETUP.md](./docs/SSH_PASSWORD_SETUP.md) để biết cách tăng bảo mật.

### 2. Cài đặt Docker và Docker Compose

```bash
# Cập nhật hệ thống
sudo apt update && sudo apt upgrade -y

# Cài đặt Docker
curl -fsSL https://get.docker.com -o get-docker.sh
sudo sh get-docker.sh

# Thêm user vào docker group
sudo usermod -aG docker $USER

# Cài đặt Docker Compose V2
sudo apt install docker-compose-plugin -y

# Logout và login lại để group có hiệu lực
exit

# Login lại và kiểm tra cài đặt
docker --version
docker compose version
```

### 3. Tạo thư mục cho project

```bash
mkdir -p ~/computer-monitoring-api
cd ~/computer-monitoring-api
```

### 4. Clone repository (lần đầu)

```bash
# Clone repository
git clone https://github.com/your-username/your-repo-name.git .

# Tạo file .env (GitHub Actions sẽ tự động tạo sau)
cp .env.example .env
```

### 5. Cấu hình Firewall (nếu có)

```bash
# Cho phép port SSH
sudo ufw allow 22/tcp

# Cho phép port API (5000)
sudo ufw allow 5000/tcp

# Cho phép port PostgreSQL (nếu cần truy cập từ bên ngoài)
sudo ufw allow 5432/tcp

# Enable firewall
sudo ufw enable
```

---

## 🚀 Triển khai

### Triển khai tự động (Recommended)

**Chỉ cần push code lên branch `main`:**

```bash
git add .
git commit -m "Deploy to production"
git push origin main
```

GitHub Actions sẽ tự động:

1. ✅ Build và test project
2. ✅ Tạo Docker image
3. ✅ Push lên Docker Hub
4. ✅ Deploy lên VPS
5. ✅ Chạy migration
6. ✅ Khởi động container

### Triển khai thủ công (Manual)

Nếu cần deploy thủ công trên VPS:

```bash
# SSH vào VPS
ssh user@your-vps-ip

# Di chuyển vào thư mục project
cd ~/computer-monitoring-api

# Pull code mới nhất
git pull origin main

# Pull Docker image mới nhất
docker pull your-dockerhub-username/computer-monitoring-api:latest

# Dừng và xóa container cũ
docker-compose down

# Khởi động lại với image mới
docker-compose up -d

# Kiểm tra logs
docker-compose logs -f
```

---

## 📁 Cấu trúc file

```
ComputerMonitoringServerAPI/
├── .github/
│   └── workflows/
│       └── deploy.yml              # GitHub Actions workflow
├── ComputerMonitoringServerAPI/
│   ├── Controllers/
│   ├── Models/
│   ├── Services/
│   ├── Data/
│   ├── Migrations/
│   └── Program.cs
├── scripts/
│   ├── migrate.sh                  # Script migration cho Linux
│   └── migrate.bat                 # Script migration cho Windows
├── Dockerfile                      # Docker multi-stage build
├── docker-compose.yml              # Docker Compose configuration
├── .env.example                    # Template cho environment variables
├── .gitignore                      # Git ignore rules
└── DEPLOYMENT.md                   # File này
```

---

## 🔍 Kiểm tra trạng thái

### Trên GitHub

- Vào **Actions** tab để xem workflow runs
- Màu xanh ✅ = Success
- Màu đỏ ❌ = Failed

### Trên VPS

```bash
# Kiểm tra container đang chạy
docker-compose ps

# Xem logs API
docker-compose logs api -f

# Xem logs PostgreSQL
docker-compose logs postgres -f

# Kiểm tra health của container
docker-compose exec api curl http://localhost:8080/health

# Kiểm tra database connection
docker-compose exec postgres psql -U postgres -d ComputerMonitoring -c "\dt"
```

### Test API

```bash
# Health check
curl http://your-vps-ip:5000/health

# Swagger UI (nếu enable trong production)
http://your-vps-ip:5000/swagger
```

---

## 🛠️ Xử lý sự cố

### Vấn đề 1: GitHub Actions failed tại bước "Build and push Docker image"

**Nguyên nhân:** Sai thông tin Docker Hub credentials

**Giải pháp:**

- Kiểm tra lại `DOCKER_USERNAME` và `DOCKER_PASSWORD` trong GitHub Secrets
- Đảm bảo Docker Hub access token còn hiệu lực

### Vấn đề 2: SSH connection failed

**Nguyên nhân:** Sai SSH key hoặc không có quyền truy cập

**Giải pháp:**

````bash
### Vấn đề 2: SSH connection failed

**Nguyên nhân:** Sai username/password hoặc không có quyền truy cập

**Giải pháp:**
```bash
# Trên máy local, test SSH connection
ssh user@vps-ip

# Nhập password khi được yêu cầu

# Kiểm tra password authentication có được enable trên VPS
# SSH vào VPS và kiểm tra:
sudo nano /etc/ssh/sshd_config
# Đảm bảo: PasswordAuthentication yes

# Restart SSH service nếu có thay đổi
sudo systemctl restart sshd
````

**Nếu cần enable password authentication:**

```bash
# Trên VPS
sudo nano /etc/ssh/sshd_config

# Tìm và sửa dòng sau:
# PasswordAuthentication no
# Thành:
PasswordAuthentication yes

# Lưu file và restart SSH
sudo systemctl restart sshd
```

````

### Vấn đề 3: Migration failed

**Nguyên nhân:**

- Database connection string sai
- PostgreSQL chưa sẵn sàng
- Migration files bị thiếu

**Giải pháp:**

```bash
# SSH vào VPS
cd ~/computer-monitoring-api

# Kiểm tra logs chi tiết
docker-compose logs api

# Thử chạy migration thủ công
docker-compose exec api dotnet ef database update --verbose

# Kiểm tra PostgreSQL
docker-compose exec postgres psql -U postgres -l
````

### Vấn đề 4: Container không start

**Nguyên nhân:** Port đã được sử dụng hoặc thiếu environment variables

**Giải pháp:**

```bash
# Kiểm tra port đang được sử dụng
sudo netstat -tulpn | grep :5000
sudo netstat -tulpn | grep :5432

# Kill process nếu cần
sudo kill -9 <PID>

# Kiểm tra .env file
cat .env

# Restart containers
docker-compose down
docker-compose up -d
```

### Vấn đề 5: Database data bị mất

**Nguyên nhân:** Volume không được persist đúng cách

**Giải pháp:**

```bash
# Kiểm tra volumes
docker volume ls

# Backup database
docker-compose exec postgres pg_dump -U postgres ComputerMonitoring > backup.sql

# Restore database
docker-compose exec -T postgres psql -U postgres ComputerMonitoring < backup.sql
```

---

## 📊 Monitoring và Logs

### Xem logs realtime

```bash
# Tất cả services
docker-compose logs -f

# Chỉ API
docker-compose logs -f api

# Chỉ PostgreSQL
docker-compose logs -f postgres

# 100 dòng log gần nhất
docker-compose logs --tail=100 api
```

### Kiểm tra resource usage

```bash
# CPU và Memory usage
docker stats

# Disk usage
docker system df
```

---

## 🔄 Rollback

Nếu deployment có vấn đề, rollback về version trước:

```bash
# SSH vào VPS
cd ~/computer-monitoring-api

# Checkout về commit trước
git log --oneline -n 5  # Xem 5 commit gần nhất
git checkout <commit-hash>

# Pull image cũ (nếu còn trên Docker Hub)
docker pull your-dockerhub-username/computer-monitoring-api:<old-tag>

# Update docker-compose.yml với tag cũ
nano docker-compose.yml
# Sửa: image: your-dockerhub-username/computer-monitoring-api:<old-tag>

# Restart
docker-compose down
docker-compose up -d
```

---

## 🎯 Best Practices

1. **Luôn test trên môi trường local trước khi push**

   ```bash
   docker-compose -f docker-compose.yml up --build
   ```

2. **Sử dụng versioning cho Docker images**

   - Thêm tags dựa trên version hoặc commit hash
   - Không chỉ dùng `latest`

3. **Backup database thường xuyên**

   ```bash
   # Tạo cron job backup hàng ngày
   crontab -e
   # Thêm: 0 2 * * * cd ~/computer-monitoring-api && docker-compose exec postgres pg_dump -U postgres ComputerMonitoring > ~/backups/db_$(date +\%Y\%m\%d).sql
   ```

4. **Monitor logs và resources**

   - Sử dụng tools như Grafana, Prometheus
   - Setup alerting cho downtime

5. **Security**
   - Luôn sử dụng strong passwords
   - Định kỳ rotate SSH keys và JWT secrets
   - Keep Docker và packages updated

---

## 📞 Support

Nếu gặp vấn đề, hãy:

1. Kiểm tra logs: `docker-compose logs -f`
2. Kiểm tra GitHub Actions logs
3. Tham khảo phần "Xử lý sự cố" ở trên

---

## 📝 License

MIT License - Computer Monitoring Server API

---

**🎉 Chúc bạn deploy thành công!**

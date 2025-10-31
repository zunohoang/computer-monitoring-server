# 🚀 Quick Start - CI/CD Setup

Hướng dẫn nhanh thiết lập CI/CD trong 10 phút!

## ✅ Checklist

- [ ] VPS Ubuntu 22.04 với Docker đã cài đặt
- [ ] Tài khoản Docker Hub
- [ ] Repository GitHub
- [ ] VPS username và password để truy cập SSH

## 📝 Các bước thực hiện

### Bước 1: Cấu hình GitHub Secrets (5 phút)

Vào **GitHub Repository → Settings → Secrets and variables → Actions**

Thêm các secrets sau:

```
DOCKER_USERNAME      = your-dockerhub-username
DOCKER_PASSWORD      = your-dockerhub-password-or-token
VPS_IP              = 192.168.1.100
VPS_USER            = ubuntu
VPS_PASSWORD        = your-vps-password
VPS_PORT            = 22
POSTGRES_USER       = postgres
POSTGRES_PASSWORD   = your-secure-password
JWT_SECRET_KEY      = your-super-secret-jwt-key-min-32-chars
```

### Bước 2: Enable SSH Password Authentication trên VPS (2 phút)

```bash
# SSH vào VPS
ssh user@vps-ip

# Edit SSH config
sudo nano /etc/ssh/sshd_config

# Đảm bảo dòng sau được enable:
# PasswordAuthentication yes

# Restart SSH service
sudo systemctl restart sshd
```

### Bước 3: Chuẩn bị VPS (3 phút)

```bash
# SSH vào VPS
ssh user@vps-ip

# Cài Docker (nếu chưa có)
curl -fsSL https://get.docker.com -o get-docker.sh
sudo sh get-docker.sh
sudo usermod -aG docker $USER

# Cài Docker Compose
sudo apt install docker-compose-plugin -y

# Tạo thư mục project
mkdir -p ~/computer-monitoring-api
cd ~/computer-monitoring-api

# Cấu hình firewall
sudo ufw allow 22/tcp
sudo ufw allow 5000/tcp
sudo ufw enable
```

### Bước 4: Deploy (2 phút)

```bash
# Trên máy local, commit và push code
git add .
git commit -m "Setup CI/CD pipeline"
git push origin main
```

Xong! 🎉 GitHub Actions sẽ tự động deploy.

### Bước 5: Kiểm tra (1 phút)

```bash
# Xem GitHub Actions
# Vào: https://github.com/your-username/your-repo/actions

# SSH vào VPS kiểm tra
ssh user@vps-ip
cd ~/computer-monitoring-api
docker-compose ps

# Test API
curl http://vps-ip:5000/health
```

## 🔧 Troubleshooting nhanh

### Lỗi: "Permission denied (password)"

```bash
# Kiểm tra password đúng chưa
ssh user@vps-ip

# Kiểm tra password authentication đã enable trên VPS
sudo grep "PasswordAuthentication" /etc/ssh/sshd_config

# Nếu chưa, enable nó:
sudo nano /etc/ssh/sshd_config
# Sửa: PasswordAuthentication yes
sudo systemctl restart sshd
```

### Lỗi: "Cannot connect to Docker daemon"

```bash
# Logout và login lại để group có hiệu lực
exit
ssh user@vps-ip
```

### Lỗi: Migration failed

```bash
# Kiểm tra logs
docker-compose logs api

# Chạy migration thủ công
docker-compose exec api dotnet ef database update
```

## 📚 Tài liệu chi tiết

Xem file [DEPLOYMENT.md](./DEPLOYMENT.md) để biết thêm chi tiết!

---

**Sau khi setup xong, mỗi lần push code lên `main` sẽ tự động deploy! 🚀**

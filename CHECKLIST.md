# ✅ CI/CD Setup Checklist

Checklist đầy đủ để thiết lập CI/CD cho dự án Computer Monitoring Server API.

## 📋 Pre-deployment Checklist

### 1. VPS Setup

- [ ] VPS Ubuntu 22.04+ đã được chuẩn bị
- [ ] SSH access với username và password đã được thiết lập
- [ ] Password authentication đã được enable trên VPS
- [ ] Docker đã được cài đặt (`docker --version`)
- [ ] Docker Compose đã được cài đặt (`docker compose version`)
- [ ] Firewall đã được cấu hình (ports 22, 5000, 5432)
- [ ] User đã được thêm vào docker group

### 2. Docker Hub Setup

- [ ] Tài khoản Docker Hub đã được tạo
- [ ] Access Token đã được tạo
- [ ] Repository `computer-monitoring-api` đã được tạo (optional)
- [ ] Đã test login: `docker login -u username`

### 3. GitHub Repository Setup

- [ ] Repository đã tồn tại trên GitHub
- [ ] Branch `main` đã được tạo
- [ ] Có quyền Settings để thêm Secrets

### 4. GitHub Secrets Configuration

- [ ] `DOCKER_USERNAME` đã được thêm
- [ ] `DOCKER_PASSWORD` đã được thêm
- [ ] `VPS_IP` đã được thêm
- [ ] `VPS_USER` đã được thêm
- [ ] `VPS_PASSWORD` đã được thêm (SSH password)
- [ ] `VPS_PORT` đã được thêm (nếu khác 22)
- [ ] `POSTGRES_USER` đã được thêm
- [ ] `POSTGRES_PASSWORD` đã được thêm
- [ ] `JWT_SECRET_KEY` đã được thêm (min 32 chars)

## 📁 Files Checklist

### Required Files

- [ ] `Dockerfile` - Multi-stage build cho ASP.NET Core
- [ ] `docker-compose.yml` - Orchestration cho API + PostgreSQL
- [ ] `.github/workflows/deploy.yml` - GitHub Actions workflow
- [ ] `.env.example` - Template cho environment variables
- [ ] `.gitignore` - Đã cập nhật với Docker rules
- [ ] `.dockerignore` - Tối ưu Docker build context

### Documentation Files

- [ ] `DEPLOYMENT.md` - Hướng dẫn chi tiết
- [ ] `QUICK_START.md` - Hướng dẫn nhanh
- [ ] `docs/DOCKER_HUB_SETUP.md` - Setup Docker Hub

### Scripts (Optional)

- [ ] `scripts/migrate.sh` - Migration script cho Linux
- [ ] `scripts/migrate.bat` - Migration script cho Windows

## 🧪 Testing Checklist

### Local Testing

- [ ] Build Docker image local: `docker build -t test-api .`
- [ ] Run docker compose local: `docker compose up`
- [ ] Kiểm tra API health: `curl http://localhost:5000/health`
- [ ] Kiểm tra Swagger: `http://localhost:5000/swagger`
- [ ] Test database connection
- [ ] Test migration chạy tự động

### GitHub Actions Testing

- [ ] Push code lên branch test trước
- [ ] Kiểm tra workflow chạy thành công
- [ ] Xem logs chi tiết từng bước
- [ ] Verify image đã được push lên Docker Hub

### VPS Testing

- [ ] SSH vào VPS thành công
- [ ] Docker commands chạy được (không cần sudo)
- [ ] Thư mục project đã được tạo
- [ ] File .env đã được tạo với đúng values
- [ ] Containers đã được start
- [ ] Health check pass
- [ ] API có thể truy cập từ bên ngoài

## 🚀 First Deployment Checklist

### Before Push

- [ ] Đã review tất cả code changes
- [ ] Đã test trên local environment
- [ ] Database migrations đã được tạo
- [ ] Secrets đã được verify
- [ ] Backup VPS trước khi deploy (nếu có data quan trọng)

### During Deployment

- [ ] Push code: `git push origin main`
- [ ] Monitor GitHub Actions workflow
- [ ] Kiểm tra từng step: Build → Docker → Deploy
- [ ] Xem logs realtime trên VPS

### After Deployment

- [ ] Verify GitHub Actions status: ✅ Success
- [ ] Kiểm tra containers trên VPS: `docker compose ps`
- [ ] Test API endpoints
- [ ] Kiểm tra database đã chạy migration
- [ ] Xem logs: `docker compose logs -f`
- [ ] Test một số features chính

## 🔍 Post-deployment Verification

### API Verification

```bash
# Health check
curl http://vps-ip:5000/health

# Test endpoints
curl http://vps-ip:5000/api/auth/login -X POST \
  -H "Content-Type: application/json" \
  -d '{"username":"test","password":"test"}'
```

### Database Verification

```bash
# SSH vào VPS
docker compose exec postgres psql -U postgres -d ComputerMonitoring

# Kiểm tra tables
\dt

# Kiểm tra migrations
SELECT * FROM "__EFMigrationsHistory";

# Exit
\q
```

### Container Health

```bash
# Check status
docker compose ps

# Check resources
docker stats

# Check logs
docker compose logs --tail=100
```

## 📊 Monitoring Setup (Post-deployment)

- [ ] Setup log aggregation (optional)
- [ ] Setup monitoring dashboard (optional)
- [ ] Configure alerts cho downtime (optional)
- [ ] Setup automated backups
- [ ] Document rollback procedures
- [ ] Create runbook cho common issues

## 🔄 Continuous Maintenance

### Daily

- [ ] Kiểm tra logs cho errors
- [ ] Monitor resource usage

### Weekly

- [ ] Review GitHub Actions workflows
- [ ] Check for security updates
- [ ] Verify backups

### Monthly

- [ ] Rotate secrets (VPS passwords, JWT secrets)
- [ ] Update dependencies
- [ ] Review and clean old Docker images
- [ ] Performance optimization

## ⚠️ Common Issues Checklist

Nếu gặp vấn đề, kiểm tra:

- [ ] GitHub Secrets đúng format (không có space thừa)
- [ ] VPS_PASSWORD đúng và không có ký tự đặc biệt gây lỗi
- [ ] Password authentication đã enable trên VPS (/etc/ssh/sshd_config)
- [ ] VPS có thể SSH được từ GitHub Actions
- [ ] Docker daemon đang chạy trên VPS
- [ ] Ports không bị firewall block
- [ ] .env file có tất cả biến cần thiết
- [ ] Connection string đúng format PostgreSQL
- [ ] JWT Secret đủ dài (min 32 chars)

## ✅ Final Verification

- [ ] ✅ CI/CD pipeline hoàn toàn tự động
- [ ] ✅ Push code = Tự động deploy
- [ ] ✅ Zero downtime deployment (hoặc minimal)
- [ ] ✅ Migration tự động chạy
- [ ] ✅ Có thể rollback nhanh chóng nếu cần
- [ ] ✅ Documentation đầy đủ
- [ ] ✅ Team members hiểu cách deploy

---

## 🎉 Congratulations!

Nếu tất cả các mục đã được check ✅, hệ thống CI/CD của bạn đã sẵn sàng!

**Next steps:**

1. Monitor first few deployments
2. Optimize workflow nếu cần
3. Add more automation (tests, linting, etc.)
4. Share knowledge với team

---

**📝 Note:** Print checklist này và dùng cho lần deploy đầu tiên!

# 📝 Tóm tắt thay đổi: SSH Password Authentication

## ✅ Đã cập nhật

Hệ thống CI/CD đã được cập nhật để sử dụng **SSH Password Authentication** thay vì SSH Key.

## 📄 Các file đã được cập nhật

### 1. `.github/workflows/deploy.yml`

- ✅ Thay `key: ${{ secrets.VPS_SSH_KEY }}` thành `password: ${{ secrets.VPS_PASSWORD }}`
- ✅ Áp dụng cho cả 2 SSH actions (Deploy và Verify)

### 2. `DEPLOYMENT.md`

- ✅ Cập nhật bảng GitHub Secrets (thay VPS_SSH_KEY bằng VPS_PASSWORD)
- ✅ Xóa phần hướng dẫn tạo SSH Key
- ✅ Thêm cảnh báo về bảo mật password authentication
- ✅ Thêm hướng dẫn enable password authentication trên VPS
- ✅ Cập nhật troubleshooting cho password authentication
- ✅ Thêm hướng dẫn enable PasswordAuthentication trong sshd_config

### 3. `QUICK_START.md`

- ✅ Cập nhật checklist
- ✅ Thay VPS_SSH_KEY bằng VPS_PASSWORD trong secrets
- ✅ Thêm bước enable SSH password authentication
- ✅ Cập nhật troubleshooting

### 4. `CHECKLIST.md`

- ✅ Cập nhật VPS Setup checklist
- ✅ Thay VPS_SSH_KEY bằng VPS_PASSWORD trong GitHub Secrets
- ✅ Cập nhật Common Issues checklist
- ✅ Cập nhật maintenance checklist

### 5. `docs/SSH_PASSWORD_SETUP.md` (MỚI)

- ✅ Hướng dẫn chi tiết enable SSH password authentication
- ✅ Cảnh báo bảo mật và best practices
- ✅ Hướng dẫn cài đặt Fail2ban
- ✅ Hướng dẫn đổi SSH port
- ✅ Hướng dẫn setup 2FA (optional)
- ✅ Hướng dẫn monitor SSH logs

## 🔐 GitHub Secrets cần cập nhật

Xóa secret cũ và thêm secret mới:

### ❌ Xóa (không còn cần)

- `VPS_SSH_KEY`

### ✅ Thêm mới

- `VPS_PASSWORD` = password SSH của VPS

### ✅ Giữ nguyên

- `DOCKER_USERNAME`
- `DOCKER_PASSWORD`
- `VPS_IP`
- `VPS_USER`
- `VPS_PORT` (optional)
- `POSTGRES_USER`
- `POSTGRES_PASSWORD`
- `JWT_SECRET_KEY`

## 📋 Các bước cần làm trước khi deploy

### Bước 1: Enable Password Authentication trên VPS

```bash
# SSH vào VPS
ssh user@vps-ip

# Backup config
sudo cp /etc/ssh/sshd_config /etc/ssh/sshd_config.backup

# Edit config
sudo nano /etc/ssh/sshd_config

# Sửa: PasswordAuthentication yes

# Restart SSH
sudo systemctl restart sshd
```

### Bước 2: Cập nhật GitHub Secrets

1. Vào **Settings → Secrets and variables → Actions**
2. Xóa `VPS_SSH_KEY` (nếu có)
3. Thêm `VPS_PASSWORD` với giá trị là SSH password của VPS

### Bước 3: Test deployment

```bash
# Push code lên main branch
git add .
git commit -m "Update to password authentication"
git push origin main

# Kiểm tra GitHub Actions workflow
```

## ⚠️ Lưu ý bảo mật

### Password authentication kém bảo mật hơn SSH key!

**Recommended actions:**

1. **Sử dụng password mạnh:**

   - Minimum 16 ký tự
   - Bao gồm chữ hoa, chữ thường, số, ký tự đặc biệt
   - Không sử dụng từ điển hoặc thông tin cá nhân

2. **Cài đặt Fail2ban:**

   ```bash
   sudo apt install fail2ban -y
   sudo systemctl enable fail2ban
   ```

3. **Giới hạn login attempts:**

   - Trong `/etc/ssh/sshd_config`: `MaxAuthTries 3`

4. **Đổi SSH port mặc định:**

   - Trong `/etc/ssh/sshd_config`: `Port 2222` (hoặc port khác)
   - Update firewall: `sudo ufw allow 2222/tcp`
   - Update GitHub Secret: `VPS_PORT=2222`

5. **Disable root login:**

   - Trong `/etc/ssh/sshd_config`: `PermitRootLogin no`

6. **Monitor SSH logs:**
   ```bash
   sudo tail -f /var/log/auth.log
   ```

## 🧪 Test checklist

- [ ] Password authentication đã enable trên VPS
- [ ] GitHub Secret `VPS_PASSWORD` đã được thêm
- [ ] Test SSH từ máy local: `ssh user@vps-ip`
- [ ] Push code test lên GitHub
- [ ] GitHub Actions workflow chạy thành công
- [ ] Container đã được deploy
- [ ] API endpoint trả về response

## 📚 Tài liệu liên quan

- [DEPLOYMENT.md](./DEPLOYMENT.md) - Hướng dẫn deployment đầy đủ
- [QUICK_START.md](./QUICK_START.md) - Hướng dẫn nhanh
- [docs/SSH_PASSWORD_SETUP.md](./docs/SSH_PASSWORD_SETUP.md) - Chi tiết về SSH password authentication
- [CHECKLIST.md](./CHECKLIST.md) - Checklist đầy đủ

## 🆘 Cần hỗ trợ?

Nếu gặp vấn đề:

1. Kiểm tra GitHub Actions logs
2. SSH vào VPS và xem logs: `docker-compose logs -f`
3. Tham khảo phần Troubleshooting trong DEPLOYMENT.md
4. Xem SSH logs: `sudo tail -f /var/log/auth.log`

---

**✅ Hệ thống đã sẵn sàng sử dụng SSH password authentication!**

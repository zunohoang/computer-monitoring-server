# 🔐 Hướng dẫn cấu hình SSH Password Authentication

## Tổng quan

Hướng dẫn này giúp bạn enable SSH password authentication trên VPS Ubuntu để sử dụng với GitHub Actions CI/CD.

## ⚠️ Lưu ý bảo mật

**Password authentication kém bảo mật hơn SSH key authentication!**

Nên:

- ✅ Sử dụng password mạnh (min 16 ký tự)
- ✅ Bao gồm chữ hoa, chữ thường, số, ký tự đặc biệt
- ✅ Giới hạn số lần thử login
- ✅ Cân nhắc đổi SSH port mặc định
- ✅ Cân nhắc sử dụng fail2ban

## 📝 Các bước thực hiện

### Bước 1: SSH vào VPS

```bash
ssh user@your-vps-ip
```

### Bước 2: Backup SSH config

```bash
# Backup config hiện tại (quan trọng!)
sudo cp /etc/ssh/sshd_config /etc/ssh/sshd_config.backup
```

### Bước 3: Edit SSH config

```bash
# Mở file config
sudo nano /etc/ssh/sshd_config
```

Tìm và sửa/thêm các dòng sau:

```bash
# Enable password authentication
PasswordAuthentication yes

# Optional: Tăng bảo mật
PermitRootLogin no                    # Không cho phép login bằng root
MaxAuthTries 3                        # Giới hạn 3 lần thử sai
LoginGraceTime 60                     # Timeout 60 giây
ClientAliveInterval 300               # Keep alive 5 phút
ClientAliveCountMax 2                 # Ngắt kết nối sau 2 lần không response

# Optional: Đổi SSH port (tăng bảo mật)
# Port 2222                           # Uncomment và đổi số port nếu muốn
```

**Lưu file:** Nhấn `Ctrl + X`, sau đó `Y`, sau đó `Enter`

### Bước 4: Restart SSH service

```bash
# Restart SSH
sudo systemctl restart sshd

# Kiểm tra status
sudo systemctl status sshd
```

### Bước 5: Test kết nối

**⚠️ QUAN TRỌNG:** Đừng đóng terminal hiện tại! Mở terminal mới để test:

```bash
# Mở terminal mới và test
ssh user@your-vps-ip

# Nhập password khi được yêu cầu
```

Nếu kết nối thành công → OK!
Nếu không → Quay lại terminal cũ và restore backup:

```bash
sudo cp /etc/ssh/sshd_config.backup /etc/ssh/sshd_config
sudo systemctl restart sshd
```

## 🔒 Tăng cường bảo mật

### 1. Cài đặt Fail2ban (Recommended)

Fail2ban tự động block IP sau nhiều lần login thất bại:

```bash
# Cài đặt
sudo apt update
sudo apt install fail2ban -y

# Tạo config
sudo cp /etc/fail2ban/jail.conf /etc/fail2ban/jail.local

# Edit config
sudo nano /etc/fail2ban/jail.local
```

Tìm section `[sshd]` và cấu hình:

```ini
[sshd]
enabled = true
port = ssh
filter = sshd
logpath = /var/log/auth.log
maxretry = 3        # Block sau 3 lần thử sai
bantime = 3600      # Block 1 giờ
findtime = 600      # Trong vòng 10 phút
```

```bash
# Khởi động fail2ban
sudo systemctl start fail2ban
sudo systemctl enable fail2ban

# Kiểm tra status
sudo fail2ban-client status sshd
```

### 2. Đổi SSH port (Optional nhưng recommended)

```bash
# Edit SSH config
sudo nano /etc/ssh/sshd_config

# Sửa dòng:
Port 2222  # Hoặc port khác (1024-65535)

# Lưu và restart
sudo systemctl restart sshd

# Cập nhật firewall
sudo ufw allow 2222/tcp
sudo ufw delete allow 22/tcp  # Xóa rule cũ (nếu muốn)
```

**⚠️ Lưu ý:** Nhớ cập nhật `VPS_PORT=2222` trong GitHub Secrets!

### 3. Sử dụng Google Authenticator 2FA (Advanced)

```bash
# Cài đặt Google Authenticator
sudo apt install libpam-google-authenticator -y

# Chạy setup
google-authenticator

# Follow instructions và scan QR code với app

# Edit PAM config
sudo nano /etc/pam.d/sshd

# Thêm dòng này vào đầu file:
auth required pam_google_authenticator.so

# Edit SSH config
sudo nano /etc/ssh/sshd_config

# Sửa/thêm:
ChallengeResponseAuthentication yes

# Restart SSH
sudo systemctl restart sshd
```

## 🧪 Kiểm tra cấu hình

### Test từ máy local

```bash
# Test SSH connection
ssh user@vps-ip

# Nếu đổi port:
ssh -p 2222 user@vps-ip
```

### Test từ GitHub Actions

Tạo một workflow test đơn giản:

```yaml
name: Test SSH Connection

on:
  workflow_dispatch:

jobs:
  test-ssh:
    runs-on: ubuntu-latest
    steps:
      - name: Test SSH
        uses: appleboy/ssh-action@v1.0.3
        with:
          host: ${{ secrets.VPS_IP }}
          username: ${{ secrets.VPS_USER }}
          password: ${{ secrets.VPS_PASSWORD }}
          port: ${{ secrets.VPS_PORT || 22 }}
          script: |
            echo "SSH connection successful!"
            whoami
            pwd
```

## 📊 Monitor SSH attempts

### Xem log SSH

```bash
# Xem các lần login
sudo tail -f /var/log/auth.log

# Lọc chỉ SSH
sudo grep "sshd" /var/log/auth.log | tail -20

# Xem failed attempts
sudo grep "Failed password" /var/log/auth.log | tail -20
```

### Xem Fail2ban status

```bash
# Status tổng quan
sudo fail2ban-client status

# Status SSH
sudo fail2ban-client status sshd

# Xem các IP bị ban
sudo fail2ban-client status sshd | grep "Banned IP"

# Unban một IP
sudo fail2ban-client set sshd unbanip <IP_ADDRESS>
```

## 🔄 Rollback về SSH Key (Nếu cần)

Nếu muốn quay lại sử dụng SSH key thay vì password:

```bash
# Edit SSH config
sudo nano /etc/ssh/sshd_config

# Sửa:
PasswordAuthentication no
PubkeyAuthentication yes

# Restart
sudo systemctl restart sshd
```

Và update GitHub workflow để sử dụng `key:` thay vì `password:`

## ✅ Checklist hoàn tất

- [ ] Password authentication đã được enable
- [ ] SSH service đã restart thành công
- [ ] Test kết nối từ máy local OK
- [ ] Firewall đã cấu hình đúng ports
- [ ] GitHub Secrets đã được cập nhật
- [ ] Test workflow GitHub Actions OK
- [ ] Fail2ban đã được cài đặt và cấu hình (recommended)
- [ ] SSH logs được monitor thường xuyên

## 📚 Tài liệu tham khảo

- [OpenSSH Server Configuration](https://www.ssh.com/academy/ssh/sshd_config)
- [Fail2ban Documentation](https://www.fail2ban.org/wiki/index.php/Main_Page)
- [Ubuntu Security Best Practices](https://ubuntu.com/server/docs/security-introduction)

---

**⚠️ Lưu ý cuối:** Luôn giữ một session SSH active khi thay đổi cấu hình để tránh bị lock out khỏi VPS!

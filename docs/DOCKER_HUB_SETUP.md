# 🐳 Hướng dẫn setup Docker Hub

## Bước 1: Tạo tài khoản Docker Hub

1. Truy cập: https://hub.docker.com/signup
2. Đăng ký tài khoản miễn phí
3. Verify email

## Bước 2: Tạo Access Token

1. Đăng nhập vào Docker Hub
2. Click vào avatar → **Account Settings**
3. Chọn **Security** tab
4. Click **New Access Token**
5. Đặt tên: `github-actions-ci-cd`
6. Chọn quyền: **Read, Write, Delete**
7. Click **Generate**
8. **⚠️ QUAN TRỌNG:** Copy token ngay (chỉ hiện 1 lần!)

## Bước 3: Tạo Repository trên Docker Hub (Optional)

1. Vào **Repositories** tab
2. Click **Create Repository**
3. Tên repository: `computer-monitoring-api`
4. Visibility: **Public** (miễn phí) hoặc **Private** (trả phí)
5. Click **Create**

## Bước 4: Thêm vào GitHub Secrets

1. Vào GitHub Repository
2. **Settings** → **Secrets and variables** → **Actions**
3. Thêm secrets:
   - `DOCKER_USERNAME`: Username Docker Hub của bạn
   - `DOCKER_PASSWORD`: Access Token vừa tạo

## 🧪 Test Docker Hub credentials

Trên máy local:

```bash
# Login vào Docker Hub
docker login -u your-username -p your-access-token

# Test push image
docker tag your-image your-username/computer-monitoring-api:test
docker push your-username/computer-monitoring-api:test
```

## ✅ Xong!

Bây giờ GitHub Actions có thể push image lên Docker Hub tự động!

## 🔐 Security Tips

- ❌ **KHÔNG** commit password vào code
- ✅ **SỬ DỤNG** Access Token thay vì password
- ✅ **ROTATE** token định kỳ (3-6 tháng)
- ✅ **XÓA** token không dùng nữa

## 📊 Monitor Docker Hub

- Xem images đã push: https://hub.docker.com/r/your-username/computer-monitoring-api
- Xem download stats
- Quản lý tags

---

**Lưu ý:** Free plan Docker Hub cho phép:

- ✅ Unlimited public repositories
- ✅ 1 private repository
- ✅ 200 pulls/6 hours

Nếu cần nhiều hơn, xem xét nâng cấp lên Pro plan.

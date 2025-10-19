# Cách đóng góp

### 1. Clone code về máy

```git
git clone https://github.com/zunohoang/computer-monitoring-server.git
cd computer-monitoring-server
```

### 2. Tạo branch mới

Sử dụng tên branch theo format:

```
feature/tinh-nang
fix/ten-bug
docs/cap-nhat-readme
```

Chuyển qua nhánh mới

```
git checkout -b feature/new-feature
```

### 3. Commit message

Theo chuẩn Conventional Commits
:

```

feat: thêm tính năng

fix: sửa bug

docs: cập nhật tài liệu

chore: công việc phụ trợ

...

```

Ví dụ:

```
git commit -m "feat: thêm API đăng ký người dùng"
```

### 4. Pull code ở main về merge xong đẩy lên

```

git pull origin develop
git push origin feature/new-feature

```

Vào GitHub tạo Pull Request vào branch main hoặc develop.

Mô tả rõ thay đổi, đính kèm issue (nếu có).

```

```

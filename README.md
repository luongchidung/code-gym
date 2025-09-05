# Code Examples - Bad Code vs Good Code

Thư mục này chứa các ví dụ về code xấu (bad code) và code tốt (good code) để minh họa các nguyên tắc Clean Code.

## Files trong thư mục:

### 1. `BadCode_Example.cs`
- **Mô tả**: Chương trình quản lý trường học với code cực kỳ xấu
- **Các vấn đề**:
  - Sử dụng ArrayList<String> để lưu trữ dữ liệu phức tạp
  - Code trùng lặp (copy-paste) rất nhiều
  - Không có separation of concerns
  - Không có proper error handling
  - Không có validation
  - Spaghetti code trong báo cáo
  - Không có OOP principles
  - Hard-coded values
  - Không có proper naming conventions

### 2. `GoodCode_Example.cs`
- **Mô tả**: Cùng chương trình quản lý trường học nhưng áp dụng Clean Code
- **Các cải tiến**:
  - Sử dụng OOP với proper classes
  - Repository pattern để quản lý dữ liệu
  - Service layer để xử lý business logic
  - Proper separation of concerns
  - Exception handling
  - Input validation
  - Clean UI layer
  - SOLID principles
  - Proper naming conventions
  - Dependency injection

## So sánh chính:

| Aspect | Bad Code | Good Code |
|--------|----------|-----------|
| **Data Storage** | ArrayList<String> với delimiter | Proper objects với properties |
| **Code Reuse** | Copy-paste everywhere | Methods và classes tái sử dụng |
| **Error Handling** | Không có | Try-catch với meaningful messages |
| **Validation** | Không có | Input validation đầy đủ |
| **Architecture** | Monolithic | Layered architecture |
| **Maintainability** | Rất khó | Dễ dàng mở rộng và bảo trì |
| **Readability** | Khó đọc | Code tự document |
| **Testing** | Không thể test | Dễ dàng unit test |

## Cách sử dụng:

1. **Đọc Bad Code trước**: Hiểu các vấn đề và anti-patterns
2. **Đọc Good Code sau**: Xem cách refactor và cải thiện
3. **So sánh**: Tìm hiểu tại sao good code tốt hơn
4. **Thực hành**: Thử refactor bad code thành good code

## Nguyên tắc Clean Code được áp dụng:

1. **Single Responsibility Principle**: Mỗi class chỉ có một trách nhiệm
2. **Open/Closed Principle**: Mở để mở rộng, đóng để sửa đổi
3. **Dependency Inversion**: Phụ thuộc vào abstraction, không phải concrete
4. **DRY (Don't Repeat Yourself)**: Không lặp lại code
5. **Meaningful Names**: Tên biến, method, class có ý nghĩa
6. **Small Functions**: Functions ngắn gọn, dễ hiểu
7. **Error Handling**: Xử lý lỗi đúng cách
8. **Comments**: Code tự document, ít cần comments

## Lưu ý:
- Bad code chỉ để học tập, KHÔNG sử dụng trong production
- Good code có thể được cải thiện thêm (ví dụ: thêm logging, configuration, database)
- Đây là ví dụ đơn giản, trong thực tế có thể phức tạp hơn nhiều

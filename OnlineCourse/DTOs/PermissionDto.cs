// DTO trả về client
public class PermissionDto
{
    public int PermissionId { get; set; }
    public string Name { get; set; } = string.Empty;       // Code quyền, ví dụ "User.View"
    public string Description { get; set; } = string.Empty; // Mô tả
}

// Dùng khi tạo quyền
public class CreatePermissionDto
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}

// Dùng khi update quyền
public class UpdatePermissionDto
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}

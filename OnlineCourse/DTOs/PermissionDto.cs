// DTO trả về client
using System.ComponentModel.DataAnnotations;

public class PermissionDto
{
    public int PermissionId { get; set; }
    public string PermissionName { get; set; } = string.Empty;
    public string? Description { get; set; }
}

// Dùng khi tạo quyền
public class PermissionCreateDto
{
    [Required]
    [StringLength(50, ErrorMessage = "Tên quyền không được vượt quá 50 ký tự")]
    public string PermissionName { get; set; } = string.Empty;

    [StringLength(255, ErrorMessage = "Mô tả không được vượt quá 255 ký tự")]
    public string? Description { get; set; }
}

// Dùng khi update quyền
public class PermissionUpdateDto
{
    [Required]
    public int PermissionId { get; set; }

    [StringLength(50, ErrorMessage = "Tên quyền không được vượt quá 50 ký tự")]
    public string? PermissionName { get; set; }

    [StringLength(255, ErrorMessage = "Mô tả không được vượt quá 255 ký tự")]
    public string? Description { get; set; }
}

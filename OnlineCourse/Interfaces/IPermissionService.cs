namespace OnlineCourse.Interfaces
{
    public interface IPermissionService
    {
        Task<IEnumerable<PermissionDto>> GetAllPermissionsAsync();
        Task<PermissionDto?> GetPermissionByIdAsync(int permissionId);
        Task<PermissionDto> CreatePermissionAsync(PermissionCreateDto createDto);
        Task<PermissionDto?> UpdatePermissionAsync(PermissionUpdateDto updateDto);
        Task<bool> DeletePermissionAsync(int permissionId);
    }
}
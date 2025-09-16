using OnlineCourse.Interfaces;
using OnlineCourse.Models.Entities;

namespace OnlineCourse.Services
{
    public class PermissionService : IPermissionService
    {
        private readonly IPermissionRepository _permissionRepository;

        public PermissionService(IPermissionRepository permissionRepository)
        {
            _permissionRepository = permissionRepository;
        }

        public async Task<IEnumerable<PermissionDto>> GetAllPermissionsAsync()
        {
            var permissions = await _permissionRepository.GetAllAsync();
            return permissions.Select(p => new PermissionDto
            {
                PermissionId = p.PermissionId,
                PermissionName = p.PermissionName,
                Description = p.Decription
            }).ToList();
        }

        public async Task<PermissionDto?> GetPermissionByIdAsync(int permissionId)
        {
            var permission = await _permissionRepository.GetByIdAsync(permissionId);
            if (permission == null)
            {
                return null;
            }

            return new PermissionDto
            {
                PermissionId = permission.PermissionId,
                PermissionName = permission.PermissionName,
                Description = permission.Decription
            };
        }

        public async Task<PermissionDto> CreatePermissionAsync(PermissionCreateDto createDto)
        {
            var permission = new Permission
            {
                PermissionName = createDto.PermissionName,
                Decription = createDto.Description
            };
            await _permissionRepository.AddAsync(permission);

            return new PermissionDto
            {
                PermissionId = permission.PermissionId,
                PermissionName = permission.PermissionName,
                Description = permission.Decription
            };
        }

        public async Task<PermissionDto?> UpdatePermissionAsync(PermissionUpdateDto updateDto)
        {
            var permission = await _permissionRepository.GetByIdAsync(updateDto.PermissionId);
            if (permission == null)
            {
                return null;
            }

            if (!string.IsNullOrEmpty(updateDto.PermissionName))
            {
                permission.PermissionName = updateDto.PermissionName;
            }
            if (!string.IsNullOrEmpty(updateDto.Description))
            {
                permission.Decription = updateDto.Description;
            }

            await _permissionRepository.UpdateAsync(permission);

            return new PermissionDto
            {
                PermissionId = permission.PermissionId,
                PermissionName = permission.PermissionName,
                Description = permission.Decription
            };
        }

        public async Task<bool> DeletePermissionAsync(int permissionId)
        {
            var permission = await _permissionRepository.GetByIdAsync(permissionId);
            if (permission == null)
            {
                return false;
            }
            await _permissionRepository.DeleteAsync(permission);
            return true;
        }
    }
}
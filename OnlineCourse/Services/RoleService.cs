using OnlineCourse.DTOs;
using OnlineCourse.Interfaces;
using OnlineCourse.Models.Entities;

namespace OnlineCourse.Services
{
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IPermissionRepository _permissionRepository;

        public RoleService(IRoleRepository roleRepository, IPermissionRepository permissionRepository)
        {
            _roleRepository = roleRepository;
            _permissionRepository = permissionRepository;
        }

        public async Task<IEnumerable<RoleDto>> GetAllRolesAsync()
        {
            var roles = await _roleRepository.GetAllAsync();
            return roles.Select(r => new RoleDto
            {
                RoleId = r.RoleId,
                RoleName = r.RoleName,
                Permissions = r.RolePermissions?.Select(rp => rp.Permission.PermissionName) ?? new List<string>()
            });
        }

        public async Task<RoleDto?> GetRoleByIdAsync(int roleId)
        {
            var role = await _roleRepository.GetByIdAsync(roleId);
            if (role == null)
            {
                return null;
            }

            return new RoleDto
            {
                RoleId = role.RoleId,
                RoleName = role.RoleName,
                Permissions = role.RolePermissions?.Select(rp => rp.Permission.PermissionName) ?? new List<string>()
            };
        }

        public async Task<RoleDto> CreateRoleAsync(RoleCreateDto createDto)
        {
            var role = new Role
            {
                RoleName = createDto.RoleName
            };
            await _roleRepository.AddAsync(role);

            return new RoleDto
            {
                RoleId = role.RoleId,
                RoleName = role.RoleName,
                Permissions = new List<string>()
            };
        }

        public async Task<RoleDto?> UpdateRoleAsync(RoleUpdateDto updateDto)
        {
            var role = await _roleRepository.GetByIdAsync(updateDto.RoleId);
            if (role == null)
            {
                return null;
            }

            if (!string.IsNullOrEmpty(updateDto.RoleName))
            {
                role.RoleName = updateDto.RoleName;
            }

            await _roleRepository.UpdateAsync(role);

            return await GetRoleByIdAsync(updateDto.RoleId);
        }

        public async Task<bool> DeleteRoleAsync(int roleId)
        {
            var role = await _roleRepository.GetByIdAsync(roleId);
            if (role == null)
            {
                return false;
            }
            await _roleRepository.DeleteAsync(role);
            return true;
        }

        public async Task<bool> AddPermissionAsync(int roleId, int permissionId)
        {
            var role = await _roleRepository.GetByIdAsync(roleId);
            var permission = await _permissionRepository.GetByIdAsync(permissionId);

            if (role == null || permission == null)
            {
                return false;
            }

            var rolePermissionExists = await _roleRepository.GetRolePermissionAsync(roleId, permissionId) != null;
            if (rolePermissionExists)
            {
                return false;
            }

            var rolePermission = new RolePermission
            {
                RoleId = roleId,
                PermissionId = permissionId
            };
            await _roleRepository.AddPermissionToRoleAsync(rolePermission);
            return true;
        }

        public async Task<bool> RemovePermissionAsync(int roleId, int permissionId)
        {
            var rolePermission = await _roleRepository.GetRolePermissionAsync(roleId, permissionId);
            if (rolePermission == null)
            {
                return false;
            }
            await _roleRepository.RemovePermissionFromRoleAsync(rolePermission);
            return true;
        }
    }
}
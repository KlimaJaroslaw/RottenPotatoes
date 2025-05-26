using Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RottenPotatoes.Models;
using RottenPotatoes.Services;

namespace RottenPotatoes.Controllers
{

    public class PermissionController : Controller
    {
        #region Constructor
        private readonly PotatoContext _context;
        private readonly SessionManager _session;
        public PermissionController(PotatoContext context, SessionManager session)
        {
            _context = context;
            _session = session;
        }
        #endregion

        public async Task<Permission?> GetPermission(User user)
        {
            Permission? p = await _context.Permissions.FirstOrDefaultAsync(per => per.Permission_ID == user.Permission_ID);
            return p;
        }

        public async Task<bool> HasAdminPermissions(Permission permission)
        {
            Permission? p = await _context.Permissions.Where(per => per.Description == "System Admin").FirstOrDefaultAsync();
            return p?.Permission_ID == permission?.Permission_ID;
        }

        public async Task<bool> HasReviewerPermissions(Permission permission)
        {
            Permission? p = await _context.Permissions.Where(per => per.Description == "Reviewer").FirstOrDefaultAsync();
            return p?.Permission_ID == permission?.Permission_ID;
        }
    }
}

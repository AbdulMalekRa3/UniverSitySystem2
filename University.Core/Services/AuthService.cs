using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using University.Core.DTOs;
using University.Core.Exceptions;
using University.Core.Forms;
using University.Core.Validations;
using UniversityData.Entities;

namespace University.Core.Services
{
    public class AuthService : IAuthService
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;

        public AuthService(
            ILogger<AuthService> logger,
            SignInManager<User> signInManager,
            UserManager<User> userManager,
            RoleManager<Role> roleManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
        }
      public async Task<UserDTO> Login(LoginForm form)
{
          if (form == null)
          throw new ArgumentNullException(nameof(form));

         var validation = FormValidator.Validate(form);
        if (!validation.IsValid)
        throw new BusinessException(validation.Errors);

        var result = await _signInManager.PasswordSignInAsync(
        form.Email, 
        form.Password, 
        form.RememberMe, 
        lockoutOnFailure: false
            );

        if (result.Succeeded)
        {
        var user = await _userManager.FindByEmailAsync(form.Email);
        if (user == null)
            throw new NotFoundException($"Unable to find account with email {form.Email}.");

        var roles = await _userManager.GetRolesAsync(user);
        
        string userRole = roles.FirstOrDefault() ?? "Student";

        var dto = new UserDTO()
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            EmailConfirmed = user.EmailConfirmed,
            Phone = user.PhoneNumber,
            PhoneNumberConfirmed = user.PhoneNumberConfirmed,
            Role = userRole  
        };

        return dto;
      }
    
    if (result.IsLockedOut)
        throw new BusinessException("Account is locked out. Please try again later.");
    
    if (result.IsNotAllowed)
        throw new BusinessException("Account is not allowed to login. Please confirm your email.");
    
    if (result.RequiresTwoFactor)
        throw new BusinessException("Two-factor authentication is required.");

    throw new BusinessException("Invalid login attempt. Please check your email and password.");
}
        public async Task<UserDTO> Register(RegisterForm form)
        {
            if (form == null) throw new ArgumentNullException("form");

            var validation = FormValidator.Validate(form);
            if (!validation.IsValid)
                throw new BusinessException(validation.Errors);

            var userExists = await _userManager.FindByEmailAsync(form.Email);
            if (userExists != null)
                throw new BusinessException("User already exists with this email.");

            var user = new User
            {
                Email = form.Email,
                UserName = form.Email,
                FirstName = form.FirstName,
                LastName = form.LastName
            };

            var result = await _userManager.CreateAsync(user, form.Password);
            if (!result.Succeeded)
            {
                throw new BusinessException(result.Errors
                .GroupBy(x => x.Code)
                .ToDictionary(x => x.Key, y => y.Select(a => a.Description).ToList()));
            }

            var roleName = form.Role.ToString(); 
            if (!await _roleManager.RoleExistsAsync(roleName))
                await _roleManager.CreateAsync(new Role { Name = roleName });

            await _userManager.AddToRoleAsync(user, roleName);

            return new UserDTO()
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                EmailConfirmed = user.EmailConfirmed,
                Phone = user.PhoneNumber,
                PhoneNumberConfirmed = user.PhoneNumberConfirmed,
                Role = form.Role 
            };
        }
    }
    public interface IAuthService
    {
        Task<UserDTO> Login(LoginForm form);
        Task<UserDTO> Register(RegisterForm form);

    }
}
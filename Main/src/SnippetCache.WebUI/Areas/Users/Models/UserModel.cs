using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace SnippetCache.WebUI.Areas.Users.Models
{
    public class UserModel
    {
        #region Nested type: EditUserProfileModel

        public class EditUserProfileModel
        {
            [DataType(DataType.EmailAddress)]
            [Required(ErrorMessage = "Email address is required.")]
            public string Email { get; set; }

            [Required]
            public byte[] AvatarImage { get; set; }

            [Required]
            [Display(Name = "User Name:")]
            public string UserName { get; set; }

            [HiddenInput(DisplayValue = false)]
            public int UserId { get; set; }

            [HiddenInput(DisplayValue = false)]
            public Guid UserGuid { get; set; }

            [DataType(DataType.Password)]
            [Required(ErrorMessage = "Original password required to save settings.")]
            [Display(Name = "Original Password:")]
            public string OriginalPassword { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "New Password:")]
            public string NewPassword { get; set; }

            [DataType(DataType.Password)]
            //[Required, Compare("NewPassword")]
            [Display(Name = "Confirm Password: ")]
            public string NewPasswordConfirmation { get; set; }

            public UserProfileOptions Options { get; set; }
        }

        #endregion

        #region Nested type: UserDetailsModel

        public class UserDetailsModel
        {
            [HiddenInput(DisplayValue = false)]
            public int UserId { get; set; }

            [Required]
            [DataType(DataType.Text)]
            public string UserName { get; set; }

            [Required]
            public byte[] AvatarImage { get; set; }

            [Required]
            [DataType(DataType.Date)]
            public DateTime RegistrationDate { get; set; }
        }

        #endregion

        #region Nested type: UserProfileOptions

        public class UserProfileOptions
        {
            public Dictionary<string, Dictionary<Type, object>> ProfileOptions { get; set; }
        }

        #endregion
    }
}
using System.ComponentModel.DataAnnotations;

namespace TravelMate.Core.DTOs
{
    /// <summary>
    /// 用户登录请求 DTO
    /// </summary>
    public class UserLoginDto
    {
        [Required(ErrorMessage = "微信授权码不能为空")]
        public string Code { get; set; }
    }

    /// <summary>
    /// 用户信息更新 DTO
    /// </summary>
    public class UserUpdateDto
    {
        [Required(ErrorMessage = "用户ID不能为空")]
        public int Id { get; set; }

        [StringLength(50, ErrorMessage = "用户名长度不能超过50个字符")]
        public string Name { get; set; }

        [Range(0, 2, ErrorMessage = "性别值必须为 0(未知)、1(男)、2(女)")]
        public int? Gender { get; set; }
    }

    /// <summary>
    /// 用户信息响应 DTO
    /// </summary>
    public class UserResponseDto
    {
        public int Id { get; set; }
        public string OpenId { get; set; }
        public string Name { get; set; }
        public int? Gender { get; set; }

        /// <summary>
        /// 性别描述
        /// </summary>
        public string GenderDesc
        {
            get
            {
                return Gender switch
                {
                    1 => "男",
                    2 => "女",
                    _ => "未知"
                };
            }
        }
    }

    /// <summary>
    /// 用户登录响应 DTO
    /// </summary>
    public class UserLoginResponseDto
    {
        public int UserId { get; set; }
        public bool IsNewUser { get; set; }
    }
}
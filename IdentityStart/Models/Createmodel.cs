using FluentValidation.Attributes;
using IdentityStart.Infrastructure;

namespace IdentityStart.Models
{
    [Validator(typeof(FluentCreateModelValidator))]
    public class CreateModel
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
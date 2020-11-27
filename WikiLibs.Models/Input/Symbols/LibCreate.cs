using System.ComponentModel.DataAnnotations;
using WikiLibs.Data.Models.Symbols;

namespace WikiLibs.Models.Input.Symbols
{
    public class LibCreate : PostModel<LibCreate, Lib>
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Lang { get; set; }
        public string UserId { get; set; }

        public override Lib CreateModel()
        {
            return new Lib
            {
                Name = Lang + "/" + Name
            };
        }
    }
}

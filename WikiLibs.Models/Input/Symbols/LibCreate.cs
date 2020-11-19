using WikiLibs.Data.Models.Symbols;

namespace WikiLibs.Models.Input.Symbols
{
    public class LibCreate : PostModel<LibCreate, Lib>
    {
        public string Name { get; set; }

        public override Lib CreateModel()
        {
            return new Lib { Name = Name };
        }
    }
}

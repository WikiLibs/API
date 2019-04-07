using System;
using System.Collections.Generic;
using System.Text;

namespace WikiLibs.Data
{
    public interface IModel<InputType, OutputType>
    {
        OutputType MapToOutput();
        void MapFromInput(InputType input);
    }
}

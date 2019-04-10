using System;
using System.Collections.Generic;

namespace WikiLibs.DTO
{
    public static class DTOManager
    {
        public static DTO MapOutput<DTO, DataModel>(in DataModel model) where DTO : IGetDTO<DataModel>, new()
        {
            DTO res = new DTO();

            res.Map(model);
            return (res);
        }

        public static DataModel CreatePatch<DTO, DataModel>(in DataModel model) where DTO : IPatchDTO<DataModel>, new()
        {
            return (new DTO().CreatePatch(model));
        }

        public static ICollection<DTO> MapOutput<DTO, DataModel>(in ICollection<DataModel> models) where DTO : IGetDTO<DataModel>, new()
        {
            var res = new HashSet<DTO>();

            foreach (DataModel model in models)
                res.Add(MapOutput<DTO, DataModel>(model));
            return (res);
        }
    }
}

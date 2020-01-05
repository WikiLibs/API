using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace WikiLibs.Data
{
    public abstract class Model<KeyType>
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public KeyType Id { get; set; }
    }

    public abstract class Model : Model<long>
    {
    }
}

namespace API.Entities
{
    public class Symbol
    {
        public class Prototype
        {
            public class Param
            {
                public string Name { get; set; }
                public string Description { get; set; }
                public string Path { get; set; }
            }

            public string Proto { get; set; }
            public string Description { get; set; }
            public Param[] Parameters { get; set; }
        }

        public string Path { get; set; }
        public string UserID { get; set; }
        public string Lang { get; set; }
        public string Type { get; set; }
        public Prototype[] Prototypes { get; set; }
        public string[] Symbols { get; set; }
    }
}

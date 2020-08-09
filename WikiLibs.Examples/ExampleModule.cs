using System;
using WikiLibs.Shared.Attributes;
using WikiLibs.Shared.Modules.Examples;

namespace WikiLibs.Examples
{
    [Module(Interface = typeof(IExampleModule))]
    public class ExampleModule : IExampleModule
    {
        public IExampleManager Manager { get; }

        public IExampleRequestManager RequestManager { get; }

        public IExampleCommentsManager CommentsManager { get; }

        public ExampleModule(Data.Context ctx, Config cfg)
        {
            Manager = new ExampleManager(ctx);
            RequestManager = new ExampleRequestManager(ctx, cfg);
            CommentsManager = new ExampleCommentManager(ctx, cfg);
        }
    }
}

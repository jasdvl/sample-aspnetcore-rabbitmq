using PubLib.Backoffice.Bootstrap;

CompositionRoot compRoot = new CompositionRoot();

WebApplicationOptions options = new WebApplicationOptions { Args = args };
WebApplicationBuilder builder = compRoot.GetBuilder(options);

var app = builder.Build();

MiddlewareConfigurator.Configure(app);

app.Run();

using PubLib.FrontDesk.Server.Messaging;

namespace PubLib.FrontDesk.Server.Bootstrap;

public class MiddlewareConfigurator
{
    internal static void Configure(WebApplication app)
    {
        app.UseStaticFiles();
        app.UseDefaultFiles();

        app.UseRouting();
        app.UseCors("CorsPolicy");

        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.MapHub<MessageHub>("/messageHub");
        app.MapControllers();
        app.MapFallbackToFile("/index.html");
    }
}

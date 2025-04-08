using System.Text.Json.Serialization;


namespace SagUnderpinings;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.ConfigureHttpJsonOptions(options =>
        {
            options.SerializerOptions.WriteIndented = true;
            options.SerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault;
        });

        // Add services to the container.
        builder.Services.AddExceptionHandler<WebApiExceptionHandler>();
        builder.Services.AddProblemDetails();  //与UseExceptionHandler同用.
        //自动映射所有实现IAutoMapEndPoints接口类的API并包装返回结果（如果指定了IsAutoMapResult为true）
        //builder.Services.AddAntiforgery(opt=>opt.SuppressXFrameOptionsHeader=true);
        builder.Services.AddAutoMapEndPoints();

        //builder.Services.AddCors(options => { /* 配置策略 */ });
        //builder.Services.AddAuthentication();
        //builder.Services.AddAuthorization();

        //builder.Services.AddSession();
        //builder.Services.AddResponseCompression();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();
        // Configure the HTTP request pipeline.
        app.UseExceptionHandler();
        if ( app.Environment.IsDevelopment() )
        {
            app.Use(async (context, next) =>
            {
                //context.Request.EnableBuffering(); // 启用缓冲，在异常处理中获取body
                await next();
            });
            //app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        app.UseHttpsRedirection();
        //app.UseCors();
        //app.UseSession();
        //app.UseAuthentication();
        //app.UseAuthorization();
        //app.UseResponseCompression();
        //app.UseAntiforgery();

        // 自动映射所有实现IAutoMapEndPoints接口类的API
        app.MapEndPoints();

        app.Run();
    }

}

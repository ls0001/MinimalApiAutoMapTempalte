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
        builder.Services.AddExceptionHandler<SystemExceptionHandler>();
        builder.Services.AddExceptionHandler<CustomExceptionHandler>();
        builder.Services.AddProblemDetails();  //��UseExceptionHandlerͬ��.
        //�Զ�ӳ������ʵ��IAutoMapEndPoints�ӿ����API����װ���ؽ�������ָ����IsAutoMapResultΪtrue��
        builder.Services.AddAutoMapEndPoints();

        //builder.Services.AddCors(options => { /* ���ò��� */ });
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

        // �Զ�ӳ������ʵ��IAutoMapEndPoints�ӿ����API
        app.MapEndPoints();

        app.Run();
    }

}

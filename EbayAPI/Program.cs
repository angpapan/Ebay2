global using System.ComponentModel.DataAnnotations;
global using System.ComponentModel.DataAnnotations.Schema;
global using System.Text.Json.Serialization;
using System.Reflection;
using EbayAPI.Data;
using EbayAPI.Helpers;
using EbayAPI.Services;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAutoMapper(typeof(Program)); //AppDomain.CurrentDomain.GetAssemblies()
builder.Services.AddControllers();
builder.Services.AddDbContext<EbayAPIDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("Default"),
        Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.27-mysql")
    )
);
// builder.Services.AddCors();
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        policy =>
        {
            policy
                .WithOrigins("*")
                .AllowAnyMethod()
                .AllowAnyHeader()
                .WithExposedHeaders("X-pagination");
        });
});
builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));

// configure DI for application services
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<AdminService>();
builder.Services.AddScoped<MessageService>();
builder.Services.AddScoped<ItemService>();
builder.Services.AddScoped<BidService>();
builder.Services.AddScoped<CategoryService>();
builder.Services.AddScoped<RecommendationService>();

builder.Services.Configure<FormOptions>(o =>
{
    o.ValueLengthLimit = int.MaxValue;
    o.MultipartBodyLengthLimit = int.MaxValue;
    o.MemoryBufferThreshold = int.MaxValue;
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options => {
    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme."
    });
    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "EbayAPI API",
        Description = @"Το backend για την εργασία του μαθήματος Τεχνολογίες Διαδικτύου. <br/><br/>
                        <b>Άγγελος Παπανικολάου - 1115201800153 <br/> Ευστράτιος Πασχαλίδης - 1115201900252</b>",
    });
    
    
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});



var app = builder.Build();



app.UseMiddleware<ExceptionMiddleware>();
app.UseMiddleware<JwtMiddleware>();
app.MapControllers();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
else
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        options.RoutePrefix = "swagger";
    });
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseCors();
app.UseAuthorization();
// app.UseEndpoints(endpoint => endpoint.MapControllers());
app.MapControllers();

// app.MapControllerRoute(
//     name: "default",
//     pattern: "{controller}/{action=Index}/{id?}");
    
// app.MapFallbackToFile("index.html");


app.Run();
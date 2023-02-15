using AdamTibi.OpenWeather;
using Uqs.Weather.Wrapper;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddSingleton<INowWrapper>(_ => new NowWrapper());
builder.Services.AddTransient<IRandomWrapper>(_ => new RandomWrapper());

builder.Services.AddSingleton<IClient>(_ =>
{
    bool isLoad = bool.Parse(builder.Configuration["LoadTest:IsActive"]);
    if (isLoad)
    {
        return new ClientStub();
    }
    else
    {
        string apiKey = builder.Configuration["OpenWeather:Key"];
        HttpClient httpClient = new HttpClient();
        return new Client(apiKey, httpClient);
    }
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

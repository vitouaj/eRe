using eRe;
using eRe.Infrastructure;
using eRe.Repository;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddScoped<IClassroomRepository, ClassroomRepository>();
builder.Services.AddScoped<IUserRepostory, UserRepository>();
builder.Services.AddScoped<IReportRepository, ReportRepository>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>();
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

int version = 1;

app.MapGroup($"/api/v{version}/user")
    .WithTags("User Endpoints")
    .MapUserEndpoints();

app.MapGroup($"/api/v{version}/classroom")
    .WithTags("Classroom Endpoints")
    .MapClassroomEndpoints();


app.Run();

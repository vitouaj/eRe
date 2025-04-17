using eRe;
using ERE.Infrastructure;
using eRe.Repository;

var builder = WebApplication.CreateBuilder(args);
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                          {
                              policy.WithOrigins("http://localhost:3000")
                                                  .AllowAnyHeader()
                                                  .AllowAnyMethod();
                          });
});

// builder.Services.AddScoped<IClassroomRepository, ClassroomRepository>();
// builder.Services.AddScoped<IUserRepostory, UserRepository>();
// builder.Services.AddScoped<IReportRepository, ReportRepository>();
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

app.UseCors(MyAllowSpecificOrigins);


app.UseHttpsRedirection();

int version = 1;

// app.MapGroup($"/api/v{version}/user")
//     .WithTags("User Endpoints")
//     .MapUserEndpoints();

// app.MapGroup($"/api/v{version}/classroom")
//     .WithTags("Classroom Endpoints")
//     .MapClassroomEndpoints();


app.Run();

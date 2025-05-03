using ERE.APIS;
using ERE.Infrastructure;
using ERE.Repository;
using ERE.Utilities;
using ERE.Validators;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var version = "1.0";
var builder = WebApplication.CreateBuilder(args);
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
var bearerSettings = builder.Configuration.GetSection("Authentication:Schemes:Bearer");

var validIssuer = bearerSettings["ValidIssuer"];
var issuerSigningKey = bearerSettings["IssuerSigningKey"];  
var validAudiences = bearerSettings.GetSection("ValidAudiences").Get<string[]>();

builder.Services.Configure<JwtSettings>(
builder.Configuration.GetSection("Authentication:Schemes:Bearer"));
builder.Services.AddScoped<UtilityService>();
builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));

builder.Services.AddTransient<IMailService, MailService>();


builder.Services.AddScoped<UserRegisterValidator>();
builder.Services.AddScoped<LoginRequestValidator>();
builder.Services.AddScoped<CreateCourseValidator>();
builder.Services.AddScoped<EnrollmentValidator>();

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                          {
                              policy.WithOrigins("http://localhost:5173")
                                                  .AllowAnyHeader()
                                                  .AllowAnyMethod();
                          });
});
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options => {
        options.TokenValidationParameters = new TokenValidationParameters {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = validIssuer,
            ValidAudiences = validAudiences,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(issuerSigningKey))
        };
    });
builder.Services.AddAuthorization();
builder.Services.AddScoped<IUserRepostory, UserRepository>();
builder.Services.AddScoped<ITeacherRepository, TeacherRepository>();
builder.Services.AddScoped<IStudentRepository, StudentRepository>();
builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGen(opt => {
    opt.SwaggerDoc("v1", new OpenApiInfo { Title = "MyAPI", Version = "v1" });
    opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme {
        In = ParameterLocation.Header,
        Description = "Please enter token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "bearer"
    });

    opt.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
            new OpenApiSecurityScheme {
                Reference = new OpenApiReference {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});

builder.Services.AddDbContext<AppDbContext>();
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

var app = builder.Build();
if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors(MyAllowSpecificOrigins);
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapGroup($"/api/v{version}/user")
    .WithTags("User Endpoints")
    .MapUserEndpoints();

app.MapGroup($"/api/v{version}/teacher")
    .WithTags("Teacher Endpoints")
    .MapTeacherEndpoints();

app.MapGroup($"/api/v{version}/student")
    .WithTags("Student Endpoints")
    .MapStudentEndpoints();
    
app.Run();

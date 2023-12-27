using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using ZionApi;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

    })
    .AddJwtBearer(options =>
    {
        var secretKey = ZionEnv.GetValue("TOKEN_SECRET_KEY");

        if (secretKey == null)
        {
            secretKey = "invalid";
        }

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = ZionEnv.GetValue("TOKEN_ISSUER"),
            ValidAudience = ZionEnv.GetValue("TOKEN_AUDIENCE"),
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
            ClockSkew = TimeSpan.Zero
        };

        ZionToken.SignApi("VAS AUTH", secretKey);
    });


var app = builder.Build();

app.UseStatusCodePages();
app.UseHttpsRedirection();
app.UseAuthorization();

// VAS Authentication - End

// App Version
ZionEnv.SetValue("apiVersion", "1.0.0");

// Add Exceptions Middleware
app.UseZionExceptionMiddleware();

if (!app.Environment.IsDevelopment())
{
    builder.WebHost.UseUrls("http://*:9002");
}
else
{
    ZionEnv.SetValue("debug_mode", "1");
}

app.MapControllers();
app.Run();
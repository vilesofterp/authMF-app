using ZionApi;
using ZionShield;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

/* provisorio
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
/*

// VAS Authentication - Begin
app.UseMiddleware<ZionShieldMiddleware>(
    ZionEnv.GetValue("VAS_AUTH_TOKEN_VALIDATE_URL"),
    ZionEnv.GetValue("TOKEN_PARTNER")
);
*/
app.UseStatusCodePages();
app.UseHttpsRedirection();
app.UseAuthorization();

// VAS Authentication - End

// App Version
Environment.SetEnvironmentVariable("apiVersion", "1.1.0");

// Add Exceptions Middleware
app.UseZionExceptionMiddleware();

if (!app.Environment.IsDevelopment())
{
    builder.WebHost.UseUrls("http://*:9002");
}

app.MapControllers();
app.Run();

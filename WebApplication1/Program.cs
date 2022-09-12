using FirebaseAdmin;
using FirebaseAdmin.Auth;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using WebApplication1;

var builder = WebApplication.CreateBuilder(args);

List<X509SecurityKey> issuerSigningKeys = FirebaseAuthConfig.GetIssuerSigningKeys();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    //options.Authority = "https://securetoken.google.com/iswork-d8ed0";
    //options.Audience = "iswork-d8ed0";
    options.TokenValidationParameters = new TokenValidationParameters

    {
        ValidateIssuer = true,
        ValidIssuer = "https://securetoken.google.com/iswork-d8ed0",
        ValidateAudience = true,
        ValidAudience = "iswork-d8ed0",
        ValidateLifetime = true,
        IssuerSigningKeys = issuerSigningKeys,
        IssuerSigningKeyResolver = (arbitrarily, declaring, these, parameters) => issuerSigningKeys
    };

});
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Admin", policy => policy.RequireClaim("Admin", "true"));
});

FirebaseApp App = FirebaseApp.Create(new AppOptions()
{
    Credential = GoogleCredential.FromFile("./iswork-d8ed0-firebase-adminsdk-fhr0y-ba251ee2b6.json"),
});

var claims = new Dictionary<string, object>()
{
    { "admin", true },
};

var uid = "Asejqo3IgvaYJURUGvVxrNmAC9B3";
await FirebaseAuth.DefaultInstance.SetCustomUserClaimsAsync(uid, claims);


string customToken = await FirebaseAuth.DefaultInstance.CreateCustomTokenAsync(uid);
// Send token back to client
Console.WriteLine(customToken);
builder.Services.AddAuthorization();
// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddPolicy("corsapp",
                      policy =>
                      {
                          string[] domains = { "http://localhost:5173", "http://127.0.0.1:5173" };
                          policy.WithOrigins("http://localhost:5173", "http://127.0.0.1:5173", "https://meow.sencha.moe").AllowAnyMethod().AllowAnyHeader();
                      });
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("corsapp");
//app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();


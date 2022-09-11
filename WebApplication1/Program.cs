using FirebaseAdmin;
using FirebaseAdmin.Auth;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

var signingkey = "-----BEGIN CERTIFICATE-----\nMIIDJzCCAg+gAwIBAgIJAOiiP+CnwEYwMA0GCSqGSIb3DQEBBQUAMDYxNDAyBgNV\nBAMMK2ZlZGVyYXRlZC1zaWdub24uc3lzdGVtLmdzZXJ2aWNlYWNjb3VudC5jb20w\nHhcNMjIwODI3MTUyMjA5WhcNMjIwOTEzMDMzNzA5WjA2MTQwMgYDVQQDDCtmZWRl\ncmF0ZWQtc2lnbm9uLnN5c3RlbS5nc2VydmljZWFjY291bnQuY29tMIIBIjANBgkq\nhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEA21mw5OBuQDYONRNRyek+5Mwe2anpgn+1\nNy/RGKU9eNO6/wWg+emzTpwKt4c7dDXgfyJEJ63L0zD/CS+FSyzksHKoGGySsDVX\n+6nD6n36MGxVCz5Z60wgM5FaSKpf7G3iOJi0IiutLcoYv5jl72g6k6nqrRTe5BSm\n7JfNedjpRzOeBm3IPQChW9OSW/fufV8q7Ty09ZbS0fU6KRnsMyCi80EYYg0ondJD\nd56iVUKR4f/OivS+EAZSUzjcu4uWYDzc9lOw8sCbb9oJE4HWLE1bgbQ05jxIqzD+\n6oztB1Mi+0fT5A8BV26MXnSLVPiTCgbSmQSiTq+I//uqxAfsg2v6OQIDAQABozgw\nNjAMBgNVHRMBAf8EAjAAMA4GA1UdDwEB/wQEAwIHgDAWBgNVHSUBAf8EDDAKBggr\nBgEFBQcDAjANBgkqhkiG9w0BAQUFAAOCAQEAg5PdBsjaj2+wcY5gKEFk8taZhXei\nR+hz1V+CWHMoj2U2uH824qr+DqtGrLviuTaY4zh6slaMc/H76swOwUhUIu0zG7n1\nFUl7A82zvX1LJYLRhzPQiegV8NusBJGDo6s10Pc238aX7/d5yovjzV9cAH3VUY2U\nWqBiy8c+0m7W3h+6eKkKLzoqnN3qGZxS9ahyNxJs6s30Ps3O7agWtFoV+9F9/fx+\nK6yoRcSu+gk1NxdlBs+OljuSQW07eW7gpDt7KnfmmAxqNZwqgwTJM7Mv80gbvAaP\ngQIWsCIrFhfAG8JD82gbqjoS+sGkfsSYDlaXule/bz+SPOnm2baB8CfMkA==\n-----END CERTIFICATE-----\n";
List<X509SecurityKey> issuerSigningKeys = FirebaseAuthWebApi.FirebaseAuthConfig.GetIssuerSigningKeys();

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


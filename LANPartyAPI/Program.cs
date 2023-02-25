using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Util;
using LANPartyAPI.DbSeeder;
using LANPartyAPI.Filters;
using LANPartyAPI.OptionsSettings;
using LANPartyAPI_Core.Models;
using LANPartyAPI_DataAccess.Data;
using LANPartyAPI_Services;
using LANPartyAPI_Utils;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
var builder = WebApplication.CreateBuilder(args);

//Init FirebaseAdmin SDK with private key
initFirebaseAdminSDK();

builder.Services.AddControllers(options => options.Filters.Add<HttpResponseExceptionFilter>());
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IEventService, EventService>();
builder.Services.AddScoped<IMatchService, MatchService>();
builder.Services.AddScoped<ITournamentsService, TournamentsService>();
builder.Services.AddScoped<ISeatService, SeatService>();
builder.Services.AddScoped<IPictureService, PictureService>();
builder.Services.AddScoped<TeamService, TeamService>();



builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin();
        builder.AllowAnyHeader();
        builder.AllowAnyMethod();
    });

});

builder.Services.AddIdentityCore<ApplicationUser>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.User.AllowedUserNameCharacters = Utils.GetAllWritableCharacters(encoding: System.Text.Encoding.UTF8);
});


builder.Services.AddAuthentication(o => o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(options =>
{
    var projectId = "lanpartye01";

    options.Authority = $"https://securetoken.google.com/{projectId}";
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = $"https://securetoken.google.com/{projectId}",
        ValidateAudience = true,
        ValidAudience = $"{projectId}",
        ValidateLifetime = true
    };
});


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

//using (var scope = app.Services.CreateScope())
//{
//    var services = scope.ServiceProvider;
//    SeedData.Initialize(services);
//}

app.UseCors("AllowAll");
app.UseHttpsRedirection();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //Error-Handling : https://www.c-sharpcorner.com/article/exception-handling-4-in-asp-net-core-mvc/
    //Approach 1?
    //app.UseDeveloperExceptionPage();

    app.UseExceptionHandler("/error-development");

    //Swagger
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseExceptionHandler("/error");
}

app.UseHttpsRedirection();

app.UseCors("AllowAll");

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();


void initFirebaseAdminSDK()
{
    if (FirebaseApp.DefaultInstance == null)
    {


        //Initialize the SDK in non-Google environments
        //https://firebase.google.com/docs/admin/setup?hl=en#initialize_the_sdk_in_non-google_environments

        //builder.Services.Configure<string>(builder.Configuration.GetSection("FirebaseAdminKey"));


        var firebaseAdminKey = builder.Configuration.GetSection("FirebaseAdminKey").Get<FirebaseAdminKey>();
        var firebaseAdminKeyJSON = Newtonsoft.Json.JsonConvert.SerializeObject(firebaseAdminKey);

        var appOptions = new AppOptions()
        {
            //Credential = await GoogleCredential.FromFileAsync(fullPath, new CancellationToken()),
            Credential = GoogleCredential.FromJson(firebaseAdminKeyJSON),
        };

        FirebaseApp.Create(appOptions);
    }
}
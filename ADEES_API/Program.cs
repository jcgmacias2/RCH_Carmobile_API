using ADDESAPI.Core.Asignacion;
using ADDESAPI.Core.BambuCQRS;
using ADDESAPI.Core.Colaborador;
using ADDESAPI.Core.CorteCQRS;
using ADDESAPI.Core.DespachosCQRS;
using ADDESAPI.Core.EstacionCQRS;
using ADDESAPI.Core.FajillaCQRS;
using ADDESAPI.Core.GetnetCQRS;
using ADDESAPI.Core.GTCQRS;
using ADDESAPI.Core.ImpuestoCQRS;
using ADDESAPI.Core.PresetCQRS;
using ADDESAPI.Core.ProducoCQRS;
using ADDESAPI.Core.TipoCambioDTO;
using ADDESAPI.Core.VentaCQRS;
using ADDESAPI.Core.VentukCQRS;
using ADDESAPI.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using RepoDb;
using RepoDb.DbHelpers;
using RepoDb.DbSettings;
using RepoDb.StatementBuilders;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

var secretKey = builder.Configuration.GetValue<string>("Token:JWT_SECRET_KEY");
var audienceToken = builder.Configuration.GetValue<string>("Token:JWT_AUDIENCE_TOKEN");
var issuerToken = builder.Configuration.GetValue<string>("Token:JWT_ISSUER_TOKEN");
var keyBytes = Encoding.UTF8.GetBytes(secretKey);

var securityKey = new SymmetricSecurityKey(System.Text.Encoding.Default.GetBytes(secretKey));

builder.Services.AddAuthentication(config =>
{
    config.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    config.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

}).AddJwtBearer(config =>
{
    config.RequireHttpsMetadata = false;
    config.SaveToken = true;
    config.TokenValidationParameters = new TokenValidationParameters
    {
        ValidAudience = audienceToken,
        ValidIssuer = issuerToken,
        ValidateIssuerSigningKey = true,
        ValidateIssuer = false,
        ValidateAudience = false,
        IssuerSigningKey = securityKey
    };
});

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddTransient<IDespachosResource, DespachosResource>();
builder.Services.AddTransient<IDespachosService, DespachosService>();
builder.Services.AddTransient<IColaboradorResource, ColaboradorResource>();
builder.Services.AddTransient<IColaboradorService, ColaboradorService>();
builder.Services.AddTransient<IVentukResource, VentukResource>();
builder.Services.AddTransient<IAsignacionResource, AsignacionResource>();
builder.Services.AddTransient<IImpuestoResource, ImpuestoResource>();
builder.Services.AddTransient<IBambuResource, BambuResource>();
builder.Services.AddTransient <IBambuService, BambuService>();
builder.Services.AddTransient<IGTResource, GTResource>();
builder.Services.AddTransient<IGTService, GTService>();
builder.Services.AddTransient<IEstacionResource, EstacionResource>();
builder.Services.AddTransient<IEstacionService, EstacionService>();
builder.Services.AddTransient<IPresetResource, PresetResource>();
builder.Services.AddTransient<IProductoResource, ProductoResource>();
builder.Services.AddTransient<IProductoService, ProductoService>();
builder.Services.AddTransient<ICorteResource, CorteResource>();
builder.Services.AddTransient<ICorteService, CorteService>();
builder.Services.AddTransient<IVentaResource, VentaResource>();
builder.Services.AddTransient<IFajillaService, FajillaService>();
builder.Services.AddTransient<IFajillaResource, FajillaResource>();
builder.Services.AddTransient<ITipoCambioResource, TipoCambioResource>();
builder.Services.AddTransient<IGetnetResource, GetnetResource>();

builder.Services.AddCors(o =>
{
    o.AddPolicy("ThePolicy", b =>
    {
        b.AllowAnyOrigin();
        b.AllowAnyMethod();
        b.AllowAnyHeader();
    });
});

builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "ADDES WebApi", Version = "v1.0.0" });
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Ingrese un token valido",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
     }
});
});

var dbSetting = new SqlServerDbSetting();
SqlServerBootstrap.Initialize();

DbSettingMapper
    .Add<System.Data.SqlClient.SqlConnection>(dbSetting, true);
DbHelperMapper
    .Add<System.Data.SqlClient.SqlConnection>(new SqlServerDbHelper(), true);
StatementBuilderMapper
    .Add<System.Data.SqlClient.SqlConnection>(new SqlServerStatementBuilder(dbSetting), true);

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI();
//}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.UseCors("ThePolicy");

app.MapControllers();

app.Run();

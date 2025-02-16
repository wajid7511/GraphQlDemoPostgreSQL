using GraphQlDemoPostgresQl.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGraphQlDemoPostgresQlMapper();
builder.Services.AddGraphQlDemoPostgresQl();
builder.Services.RegisterGraphQlDemoPostgresQlIServicesRegisterModules(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) { }

app.UseHttpsRedirection();

app.MapGraphQL("/graphql");

app.Run();


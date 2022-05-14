using Microsoft.AspNetCore.HttpOverrides;
using ESI.Data.CosmosDb;
using ESI.Data.GraphQL;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers()
        .AddJsonOptions(options =>
        {
            // options.JsonSerializerOptions.WriteIndented = true;
            // options.JsonSerializerOptions.PropertyNamingPolicy = null;
        });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
    options.AddPolicy(name: "AngularPolicy",
        cfg => {
            cfg.AllowAnyHeader();
            cfg.AllowAnyMethod();
            cfg.WithOrigins(builder.Configuration["AllowedCORS"]);
        }));

builder.Services.AddSingleton<ICosmosContext, CosmosContext>();

builder.Services.AddGraphQLServer()
    .AddAuthorization()
    .AddQueryType<Query>()
    .AddMutationType<Mutation>()
    .AddFiltering()
    .AddSorting();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseExceptionHandler("/Error");
    app.MapGet("/Error", () => Results.Problem());
    app.UseHsts();
}

app.UseHttpsRedirection();

// Invoke the UseForwardedHeaders middleware and configure it 
// to forward the X-Forwarded-For and X-Forwarded-Proto headers.
// NOTE: This must be put BEFORE calling UseAuthentication 
// and other authentication scheme middlewares.
app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor
    | ForwardedHeaders.XForwardedProto
});

app.UseCors("AngularPolicy");

app.MapControllers();

app.MapGraphQL("/api/graphql");

app.MapMethods("/api/heartbeat", new[] { "HEAD" },
    () => Results.Ok());

app.Run();

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SimpleDictionary.Data;
using SimpleDictionary.Models.DataModels;
using SimpleDictionary.Services;

namespace SimpleDictionary
{
    public class Startup
    {
        public Startup(Microsoft.AspNetCore.Hosting.IHostingEnvironment env)
        {
            var configurationBuilder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("secrets.json")
                .AddJsonFile("appsettings.json");

            Configuration = configurationBuilder.Build();
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            //Controllers with views
            services.AddControllersWithViews();
            
            //DbContext
            //Add your own .json config with a connection string to the database you're using to be able to run the project
            //And apply migrations through Package Manager by typing Update-Database
            services.AddDbContext<DictionaryContext>(options => options.UseSqlServer(Configuration["ConnectionStrings:DefaultConnection"]));

            //Identity Context Options
            services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<DictionaryContext>()
                .AddDefaultTokenProviders();

            //Dependencies
            services.AddTransient<IHashtagParser<Hashtag>, HashtagParser>();
            services.AddTransient<IMailSender, MailSender>();

            //Identity Config
            //Password Config
            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 0;
            });
        }
     
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            //Endpoints config
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Feed}/{action=Index}/{id?}");
            });
        }
    }
}

namespace Pianomusic
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Додаємо підтримку сесій перед побудовою аплікації
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(10); // Можна змінити за потребою
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            // Додаємо Razor Pages
            builder.Services.AddRazorPages();

            builder.Services.AddHttpContextAccessor();

            var app = builder.Build();

            // Налаштування HTTP request pipeline
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            // Додаємо сесію до pipeline
            app.UseSession();

            app.UseRouting();

            app.UseAuthorization();

            app.MapRazorPages();

            app.Run();
        }

    }
}

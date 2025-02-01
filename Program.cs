namespace Pianomusic
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // ������ �������� ���� ����� ��������� ��������
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(10); // ����� ������ �� ��������
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            // ������ Razor Pages
            builder.Services.AddRazorPages();

            builder.Services.AddHttpContextAccessor();

            var app = builder.Build();

            // ������������ HTTP request pipeline
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            // ������ ���� �� pipeline
            app.UseSession();

            app.UseRouting();

            app.UseAuthorization();

            app.MapRazorPages();

            app.Run();
        }

    }
}

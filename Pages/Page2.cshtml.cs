using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Pianomusic.Models;

namespace Pianomusic.Pages
{
    public class Page2Model : PageModel
    {

        public string msg { get; set; }

        public Page2Model()
        {
            msg = string.Empty;
        }

        public void OnGet()
        {
            msg = "Ready to play";
            Globals.notation = Notation.eu;
            Console.WriteLine("loading piano page");
        }

       
        public IActionResult OnPostPlay(string note)
        {
            if (!string.IsNullOrEmpty(note))
            {
                Console.WriteLine($"OnPostPlay: {note}");
            }

            msg = note;  // Зберігаємо ноту в змінну msg
            return Page(); // Повертаємо поточну сторінку
        }

        public IActionResult OnPost(string key)
        {
            if (!string.IsNullOrEmpty(key))
            {
                Console.WriteLine($"OnPost: {key}");
            }
            else
            {
                Console.WriteLine($"OnPost: empty key");
                msg = "OnPost: empty key";
                return Page();                    
            }
            msg = key;

            var note = new Note(key);
            try
            {
                string filename;
                note.MakeMP3(out filename);
                ViewData["AudioFile"] = $"lib/{filename}";
                msg = $"playing mp3 {filename}";
            }
            catch (Exception ex)
            {
                msg = ex.ToString();
            }

            return Page(); 
        }
    }
}

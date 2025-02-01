using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Pianomusic.Models;
using System;
using System.Numerics;

namespace Pianomusic.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        public bool IsButtonDisabled { get; set; }
        PLAYER player { get; set; }

        public string Filepath{ get; set; }
        public string[] Notes { get; set; }

        [BindProperty]
        public string SelectedNote { get; set; }
        public string msg { get; set; }

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
            Notes = new string[] {
            "c", "cis/des", "d", "dis/es", "e", "f", "fis/ges", "g", "gis/as", "a", "b/ais", "h"
            };
            SelectedNote = HttpContext?.Session?.GetString("SelectedNote") ?? "c";
            

        }

        public void OnGet()
        {
            SelectedNote = HttpContext.Session.GetString("SelectedNote")??"c";
            Globals.player = PLAYER.naudio;
            Globals.notation = Notation.eu;
            Console.WriteLine("OnGet method starts");
            Page();
            
        }

        public IActionResult OnPostPlay(string action)
        {
            SelectedNote = HttpContext.Session.GetString("SelectedNote") ?? "c";
            var play = Adjust(SelectedNote);
            Console.WriteLine($"OnPost called with action: {action} and note {play}");
            if (action == "playDo")
            {
                try
                {
                    Globals.player = PLAYER.naudio;
                    Console.WriteLine("Naudio playing starts");
                    Note note = new Note(play);
                    note.Play();
                    msg = "naudio sound";
                }
                catch (Exception ex)
                {
                    msg = ex.ToString();
                }

            }
            else if (action == "playMp3")
            {
                Console.WriteLine("post method for playMp3 starts");
                Note note = new Note(play);
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
            }
            else if (action == "createMp3")
            {
                Console.WriteLine("post method for createMp3 starts");
                Note note = new Note(play);
                var file = note.GetFile();
                msg = "file is ready";
                return File(file, "application/octet-stream", $"output_{note.AbsHz()}_{note.AbsDuration()}.mp3");

            }


            Console.WriteLine("post method ends");
            action = "null";
            return Page();
        }

        public void OnPostChooseNote()
        {
            Console.WriteLine($"OnPostChooseNote: {SelectedNote}");
            HttpContext.Session.SetString("SelectedNote", SelectedNote);          

        }

       
        public IActionResult OnPost()
        {
            if (string.IsNullOrEmpty(SelectedNote))
            {
                // Якщо параметр пустий, можна повернути повідомлення про помилку
                ViewData["Error"] = "Нота не обрана.";
            }
            else
            {
                Console.WriteLine($"SelectNote from js: {SelectedNote}");
                HttpContext.Session.SetString("SelectedNote", SelectedNote);

                // Наприклад, збереження вибору або подальше оброблення
                Console.WriteLine($"Selected note: {SelectedNote}");
            }

            return Page();
        }

        private string Adjust(string input)
        {
            int index = input.IndexOf('/');  
            if (index >= 0)  
            {
                string result = input.Substring(0, index);  // Обрізаємо рядок до знака "/"
                return result;
            }
            else return input;
        }

    }
}

using NAudio.Lame;
using NAudio.Wave.SampleProviders;
using NAudio.Wave;
using static Pianomusic.Models.NAPlayer;
using static Pianomusic.Models.Engine;
using Microsoft.AspNetCore.Routing.Constraints;

namespace Pianomusic.Models
{
    public class MP3player
    {
        
        public static void MakeMP3(Note note, out string filepath)
        {
            WriteMP3(note, out filepath);
        }

        public static void PlayMp3File(string filePath)
        {
            // Перевіряємо, чи файл існує
            if (!File.Exists(filePath))
            {
                Console.WriteLine("Файл не знайдено!");
                return;
            }

            using (var audioFileReader = new AudioFileReader(filePath))
            using (var outputDevice = new WaveOutEvent())
            {
                outputDevice.Init(audioFileReader);
                outputDevice.Play();
                Console.WriteLine($"Відтворення файлу: {filePath}");

                while (outputDevice.PlaybackState == PlaybackState.Playing)
                {
                    System.Threading.Thread.Sleep(100); // Чекаємо завершення програвання
                }
            }

            Console.WriteLine("Відтворення завершено.");
        }


        public static byte[] FileMP3(int hz, int ms)
        {
            // Генерація частоти та тривалості
            double frequency = hz;
            double duration = ms;
            var type = choosetimbre();

            // Створення звукового сигналу
            var waveFormat = WaveFormat.CreateIeeeFloatWaveFormat(44100, 1); // 44.1kHz, моно
            var signalGenerator = new SignalGenerator(44100, 1)
            {
                Gain = 0.5,
                Frequency = frequency,
                Type = type // Генерація синусоїдального сигналу
            };

            // Конвертація в MP3
            using (var memoryStream = new MemoryStream())  // Використовуємо MemoryStream для збереження MP3 в пам'яті
            {
                var waveProvider = new SampleToWaveProvider(signalGenerator);
                using (var writer = new LameMP3FileWriter(memoryStream, waveFormat, LAMEPreset.VBR_100))
                {
                    byte[] buffer = new byte[waveProvider.WaveFormat.AverageBytesPerSecond];
                    int bytesRead;
                    double elapsedTime = 0;

                    while (elapsedTime < duration / 1000.0) // duration у мілісекундах
                    {
                        bytesRead = waveProvider.Read(buffer, 0, buffer.Length);
                        if (bytesRead == 0)
                            break;

                        writer.Write(buffer, 0, bytesRead);
                        elapsedTime += (double)bytesRead / waveProvider.WaveFormat.AverageBytesPerSecond;
                    }
                }

                // Повертаємо вміст memoryStream як байтовий масив
                return memoryStream.ToArray();
            }
        }


        public static byte[] FileMP3(Note note)
        {
            return FileMP3(Pitch_to_hz(note.AbsPitch()), note.AbsDuration());
        }

        public static void PlayMp3Bytes(byte[] mp3Data)
        {
            // Перевіряємо, чи масив не порожній
            if (mp3Data == null || mp3Data.Length == 0)
            {
                Console.WriteLine("Масив даних порожній!");
                return;
            }

            try
            {
                using (var memoryStream = new MemoryStream(mp3Data))
                using (var mp3Reader = new Mp3FileReader(memoryStream))
                using (var outputDevice = new WaveOutEvent())
                {
                    outputDevice.Init(mp3Reader);
                    outputDevice.Play();
                    Console.WriteLine("playing bytes...");

                    while (outputDevice.PlaybackState == PlaybackState.Playing)
                    {
                        Thread.Sleep(100); // Чекаємо завершення програвання
                    }
                }

                Console.WriteLine("end playing.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка при відтворенні: {ex.Message}");
            }
        }

        public static void Play(Note note, out string filepath)
        {
            MakeMP3(note, out filepath);            
            PlayMp3File(filepath);
        }
        public static void Play2(Note note)
        {
            string outputFilePath;
            WriteMP3(note, out outputFilePath);
            PlayMp3File(outputFilePath);
        }

        public static void WriteMP3(Note note, out string filename)
        {
            string outputDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "lib");            
            byte[] file = FileMP3(note);
            if (!Directory.Exists(outputDir))
            {
                try
                {
                    Console.WriteLine("directory created");
                    Directory.CreateDirectory(outputDir);                   
                }
                catch
                {
                    Console.WriteLine("failed to create directory");
                }
            }
            filename = $"output_{note.AbsHz()}_{note.AbsDuration()}.mp3";
            var outputFilePath = Path.Combine(outputDir, filename);
            File.WriteAllBytes(outputFilePath, file);
            Console.WriteLine($"MP3 saved: {outputFilePath}");
        }

    }
}

using Newtonsoft.Json;

namespace FlashcardApp
{
    class Program
    {
        static List<Flashcard> flashcards = new List<Flashcard>();
        static string filePath = "flashcards.json";

        static void Main(string[] args)
        {
            LoadFlashcards();

            string[] keys = { "Dodaj fiszki", "Trenuj" };
            int number = 1;
            foreach (string key in keys)
            {
                Console.WriteLine($"{number}.{key}");
                number++;
            }

            Console.Write("Wybierz opcję żeby przejść dalej: ");
            string option = Console.ReadLine();

            switch (option)
            {
                case "1":
                    Console.Clear();
                    AddFlashcard();
                    break;
                case "2":
                    Console.Clear();
                    Training();
                    break;
                case "3":
                    Console.Clear();
                    break;
                default:
                    Console.WriteLine("Nieprawidłowy wybór.");
                    break;
            }
        }

        static void LoadFlashcards()
        {
            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                flashcards = JsonConvert.DeserializeObject<List<Flashcard>>(json);
            }
        }



        static void AddFlashcard()
        {
            Console.WriteLine("Wpisz wyrazy w języku polskim (oddzielone przecinkami bez spacji):");
            string sourceInput = Console.ReadLine();
            List<string> sourceWords = new List<string>(sourceInput.Split(','));

            Console.WriteLine("Wpisz tłumaczenia w języku angielskim (oddzielone przecinkami bez spacji):");
            string targetInput = Console.ReadLine();
            List<string> targetWords = new List<string>(targetInput.Split(','));
            flashcards.Add(new Flashcard(sourceWords, targetWords));

            string json = JsonConvert.SerializeObject(flashcards);
            File.WriteAllText(filePath, json);
        }



        static void Training()
        {
            if (flashcards.Count == 0)
            {
                Console.WriteLine("Brak fiszek do treningu.");
                return;
            }

            Console.WriteLine("Wybierz tryb (1 - łatwy, 2 - trudny):");
            string mode = Console.ReadLine();

            Console.WriteLine("Ile fiszek chcesz przećwiczyć?");
            int count = int.Parse(Console.ReadLine());

            bool isValid = false;
            string input = "";

            while (!isValid)
            {
                Console.WriteLine("Wybierz język początkowy. Wpisz 'polski' lub 'angielski':");
                input = Console.ReadLine().ToLower();

                if (input == "polski" || input == "angielski")
                {
                    isValid = true;
                }
                else
                {
                    Console.WriteLine("Nieprawidłowy wybór. Spróbuj jeszcze raz.");
                }
            }


            Random rnd = new Random();
            List<Flashcard> selectedFlashcards = new List<Flashcard>();

            for (int i = 0; i < count; i++)
            {
                selectedFlashcards.Add(flashcards[rnd.Next(flashcards.Count)]);
            }

            int score = 0;

            foreach (var card in selectedFlashcards)
            {
                if (input == "polski")
                {
                    Console.WriteLine($"Przetłumacz: {string.Join(", ", card.SourceLanguage)}\n\n");
                    string answer = Console.ReadLine();

                    if (mode == "1")
                    {
                        if (card.TargetLanguage.Contains(answer))
                        {
                            score++;
                            Console.WriteLine("Poprawnie!\n\n");
                        }
                        else
                        {
                            Console.WriteLine($"Błąd. Poprawne odpowiedzi: {string.Join(", ", card.TargetLanguage)}\n\n");
                        }
                    }
                    else if (mode == "2")
                    {
                        List<string> answers = new List<string>(answer.Split(','));
                        if (answers.Count == card.TargetLanguage.Count && card.TargetLanguage.TrueForAll(answers.Contains))
                        {
                            score++;
                            Console.WriteLine("Poprawnie!\n\n");
                        }
                        else
                        {
                            Console.WriteLine($"Błąd. Poprawne odpowiedzi: {string.Join(", ", card.TargetLanguage)}\n\n");
                        }
                    }
                }
                else
                {
                    Console.WriteLine($"Przetłumacz: {string.Join(", ", card.TargetLanguage)}");

                    Console.Write("Tłumaczenie: ");
                    string answer = Console.ReadLine();

                    if (mode == "1")
                    {
                        if (card.SourceLanguage.Contains(answer))
                        {
                            score++;
                            Console.WriteLine("Poprawnie!\n\n");
                        }
                        else
                        {
                            Console.WriteLine($"Błąd. Poprawne odpowiedzi: {string.Join(", ", card.SourceLanguage)}\n\n");
                        }
                    }
                    else if (mode == "2")
                    {
                        List<string> answers = new List<string>(answer.Split(','));
                        if (answers.Count == card.SourceLanguage.Count && card.SourceLanguage.TrueForAll(answers.Contains))
                        {
                            score++;
                            Console.WriteLine("Poprawnie!\n\n");
                        }
                        else
                        {
                            Console.WriteLine($"Błąd. Poprawne odpowiedzi: {string.Join(", ", card.SourceLanguage)}\n\n");
                        }
                    }
                }
            }
            Console.WriteLine($"Wynik: {score}/{count} ({(double)score / count * 100}%)");
        }
    }
}
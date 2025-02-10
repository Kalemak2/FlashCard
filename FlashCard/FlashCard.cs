using System;
using System.Collections.Generic;

namespace FlashcardApp
{
    public class Flashcard
    {
        public List<string>? SourceLanguage { get; set; }
        public List<string>? TargetLanguage { get; set; }

        public Flashcard(List<string> source, List<string> target)
        {
            SourceLanguage = source;
            TargetLanguage = target;
        }
    }
}
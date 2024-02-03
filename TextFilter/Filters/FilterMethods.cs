public static class FilterMethods
{
    const string VOWELS = "aeiouAEIOU";

    // filter out all the words that contains a vowel in the middle of the word – the centre 1 or 2 letters
    // ("clean" middle is 'e', "what" middle is 'ha', "currently" middle is 'e' and should be filtered, "the", "rather"
    // should not be)
    public static bool MiddleVowelFilter(string word)
    {
        int mid = word.Length / 2;
        if (word.Length % 2 != 0)
            return VOWELS.Contains(word[mid]);
        else
            return VOWELS.Contains(word[mid - 1]) || VOWELS.Contains(word[mid]);
    }

    // filter out words that have length less than 3
    public static bool LessThanThreeCharactersFilter(string word) =>  word.Length < 3;

    // filter out words that contains the letter ‘t’
    public static bool WordContainingTFilter(string word) =>  word.Contains('t') || word.Contains('T');
}
using System.Security.Cryptography;
using PasswordGenerator.Models;

namespace PasswordGenerator.Services;

public static class PasswordGeneratorService
{
    private const string Upper = "ABCDEFGHJKLMNPQRSTUVWXYZ";
    private const string Lower = "abcdefghijkmnopqrstuvwxyz";
    private const string Digits = "23456789";
    private const string Symbols = "!@#$%^&*()-_=+[]{};:,.?";

    public static string Generate(PasswordOptions options)
    {
        var pools = new List<string>();
        if (options.IncludeUppercase) pools.Add(Upper);
        if (options.IncludeLowercase) pools.Add(Lower);
        if (options.IncludeNumbers) pools.Add(Digits);
        if (options.IncludeSymbols) pools.Add(Symbols);

        var allChars = string.Concat(pools);
        var chars = new List<char>();

        // Guarantee at least one character from every selected type.
        foreach (var pool in pools)
        {
            chars.Add(pool[RandomNumberGenerator.GetInt32(pool.Length)]);
        }

        while (chars.Count < options.Length)
        {
            chars.Add(allChars[RandomNumberGenerator.GetInt32(allChars.Length)]);
        }

        // Fisher-Yates shuffle with a cryptographic random generator.
        for (var i = chars.Count - 1; i > 0; i--)
        {
            var j = RandomNumberGenerator.GetInt32(i + 1);
            (chars[i], chars[j]) = (chars[j], chars[i]);
        }

        return new string(chars.Take(options.Length).ToArray());
    }

    public static string GetStrength(PasswordOptions options)
    {
        var poolSize = 0;
        if (options.IncludeUppercase) poolSize += Upper.Length;
        if (options.IncludeLowercase) poolSize += Lower.Length;
        if (options.IncludeNumbers) poolSize += Digits.Length;
        if (options.IncludeSymbols) poolSize += Symbols.Length;

        var entropyBits = options.Length * Math.Log2(Math.Max(poolSize, 2));
        return entropyBits switch
        {
            < 40 => "Débil",
            < 60 => "Media",
            < 80 => "Fuerte",
            _ => "Muy fuerte"
        };
    }
}

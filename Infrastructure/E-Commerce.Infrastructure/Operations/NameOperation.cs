﻿namespace E_Commerce.Infrastructure.Operations
{
	public class NameOperation
	{
		public static string CharacterRegulatory(string name)
			=> name.Replace("\"", "")
			   .Replace("!", "")
			   .Replace("'", "")
			   .Replace("^", "")
			   .Replace("+", "")
			   .Replace("%", "")
			   .Replace("&", "")
			   .Replace("/", "")
			   .Replace("(", "")
			   .Replace(")", "")
			   .Replace("=", "")
			   .Replace("?", "")
			   .Replace("_", "")
			   .Replace("-", "")
			   .Replace("@", "")
			   .Replace("€", "")
			   .Replace("~", "")
			   .Replace(",", "")
			   .Replace(";", "")
			   .Replace(".", "-")
			   .Replace("Ö", "O")
			   .Replace("ö", "o")
			   .Replace("Ü", "U")
			   .Replace("ü", "u")
			   .Replace("ı", "i")
			   .Replace("İ", "I")
			   .Replace("ğ", "g")
			   .Replace("Ğ", "G")
			   .Replace("æ", "")
			   .Replace("ß", "")
			   .Replace("î", "i")
			   .Replace("â", "a")
			   .Replace("ş", "s")
			   .Replace("Ş", "Ş")
			   .Replace("Ç", "C")
			   .Replace("ç", "c")
			   .Replace("<", "")
			   .Replace(">", "")
			   .Replace("|", "");
	}
}

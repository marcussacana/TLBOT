#IMPORT System.Linq.dll
using System;
using System.Collections.Generic;
using System.Linq;

class CustomClearner : IPlugin {
	
    public void AfterTranslate(ref string Line, uint ID) {}
    public void BeforeTranslate(ref string Line, uint ID) {}
		
    public void AfterOpen(ref string Line, uint ID) {
		string[] Filter = new string[] { 
		"%i1", "%id", "\\&", "${$38}", "\\n", "/i", 
		"%l機関助士;", "#00ffc040;", "%l;", "#;", "[br]" };
		foreach (string Entry in Filter)
			Line = Line.Replace(Entry, string.Empty);
    }

    public void BeforeSave(ref string Line, uint ID) {}
	
    public string GetName() {
		return "Custom Cleaner";
    }
}


class KrKrFntFilter : IPlugin {
	
    public void AfterTranslate(ref string Line, uint ID) {}
    public void BeforeTranslate(ref string Line, uint ID) {}
		
    public void AfterOpen(ref string Line, uint ID) {
		while (Line.IndexOf("%l") >= 0) {
			int Beg = Line.IndexOf("%l");
			int End = Line.IndexOf(";", Beg) + 1;
			
			Line = Line.Substring(0, Beg) + Line.Substring(End);
		}
		while (Line.IndexOf("%f") >= 0) {
			int Beg = Line.IndexOf("%f");
			int End = Line.IndexOf(";", Beg) + 1;
			
			Line = Line.Substring(0, Beg) + Line.Substring(End);
		}
		while (Line.IndexOf("%p") >= 0) {
			int Beg = Line.IndexOf("%p");
			int End = Line.IndexOf(";", Beg) + 1;
			
			Line = Line.Substring(0, Beg) + Line.Substring(End);
		}
    }

    public void BeforeSave(ref string Line, uint ID) {}
	
    public string GetName() {
		return "KrKr Font Filter";
    }
	
}


class CustomReplacer : IPlugin {
	
    public void AfterTranslate(ref string Line, uint ID) {
		string[] Filter = new string[] { "/ ", "/", "\\ ", "", " $ ", "$", "%  ", "% ",
			"(Lauro de Paula)", "Kaim", " .", ".", " ?", "?", " !", "!", " ??", "", " ？？", "",
			"O que? O que? O que?", "???", "Que? Que? Que?", "???",
			//"Kurusu", "Lux", "SPHIA", "Sphere", "Esfera", "Sphere", "Lucux", "Lux", "Netos", "Nect",
			/*"MOSTRAR", "Shou", "Mostrar", "Shou",*/ "Sufia", "Sphere", "SPHERE", "Sphere", "NETOS", "Nect",
			//"Lucus", "Lux", "LUX", "Lux", "Néctar", "Nect", "Necto", "Nect", "Neto", "Nect", "Sofia", "Sphere",
			"Etch", "Ecchi", "% ", "%", " %", "%", "[Nome]", "[firstname]", " = ", "=", "[/ ", "[/",
			"[F tamanho=", "[f size=", "[Tamanho F=", "[f size=", "[/F]", "[/f]", " * ", "*",
			"Milimetros", "Mm", "Milímetros", "Mm", "Milímetro", "Mm", "Milimetro", "Mm",
			"Bytes", "Trabalho", "Ele Ele", "Hehe", 
			//"Kyndra", "Elina", "Antwon", "Yuuto", "Shaquana", "Azusa", "Marquis", 
			//"Motoki", "Naquisha", "Nicola", "Big Tiddies", "Rio", "Lafawnduh", "Miu",
			/*"Gangster", "Vampire", "Gângster", "Vampiro", "Kush", "Sangue", */ "etcetera", "etc",
			"incómodo", "incômodo", /*"Três Putas", "Miu", "Zero", "Reiichi", "Muito Longe", "Kuon" */
			/*"Longa distância", "Kuon", "Muito tempo", "Kuon", "Muito Tempo", "Kuon", */ "Apelia", "Apeiria",
			/*"Leste", "Higashi", "Vista Aérea", "Kuugan", "Vista aérea", "Kuugan", */" ??", " ",
			"Círculo perfeito", "Shouen", "Sinker", "Thinker", "Três Pássaros", "Miu", "Miwa", "Miu",
			
			"Minha Linha", "Minha Fala", "Volte Novamente", "Pode Repetir",
			"Minha linha", "Minha fala", "Volte novamente", "Pode repetir",
			
			//Replace with _ to prevent the next replaces do bad fixes
			"certo", "cer__to", "direito", "certo", "o certo", "o direito", "cer__to", "certo"
		};
		string[] FilterCase = new string[] {
			//"Três", "Miu", "Zero", "Reiichi", "Muito Longe", "Kuon",
			/*"Longa distância", "Kuon", "Muito tempo", "Kuon", "Muito Tempo", "Kuon", */ "Apelia", "Apeiria",
			//"Leste", "Higashi", "Vista Aérea", "Kuugan", "Vista aérea", "Kuugan",
			//"Círculo perfeito", "Shouen", "Sinker", "Thinker",
			
			//Replace with _ to prevent the next replaces do bad fixes
			"certo", "cer__to", "direito", "certo", "o certo", "o direito", "cer__to", "certo"
		};
		for (int i = 0; i < Filter.Length; i+=2){
			Line = Line.Replace(Filter[i], Filter[i+1]);
			Line = Line.Replace(Filter[i].ToLower(), Filter[i+1].ToLower());
		}
		for (int i = 0; i < FilterCase .Length; i+=2){
			Line = Line.Replace(FilterCase[i], FilterCase[i+1]);
		}
	}
    public void BeforeTranslate(ref string Line, uint ID) {}
		
    public void AfterOpen(ref string Line, uint ID) {		
		string[] Filter = new string[] { 
			"\\_", " ", "$$0xe9;", "e"
		};
		for (int i = 0; i < Filter.Length; i+=2){
			Line = Line.Replace(Filter[i], Filter[i+1]);
			Line = Line.Replace(Filter[i].ToLower(), Filter[i+1].ToLower());
		}
	}

    public void BeforeSave(ref string Line, uint ID) {}
	
    public string GetName() {
		return "Custom Replacer";
    }
}

class BlankTrim : IPlugin {
	
	static Dictionary<uint, string> PrefixDB = new Dictionary<uint, string>();
	static Dictionary<uint, string> SufixDB = new Dictionary<uint, string>();
    public void BeforeTranslate(ref string Line, uint ID) {
		string Prefix = Line.Substring(0, Line.Length - Line.TrimStart().Length);	
		string Sufix = Line.Substring(Line.TrimEnd().Length);
		PrefixDB[ID] = Prefix;
		SufixDB[ID] = Sufix;
		Line = Line.Trim();
	}
    public void AfterTranslate(ref string Line, uint ID) {
		Line = PrefixDB[ID] + Line.Trim() + SufixDB[ID];
	}
		
    public void AfterOpen(ref string Line, uint ID) {}
    public void BeforeSave(ref string Line, uint ID) {}
	
    public string GetName() {
		return "Blank Trim";
    }
}


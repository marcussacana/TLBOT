#IMPORT System.Linq.dll
using System;
using System.Collections.Generic;
using System.Linq;

class TagClearner : IPlugin {
    public void AfterTranslate(ref string Line, uint ID) {}
    
    public void BeforeTranslate(ref string Line, uint ID) {}
	char Open = '<', Close = '>';
	string[] Allowed = new string[] {
		"firstname", "voice"
	};
	private void DelTags(ref string Line){
		string Buff = Line;
		Line = string.Empty;
		bool InTag = false;
		string Tag = string.Empty;
		while (!string.IsNullOrEmpty(Buff)){
			char c = Buff[0];
			Buff = Buff.Substring(1, Buff.Length - 1);
			if (c == Open)
				InTag = true;
			if (c == Close && InTag){
				InTag = false;
				bool Bypass = false;
				Tag += c;
				
				foreach (string Allow in Allowed)
					Bypass |= Tag.Contains(Allow);
					
				if (Bypass)
					Line += Tag;
				
				Tag = string.Empty;
				continue;
			}			
			
			if (InTag){
				Tag += c;
				continue;
			}			
			Line += c;
		}
	}
			
    public void AfterOpen(ref string Line, uint ID) {
    	DelTags(ref Line);
	}

    public void BeforeSave(ref string Line, uint ID) {}
	
    public string GetName() {
		return "Tag Cleaner";
    }
}

class SakuHelper : IPlugin {
    public void AfterTranslate(ref string Line, uint ID) {}
    
    public void BeforeTranslate(ref string Line, uint ID) {}
	char Open = '[', Close = ']';
	private void DelTags(ref string Line){
		string Buff = Line;
		Line = string.Empty;
		bool InTag = false;
		string Tag = string.Empty;
		while (!string.IsNullOrEmpty(Buff)){
			char c = Buff[0];
			Buff = Buff.Substring(1, Buff.Length - 1);
			if (c == Open)
				InTag = true;
			if (c == Close && InTag){
				InTag = false;
				bool Bypass = false;
				Tag += c;
				
				if (Bypass)
					Line += Tag;
				
				Tag = string.Empty;
				continue;
			}			
			
			if (InTag){
				Tag += c;
				continue;
			}			
			Line += c;
		}
	}
	
	
	static Dictionary<uint, bool> Map = new Dictionary<uint, bool>();
	
    public void AfterOpen(ref string Line, uint ID) {
		string Ori = Line;
    	DelTags(ref Line);
		if (Ori != Line)
			Map[ID] = true;
		else
			Map[ID] = false;
	}

    public void BeforeSave(ref string Line, uint ID) {
		if (!Map.ContainsKey(ID))
			return;
		if (!Map[ID])
			return;
		
		string[] Words = Line.Split(' ');
		string Output = string.Empty;
		
		for (int i = 0; i < Words.Length; i++){
			bool Last = i + 1 >= Words.Length;
			if (string.IsNullOrEmpty(Words[i]))
				continue;
			
			Output += "[nextword text=\""+Words[i].Replace("\\\"", "\"").Replace("\"", "\\\"")+" \"]";
			Output += Words[i];
			if (!Last)
				Output += ' ';
		}
		
		Line = Output;
	}
	
    public string GetName() {
		return "Sakura Sakura Helper";
    }
}

class FuriganaClearner : IPlugin {
    public void AfterTranslate(ref string Line, uint ID) {}
    
    public void BeforeTranslate(ref string Line, uint ID) {}
    //string Open = "{", Close = "}", Split = ":";
	//string Open = "{", Close = "}", Split = "/";
	//string Open = "[", Close = "]", Split = "'";
	//string Open = "[", Close = "]", Split = "*";
	//string Open = "\\", Close = ";", Split = "\x0";
	//string Open = "<", Close = ">", Split = "?";
	//string Open = "[rb", Close = "]", Split = ",";
	string Open = "【", Close = "】", Split = "／";
	int Index = 1;
	
	private void CutTags(ref string Line, out string Prefix, out string Sufix){
		string Buff = Line;
		Line = string.Empty;
		Prefix = string.Empty;
		Sufix = string.Empty;
		while (!string.IsNullOrEmpty(Buff)){
			if (Buff.StartsWith(Open)){	
				string Tag = Open;
				Buff = Buff.Substring(Open.Length);
				bool Furigana = false;
				while (!string.IsNullOrEmpty(Buff)){
					Tag += Buff[0];
					Buff = Buff.Substring(1);
					if (Buff.StartsWith(Split)){
						Furigana = true;
						while (!string.IsNullOrEmpty(Buff)){
							Tag += Buff[0];
							Buff = Buff.Substring(1);
							if (Buff.StartsWith(Close)){
								Tag += Buff[0];
								Buff = Buff.Substring(1);
								break;
							}
						}
						break;
					} else if (Buff.StartsWith(Close)){
						Tag += Buff[0];
						Buff = Buff.Substring(1);	
						break;							
					}
				}
				if (Furigana){
					Tag = Tag.Substring(Open.Length);
					Tag = Tag.Substring(0, Tag.Length - Close.Length);
					Line += Tag.Split(new string[] { Split }, StringSplitOptions.None)[Index];
				} else {					
					bool IsPrefix = Buff.Length > Line.Length;
					if (IsPrefix)
						Prefix += Tag;
					else 
						Sufix = Tag + Sufix;
				}
				continue;
			}
			char c = Buff[0];
			Buff = Buff.Substring(1);
			Line += c;
		}
	}
	
	static Dictionary<uint, string> Prefixes = new Dictionary<uint, string>();
	static Dictionary<uint, string> Sufixes = new Dictionary<uint, string>();
    public void AfterOpen(ref string Line, uint ID) {
		string Prefix, Sufix;
    	CutTags(ref Line, out Prefix, out Sufix);
		Prefixes[ID] = Prefix;
		Sufixes[ID] = Sufix;
	}

    public void BeforeSave(ref string Line, uint ID) {
		Line = Prefixes[ID] + Line + Sufixes[ID];
	}
	
    public string GetName() {
		return "Furigana Cleaner";
    }
}

class FuriganaClearner2 : IPlugin {
    public void AfterTranslate(ref string Line, uint ID) {}
    
    public void BeforeTranslate(ref string Line, uint ID) {}
    //char Open = '{', Close = '}', Split = ':';
	//char Open = '[', Close = ']', Split = '\'';
	//char Open = '\\', Close = ';', Split = '\x0';
	string Open = "(", Close = ")", Split = "*";
	int Index = 0;
	
	private void CutTags(ref string Line, out string Prefix, out string Sufix){
		string Buff = Line;
		Line = string.Empty;
		Prefix = string.Empty;
		Sufix = string.Empty;
		while (!string.IsNullOrEmpty(Buff)){
			if (Buff.StartsWith(Open)){	
				string Tag = Open;
				Buff = Buff.Substring(Open.Length);
				bool Furigana = false;
				while (!string.IsNullOrEmpty(Buff)){
					Tag += Buff[0];
					Buff = Buff.Substring(1);
					if (Buff.StartsWith(Split)){
						Furigana = true;
						while (!string.IsNullOrEmpty(Buff)){
							Tag += Buff[0];
							Buff = Buff.Substring(1);
							if (Buff.StartsWith(Close)){
								Tag += Buff[0];
								Buff = Buff.Substring(1);
								break;
							}
						}
						break;
					} else if (Buff.StartsWith(Close)){
						Tag += Buff[0];
						Buff = Buff.Substring(1);	
						break;							
					}
				}
				if (Furigana){
					Tag = Tag.Substring(Open.Length);
					Tag = Tag.Substring(0, Tag.Length - Close.Length);
					Line += Tag.Split(new string[] { Split }, StringSplitOptions.None)[Index];
				} else {					
					bool IsPrefix = Buff.Length > Line.Length;
					if (IsPrefix)
						Prefix += Tag;
					else 
						Sufix = Tag + Sufix;
				}
				continue;
			}
			char c = Buff[0];
			Buff = Buff.Substring(1);
			Line += c;
		}
	}
	
	static Dictionary<uint, string> Prefixes = new Dictionary<uint, string>();
	static Dictionary<uint, string> Sufixes = new Dictionary<uint, string>();
    public void AfterOpen(ref string Line, uint ID) {
		string Prefix, Sufix;
    	CutTags(ref Line, out Prefix, out Sufix);
		Prefixes[ID] = Prefix;
		Sufixes[ID] = Sufix;
	}

    public void BeforeSave(ref string Line, uint ID) {
		Line = Prefixes[ID] + Line + Sufixes[ID];
	}
	
    public string GetName() {
		return "Furigana Cleaner - 2";
    }
}

class TagClearnerExtra : IPlugin {
    public void AfterTranslate(ref string Line, uint ID) {}
    
    public void BeforeTranslate(ref string Line, uint ID) {}
	/*char Open = '\\';
	//@s1234 = 6 Len
	static Dictionary<string, int> LenMap = new Dictionary<string, int>{
		{"t", 6},
		{"w", 6},
		{"s", 6},
		{"m", 4},
		{"g", 4},
		{"e", 2},
	};*/	
	char Open = '%';
	//@s1234 = 6 Len
	static Dictionary<string, int> LenMap = new Dictionary<string, int>{
		{"2", 3},
		{"3", 3},
		{"0", 2}
	};
	
	private void CutTags(ref string Line, out string Prefix, out string Sufix){
		string Buff = Line;
		Line = string.Empty;
		Prefix = string.Empty;
		Sufix = string.Empty;
		bool InTag = false;
		while (!string.IsNullOrEmpty(Buff)){
			char c = Buff[0];
			Buff = Buff.Substring(1);
			if (c == Open){
				int Len = 2;
				foreach (string TKey in LenMap.Keys){
					if (Buff.StartsWith(TKey))
						Len = LenMap[TKey];
				}
				if (/*Len == 2 ||*/ Len - 1 > Buff.Length){
					Line += c;
					continue;
				}
				Len -= 1;
				string Tag = Buff.Substring(0, Len);
				Buff = Buff.Substring(Len);
				Tag = Open + Tag;
				bool IsPrefix = Buff.Length > Line.Length;
				if (IsPrefix)
					Prefix += Tag;
				else 
					Sufix += Tag;
				
				continue;
			}				
			Line += c;
		}
	}
	
	static Dictionary<uint, string> Prefixes = new Dictionary<uint, string>();
	static Dictionary<uint, string> Sufixes = new Dictionary<uint, string>();
    public void AfterOpen(ref string Line, uint ID) {
		string Prefix, Sufix;
    	CutTags(ref Line, out Prefix, out Sufix);
		Prefixes[ID] = Prefix;
		Sufixes[ID] = Sufix;
	}

    public void BeforeSave(ref string Line, uint ID) {
		Line = Prefixes[ID] + Line + Sufixes[ID];
	}
	
    public string GetName() {
		return "Tag Cleaner - Dynamic";
    }
}

class FuriganaClearnerExtra : IPlugin {
    public void AfterTranslate(ref string Line, uint ID) {}
    
    public void BeforeTranslate(ref string Line, uint ID) {
		if (Line.Contains(Split.ToString()))
			Line = Line.Split(Split)[1];
		
	}

	char Split = '＠';
	
    public void AfterOpen(ref string Line, uint ID) {}

    public void BeforeSave(ref string Line, uint ID) {}
	
    public string GetName() {
		return "Furigana Cleaner - Extra";
    }
}


class TotonoHelper : IPlugin {
	
    public void AfterTranslate(ref string Line, uint ID) {}
    
    public void BeforeTranslate(ref string Line, uint ID) {}
	
	public Dictionary<uint, string> CommentMap = new Dictionary<uint, string>();
	public Dictionary<uint, string> Original = new Dictionary<uint, string>();
	
    public void AfterOpen(ref string Line, uint ID) {
		if (ID == 0){
			Original   = new Dictionary<uint, string>();
			CommentMap = new Dictionary<uint, string>();			
		}
		
		CommentMap.Remove(ID);
		Original[ID] = Line;
		Retry:;
		if (Line.TrimStart().StartsWith("//")){
			SkipLine(ref Line, ID);
			goto Retry;
		}		
		if (Line.TrimStart().StartsWith("<voice")){
			SkipLine(ref Line, ID);
			goto Retry;
		}
	}
	
	void SkipLine(ref string Line, uint ID){
			int NextCRLF = Line.TrimStart().IndexOf("\r\n");
		    int NextLF = Line.TrimStart().IndexOf("\n");
			
			if (NextCRLF == -1)
				NextCRLF = int.MaxValue;
			else
				NextCRLF += 2;
			
			if (NextLF == -1)
				NextLF = int.MaxValue;
			else
				NextLF += 1;
			
			int LineEnd = NextCRLF;
			if (NextLF < LineEnd)
				LineEnd = NextLF;
			
			if (LineEnd == int.MaxValue)
				return;
			
			if (!CommentMap.ContainsKey(ID))
				CommentMap[ID] = "";
			
			CommentMap[ID] += Line.Substring(0, LineEnd);
			Line = Line.Substring(LineEnd);
	}

    public void BeforeSave(ref string Line, uint ID) {
		if (Original[ID] != Line)
			Line = Line.TrimStart();
		
		if (CommentMap.ContainsKey(ID))
			Line = CommentMap[ID].TrimEnd() + "\r\n" + Line;
	}
	
    public string GetName() {
		return "ToToNo Helper";
    }
}
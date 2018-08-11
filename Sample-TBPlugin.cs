using System;

class TagClearner : IPlugin {
    public void AfterTranslate(ref string Line, uint ID) {}
    
    public void BeforeTranslate(ref string Line, uint ID) {}
	
	private void DelTags(ref string Line){
		string Buff = Line;
		Line = string.Empty;
		bool InTag = false;
		while (!string.IsNullOrEmpty(Buff)){
			char c = Buff[0];
			Buff = Buff.Substring(1, Buff.Length - 1);
			if (c == '<')
				InTag = true;
			if (c == '>'){
				InTag = false;
				continue;
			}			
			
			if (InTag)
				continue;
			
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

class FuriganaClearner : IPlugin {
    public void AfterTranslate(ref string Line, uint ID) {}
    
    public void BeforeTranslate(ref string Line, uint ID) {}
	
	private void DelTags(ref string Line){
		string Buff = Line;
		Line = string.Empty;
		string Tag = string.Empty;
		bool InTag = false;
		while (!string.IsNullOrEmpty(Buff)){
			char c = Buff[0];
			Buff = Buff.Substring(1, Buff.Length - 1);
			if (c == '{'){
				InTag = true;
				continue;
			}
			if (c == '}'){
				InTag = false;
				Line += Tag.Split(':')[0];
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
		return "Furigana Cleaner";
    }
}
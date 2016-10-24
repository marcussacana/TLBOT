class Main{
	public string Filter(string line){
		string Line = line;
		while (Line.StartsWith("."))
			Line = Line.Substring(1, Line.Length - 1);
		return Line;
	}

	public string[] GetBlackList(){
		return new string[] { };
	}
	
	public string[] GetReplaces(){
		return new string[] { "oadmesskip advset", "loadmesskip advset", "avemesskip advend", "savemesskip advend", "ABEL", "LABEL" };
	}
}
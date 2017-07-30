//Para ativar isso, renomeie para Filter.cs
//É possivel criar variavies globais na classe main
//A linha abaixo importa a dll System.Linq.dll, lógicamente deve-se descomentar a linha. 
//#IMPORT System.Linq.dll
//colocando %CD% faz procurar a dll a importar na "AppDomain.CurrentDomain.BaseDirectory"


class Main {	
	
	//Permite aplicar qualquer alteração na linha antes de traduzi-la.
	public string BeforeTL(string line){
		string Line = line;
		return Line;
	}
	
	
	//Permite aplicar qualquer alteração na linha depois de traduzida.
	public string AfterTL(string line){
		string Line = line;
		Replace(ref Line);
		return Line;
	}
	
	//Faz substituições em massa na linha traduzida...
	private void Replace(ref string txt){
		string[] In = new string[] { " .", " ?", " !", " :"};
		string[] Ou = new string[] { ".",  "?",  "!",  ":"};
		for (int i = 0; i < In.Length; i++)
			txt = txt.Replace(In[i], Ou[i]);
	}
	
	//Permite filtar coisas do começo e fim de todas as linhas do script.
	public string[] Trim(){
		return new string[] {
			"%f;",
			"%fSourceHanSansCN-M;"
		};		
	}
	
	//Qualquer linha que for exatamente a uma aqui presente, será ignorada	
	public string[] GetBlackList(){
		return new string[] { 
			"Exemplo de linha para ignorar 1",
			"Exemplo de linha para ignorar 2",
			"Exemplo de linha para ignorar 3"
		};
	}
	
	//Qualquer palavra/frase aqui presente será substituida com a linha ao lado e não será traduzida pelo bot.
	public string[] GetReplaces(){
		return new string[] { 
			"Alvo de substituição", "Substituir para",
			"load", "carregar",
			"save", "salvar"		
		};
	}
}
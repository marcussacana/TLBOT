//Para ativar isso, renomeie para Filter.cs

class Main {
	
	//Permite aplicar qualquer alteração na linha depois de traduzida.
	public string Filter(string line){
		string Line = line;
		return Line;
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
	
	//Qualquer palavra/frase aqui presente será substituida com a linha ao lado
	public string[] GetReplaces(){
		return new string[] { 
			"Alvo de substituição", "Substituir para",
			"load", "carregar",
			"save", "salvar"			
		};
	}
}
#define NODEBUG
using AdvancedBinary;
using CefSharp.DevTools.Target;
using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Windows.Documents.Serialization;
using TLBOT.DataManager;
using TLBOT.Optimizator;
using TLIB;
using static System.Windows.Forms.AxHost;

namespace TLBOT
{
    public class OllamaTranslationTask : ITask
    {
        string LLmModel;
        string TargetLanguage;
        public string[] Lines { get; set; }

        public enum Status : byte
        {
            IDLE, 
            PreProcessing, 
            Analyzing, 
            Translating, 
            PostProcessing, 
            Finished
        }

        public enum Role : byte
        {
            Protagonist,
            Antagonist,
            Supporting,
            Unknown
        }

        public enum Gender : byte
        {
            Male,
            Female,
            Unknown
        }

        public struct Affiliation
        {
            [PString, FNullable]
            public string OriginalName;
            [PString, FNullable]
            public string LocalizedName;
            [PArray, PString, FNullable]
            public string[] Aliases;
        }

        public struct Character
        {
            [PString, FNullable]
            public string OriginalName;
            [PString, FNullable]
            public string LocalizedName;
            [PArray, PString, FNullable]
            public string[] Aliases;
            public Gender Gender;
            [PString, FNullable]
            public string Specie;
            public Role PlotRole;

            [PArray, PString, FNullable]
            public string[] Affiliations;
        }

        public struct LLMSaveState
        {
            public uint AnalyzeProgress;
            public uint TranslateProgress;
            public uint CurrentChecksum;
            [PArray(), StructField]
            public Character[] Characters;
            [PArray(), StructField]
            public Affiliation[] Affiliations;
        }

        public List<Character> Characters = new List<Character>();
        public List<Affiliation> Affiliations = new List<Affiliation>();

        public Dictionary<uint, TaskCompletionSource<string>> Tasks = new Dictionary<uint, TaskCompletionSource<string>>();

        public Status TaskStatus = Status.IDLE;

        public string CurrentStatus => TaskStatus.ToString();

        public bool Finished => TaskStatus == Status.Finished;

        public uint Progress { private set; get; }
        private uint AnalyzerProgress;
        private uint TranslationProgress;
        private uint CurrentChecksum;

        IOptimizator[] Optimizators;
        public OllamaTranslationTask(string SourceLanguage, string TargetLanguage, IOptimizator[] Optimizators)
        {
            this.LLmModel = SourceLanguage;
            this.TargetLanguage = TargetLanguage;
            this.Optimizators = Optimizators;
        }

        public void UpdateLines(string[] Lines)
        {
            AnalyzerProgress = 0;
            TranslationProgress = 0;
            TaskStatus = Status.IDLE;
            this.Lines = Lines;

            CurrentChecksum = unchecked((uint)Lines
                .Select(x => x.GetHashCode())
                .Aggregate((a, b) => a ^ b));
        }

        private Ollama.LLMTool[] AnalyzeTools = new Ollama.LLMTool[]
        {
            new Ollama.LLMTool()
            {
                type = "function",
                function = new Ollama.LLMFunctionTool() {
                    name = "update_character_by_name",
                    description = "Insert/Update character information by the original name",
                    parameters = new Ollama.LLMFunctionParams() {
                        type = "object",
                        required = new string[] { "OriginalName" },
                        properties = new Dictionary<string, Ollama.LLMFunctionArgument>() {
                            {
                                "OriginalName", new Ollama.LLMFunctionArgument() {
                                    type = "string",
                                    description = "The name of the character in original language, if not found, a new character is created"
                                }
                            },
                            {
                                "Gender", new Ollama.LLMFunctionArgument() {
                                    type = "string",
                                    description = "When Set, the gender of the character will be updated",
                                    enums = new string[]
                                    {
                                        "Male",
                                        "Female"
                                    }
                                }
                            },
                            {
                                "PlotRole", new Ollama.LLMFunctionArgument() {
                                    type = "string",
                                    description = "When Set, the role of the character will be updated",
                                    enums = new string[]
                                    {
                                        "Protagonist",
                                        "Antagonist",
                                        "Supporting"
                                    }
                                }
                            },
                            /*{
                                "Specie", new Ollama.LLMFunctionArgument() {
                                    type = "string",
                                    description = "When Set, the specie of the character will be updated"
                                }
                            },*/
                            {
                                "NewAffiliation", new Ollama.LLMFunctionArgument() {
                                    type = "string",
                                    description = "When Set, the character will be included as member of a affiliation with the given name"
                                }
                            },
                            {
                                "RemoveAffiliation", new Ollama.LLMFunctionArgument() {
                                    type = "string",
                                    description = "When Set, the character will be removed as member of a affiliation with the given name"
                                }
                            }
                        }
                    }
                }
            },
            new Ollama.LLMTool()
            {
                type = "function",
                function = new Ollama.LLMFunctionTool()
                {
                    name = "update_affiliation_by_name",
                    description = "Insert/Update affiliation information by the original name",
                    parameters = new Ollama.LLMFunctionParams()
                    {
                        type = "object",
                        required = new string[] { "OriginalName" },
                        properties = new Dictionary<string, Ollama.LLMFunctionArgument>()
                        {
                            {
                                "OriginalName", new Ollama.LLMFunctionArgument()
                                {
                                    type = "string",
                                    description = "The name of the affiliation in original language, when not found a new affiliation is created"
                                }
                            },
                            {
                                "NewAlias", new Ollama.LLMFunctionArgument()
                                {
                                    type = "string",
                                    description = "When Set, the affiliation will be updated to include the given aliase"
                                }
                            }
                        }
                    }
                }
            },
            new Ollama.LLMTool()
            {
                type = "function",
                function = new Ollama.LLMFunctionTool()
                {
                    name = "get_all_character_names",
                    description = "Returns the full list of characters names that currently exists"
                }
            },
            new Ollama.LLMTool()
            {
                type = "function",
                function = new Ollama.LLMFunctionTool()
                {
                    name = "get_all_affiliation_names",
                    description = "Returns the full list of affiliation names that currently exists"
                }
            }
        };


        private Ollama.LLMTool[] TranslationTools = new Ollama.LLMTool[]
        {
            new Ollama.LLMTool()
            {
                type = "function",
                function = new Ollama.LLMFunctionTool()
                {
                    name = "get_all_character_names",
                    description = "Returns the full list of characters names that currently exists"
                }
            },
            new Ollama.LLMTool()
            {
                type = "function",
                function = new Ollama.LLMFunctionTool()
                {
                    name = "get_all_affiliation_names",
                    description = "Returns the full list of affiliation names that currently exists"
                }
            },
            new Ollama.LLMTool()
            {
                type = "function",
                function = new Ollama.LLMFunctionTool()
                {
                    name = "register_translation",
                    description = "Saves the translation result requested the user for futher usage",
                    parameters = new Ollama.LLMFunctionParams()
                    {
                        type = "object",
                        required = new string[] { "ContentId", "Translation" },
                        properties = new Dictionary<string, Ollama.LLMFunctionArgument>()
                        {
                            {
                                "ContentId", new Ollama.LLMFunctionArgument()
                                {
                                    type = "int",
                                    description = "The index informed by the translation contex"
                                }
                            },
                            {
                                "Translation", new Ollama.LLMFunctionArgument()
                                {
                                    type = "string",
                                    description = "The translated text to be saved"
                                }
                            },
                            {
                                "Comment", new Ollama.LLMFunctionArgument()
                                {
                                    type = "string",
                                    description = "An optional note field for the translation"
                                }
                            }
                        }
                    }
                }
            }
        };

        public Dictionary<string, Func<Dictionary<string, object>, string>> AnalyzerMapping = new Dictionary<string, Func<Dictionary<string, object>, string>>()
        {
            { "update_character_by_name", UpdateCharacter },
            { "update_affiliation_by_name", UpdateAffiliation },
            { "get_all_character_names", GetCharactersName },
            { "get_all_affiliation_names", GetAffiliationNames }
        };

        public Dictionary<string, Func<Dictionary<string, object>, string>> TranslatorMapping = new Dictionary<string, Func<Dictionary<string, object>, string>>()
        {
            { "get_all_character_names", GetCharactersName },
            { "get_all_affiliation_names", GetAffiliationNames },
            { "register_translation", RegisterTranslation }
        };

        private static string RegisterTranslation(Dictionary<string, object> Info)
        {
            if (!Info.ContainsKey("ContentId"))
                return "ERROR: Missing ContentId Param";

            if (!Info.ContainsKey("CallerInstance"))
                return "ERROR: Missing CallerInstance Param";

            var This = Info["CallerInstance"] as OllamaTranslationTask;

            var ContentId = TryGetUIntValue(Info, "ContentId");
            var Translation = TryGetStringValue(Info, "Translation");

            var Comment = TryGetStringValue(Info, "Comment"); // dummy for force the LLM separete the translation and any comment


            if (!This.Tasks.ContainsKey(ContentId))
                return "ERROR: Invalid ContentId";

            if (This.Tasks[ContentId].Task.IsCompleted) {
                var pendingTasks = This.Tasks.Where(x => !x.Value.Task.IsCompleted).Select(x => x.Key.ToString());
                return $"ERROR: The given ContentID it was already translated before, ContentIDs waiting translation: {string.Join(", ", pendingTasks)}";
            }

            This.Tasks[ContentId].SetResult(Translation);

            return null;
        }

        private static string GetAffiliationNames(Dictionary<string, object> Info)
        {
            if (!Info.ContainsKey("CallerInstance"))
                return "ERROR: Missing CallerInstance Param";

            var This = Info["CallerInstance"] as OllamaTranslationTask;

            StringBuilder AffNames = new StringBuilder();
            foreach (var Affiliation in This.Affiliations)
            {
                AffNames.AppendLine("Name: " + Affiliation.OriginalName);
                AffNames.AppendLine("Aliases: " + string.Join("; ", Affiliation.Aliases ?? new string[0]));
                AffNames.AppendLine();
                AffNames.AppendLine();
            }

            return AffNames.ToString();
        }

        private static string GetCharactersName(Dictionary<string, object> Info)
        {
            if (!Info.ContainsKey("CallerInstance"))
                return "ERROR: Missing CallerInstance Param";

            var This = Info["CallerInstance"] as OllamaTranslationTask;


            StringBuilder CharNames = new StringBuilder();
            foreach (var Character in This.Characters)
            {
                CharNames.AppendLine("Name: " + Character.OriginalName);
                CharNames.AppendLine("Aliases: " + string.Join("; ", Character.Aliases ?? new string[0]));
                CharNames.AppendLine();
                CharNames.AppendLine();
            }

            return CharNames.ToString();
        }

        private static string UpdateAffiliation(Dictionary<string, object> Info)
        {
            if (!Info.ContainsKey("OriginalName"))
                return "ERROR: Missing OriginalName Param";

            if (!Info.ContainsKey("CallerInstance"))
                return "ERROR: Missing CallerInstance Param";

            var This = Info["CallerInstance"] as OllamaTranslationTask;

            string OriginalName = Info["OriginalName"].ToString();

            var NewAlias = TryGetStringValue(Info, "NewAlias");

            var Target = GetAffiliation(This.Affiliations, OriginalName);

            if (NewAlias != null)
            {
                if (Target.Aliases == null)
                    Target.Aliases = new string[0];
                Array.Resize(ref Target.Aliases, Target.Aliases.Length + 1);
                Target.Aliases[Target.Aliases.Length - 1] = NewAlias;
            }

            UpdateAffiliation(This.Affiliations, Target);

            return null;
        }

        private static string UpdateCharacter(Dictionary<string, object> Info)
        {
            if (!Info.ContainsKey("OriginalName"))
                return "ERROR: Missing OriginalName Param";

            if (!Info.ContainsKey("CallerInstance"))
                return "ERROR: Missing CallerInstance Param";

            var This = Info["CallerInstance"] as OllamaTranslationTask;


            string OriginalName = Info["OriginalName"].ToString();

            var Gender = TryGetStringValue(Info, "Gender");
            var PlotRole = TryGetStringValue(Info, "PlotRole");
            var Specie = TryGetStringValue(Info, "Specie");
            var NewAffiliation = TryGetStringValue(Info, "NewAffiliation");
            var RemoveAffiliation = TryGetStringValue(Info, "RemoveAffiliation");


            var Target = GetCharacter(This.Characters, OriginalName);

            try
            {

                if (Gender != null)
                    Target.Gender = (Gender)Enum.Parse(typeof(Gender), Gender);
            }
            catch
            {
                return $"ERROR: {Gender} is not a valid Gender enum value";
            }

            try
            {
                if (PlotRole != null)
                    Target.PlotRole = (Role)Enum.Parse(typeof(Role), PlotRole);
            }
            catch
            {
                return $"ERROR: {PlotRole} is not a valid PlotRole enum value";

            }

            Target.Specie = Target.Specie ?? Specie;

            if (NewAffiliation != null)
            {
                if (Target.Affiliations == null)
                    Target.Affiliations = new string[0];

                Array.Resize(ref Target.Affiliations, Target.Affiliations.Length + 1);

                var Afilliation = GetAffiliation(This.Affiliations, NewAffiliation);

                Target.Affiliations[Target.Affiliations.Length - 1] = Afilliation.OriginalName;

                UpdateAffiliation(This.Affiliations, Afilliation);
            }

            if (RemoveAffiliation != null && Target.Affiliations != null)
            {
                var target = Array.FindIndex(Target.Affiliations, x => Equals(x, RemoveAffiliation));
                
                if (target >= 0)
                    Target.Affiliations = Target.Affiliations.Where((source, index) => index != target).ToArray();
            }

            Updatecharacter(This.Characters, Target);

            return null;
        }

        private static Affiliation GetAffiliation(List<Affiliation> Affilations, string Name)
        {
            return Affilations
                .Cast<Affiliation?>()
                .FirstOrDefault(x =>
                    Equals(x?.OriginalName, Name) ||
                    (x?.Aliases?.Any(y=> Equals(y, Name)) ?? false)
                )
                ??
                new Affiliation()
                {
                    OriginalName = Name
                };
        }

        private static void UpdateAffiliation(List<Affiliation> Affilations, Affiliation Affiliation)
        {
            var CharIndex = Affilations.FindIndex(x => Equals(x.OriginalName, Affiliation.OriginalName));

            if (CharIndex >= 0)
                Affilations.RemoveAt(CharIndex);

            Affilations.Add(Affiliation);
        }

        private static Character GetCharacter(List<Character> Characters, string Name)
        {
            return Characters
                .Cast<Character?>()
                .FirstOrDefault(x => 
                    Equals(x?.OriginalName, Name) || 
                    (x?.Aliases?.Any(y => Equals(y, Name)) 
                    ?? false))
                ??
                new Character()
                {
                    OriginalName = Name,
                    Gender = Gender.Unknown,
                    PlotRole = Role.Unknown
                };
        }

        private static void Updatecharacter(List<Character> Characters, Character NewChar)
        {
            var CharIndex = Characters.FindIndex(x => Equals(x.OriginalName, NewChar.OriginalName));

            if (CharIndex >= 0)
                Characters.RemoveAt(CharIndex);

            Characters.Add(NewChar);
        }


        private static bool Equals(string A, string B)
        {
            if (A == null)
                return false;

            return A.Equals(B, StringComparison.InvariantCultureIgnoreCase);
        }

        private static string TryGetStringValue(Dictionary<string, object> Info, string Key)
        {
            return Info.ContainsKey(Key) ? Info[Key]?.ToString() : null;
        }

        private static uint TryGetUIntValue(Dictionary<string, object> Info, string Key)
        {
            return Info.ContainsKey(Key) ? uint.Parse(Info[Key].ToString()) : 0;
        }

        static string AnalyzerCache = Path.Combine(Environment.CurrentDirectory, "TLBOT.las");
        public void SaveAnalyzerStatus()
        {
            var state = new LLMSaveState()
            {
                AnalyzeProgress = AnalyzerProgress,
                TranslateProgress = TranslationProgress,
                CurrentChecksum = CurrentChecksum,
                Characters = Characters.ToArray(),
                Affiliations = Affiliations.ToArray()
            };

            File.WriteAllBytes(AnalyzerCache, SaveState(state));   
        }

        public static byte[] SaveState(LLMSaveState State)
        {
            using (var tmp = new MemoryStream())
            using (var Writer = new StructWriter(tmp))
            {
                Writer.WriteStruct(ref State);
                Writer.Flush();

                return tmp.ToArray();
            }
        }

        public static LLMSaveState? LoadState(string Path)
        {
            if (!File.Exists(Path))
                return null;

            using (var tmp = new StreamReader(Path))
            using (var Reader = new StructReader(tmp.BaseStream))
            {
                var state = new LLMSaveState();
                Reader.ReadStruct(ref state);
                return state;
            }
        }

        public void RestoreAnalyzerStatus()
        {
            Progress = 0;

            if (!File.Exists(AnalyzerCache))
                return;

            var state = LoadState(AnalyzerCache) ?? throw new Exception();
            UpdateState(state);
        }

        public void UpdateState(LLMSaveState state)
        {
            AnalyzerProgress = state.AnalyzeProgress;
            TranslationProgress = state.TranslateProgress;
            CurrentChecksum = state.CurrentChecksum;
            Characters = state.Characters.ToList();
            Affiliations = state.Affiliations.ToList();
        }

        public Task Build(Action OnFinish = null)
        {
            return new Task(async () =>
            {

                var checksum = CurrentChecksum;

                RestoreAnalyzerStatus();

                if (checksum != CurrentChecksum) {
                    AnalyzerProgress = 0;
                    TranslationProgress = 0;
                }

                CurrentChecksum = unchecked((uint)Lines
                    .Select(x => x.GetHashCode())
                    .Aggregate((a, b) => a ^ b));

                TaskStatus = Status.PreProcessing;

                Parallel.For(0, Lines.LongLength, new Action<long>((a) =>
                {
                    uint i = (uint)a;
                    foreach (IOptimizator Optimizator in Optimizators)
                    {
#if !DEBUG || NODEBUG
                        try
                        {
#endif
                            Optimizator.BeforeTranslate(ref Lines[i], i);
#if !DEBUG || NODEBUG
                        }
                        catch { }
#endif
                    }
                    Progress = i;
                }));

                Ollama Client = new Ollama();

                TaskStatus = Status.Analyzing;

                var Prompt = "You are an assistant who extracts relevant information from user-provided content.\r\n- You should use the tools available to save and update the glossary as needed.\r\n- You should consider from the context whether any name/term is an alias to something known and register it.\r\n- You should update the glossary character profiles if necessary whenever new information is obtained.\r\n- You can use the available tools to consult the current glossary as needed.\r\n- Ensure to never insert data on the wrong glossary entry\r\n\r\n";
                var InputPrompt = "Complete the glossary with the given info and you need call the available functions for save each information individually";

                //Execute Script Analyzer
                StringBuilder input = new StringBuilder();
                input.AppendLine(InputPrompt);

                if (TranslationProgress == 0 && Program.TLMode != TransMode.StaticContext) {

                    for (uint i = AnalyzerProgress; i < Lines.Length; i++, AnalyzerProgress = i)
                    {
                        float EstimatedToken = input.Length / 2.5f;//better for japanese or chinese, should count space for latin-based languages

                        AnalyzerProgress = i;
                        Progress = i;

                        if (EstimatedToken > 500)
                        {
                            await AnalyzeBufferedData(Client, Prompt, InputPrompt, input);
                            await DoStateOptimizations(Client, false);
                        }

                        if (!Program.Cache.ContainsKey(Lines[i]) && !string.IsNullOrWhiteSpace(Lines[i]))
                            input.AppendLine(Lines[i]);
                    }

                    if (input.Length > 0 && !input.Equals(InputPrompt))
                    {
                        await AnalyzeBufferedData(Client, Prompt, InputPrompt, input);
                        await DoStateOptimizations(Client, false);
                    }

                    TaskStatus = Status.PreProcessing;

                    //Translate and Optimize the Analyzer Results
                    await DoStateOptimizations(Client, true);

                }

                TaskStatus = Status.Translating;

                // Execute the translation
                for (uint i = 0; i < Lines.Length; i++)
                {
                    Progress = (uint)i;
                    TranslationProgress = i;

                    var PreviousLines = GetLines(i, 5);
                    for (var x = 5; x > 0 && PreviousLines.Length / 2.5f > 2000; x--)
                    {
                        PreviousLines = GetLines((uint)(i + (5 - x)), x);
                    }

                    var NextLine = i + 1 < Lines.Length ? Lines[i + 1] : null;

                    var CurrentLine = Lines[i];

                    var Result = await DoTranslation(Client, i, PreviousLines, NextLine, CurrentLine, false);
                    Lines[i] = Result;
                }

                TaskStatus = Status.PostProcessing;
                Progress = 0;

                Parallel.For(0, Lines.LongLength, new Action<long>((a) =>
                {
                    Progress++;
                    uint i = (uint)a;
                    foreach (IOptimizator Optimizator in Optimizators)
#if !DEBUG || NODEBUG
                        try
                        {
#endif
                            Optimizator.AfterTranslate(ref Lines[i], i);
#if !DEBUG || NODEBUG
                        }
                        catch { }
#endif
                    Progress = i;
                }));



                TaskStatus = Status.Finished;
                OnFinish?.Invoke();
            });
        }

        private string GetLines(uint startIndex, int count)
        {
            return string.Join("\r\n", Lines.Skip((int)(startIndex - Math.Min(count, startIndex))).Take((int)Math.Min(count, startIndex)));
        }

        private async Task DoStateOptimizations(Ollama Client, bool Translation)
        {
            for (var i = 0; i < Characters.Count; i++)
            {
                var Character = Characters[i];

                Character = await OptimizeCharacter(Client, Character, Translation);

                Characters[i] = Character;
            }


            for (var i = 0; i < Affiliations.Count; i++)
            {
                var Affiliation = Affiliations[i];

                Affiliation = await OptimizeAffiliations(Client, Affiliation, Translation);
                Affiliations[i] = Affiliation;
            }
        }

        private async Task<Affiliation> OptimizeAffiliations(Ollama Client, Affiliation Affiliation, bool Translate)
        {
            if (Affiliation.Aliases == null)
                Affiliation.Aliases = new string[0];

            //The analyzer may put ; at the name, move anything after the first ; to aliases
            Affiliation.Aliases = Affiliation.Aliases
                .Concat(Affiliation
                    .OriginalName.Split(';')
                    .Skip(1)
                    .Select(x => x.Trim()))
                .Distinct()
                .ToArray();

            Affiliation.OriginalName = Affiliation.OriginalName.Split(';').First().Trim();

            if (Affiliation.LocalizedName != null || !Translate)
            {
                return Affiliation;
            }

            int tries = 3;
            string Result = null;

            while ((Result == null || Result.Length > Affiliation.OriginalName.Length * 4) && tries-- > 0)
                Result = await DoTranslation(Client, 0, string.Empty, string.Empty, Affiliation.OriginalName, true);

            Affiliation.LocalizedName = Result;
            return Affiliation;
        }

        private async Task<Character> OptimizeCharacter(Ollama Client, Character Character, bool Translate)
        {
            if (Character.Aliases == null)
                Character.Aliases = new string[0];

            //The analyzer may put ; at the name, move anything after the first ; to aliases
            Character.Aliases = Character.Aliases
                .Concat(Character
                    .OriginalName.Split(';')
                    .Skip(1)
                    .Select(x => x.Trim()))
                .Distinct()
                .ToArray();

            Character.OriginalName = Character.OriginalName.Split(';').First().Trim();

            if (Character.Affiliations == null)
                Character.Affiliations = new string[0];

            Character.Affiliations = Character.Affiliations.Distinct().ToArray();

            if (Character.LocalizedName != null || !Translate)
            {
                return Character;
            }

            int tries = 3;
            string Result = null;

            while ((Result == null || Result.Length > Character.OriginalName.Length * 4) && tries-- > 0)
                Result = await DoTranslation(Client, 0, string.Empty, string.Empty, Character.OriginalName, true);

            Character.LocalizedName = Result;
            return Character;
        }

        private async Task<string> DoTranslation(Ollama Client, uint contentId, string PreviousLines, string NextLine, string CurrentLine, bool NameMode)
        {
            if (Program.Cache.ContainsKey(CurrentLine))
                return Program.Cache[CurrentLine];

            var Prompt = "You are an automated translation assistant that uses the tools provided and context to translate the line provided by the user.\r\n- You can use the available tools to consult the current glossary as needed.\r\n- You need to register each translation using the `register_translation(int ContentId, string Translation)`\r\n- You can't forget to call the `register_translation` otherwise the program will hangs\r\n- You can also trigger a call by returning a plain json with this format: { \"ContentId\": INTEGER, \"Translation\": STRING }\r\n- Be sure that the json fields matches with the given sample";
            var InputPrompt = $"Translate the given content in the current line to {Program.Settings.TargetLang} and register the translation using the available function";

            if (NameMode)
                Prompt = "- You are translating names, therefore when interacting with two languages that use different scripts (such as English and Japanese), just adapt to a phonetic transliteration of the target script language.\r\n";

            string data = null;

            for (int i = 0; i < 3; i++) { 
                var input = new StringBuilder();
                input.AppendLine(InputPrompt);
                input.AppendLine();

                if (!string.IsNullOrEmpty(PreviousLines))
                {
                    input.AppendLine("Previous Lines:");
                    input.AppendLine(PreviousLines);
                    input.AppendLine();
                }

                if (!string.IsNullOrEmpty(NextLine))
                {
                    input.AppendLine("Upcoming Line:");
                    input.AppendLine(NextLine);
                    input.AppendLine();
                }

                input.AppendLine("--- Context End ---");
                input.AppendLine();
                input.AppendLine("ContentId: " + contentId);
                input.AppendLine("Current Line:");
                input.AppendLine(CurrentLine);
                input.AppendLine("--- Request End ---");

                if (input.Length / 2.5f > 2000)
                {
                    input.AppendLine();
                    input.AppendLine();
                    input.AppendLine(Prompt);
                    input.AppendLine("--- Instructions End ---");
                }

                data = input.ToString();
                InsertContext(ref data, i);

                if (data.Length/2.5f > 4000)
                    continue;
                break;
            }

            if (data.Length/2.5f > 2000)
            {
                data += $"\r\n\r\n{Prompt}\r\n--- Prompt End ---\r\n\r\n";
            }

            Tasks[contentId] = new TaskCompletionSource<string>();

            while (!Tasks[contentId].Task.IsCompleted)
                await Client.PromptAsync(Prompt, data, LLmModel, TranslationTools, TranslatorMapping, this);

            var Result = await Tasks[contentId].Task;


            Program.Cache[CurrentLine] = Result;

            return Result;
        }

        private async Task AnalyzeBufferedData(Ollama Client, string Prompt, string InputPrompt, StringBuilder input)
        {
            var data = input.ToString();
            InsertContext(ref data, 0);

            await Client.PromptAsync(Prompt, data, LLmModel, AnalyzeTools, AnalyzerMapping, this);

            input.Clear();
            input.AppendLine(InputPrompt);
        }

        private void InsertContext(ref string data, int compactLevel)
        {
            var ReferencedChars = new List<Character>();
            foreach (var Char in Characters)
            {
                if (data.Contains(Char.OriginalName))
                {
                    ReferencedChars.Add(Char);
                    continue;
                }

                if (Char.Aliases != null)
                {
                    foreach (var alias in Char.Aliases)
                    {
                        if (data.Contains(alias))
                        {
                            ReferencedChars.Add(Char);
                            break;
                        }
                    }
                }
            }

            var ReferencedAffs = new List<Affiliation>();
            foreach (var Aff in Affiliations)
            {
                if (data.Contains(Aff.OriginalName))
                {
                    ReferencedAffs.Add(Aff);
                    continue;
                }

                if (Aff.Aliases != null) {
                    foreach (var alias in Aff.Aliases)
                    {
                        if (data.Contains(alias))
                        {
                            ReferencedAffs.Add(Aff);
                            break;
                        }
                    }
                }
            }

            var Context = new StringBuilder();
            Context.AppendLine("Context:");
            Context.AppendLine(" - Characters:");
            foreach (var Character in ReferencedChars)
            {
                Context.AppendLine("  - " + Character.OriginalName);

                if (!string.IsNullOrWhiteSpace(Character.LocalizedName))
                    Context.AppendLine("  - " + Character.LocalizedName);

                if (Character.Aliases != null && Character.Aliases.Length > 0 && compactLevel < 2) {
                    Context.AppendLine("  - Aliases:");
                    foreach (var Alias in Character.Aliases)
                    {
                        Context.AppendLine("   - " + Alias);
                    }
                }

                if (Character.Specie != null)
                    Context.AppendLine("  - Specie: " + Character.Specie);

                Context.AppendLine("  - Gender: " + Character.Gender.ToString());
                Context.AppendLine("  - Role: " + Character.PlotRole.ToString());

                if (Character.Affiliations != null && Character.Affiliations.Length > 0)
                {
                    Context.AppendLine("  - Affiliations: " + string.Join("; ", Affiliations.Select(x => x.OriginalName).Distinct()));
                }
                Context.AppendLine();
            }

            Context.AppendLine(" - Affiliations:");
            foreach (var Affiliation in ReferencedAffs)
            {
                Context.AppendLine("  - " + Affiliation.OriginalName);

                if (!string.IsNullOrWhiteSpace(Affiliation.LocalizedName))
                    Context.AppendLine("  - " + Affiliation.LocalizedName);

                if (Affiliation.Aliases != null && compactLevel < 1)
                {
                    Context.AppendLine("  - Affiliation Aliases: " + string.Join("; ", Affiliation.Aliases.Where(x => !string.IsNullOrWhiteSpace(x)).Distinct()));
                }
                Context.AppendLine();
            }

            Context.AppendLine();
            Context.AppendLine();

            data = Context.ToString() + data;
        }

        private bool Attestation(string Result)
        {
            if (Result.KanjiCount() > 0)
            {
                var TargLang = TargetLanguage.ToLowerInvariant().Trim();
                if (!TargLang.StartsWith("jap") && !TargLang.StartsWith("ch"))
                {
                    return false;
                }
            }
            return true;
        }

    }
}
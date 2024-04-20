using System.Collections.Generic;

public class Mod
{
    public string category { get; set; }
    public string preview_path { get; set; }
    public string file_path { get; set; }
    public string title { get; set; }
    public string description { get; set; }
}

public class Root
{
    public List<Mod> mods { get; set; }
    public List<string> categories { get; set; }
}


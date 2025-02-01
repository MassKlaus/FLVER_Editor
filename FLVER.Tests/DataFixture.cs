using FbxDataExtractor;
using FLVER_Editor;
using FLVER_Editor.FbxImporter.ViewModels;
using SoulsAssetPipeline.FLVERImporting;
using SoulsFormats;

[assembly: AssemblyFixture(typeof(DataFixture))]
public class DataFixture : IDisposable
{
    public DataFixture()
    {
        // Preload all test files here
        Bnd3_1 = File.ReadAllBytes("./Assets/bnd3_1.dcx");
        
        Flver0_1 = File.ReadAllBytes("./Assets/flver0_1.flver");
        Flver0_1_Read = FLVER0.Read(Flver0_1);
        
        Flver2_1 = File.ReadAllBytes("./Assets/flver2_1.flver");
        Flver2_1_Read = FLVER2.Read(Flver2_1);

        Fbx_1 = FbxMeshData.Import("./Assets/fbx_1.fbx").Select(x => new FbxMeshDataViewModel(x)).ToList();
        Flver2_1_Fbx_Imported = File.ReadAllBytes("./Assets/flver2_1_fbx_imported.flver");
        Flver2_1_Fbx_Imported_Read = FLVER2.Read(Flver2_1_Fbx_Imported);
    }

    public MeshImportOptions GetImportOptions(FLVER2 flver)
    {
        string basePath = Path.Join(AppDomain.CurrentDomain.BaseDirectory, "SapResources", "FLVER2MaterialInfoBank");
        string bankFileName = $"Bank{VersionString(flver)}.xml";
        string xmlPath = Path.Join(basePath, bankFileName);
        FLVER2MaterialInfoBank MaterialInfoBank = FLVER2MaterialInfoBank.ReadFromXML(xmlPath);
        var MTDs = new List<string>(MaterialInfoBank.MaterialDefs.Keys.Where(x => !string.IsNullOrEmpty(x)).OrderBy(x => x));

        var options = Importer.GetImportOptions(MTDs, MaterialInfoBank);
        options.Weighting = WeightingMode.Values[0];
        options.MTD = "c[amsn]_e.matxml";

        return options;
    }

    public Dictionary<FbxMeshDataViewModel, MeshImportOptions> GetFbxDictionary(FLVER2 file) {
        var options = GetImportOptions(file);

        Dictionary<FbxMeshDataViewModel, MeshImportOptions> meshData = new();

        foreach (var fbx in Fbx_1)
        {
            meshData.Add(fbx, options);
        }

        return meshData;
    }

    public string VersionString(FLVER2 flver) {
        return flver.Header.Version switch
        {
            131084 => "DS1",
            131088 => "DS2",
            131092 => "DS3",
            131098 when flver.Materials.Any(x => x.MTD.Contains(".matxml")) => "ER",
            131098 => MainWindow.ShowSelectorDialog("Choose target game:", "Target Game",
                    new object[] { "Elden Ring", "Sekiro" }) switch
                {
                    "Elden Ring" => "ER",
                    "Sekiro" => "SDT",
                    null => null,
                    _ => throw new ArgumentOutOfRangeException()
                },
            131099 => "AC6",
            // TODO: Create a dedicated material bank for Demon's Souls...
            131072 => "ER",
            _ => throw new InvalidDataException("Invalid Flver Version")
        };
    }

    public void Dispose()
    {
    }

    public byte[] Bnd3_1 { get; private set; }
    
    public byte[] Flver0_1 { get; private set; }
    public FLVER0 Flver0_1_Read { get; private set; }

    public byte[] Flver2_1 { get; private set; }
    public FLVER2 Flver2_1_Read { get; private set; }
    
    public List<FbxMeshDataViewModel> Fbx_1 { get; private set; }
    public byte[] Flver2_1_Fbx_Imported { get; private set; }
    public FLVER2 Flver2_1_Fbx_Imported_Read { get; private set; }

}
using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;

public class MakeAtomCraftData : EditorWindow {

	private Vector2 scrollPos;
	private Vector2 scrollPos_Window;
	private Rect windowRect = new Rect(10, 10, 100, 100);
	private bool scaling = true;

	public string outputCueSheetName = "TestCueSheet";		//	出力するキューシート名
	public string srcMaterialsFolder = "Materials";	//	キューを作成するwavファイルのあるフォルダ名
	public string defaultGroupCategory = "Category_0";		//	キューに設定するカテゴリ名
	public float pos3dDistanceMin = 10.0f;
	public float pos3dDistanceMax = 50.0f;
	public float pos3dDopplerCoefficient = 0.0f;

	[MenuItem("CRI/My/Open MakeAtomCraftData ...")]
	static void OpenWindow()
	{
		EditorWindow.GetWindow<MakeAtomCraftData>(false, "MakeAtomCraftData");
	}

	private void ScalingWindow(int windowID)
	{
		GUILayout.Box("", GUILayout.Width(20), GUILayout.Height(20));
		if (Event.current.type == EventType.MouseUp)
			this.scaling = false;
		else if (Event.current.type == EventType.MouseDown && GUILayoutUtility.GetLastRect().Contains(Event.current.mousePosition))
			this.scaling = true;
		
		if (this.scaling)
			this.windowRect = new Rect(windowRect.x, windowRect.y, windowRect.width + Event.current.delta.x, windowRect.height + Event.current.delta.y);		
	}

	private void OnGUI()
	{
		this.windowRect = GUILayout.Window(0, windowRect, ScalingWindow, "resizeable", GUILayout.MinHeight(80), GUILayout.MaxHeight(200));
		
		this.scrollPos_Window = GUILayout.BeginScrollView(this.scrollPos_Window);
		{
			GUIMain();
		}
		GUILayout.EndScrollView();
	}

	void GUIMain()
	{
		EditorGUILayout.BeginVertical();
		EditorGUILayout.LabelField("CueSheet Name");
		outputCueSheetName = EditorGUILayout.TextField(outputCueSheetName);	
		EditorGUILayout.Space();
		EditorGUILayout.LabelField("Materials Folder Path");
		srcMaterialsFolder = EditorGUILayout.TextField(srcMaterialsFolder);
		EditorGUILayout.Space();
		EditorGUILayout.LabelField("Cagetgory Name");
		defaultGroupCategory = EditorGUILayout.TextField(defaultGroupCategory);
		EditorGUILayout.Space();

		
		EditorGUILayout.BeginHorizontal();		
		EditorGUILayout.LabelField("3DPos Min",GUILayout.Width(80));
		EditorGUILayout.LabelField("Max",GUILayout.Width(80));
		EditorGUILayout.LabelField("Doppler",GUILayout.Width(80));
		EditorGUILayout.EndHorizontal();
		EditorGUILayout.BeginHorizontal();		
		pos3dDistanceMin = EditorGUILayout.FloatField(pos3dDistanceMin,GUILayout.Width(80));		
		pos3dDistanceMax = EditorGUILayout.FloatField(pos3dDistanceMax,GUILayout.Width(80));
		pos3dDopplerCoefficient = EditorGUILayout.FloatField(pos3dDopplerCoefficient,GUILayout.Width(80));
		EditorGUILayout.EndHorizontal();
		EditorGUILayout.Space();

		if(GUILayout.Button("Make Atom Craft Data")){
			DoMake();
		}
		EditorGUILayout.EndVertical();
	}

	// Use this for initialization
	void DoMake () {
		List<string> wavList = new List<string>();
		string matelialsPath = Application.dataPath + "/" + srcMaterialsFolder;
		
		if(srcMaterialsFolder != string.Empty){
			string[] fileList = Directory.GetFiles(matelialsPath);
			if(fileList != null)
			{
				foreach(string tmpFile in fileList){
					if(Path.GetExtension(tmpFile) == ".wav"){
						wavList.Add(Path.GetFileNameWithoutExtension(tmpFile));
					}
				}
			}
			
			MakeData(outputCueSheetName,wavList);
			
			CopyMaterialsFolder(outputCueSheetName,fileList,matelialsPath);

			Debug.Log("Make Atom Craft Data Finish!");
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	void MakeData(string cuesheetName,List<string> wavList)
	{
		MakeAtmcunit(cuesheetName,wavList);
		MakeMaterialinfo(cuesheetName,wavList);
	}
	
	void MakeAtmcunit(string cuesheetName,List<string> wavList)
	{
		string filePath = Application.dataPath + "/" + cuesheetName +"/" + cuesheetName +".atmcunit";
		
		Debug.Log("Make Workunit FilePath: " + filePath);

		if(Directory.Exists(Path.GetDirectoryName(filePath)) == false){
			Directory.CreateDirectory(Path.GetDirectoryName(filePath));
		}
		
		if(File.Exists(filePath))
		{
			File.Delete(filePath);
		}
		
		StreamWriter sw;
		FileInfo fi;
		fi = new FileInfo(filePath);
		sw = fi.AppendText();
		
		Guid guid = Guid.NewGuid();
		
		sw.WriteLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
		sw.WriteLine("<!-- Orca XML File Format -->");
		sw.WriteLine("<OrcaTrees ObjectTypeExpression=\"Full\" BinaryEncodingType=\"Base64\" FileVersion=\"3\" FileRevision=\"0\">");
		sw.WriteLine("  <OrcaTree OrcaName=\"(no name)\">");
		sw.WriteLine("    <Orca OrcaName=\"WorkUnit_" + cuesheetName +"\" VersionInfo=\"Ver.2.14.00\" FormatVersion=\"Ver.1.00.01\" WorkUnitPath=\"WorkUnits/WorkUnit_" + cuesheetName +"/WorkUnit_" + cuesheetName +".atmcunit\" UsedMaterialFlag=\"True\" Expand=\"True\" OrcaType=\"CriMw.CriAtomCraft.AcCore.AcOoWorkUnit\">");			
		sw.WriteLine("      <Orca OrcaName=\"References\" OrcaType=\"CriMw.CriAtomCraft.AcCore.AcOoReferenceFolder\">");
		sw.WriteLine("        <Orca OrcaName=\"AISAC\" OrcaType=\"CriMw.CriAtomCraft.AcCore.AcOoReferenceAisacFolder\" />");
		sw.WriteLine("      </Orca>");
		sw.WriteLine("      <Orca OrcaName=\"CueSheetFolder\" OrcaType=\"CriMw.CriAtomCraft.AcCore.AcOoCueSheetFolder\">");
		sw.WriteLine("        <Orca OrcaName=\"WorkUnit_" + cuesheetName +"\" Expand=\"True\" OrcaType=\"CriMw.CriAtomCraft.AcCore.AcOoCueSheetSubFolder\">");
		sw.WriteLine("          <Orca OrcaName=\"" + cuesheetName +"\" Expand=\"True\" OoUniqId=\"" + guid.ToString() + "\" CueSheetPaddingSize=\"2048\" OrcaType=\"CriMw.CriAtomCraft.AcCore.AcOoCueSheet\">");
		
		//	------ CUE -------
		int cueId = 0;
		foreach(string wavName in wavList){
			string cueString  = "            <Orca OrcaName=\"" + wavName + "\" SynthType=\"SynthPolyphonic\" CueID=\"" + cueId.ToString() + "\" ";
			if(defaultGroupCategory != String.Empty){
				//	CategoryGroup/CategoryName
				//	Category0="/CriAtomCraftV2Root/GlobalSettings/Categories/CategoryGroup_0/Category_0"
				cueString += "Category0=\"/CriAtomCraftV2Root/GlobalSettings/Categories/CategoryGroup_0/";
				cueString += defaultGroupCategory;
				cueString += "\" ";
			}
			cueString += "Pos3dDistanceMin=\""+pos3dDistanceMin+"\" Pos3dDistanceMax=\""+pos3dDistanceMax+"\" "; 
			cueString += "Pos3dDopplerCoefficient\""+pos3dDopplerCoefficient+"\" ";

			cueString += "DisplayUnit=\"Frame5994\" OrcaType=\"CriMw.CriAtomCraft.AcCore.AcOoCueSynthCue\">";
			
			sw.WriteLine(cueString);
			sw.WriteLine("              <Orca OrcaName=\"Track_" + wavName + "\" SynthType=\"Track\" SwitchRange=\"0.5\" DisplayUnit=\"Frame5994\" ObjectColor=\"200, 30, 100, 180\" OrcaType=\"CriMw.CriAtomCraft.AcCore.AcOoCueSynthTrack\">");
			sw.WriteLine("                <Orca OrcaName=\"" + wavName + ".wav\" PanType=\"Auto\" LinkWaveform=\"/CriAtomCraftV2Root/WorkUnits/WorkUnit_" + cuesheetName +"_MaterialInfo/MaterialRootFolder/" + wavName + ".wav\" SynthType=\"Waveform\" LinkWaveformPathName=\"" + wavName + ".wav\" OrcaType=\"CriMw.CriAtomCraft.AcCore.AcOoCueSynthWaveform\" />");
			sw.WriteLine("              </Orca>");
			sw.WriteLine("            </Orca>");
			
			cueId++;
		}
		//	--------------------
		
		sw.WriteLine("          </Orca>");
		sw.WriteLine("        </Orca>");
		sw.WriteLine("      </Orca>");
		sw.WriteLine("    </Orca>");
		sw.WriteLine("  </OrcaTree>");
		sw.WriteLine("</OrcaTrees>");
		sw.WriteLine("<!-- Copyright (c) CRI Middleware Co.,LTD. -->");
		sw.WriteLine("<!-- end of document -->");
		
		sw.Flush();
		sw.Close();
	}
	
	void MakeMaterialinfo(string cuesheetName,List<string> wavList)
	{
		string filePath = Application.dataPath + "/" + cuesheetName +"/" + cuesheetName +".materialinfo";

		Debug.Log("Make MaterialInfo FilePath: " + filePath);
		
		if(Directory.Exists(Path.GetDirectoryName(filePath)) == false){
			Directory.CreateDirectory(Path.GetDirectoryName(filePath));
		}
		
		if(File.Exists(filePath))
		{
			File.Delete(filePath);
		}
		
		StreamWriter sw;
		FileInfo fi;
		fi = new FileInfo(filePath);
		sw = fi.AppendText();
		
		sw.WriteLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
		sw.WriteLine("<!-- Orca XML File Format -->");
		sw.WriteLine("<OrcaTrees ObjectTypeExpression=\"Full\" BinaryEncodingType=\"Base64\" FileVersion=\"3\" FileRevision=\"0\">");
		sw.WriteLine("  <OrcaTree OrcaName=\"(no name)\">");
		sw.WriteLine("    <Orca OrcaName=\"WorkUnit_" + cuesheetName +"_MaterialInfo\" VersionInfo=\"Ver.2.14.00\" FormatVersion=\"Ver.1.00.00\" MaterialInfoPath=\"\" MaterialRootPath=\"Materials\" OrcaType=\"CriMw.CriAtomCraft.AcCore.AcOoMaterialInfoFile\">");
		sw.WriteLine("      <Orca OrcaName=\"MaterialRootFolder\" OrcaType=\"CriMw.CriAtomCraft.AcCore.AcOoWaveformFolder\">");
		
		//	----- WAV ------
		
		foreach(string wavName in wavList){
			sw.WriteLine("        <Orca OrcaName=\"" + wavName + ".wav\" OrcaType=\"CriMw.CriAtomCraft.AcCore.AcOoWaveform\" />");
		}
		//	--------------------
		
		sw.WriteLine("      </Orca>");
		sw.WriteLine("    </Orca>");
		sw.WriteLine("  </OrcaTree>");
		sw.WriteLine("</OrcaTrees>");
		sw.WriteLine("<!-- Copyright (c) CRI Middleware Co.,LTD. -->");
		sw.WriteLine("<!-- end of document -->");
		
		sw.Flush();
		sw.Close();
	}
	
	void CopyMaterialsFolder(string cuesheetName,string[] fileList,string srcMaterialsPath)
	{
		string filePath_materials = Application.dataPath + "/" + cuesheetName +"/Materials";

		Debug.Log("Copy MaterialFolder Path: " + filePath_materials);

		if(Directory.Exists(Path.GetDirectoryName(filePath_materials)) == false){
			Directory.CreateDirectory(Path.GetDirectoryName(filePath_materials));
		}
		
		if(Directory.Exists(filePath_materials) == false){
			Directory.CreateDirectory(filePath_materials);
		}
		
		foreach(string tmpFile in fileList){
			if(Path.GetExtension(tmpFile) == ".wav"){
				
				string filePath = filePath_materials + "/" + Path.GetFileName(tmpFile);
				if(File.Exists(filePath))
				{
					File.Delete(filePath);
				}
				
				File.Copy(tmpFile,filePath);
			}
		}
	}
}
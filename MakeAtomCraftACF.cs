using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;

public class AtomCategory
{
	public AtomCategory(string inName,int inLimit)
	{
		name = inName;
		limit = inLimit;
	}
	public string name = "Category_0";	
	public int limit = 8;
}

public class MakeAtomCraftACF : EditorWindow {

	private Vector2 scrollPos;
	private Vector2 scrollPos_Window;
	private Rect windowRect = new Rect(10, 10, 100, 100);
	private bool scaling = true;

	public string outputAcfName = "NewProject";		//	出力するACF名
	public List<AtomCategory> defaultGroupCategoryList = new List<AtomCategory>();

	[MenuItem("CRI/My/Open MakeAtomCraftACF ...")]
	static void OpenWindow()
	{
		EditorWindow.GetWindow<MakeAtomCraftACF>(false, "MakeAtomCraftACF");
	}

	void OnEnable()
	{
		//Debug.Log("Start");
		if(defaultGroupCategoryList.Count == 0){
			InitTemplateCategory();
		}
	}

	void InitTemplateCategory()
	{
		defaultGroupCategoryList.Add(new AtomCategory("SE",8));
		defaultGroupCategoryList.Add(new AtomCategory("BGM",1));
		defaultGroupCategoryList.Add(new AtomCategory("VOICE",1));
		defaultGroupCategoryList.Add(new AtomCategory("SYSTEM",2));
		defaultGroupCategoryList.Add(new AtomCategory("AMBIENT",2));
		defaultGroupCategoryList.Add(new AtomCategory("SPECIAL",1));
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
		EditorGUILayout.LabelField("ACF Name");
		outputAcfName = EditorGUILayout.TextField(outputAcfName);	
		EditorGUILayout.Space();
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("Cagetgory Name");
		EditorGUILayout.LabelField("Cagetgory Cue Limit");
		EditorGUILayout.EndHorizontal();
		for(int i = 0;i<defaultGroupCategoryList.Count;i++){
			EditorGUILayout.BeginHorizontal();
			defaultGroupCategoryList[i].name = EditorGUILayout.TextField(defaultGroupCategoryList[i].name);
			defaultGroupCategoryList[i].limit = EditorGUILayout.IntField(defaultGroupCategoryList[i].limit);
			EditorGUILayout.EndHorizontal();
		}
		EditorGUILayout.BeginHorizontal();
		if(GUILayout.Button("Add Category")){
			defaultGroupCategoryList.Add(new AtomCategory("",1));
		}
		if(GUILayout.Button("Remove Category")){
			if(defaultGroupCategoryList.Count > 0){
				defaultGroupCategoryList.RemoveAt(defaultGroupCategoryList.Count-1);
			} else {
				InitTemplateCategory();
			}
		}
		EditorGUILayout.EndHorizontal();
		EditorGUILayout.Space();
		if(GUILayout.Button("Make ACF Data")){
			DoMake();
		}
		EditorGUILayout.EndVertical();
	}

	// Use this for initialization
	void DoMake () {
		
		if(outputAcfName != string.Empty){

			MakeData(outputAcfName);

			Debug.Log("Make Atom Craft Data Finish!");
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	void MakeData(string outputAcfName)
	{
		MakeAtmcglobal(outputAcfName);
	}
	
	void MakeAtmcglobal(string outputAcfName)
	{
		string filePath = Application.dataPath + "/" + outputAcfName +"/" + outputAcfName +".atmcglobal";
		
		Debug.Log("Make Atom Craft Global FilePath: " + filePath);

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


		sw.WriteLine("    <Orca OrcaName=\"GlobalSettings\" VersionInfo=\"Ver.2.14.00\" FormatVersion=\"Ver.1.00.01\" OoUniqId=\"" + guid.ToString() + "\" OrcaType=\"CriMw.CriAtomCraft.AcCore.AcOoGlobalFolder\">");
		sw.WriteLine("      <Orca OrcaName=\"Categories\" Expand=\"True\" OrcaType=\"CriMw.CriAtomCraft.AcCore.AcOoCategoryFolder\">");

		sw.WriteLine("        <Orca OrcaName=\"CategoryGroup_0\" Expand=\"True\" OrcaType=\"CriMw.CriAtomCraft.AcCore.AcOoCategoryGroup\">");

		//	------ CATEGORY -------

		//	<Orca OrcaName="CategoryGroup_0" Expand="True" OrcaType="CriMw.CriAtomCraft.AcCore.AcOoCategoryGroup">

		int categoryId = 0;
		foreach(AtomCategory category in defaultGroupCategoryList){
			if(category.name != String.Empty){
				string categoryString  = "            <Orca OrcaName=\"" + category.name + "\" CategoryId=\"" + categoryId + "\" ";
				categoryString += "CueLimitEnableFlag=\"True\" CueLimits=\""+ category.limit +"\" "; 
				categoryString += "OrcaType=\"CriMw.CriAtomCraft.AcCore.AcOoCategory\"/>";
				
				sw.WriteLine(categoryString);

				categoryId++;
			}
		}
		//	--------------------

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

}
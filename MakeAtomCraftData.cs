using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;

public class MakeAtomCraftData : MonoBehaviour {
	
	public string outputCueSheetName = "Test";
	public string srcMaterialsFolder = "Materials";
	
	// Use this for initialization
	void Start () {
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
	
			sw.WriteLine("            <Orca OrcaName=\"" + wavName + "\" SynthType=\"SynthPolyphonic\" CueID=\"" + cueId.ToString() + "\" DisplayUnit=\"Frame5994\" OrcaType=\"CriMw.CriAtomCraft.AcCore.AcOoCueSynthCue\">");
			sw.WriteLine("              <Orca OrcaName=\"Track_" + wavName + "\" SynthType=\"Track\" SwitchRange=\"0.5\" DisplayUnit=\"Frame5994\" ObjectColor=\"200, 30, 100, 180\" OrcaType=\"CriMw.CriAtomCraft.AcCore.AcOoCueSynthTrack\">");
			sw.WriteLine("                <Orca OrcaName=\"" + wavName + ".wav\" LinkWaveform=\"/CriAtomCraftV2Root/WorkUnits/WorkUnit_" + cuesheetName +"_MaterialInfo/MaterialRootFolder/" + wavName + ".wav\" SynthType=\"Waveform\" LinkWaveformPathName=\"" + wavName + ".wav\" OrcaType=\"CriMw.CriAtomCraft.AcCore.AcOoCueSynthWaveform\" />");
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
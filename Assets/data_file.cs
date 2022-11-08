using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Collections.Generic;

public class data_file{

	public static void Create_file(string path, string name, string info){



		string title = "-----------"+System.DateTime.Now+"----------------";
		title += "\r\n"+"-----------"+"Mental Rotation Task of Shaperd"+"----------------";
		title += "\r\n" + "-----------" + "subject & session: " + name + "----------------" + "\r\n";
		info = title + info;

		StreamWriter sw;
		FileInfo t = new FileInfo (path + "//" + name);
		if (!t.Exists) {
			sw = t.CreateText ();
		} else {
			sw = t.AppendText ();
		}
		sw.WriteLine (info);
		sw.Close ();
		sw.Dispose ();



	}
}

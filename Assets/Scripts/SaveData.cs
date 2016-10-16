using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public static class SaveData {
	

	public static void storeScore(int sc) {
		BinaryFormatter bf = new BinaryFormatter ();
		FileStream fs = File.Create (Application.persistentDataPath + "/game_save_data.gd");
		List<int> savedGames = new List<int>();
		savedGames.Add (sc);
		bf.Serialize (fs, savedGames);
		fs.Close ();
	}

	public static int loadScore() {
		Debug.Log ("File location: " + Application.persistentDataPath + "/game_save_data.gd");
		if (File.Exists (Application.persistentDataPath + "/game_save_data.gd")) {
			BinaryFormatter bf = new BinaryFormatter ();
			List<int> savedGames;
			FileStream fs = File.Open (Application.persistentDataPath + "/game_save_data.gd", FileMode.Open);
			savedGames = (List<int>)bf.Deserialize (fs);
			fs.Close ();
			return savedGames[0];
		} else {
			return 0;
		}

	}
}

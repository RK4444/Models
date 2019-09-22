using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Unity.Mathematics;
using System.Runtime.Serialization.Formatters.Binary;

namespace AdditionalContent
{
	namespace ConversionClasses
	{
		public static class AdditionalVectorOperations {
			public static Vector3 With(this Vector3 original, float? x = null, float? y = null, float? z = null) {
				return new Vector3(x ?? original.x, y ?? original.y, z ?? original.z);
			}

			public static Vector3 DirectionTo(this Vector3 source, Vector3 destination) {
				return Vector3.Normalize(destination - source);
			}

			public static Quaternion ToOldQuaternion(this quaternion newQuaternion) {
				return new Quaternion(newQuaternion.value.x, newQuaternion.value.y, newQuaternion.value.z, newQuaternion.value.w);
			}

			public static quaternion ToNewQuaternion(this Quaternion oldQuaternion) {
				return new quaternion(newQuaternion.x, newQuaternion.y, newQuaternion.z, newQuaternion.w);
			}

			public static Vector3 ToVector3(this float3 origin) {
				return new Vector3(origin.x, origin.y, origin.z);
			}

			public static float3 ToFloat3(this Vector3 origin) {
				return new float3(origin.x, origin.y, origin.z);
			}
		}
		
	}

	namespace Data
	{
		#region Save-Load mecanism
			
		public static class SaveLoad
		{
			public static void Save (Vector3 playerPosition, Quaternion playerRotation, string SaveFileName) {
				//string ser1 = JsonUtility.ToJson (Player);
				BinaryFormatter bf = new BinaryFormatter();
				FileStream file = File.Create (Application.persistentDataPath + "/" + SaveFileName + ".dat");

				SaveField data = new SaveField (playerPosition, playerRotation);

				bf.Serialize (file, data);
				file.Close();


			}

			public static void Save (Vector3 playerPosition, Quaternion playerRotation, int playerHealth, float playerCooldown, float playerStrength, string SaveFileName) {
				//string ser1 = JsonUtility.ToJson (Player);
				BinaryFormatter bf = new BinaryFormatter();
				FileStream file = File.Create (Application.persistentDataPath + "/" + SaveFileName + ".dat");

				SaveField data = new SaveField (playerPosition, playerRotation, playerHealth, playerCooldown, playerStrength);

				bf.Serialize (file, data);
				file.Close();


			}

			public static PlayerInfo Load (string SaveFileName) {
				//path = Application.persistentDataPath + "/Storage.json";
				//jsonstring1 = File.ReadAllText (path);
				//Player = JsonUtility.FromJson<SaveField> (jsonstring1);

				if (File.Exists (Application.persistentDataPath + "/" + SaveFileName + ".dat")) {
					BinaryFormatter bf = new BinaryFormatter ();
					FileStream file = File.Open (Application.persistentDataPath + "/" + SaveFileName + ".dat", FileMode.Open);

					SaveField data = (SaveField)bf.Deserialize (file);
					file.Close ();

					PlayerInfo PInfo = new PlayerInfo(data.GetPosition(), data.GetRotation(), data.health, data.coolDown, data.Strength);

					return PInfo;
					
				}

				return null;
			}
		}

		public static class SaveLoadUpgraded
		{
			T holder;

			public static void Save<T> (T toSerialize) {
				//string ser1 = JsonUtility.ToJson (Player);
				holder = toSerialize;
				BinaryFormatter bf = new BinaryFormatter();
				FileStream file = File.Create (Application.persistentDataPath + "/" + SaveFileName + ".dat");

				bf.Serialize (file, holder);
				file.Close();


			}

			public static T Load (string SaveFileName) {
				//path = Application.persistentDataPath + "/Storage.json";
				//jsonstring1 = File.ReadAllText (path);
				//Player = JsonUtility.FromJson<SaveField> (jsonstring1);

				if (File.Exists (Application.persistentDataPath + "/" + SaveFileName + ".dat")) {
					BinaryFormatter bf = new BinaryFormatter ();
					FileStream file = File.Open (Application.persistentDataPath + "/" + SaveFileName + ".dat", FileMode.Open);

					T data = (T)bf.Deserialize (file);
					file.Close ();

					return data;
					
				}

				return null;
			}
		}
		#endregion
		
		#region Textures
		public static class AdditionalTextureOperations
		{
			public static Texture2DArray GenerateTextureArray(Texture2D[] texes1, int TextureSize, TextureFormat texforMat1) {
				Texture2DArray texArray1 = new Texture2DArray(TextureSize, TextureSize, texes1.Length, texforMat1, true);
				for (int i = 0; i < texes1.Length; i++)
				{
					texArray1.SetPixels(texes1[i].GetPixels(), i);
				}
				texArray1.Apply();
				return texArray1;
			}
		}
			
		#endregion
		interface ICustomUpdateMethod
		{
			void OnOwnUpdate();
		}

		#region Helperclasses for SaveLoadSystem
			public class PlayerInfo
			{
				public Vector3 PPosition;
				public Quaternion RRotation;
				public int PHealth;
				public float PCooldown;
				public float PStrength;

				public PlayerInfo(Vector3 nPos, Quaternion nRot, int nHealth, float nCooldown, float nStrength) {
					PPosition = nPos;
					RRotation = nRot;
					PHealth = nHealth;
					PCooldown = nCooldown;
					PStrength = nStrength;
				}

				[System.Serializable]
				public class SaveField {
					public float posx;
					public float posy;
					public float posz;
					public float rotx;
					public float roty;
					public float rotz;
					public float rotw;
					public int health;
					public float coolDown;
					public float Strength;

					public SaveField(Vector3 position, Quaternion rotation) {
						posx = position.x;
						posy = position.y;
						posz = position.z;

						rotx = rotation.x;
						roty = rotation.y;
						rotz = rotation.z;
						rotw = rotation.w;
					}
					
					public SaveField(Vector3 position, Quaternion rotation, int newHelth, float newCooldown, float newStrength) {
						posx = position.x;
						posy = position.y;
						posz = position.z;

						rotx = rotation.x;
						roty = rotation.y;
						rotz = rotation.z;
						rotw = rotation.w;

						health = newHelth;
						coolDown = newCooldown;
						Strength = newStrength;
					}

					public Vector3 GetPosition() {
						return new Vector3(posx, posy, posz);
					}

					public Quaternion GetRotation() {
						return new Quaternion(rotx, roty, rotz, rotw);
					}

				}
			}
			
		#endregion

	}


}

﻿using Asyncoroutine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace R1Engine {
    public static class FileSystem {
		public enum Mode {
			Normal, Web
		}
		public static Mode mode = Mode.Normal;
		private struct BigFileEntry {
			public int cacheLength;
			public long fileLength;
			public BigFileEntry(int cacheLength, long fileLength) {
				this.cacheLength = cacheLength;
				this.fileLength = fileLength;
			}
		}

		private static Dictionary<string, byte[]> virtualFiles = new Dictionary<string, byte[]>();
		private static Dictionary<string, BigFileEntry> virtualBigFiles = new Dictionary<string, BigFileEntry>();
		private static Dictionary<string, bool> existingDirectories = new Dictionary<string, bool>();
        public static string serverAddress = "https://raym.app/maps/data/";

        public static bool DirectoryExists(string path) {
			if (path == null || path.Trim() == "") return false;
			if (FileSystem.mode == FileSystem.Mode.Web) { // || (Application.isEditor && UnityEditor.EditorUserBuildSettings.activeBuildTarget == UnityEditor.BuildTarget.WebGL)) {
				if (existingDirectories.ContainsKey(path) && existingDirectories[path] == true) return true;
				return false;
			} else {
                return Directory.Exists(path);
            }
        }

        public static bool FileExists(string path) {
			if (path == null || path.Trim() == "") return false;
            if (FileSystem.mode == FileSystem.Mode.Web) { // || (Application.isEditor && UnityEditor.EditorUserBuildSettings.activeBuildTarget == UnityEditor.BuildTarget.WebGL)) {
                return ((virtualFiles.ContainsKey(path) && virtualFiles[path] != null) || (virtualBigFiles.ContainsKey(path)));
            } else {
                return File.Exists(path);
            }
        }

        public static void AddVirtualFile(string path, byte[] data) {
            virtualFiles[path] = data;
        }
		public static void AddVirtualBigFile(string path, long size, int cacheLength) {
			virtualBigFiles[path] = new BigFileEntry(cacheLength, size);
		}

		public static Stream GetFileReadStream(string path) {
			if (path == null || path.Trim() == "") return null;
			if (FileSystem.mode == FileSystem.Mode.Web) { // || (Application.isEditor && UnityEditor.EditorUserBuildSettings.activeBuildTarget == UnityEditor.BuildTarget.WebGL)) {
				if (virtualFiles.ContainsKey(path)) {
					return new MemoryStream(virtualFiles[path]);
				} else if (virtualBigFiles.ContainsKey(path)) {
					return new PartialHttpStream(serverAddress + path, cacheLen: virtualBigFiles[path].cacheLength, length: virtualBigFiles[path].fileLength);
				} else return null;
            } else {
                return File.OpenRead(path);
            }
        }

		public static Stream GetFileWriteStream(string path) {
			if (path == null || path.Trim() == "") return null;
			if (FileSystem.mode == FileSystem.Mode.Web) { // || (Application.isEditor && UnityEditor.EditorUserBuildSettings.activeBuildTarget == UnityEditor.BuildTarget.WebGL)) {
				return null; // Can't write in web mode
			} else {
				return File.Create(path);
			}
		}

        public static async Task DownloadFile(string path) {
			if (virtualFiles.ContainsKey(path) && virtualFiles[path] != null) return;
			Debug.Log("Downloading " + path);
			await Controller.WaitIfNecessary();
			UnityWebRequest www = UnityWebRequest.Get(serverAddress + path);
            await www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError) {
                Debug.Log(www.error);
                virtualFiles[path] = null;
            } else {
                // Or retrieve results as binary data
                AddVirtualFile(path, www.downloadHandler.data);
            }
        }

		public static async Task CheckDirectory(string path) {
			if (existingDirectories.ContainsKey(path)) return;
			await Controller.WaitIfNecessary();
			UnityWebRequest www = UnityWebRequest.Head(serverAddress + path + "/");
			await www.SendWebRequest();
			while (!www.isDone) {
				await new WaitForEndOfFrame();
			}
			if (!www.isHttpError && !www.isNetworkError) {
				existingDirectories.Add(path, true);
			} else {
				existingDirectories.Add(path, false);
			}
		}

		public static async Task InitBigFile(string path, int cacheLength) {
			UnityWebRequest www = UnityWebRequest.Head(serverAddress + path);
			await Controller.WaitIfNecessary();
			await www.SendWebRequest();
			while (!www.isDone) {
				await new WaitForEndOfFrame();
			}
			if (!www.isHttpError && !www.isNetworkError) {
				long contentLength;
				if (long.TryParse(www.GetResponseHeader("Content-Length"), out contentLength)) {
					AddVirtualBigFile(path, contentLength, cacheLength);
				}
			}
		}

        public static long GetFileLength(string path) {
            if (FileExists(path)) {
                if (FileSystem.mode == FileSystem.Mode.Web) {
					if (virtualFiles.ContainsKey(path)) {
						return virtualFiles[path].Length;
					} else if (virtualBigFiles.ContainsKey(path)) {
						return virtualBigFiles[path].fileLength;
					} else return 0;
                } else {
                    return new System.IO.FileInfo(path).Length;
                }
            } else {
                return 0;
            }
        }

		public static async Task PrepareFile(string path) {
			if (FileSystem.mode == FileSystem.Mode.Web && !string.IsNullOrEmpty(path)) {
				string state = Controller.status;
				Controller.status = state + "\nDownloading file: " + path;
				await FileSystem.DownloadFile(path);
				Controller.status = state;
				await Controller.WaitIfNecessary();
			}
		}

		public static async Task PrepareBigFile(string path, int cacheLength) {
			if (FileSystem.mode == FileSystem.Mode.Web) {
				string state = Controller.status;
				Controller.status = state + "\nInitializing bigfile: " + path + " (Cache size: " + Util.SizeSuffix(cacheLength, 0) + ")";
				await FileSystem.InitBigFile(path, cacheLength);
				Controller.status = state;
				await Controller.WaitIfNecessary();
			}
		}

		public static string GetFileNameWithoutExtensions(string path)
        {
			return Path.GetFileName(path)?.Split('.').FirstOrDefault();
        }
	}
}

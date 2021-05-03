﻿using System.IO;
using System.Text;
using System.Threading.Tasks;
using BinarySerializer;
using UnityEngine;
using ILogger = BinarySerializer.ILogger;

namespace R1Engine
{
    public class R1Context : Context
    {
        public R1Context(string basePath, GameSettings settings) : base(
            basePath: basePath, // Pass in the base path
            settings: settings, // Pass in the settings
            serializerLog: new R1SerializerLog(), // Use R1 serializer log for logging to a file
            fileManager: new R1FileManager(), // Use R1 file manager for use with FileSystem
            logger: new UnityLogger()) // Use Unity logger
        { }
        public R1Context(GameSettings settings) : this(settings.GameDirectory, settings) { }

        public new GameSettings Settings => GetSettings<GameSettings>();

        public class R1FileManager : IFileManager
        {
            public bool DirectoryExists(string path) => FileSystem.DirectoryExists(path);

            public bool FileExists(string path) => FileSystem.FileExists(path);

            public Stream GetFileReadStream(string path) => FileSystem.GetFileReadStream(path);

            public Stream GetFileWriteStream(string path, bool recreateOnWrite = true) => FileSystem.GetFileWriteStream(path, recreateOnWrite);

            public async Task FillCacheForReadAsync(int length, Reader reader)
            {
                if (reader.BaseStream.InnerStream is PartialHttpStream httpStream)
                    await httpStream.FillCacheForRead(length);
            }
        }
        
        public class UnityLogger : ILogger
        {
            public void Log(object log) => Debug.Log(log);
            public void LogWarning(object log) => Debug.LogWarning(log);
            public void LogError(object log) => Debug.LogError(log);
        }

        public class R1SerializerLog : ISerializerLog
        {
            public bool IsEnabled => R1Engine.Settings.Log;

            private StreamWriter _logWriter;

            protected StreamWriter LogWriter => _logWriter ??= GetFile();

            public string OverrideLogPath { get; set; }
            public string LogFile => OverrideLogPath ?? R1Engine.Settings.LogFile;
            public int BufferSize => 0x8000000; // 1 GB

            public StreamWriter GetFile()
            {
                return new StreamWriter(File.Open(LogFile, FileMode.Create, FileAccess.Write, FileShare.ReadWrite), Encoding.UTF8, BufferSize);
            }

            public void Log(object obj)
            {
                if (IsEnabled)
                    LogWriter.WriteLine(obj != null ? obj.ToString() : "");
            }

            public void Dispose()
            {
                _logWriter?.Dispose();
                _logWriter = null;
            }
        }
    }
}
﻿using System;
using System.Collections.Generic;
using System.IO;

namespace cake
{
	public class GenerationRecord
	{
		public string TargetFile { get; private set; }
		public TargetGenerateSettings Settings { get; private set; }

		private readonly Dictionary<string, DateTime> _modificationDates = new Dictionary<string, DateTime>();

		public GenerationRecord(string targetFile, TargetGenerateSettings settings)
		{
			TargetFile = targetFile;
			Settings = settings;

			RecordModificationDate(targetFile);
			foreach (var inputFile in settings.InputFiles)
				RecordModificationDate(inputFile);
		}

		private void RecordModificationDate(string file)
		{
			if (!File.Exists(file))
				throw new ArgumentException("GenerationRecord being generated with inputfile: "+file+" which does not exist.");
			_modificationDates.Add(file,File.GetLastWriteTimeUtc(file));
		}

		public DateTime ModificationTimeOf(string file)
		{
			return _modificationDates[file];
		}

		public DateTime ModificationTimeOfTargetFile()
		{
			return ModificationTimeOf(TargetFile);
		}
	}
}